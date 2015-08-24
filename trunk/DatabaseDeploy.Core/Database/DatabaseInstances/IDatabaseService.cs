// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IDatabaseService.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Database.DatabaseInstances
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    using DatabaseDeploy.Core.Configuration;

    /// <summary>
    /// Represents a class responsible for making calls to the database.
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// Gets or sets the configuration service to use for generation
        /// </summary>
        /// <value>The configuration service.</value>
        IConfigurationService ConfigurationService { get; set; }

        /// <summary>
        /// Gets the database type for the class.
        /// </summary>
        /// <value>The type of the database.</value>
        string DatabaseType { get; }

        /// <summary>
        /// Runs a script without returning results. Use RunScript if a result is expected.
        /// </summary>
        /// <param name="scriptFileName">The name of the scrip that will be run.</param>
        /// <param name="parameters">The parameters for the script run.</param>
        void ExecuteScript(string scriptFileName, params DbParameter[] parameters);

        /// <summary>
        /// Gets the applied changes already in the database
        /// </summary>
        /// <returns>A dictionary containing the change logs and the ids for those change logs</returns>
        IDictionary<decimal, IChangeLog> GetAppliedChanges();

        /// <summary>
        /// Gets the database script from a file.
        /// </summary>
        /// <param name="scriptFileName">The script to retrieve.</param>
        /// <returns>A string containing the script to be run.</returns>
        string GetScriptFromFile(string scriptFileName);

        /// <summary>
        /// Runs a script and returns a result.
        /// </summary>
        /// <param name="scriptFileName">The name of a script that will be executed.</param>
        /// <param name="parameters">The parameters for the script</param>
        /// <returns>A dataset containing the results from the script run</returns>
        DataSet RunScript(string scriptFileName, params DbParameter[] parameters);
    }
}