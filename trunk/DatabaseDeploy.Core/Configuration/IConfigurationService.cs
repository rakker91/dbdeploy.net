// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IConfigurationService.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Configuration
{
    using DatabaseDeploy.Core.Database;
    using DatabaseDeploy.Core.Database.DatabaseInstances;

    /// <summary>
    /// The Configuration service service.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets or sets the name of the change log table.
        /// </summary>
        /// <value>The change log.</value>
        string ChangeLog { get; set; }

        /// <summary>
        /// Gets or sets a connection string to be used by the system
        /// </summary>
        /// <value>The connection string.</value>
        string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the type of DBMS to use.
        /// </summary>
        /// <value>The database management system.</value>
        DatabaseTypesEnum DatabaseManagementSystem { get; set; }

        /// <summary>
        /// Gets or sets the database script path
        /// </summary>
        /// <value>The database script path.</value>
        string DatabaseScriptPath { get; set; }

        /// <summary>
        /// Gets or sets the pattern to use for parsing the name of the script file
        /// </summary>
        /// <value>The file name pattern.</value>
        string FileNamePattern { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the last change that should be applied to the database.
        /// </summary>
        /// <value>The last change to apply.</value>
        /// <remarks>Set to 0 or max value (the default) to apply all changes. Any other positive number will stop applying changes at
        /// that level.</remarks>
        int LastChangeToApply { get; set; }

        /// <summary>
        /// Gets or sets the directory and file name that will be used for writing out the change script
        /// </summary>
        /// <value>The output file.</value>
        string OutputFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the script search should be recursive
        /// </summary>
        /// <value><c>true</c> if recursive; otherwise, <c>false</c>.</value>
        bool Recursive { get; set; }

        /// <summary>
        /// Gets or sets the root directory for processong
        /// </summary>
        /// <value>The root directory.</value>
        string RootDirectory { get; set; }

        /////// <summary>
        ///////   Gets or sets a value indicating whether or not transactions should be used for each script
        /////// </summary>
        /////// <remarks>
        ///////   In my opinion, this option should not be used. Instead, put a transaction in the actual script file itself, as needed.
        /////// </remarks>
        ////bool UseTransactions { get; set; }

        /// <summary>
        /// Gets or sets the schema to use as a prefix for the change log table.
        /// </summary>
        /// <value>The schema.</value>
        string Schema { get; set; }

        /// <summary>
        /// Gets or sets the name of the file where the list of found scripts should be written.
        /// </summary>
        /// <value>The script list file.</value>
        string ScriptListFile { get; set; }

        /// <summary>
        /// Gets or sets the search pattern to use for finding script files
        /// </summary>
        /// <value>The search pattern.</value>
        string SearchPattern { get; set; }

        /// <summary>
        /// Gets or sets the directory and file name that will be used for writing out the undo change script
        /// </summary>
        /// <value>The undo output file.</value>
        string UndoOutputFile { get; set; }

        /// <summary>
        /// Gets the database service that should be used for database operations.
        /// </summary>
        /// <value>The database service.</value>
        IDatabaseService DatabaseService { get; }
    }
}