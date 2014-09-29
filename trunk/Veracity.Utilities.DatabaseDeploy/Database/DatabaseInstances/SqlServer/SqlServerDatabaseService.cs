// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlServerDatabaseService.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances.SqlServer
{
    #region Usings

    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    using log4net;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.FileManagement;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    #endregion

    /// <summary>
    /// Represents a SQL Server database instance
    /// </summary>
    public class SqlServerDatabaseService : DatabaseServiceBase, ISqlServerDatabaseService
    {
        #region Constants and Fields

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(SqlServerDatabaseService));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDatabaseService"/> class.
        /// </summary>
        /// <param name="configurationService">
        /// The configuration service. 
        /// </param>
        /// <param name="fileService">
        /// The file service to use. 
        /// </param>
        /// <param name="tokenReplacer">The token replacer to use.</param>
        public SqlServerDatabaseService(IConfigurationService configurationService, IFileService fileService, ITokenReplacer tokenReplacer)
            : base(configurationService, fileService, tokenReplacer)
        {
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
            log.DebugIfEnabled(LogUtility.GetContext(scriptFileName, parameters));

            string script = GetCommandText(scriptFileName);

            string[] commands = script.Split(new [] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            using (var connection = new SqlConnection(ConfigurationService.ConnectionString))
            {
                connection.Open();

                foreach (string subScript in commands)
                {
                    using (var command = new SqlCommand(subScript, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Runs a script and returns a result.
        /// </summary>
        /// <param name="scriptFileName">
        /// The name of a script file that will be executed
        /// </param>
        /// <param name="parameters">
        /// The parameters for the script 
        /// </param>
        /// <returns>
        /// A dataset containing the results from the script run 
        /// </returns>
        public override DataSet RunScript(string scriptFileName, params DbParameter[] parameters)
        {
            log.DebugIfEnabled(LogUtility.GetContext(scriptFileName, parameters));

            string script = GetCommandText(scriptFileName);
            var result = new DataSet();

            using (var adapter = new SqlDataAdapter(script, ConfigurationService.ConnectionString))
            {
                adapter.SelectCommand.Parameters.AddRange(parameters);
                adapter.Fill(result);
            }

            log.DebugIfEnabled(LogUtility.GetResult(result));

            return result;
        }

        #endregion
    }
}