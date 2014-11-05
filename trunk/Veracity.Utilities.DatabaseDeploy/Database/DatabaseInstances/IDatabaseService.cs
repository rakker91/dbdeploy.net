// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDatabaseService.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances
{
    #region Usings

    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    using Veracity.Utilities.DatabaseDeploy.Configuration;

    #endregion

    /// <summary>
    /// Represents a class responsible for making calls to the database.
    /// </summary>
    public interface IDatabaseService
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the configuration service to use for generation
        /// </summary>
        IConfigurationService ConfigurationService { get; set; }

        /// <summary>
        ///   Gets the database type for the class.
        /// </summary>
        string DatabaseType { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs a script without returning results. Use RunScript if a result is expected.
        /// </summary>
        /// <param name="scriptFileName">
        /// The name of the scrip that will be run. 
        /// </param>
        /// <param name="parameters">
        /// The parameters for the script run. 
        /// </param>
        void ExecuteScript(string scriptFileName, params DbParameter[] parameters);

        /// <summary>
        /// Gets the applied changes already in the database
        /// </summary>
        /// <returns>
        /// A dictionary containing the change logs and the ids for those change logs 
        /// </returns>
        IDictionary<decimal, IChangeLog> GetAppliedChanges();

        /// <summary>
        /// Gets the database script from a file.
        /// </summary>
        /// <param name="scriptFileName">
        /// The script to retrieve. 
        /// </param>
        /// <returns>
        /// A string containing the script to be run. 
        /// </returns>
        string GetScriptFromFile(string scriptFileName);

        /// <summary>
        /// Runs a script and returns a result.
        /// </summary>
        /// <param name="scriptFileName">
        /// The name of a script that will be executed.
        /// </param>
        /// <param name="parameters">
        /// The parameters for the script 
        /// </param>
        /// <returns>
        /// A dataset containing the results from the script run 
        /// </returns>
        DataSet RunScript(string scriptFileName, params DbParameter[] parameters);

        #endregion
    }
}