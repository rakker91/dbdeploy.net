// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Options.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Console
{
    using CommandLine;

    /// <summary>
    ///     Contains the command line options for the application
    /// </summary>
    public class Options
    {
        /// <summary>
        ///     Gets or sets the name of the change log table.
        /// </summary>
        /// <value>The change log.</value>
        [Option('g', "ChangeLog", HelpText = "The name of the ChangeLog table.", Required = false)]
        public string ChangeLog { get; set; }

        /// <summary>
        ///     Gets or sets a connection string to be used by the system
        /// </summary>
        /// <value>The connection string.</value>
        [Option('c', "ConnectionString", HelpText = "The connection string that will be used to connect to the database to evaluate dbdeploy scripts.", Required = false)]
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating the type of DBMS to use.
        /// </summary>
        /// <value>The database management system.</value>
        [Option('d', "DatabaseManagementSystem", HelpText = "The database to use. (mssql, mysql, ora)", Required = false)]
        public string DatabaseManagementSystem { get; set; }

        /// <summary>
        ///     Gets or sets the database script path
        /// </summary>
        /// <value>The database script path.</value>
        [Option('p', "DatabaseScriptPath", HelpText = "The path where database scripts can be found.", Required = false)]
        public string DatabaseScriptPath { get; set; }

        /// <summary>
        ///     Gets or sets the search pattern to use for file parsing
        /// </summary>
        /// <value>The file name pattern.</value>
        [Option('a', "FileNamePattern", HelpText = "The pattern that should be used to parse the script file names.", Required = false)]
        public string FileNamePattern { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating the last change that should be applied to the database.
        /// </summary>
        /// <value>The last change to apply.</value>
        /// <remarks>
        ///     Set to 0 or max value (the default) to apply all changes. Any other positive number will stop applying changes at
        ///     that level.
        /// </remarks>
        [Option('n', "LastChangeToApply", HelpText = "The number of the last script to apply to the database.  Defaults to 0, or all available scripts.", Required = false)]
        public int LastChangeToApply { get; set; }

        /// <summary>
        ///     Gets or sets the directory and file name that will be used for writing out the change script
        /// </summary>
        /// <value>The output file.</value>
        [Option('f', "OutputFile", HelpText = "The name of the file that will be created for the updates that need to be run. Defaults to DbDeploy", Required = false)]
        public string OutputFile { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not the script search should be recursive
        /// </summary>
        /// <value><c>true</c> if recursive; otherwise, <c>false</c>.</value>
        [Option('r', "Recursive", HelpText = "Whether or not the path should be crawled recursively looking for database scripts.", Required = false)]
        public bool Recursive { get; set; }

        /// <summary>
        ///     Gets or sets the root directory for processing
        /// </summary>
        /// <value>The root directory.</value>
        [Option('o', "RootDirectory", HelpText = "The path where dbdeploy should start running.", Required = false)]
        public string RootDirectory { get; set; }

        /////// <summary>
        ///////   Gets or sets a value indicating whether or not transactions should be used for each script
        /////// </summary>
        /////// <remarks>
        ///////   In my opinion, this option should not be used. Instead, put a transaction in the actual script file itself, as needed.
        /////// </remarks>
        ////[Option('t', "UseTransactions", HelpText = "Whether or not transactions should be used.  Personally, I'd put them in the script.", Required = false)]
        ////public bool UseTransactions { get; set; }

        /// <summary>
        ///     Gets or sets the schema to use as a prefix for the change log table.
        /// </summary>
        /// <value>The schema.</value>
        [Option('s', "Schema", HelpText = "The schema to use for scripts.", Required = false)]
        public string Schema { get; set; }

        /// <summary>
        ///     Gets or sets the name of the file where the list of found scripts should be written.
        /// </summary>
        /// <value>The script list file.</value>
        [Option('l', "ScriptListFile", HelpText = "The file that will contain a list of all of the scripts found.", Required = false)]
        public string ScriptListFile { get; set; }

        /// <summary>
        ///     Gets or sets the search pattern to use for file parsing
        /// </summary>
        /// <value>The search pattern.</value>
        [Option('w', "SearchPattern", HelpText = "The wildcard pattern that should be used to match for scripts.", Required = false)]
        public string SearchPattern { get; set; }

        /// <summary>
        ///     Gets or sets the directory and file name that will be used for writing out the undo change script
        /// </summary>
        /// <value>The undo output file.</value>
        [Option('u', "UndoOutputFile", HelpText = "The file that will contain undo scripts.", Required = false)]
        public string UndoOutputFile { get; set; }
    }
}