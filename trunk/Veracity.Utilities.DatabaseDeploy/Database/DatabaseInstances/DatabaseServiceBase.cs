// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseServiceBase.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances
{
    #region Usings

    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;

    using log4net;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.FileManagement;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    #endregion

    /// <summary>
    /// A base class for all database implementations.
    /// </summary>
    public abstract class DatabaseServiceBase : IDatabaseService
    {
        #region Constants and Fields

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(DatabaseServiceBase));

        /// <summary>
        ///   The token replacer to use for script token replacement.
        /// </summary>
        private ITokenReplacer tokenReplacer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="DatabaseServiceBase" /> class.
        /// </summary>
        public DatabaseServiceBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseServiceBase"/> class.
        /// </summary>
        /// <param name="configurationService">
        /// The configuration service. 
        /// </param>
        /// <param name="fileService">
        /// The file service to use for file operations. 
        /// </param>
        /// <param name="tokenReplacer">
        /// The token Replacer.
        /// </param>
        public DatabaseServiceBase(IConfigurationService configurationService, IFileService fileService, ITokenReplacer tokenReplacer)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(configurationService, fileService, tokenReplacer));
            }

            this.ConfigurationService = configurationService;
            this.FileService = fileService;
            this.tokenReplacer = tokenReplacer;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the configuration service to use for generation
        /// </summary>
        public IConfigurationService ConfigurationService { get; set; }

        /// <summary>
        ///   Gets the database type for the class.
        /// </summary>
        public abstract string DatabaseType { get; }

        /// <summary>
        ///   Gets or sets the file service to use for file operations.
        /// </summary>
        public IFileService FileService { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs a script without returning results. Use RunScript if a result is expected.
        /// </summary>
        /// <param name="scriptFileName">
        /// The name of a script file that will be executed. 
        /// </param>
        /// <param name="parameters">
        /// The parameters for the script run. 
        /// </param>
        public abstract void ExecuteScript(string scriptFileName, params DbParameter[] parameters);

        /// <summary>
        /// Gets the applied changes already in the database
        /// </summary>
        /// <returns>
        /// A dictionary containing the change logs and the ids for those change logs 
        /// </returns>
        public IDictionary<int, IChangeLog> GetAppliedChanges()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext());
            }

            IDictionary<int, IChangeLog> result = new ConcurrentDictionary<int, IChangeLog>();

            this.EnsureChangelogExists();

            DataSet changeLogDataSet = this.GetChangelog();

            // we're assuming that this will return a single table.  Can't be sure what the name will be so using the index.
            foreach (DataRow row in changeLogDataSet.Tables[0].Rows)
            {
                IChangeLog changeLog = new ChangeLog();
                changeLog.Parse(row);
                result.Add(changeLog.ChangeNumber, changeLog);
            }

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult(result));
            }

            return result;
        }

        /// <summary>
        /// Gets the database script from a file.
        /// </summary>
        /// <param name="scriptFileName">
        /// The script to retrieve. 
        /// </param>
        /// <returns>
        /// A string containing the script to be run. 
        /// </returns>
        public string GetScriptFromFile(string scriptFileName)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(scriptFileName));
            }

            string fileName = Path.Combine(this.ConfigurationService.DatabaseScriptPath, scriptFileName);

            string result = this.FileService.GetFileContents(fileName, true);            

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult(result));
            }

            return result;
        }

        /// <summary>
        /// Gets a script and prepares it for execution.
        /// </summary>
        /// <param name="scriptFileName">The name of the script to get</param>
        /// <returns>A sql string read for execution.</returns>
        protected internal virtual string GetCommandText(string scriptFileName)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(scriptFileName));
            }

            string contents = this.GetScriptFromFile(scriptFileName);

            string result = this.tokenReplacer.Replace(contents);

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult(result));
            }

            return result;
        }

        /// <summary>
        /// Runs a script and returns a result.
        /// </summary>
        /// <param name="scriptFileName">
        /// The name of a script file that will be executed. 
        /// </param>
        /// <param name="parameters">
        /// The parameters for the script 
        /// </param>
        /// <returns>
        /// A dataset containing the results from the script run 
        /// </returns>
        public abstract DataSet RunScript(string scriptFileName, params DbParameter[] parameters);

        #endregion

        #region Methods

        /// <summary>
        /// Ensures that the changelog table exists.
        /// </summary>
        private void EnsureChangelogExists()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext());
            }

            this.ExecuteScript(DatabaseScriptEnum.EnsureChangeLogExists);

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult());
            }
        }

        /// <summary>
        /// Gets the changelog table as a dataset
        /// </summary>
        /// <returns>
        /// A dataset containing the changelogs 
        /// </returns>
        private DataSet GetChangelog()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext());
            }

            DataSet result = this.RunScript(DatabaseScriptEnum.GetChangeLog);

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult(result));
            }

            return result;
        }

        #endregion
    }
}