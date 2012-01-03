// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OracleDatabaseService.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances.Oracle
{
    #region Usings

    using System;
    using System.Data;
    using System.Data.Common;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.FileManagement;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    #endregion

    /// <summary>
    /// Represents an oracle database instance
    /// </summary>
    public class OracleDatabaseService : DatabaseServiceBase, IOracleDatabaseService
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDatabaseService"/> class.
        /// </summary>
        /// <param name="configurationService">
        /// The configuration service. 
        /// </param>
        /// <param name="fileService">
        /// The file service to use 
        /// </param>
        /// <param name="tokenReplacer">The token replacer to use</param>
        public OracleDatabaseService(IConfigurationService configurationService, IFileService fileService, ITokenReplacer tokenReplacer)
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
                return "ora";
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs a script without returning results. Use RunScript if a result is expected.
        /// </summary>
        /// <param name="scriptFileName">
        /// The name of a script file that will be executed
        /// </param>
        /// <param name="parameters">
        /// The parameters for the script run. 
        /// </param>
        public override void ExecuteScript(string scriptFileName, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        #endregion
    }
}