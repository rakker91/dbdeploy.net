// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BogusDatabaseMock.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test.Database
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances;
    using Veracity.Utilities.DatabaseDeploy.FileManagement;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    /// <summary>
    /// A bogus implementation of DatabaseSerivceBase
    /// </summary>
    public class BogusDatabaseMock : DatabaseServiceBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the BogusDatabaseMock class
        /// </summary>
        public BogusDatabaseMock()
            : base()
        {
            this.ScriptsRun = new List<string>();
            this.ScriptsRunNames = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BogusDatabaseMock"/> class.
        /// </summary>
        /// <param name="configurationService">
        /// The configuration service. 
        /// </param>
        /// <param name="fileService">
        /// The file service to use for file operations. 
        /// </param>
        /// <param name="tokenReplacer">The token replacer to use.</param>
        public BogusDatabaseMock(IConfigurationService configurationService, IFileService fileService, ITokenReplacer tokenReplacer)
            : base(configurationService, fileService, tokenReplacer)
        {
            this.ScriptsRun = new List<string>();
            this.ScriptsRunNames = new List<string>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the database type for the class.
        /// </summary>
        public override string DatabaseType
        {
            get
            {
                return "mssql";
            }
        }

        /// <summary>
        ///   Gets or sets the list of scripts that have been run.
        /// </summary>
        public IList<string> ScriptsRun { get; set; }

        /// <summary>
        /// Gets or sets the list of scripts names that have been run.
        /// </summary>
        public IList<string> ScriptsRunNames { get; set; }

        /// <summary>
        /// Gets or sets the dataset to return.
        /// </summary>
        public DataSet DataSetToReturn { get; set; }

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
        public override void ExecuteScript(string scriptFileName, params DbParameter[] parameters)
        {
            string script = this.GetCommandText(scriptFileName);

            this.ScriptsRun.Add(script);
            this.ScriptsRunNames.Add(scriptFileName);
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
        public override DataSet RunScript(string scriptFileName, params DbParameter[] parameters)
        {
            string script = this.GetCommandText(scriptFileName);
            this.ScriptsRun.Add(script);
            this.ScriptsRunNames.Add(scriptFileName);

            DataSet result = this.DataSetToReturn;

            return result;
        }

        #endregion
    }
}