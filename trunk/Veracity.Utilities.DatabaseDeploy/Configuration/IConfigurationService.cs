// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigurationService.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Configuration
{
    #region Usings

    using Veracity.Utilities.DatabaseDeploy.Database;

    #endregion

    /// <summary>
    /// The Configuration service service.
    /// </summary>
    public interface IConfigurationService
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets a connection string to be used by the system
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating the type of DBMS to use.
        /// </summary>
        DatabaseTypesEnum DatabaseManagementSystem { get; set; }

        /// <summary>
        ///   Gets or sets the database script path
        /// </summary>
        string DatabaseScriptPath { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating the last change that should be applied to the database.
        /// </summary>
        /// <remarks>
        ///   Set to 0 or max value (the default) to apply all changes. Any other positive number will stop applying changes at that level.
        /// </remarks>
        int LastChangeToApply { get; set; }

        /// <summary>
        ///   Gets or sets the directory and file name that will be used for writing out the change script
        /// </summary>
        string OutputFile { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether or not the script search should be recursive
        /// </summary>
        bool Recursive { get; set; }

        /// <summary>
        ///   Gets or sets the root directory for processong
        /// </summary>
        string RootDirectory { get; set; }

        /// <summary>
        ///   Gets or sets the name of the file where the list of found scripts should be written.
        /// </summary>
        string ScriptListFile { get; set; }

        /// <summary>
        ///   Gets or sets the search pattern to use for finding script files
        /// </summary>
        string SearchPattern { get; set; }

        /// <summary>
        ///   Gets or sets the pattern to use for parsing the name of the script file
        /// </summary>
        string FileNamePattern { get; set; }

        /// <summary>
        ///   Gets or sets the directory and file name that will be used for writing out the undo change script
        /// </summary>
        string UndoOutputFile { get; set; }

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
        string Schema { get; set; }

        /// <summary>
        /// Gets or sets the name of the change log table.
        /// </summary>
        string ChangeLog { get; set; }

        #endregion
    }
}