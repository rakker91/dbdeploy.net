// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MySqlDatabaseService.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Database.DatabaseInstances.MySql
{
    using System;
    using System.Data;
    using System.Data.Common;

    using DatabaseDeploy.Core.Configuration;
    using DatabaseDeploy.Core.FileManagement;
    using DatabaseDeploy.Core.Utilities;

    /// <summary>
    /// Represents a MySql database implementation
    /// </summary>
    public class MySqlDatabaseService : DatabaseServiceBase, IMySqlDatabaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDatabaseService" /> class.
        /// </summary>
        /// <param name="configurationService">The configuration service.</param>
        /// <param name="fileService">The file service to use</param>
        /// <param name="tokenReplacer">The token replacer to use.</param>
        public MySqlDatabaseService(
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
                return "mysql";
            }
        }

        /// <summary>
        /// Runs a script without returning results. Use RunScript if a result is expected.
        /// </summary>
        /// <param name="scriptFileName">The name of the script to be executed.</param>
        /// <param name="parameters">The parameters for the script run.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void ExecuteScript(string scriptFileName, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Runs a script and returns a result.
        /// </summary>
        /// <param name="scriptFileName">The name of the script to be executed.</param>
        /// <param name="parameters">The parameters for the script</param>
        /// <returns>A dataset containing the results from the script run</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override DataSet RunScript(string scriptFileName, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}