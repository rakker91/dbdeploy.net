// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SqlServerDatabaseService.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Database.DatabaseInstances.SqlServer
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    using DatabaseDeploy.Core.Configuration;
    using DatabaseDeploy.Core.FileManagement;
    using DatabaseDeploy.Core.Utilities;

    /// <summary>
    /// Represents a SQL Server database instance
    /// </summary>
    public class SqlServerDatabaseService : DatabaseServiceBase, ISqlServerDatabaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDatabaseService" /> class.
        /// </summary>
        /// <param name="configurationService">The configuration service.</param>
        /// <param name="fileService">The file service to use.</param>
        /// <param name="tokenReplacer">The token replacer to use.</param>
        public SqlServerDatabaseService(
            IConfigurationService configurationService,
            IFileService fileService,
            ITokenReplacer tokenReplacer)
            : base(configurationService, fileService, tokenReplacer)
        {
        }

        /// <summary>
        /// Gets the database type for the class.
        /// </summary>
        /// <value>The type of the database.</value>
        public override string DatabaseType
        {
            get
            {
                return "mssql";
            }
        }

        /// <summary>
        /// Runs a script without returning results. Use RunScript if a result is expected.
        /// </summary>
        /// <param name="scriptFileName">The name of a script file that will be executed.</param>
        /// <param name="parameters">The parameters for the script run.</param>
        public override void ExecuteScript(string scriptFileName, params DbParameter[] parameters)
        {
            string script = this.GetCommandText(scriptFileName);

            string[] commands = script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            using (SqlConnection connection = new SqlConnection(this.ConfigurationService.ConnectionString))
            {
                connection.Open();

                foreach (string subScript in commands)
                {
                    using (SqlCommand command = new SqlCommand(subScript, connection))
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
        /// <param name="scriptFileName">The name of a script file that will be executed</param>
        /// <param name="parameters">The parameters for the script</param>
        /// <returns>A dataset containing the results from the script run</returns>
        public override DataSet RunScript(string scriptFileName, params DbParameter[] parameters)
        {
            string script = this.GetCommandText(scriptFileName);
            DataSet result = new DataSet();

            using (SqlDataAdapter adapter = new SqlDataAdapter(script, this.ConfigurationService.ConnectionString))
            {
                adapter.SelectCommand.Parameters.AddRange(parameters);
                adapter.Fill(result);
            }

            return result;
        }
    }
}