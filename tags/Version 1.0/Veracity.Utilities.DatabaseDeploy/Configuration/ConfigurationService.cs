// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationService.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Configuration
{
    #region Usings

    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using log4net;

    using Microsoft.Practices.Unity;

    using Veracity.Utilities.DatabaseDeploy.Database;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances.MySql;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances.Oracle;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances.SqlServer;
    using Veracity.Utilities.DatabaseDeploy.IoC;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    #endregion

    /// <summary>
    /// Provides a service for managing configuration values.
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        #region Constants and Fields

        /// <summary>
        ///   The default database path
        /// </summary>
        private const string DefaultDatabasePath = @"Database\Scripts";

        /// <summary>
        ///   The default output filename
        /// </summary>
        private const string DefaultOutputFile = "DbDeploy.sql";

        /// <summary>
        ///   The default file to contain the list of scripts.
        /// </summary>
        private const string DefaultScriptListFile = "ScriptList.txt";

        /// <summary>
        ///   The default undo filename
        /// </summary>
        private const string DefaultUndoFile = "DbDeployUndo.sql";

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(ConfigurationService));

        /// <summary>
        ///   The database management type
        /// </summary>
        private DatabaseTypesEnum databaseManagementSystem = DatabaseTypesEnum.SqlServer;

        /// <summary>
        ///   The last change to apply in the database.
        /// </summary>
        private int lastChangeToApply = int.MaxValue;

        /// <summary>
        ///   The output file to use for this run.
        /// </summary>
        private string outputFile = DefaultOutputFile;

        /// <summary>
        ///   The root directory to use for this run.
        /// </summary>
        private string rootDirectory;

        /// <summary>
        ///   Contains the name of the script file where scripts should be written.
        /// </summary>
        private string scriptListFile = DefaultScriptListFile;

        /// <summary>
        ///   The search pattern to use for queries
        /// </summary>
        private string searchPattern = "*.sql";

        /// <summary>
        ///   The undo output file.
        /// </summary>
        private string undoOutputFile = DefaultUndoFile;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConfigurationService" /> class.
        /// </summary>
        public ConfigurationService()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext());
            }

            this.SetupDatabaseType();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a connection string to be used by the system
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating the type of DBMS to use.
        /// </summary>
        public DatabaseTypesEnum DatabaseManagementSystem
        {
            get
            {
                return this.databaseManagementSystem;
            }

            set
            {
                this.databaseManagementSystem = value;
                this.SetupDatabaseType();
            }
        }

        /// <summary>
        ///   Gets or sets the database script path
        /// </summary>
        public string DatabaseScriptPath { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating the last change that should be applied to the database.
        /// </summary>
        /// <remarks>
        ///   Set to 0 or int.maxvalue (the default) to apply all changes. Any other positive number will stop applying changes at that level.
        /// </remarks>
        public int LastChangeToApply
        {
            get
            {
                return this.lastChangeToApply;
            }

            set
            {
                if (value <= 0)
                {
                    this.lastChangeToApply = int.MaxValue;
                }
                else
                {
                    this.lastChangeToApply = value;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the directory and file name that will be used for writing out the change script
        /// </summary>
        public string OutputFile
        {
            get
            {
                return this.outputFile;
            }

            set
            {
                if (value != null && value.Trim() != string.Empty)
                {
                    this.outputFile = value;
                }
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether or not the script search should be recursive
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        ///   Gets or sets the root directory for processong
        /// </summary>
        public string RootDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(this.rootDirectory) || this.rootDirectory.Trim() == string.Empty)
                {
                    this.rootDirectory = Environment.CurrentDirectory;
                }

                string result;

                if (Path.IsPathRooted(this.rootDirectory))
                {
                    result = this.rootDirectory;
                }
                else
                {
                    result = Path.Combine(Environment.CurrentDirectory, this.rootDirectory);
                }

                return result;
            }

            set
            {
                if (value.Trim() != string.Empty)
                {
                    this.rootDirectory = value;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the name of the file where the list of found scripts should be written.  If no path is provided, a relative path is assumed and the file will be in the RootDirectory directory.
        /// </summary>
        public string ScriptListFile
        {
            get
            {
                return Path.Combine(this.RootDirectory, this.scriptListFile);
            }

            set
            {
                if (value != null && value.Trim() != string.Empty)
                {
                    this.scriptListFile = value;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the search pattern to use for file parsing
        /// </summary>
        public string SearchPattern
        {
            get
            {
                return this.searchPattern;
            }

            set
            {
                if (value != null && value.Trim() != string.Empty)
                {
                    this.searchPattern = value;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the directory and file name that will be used for writing out the undo change script
        /// </summary>
        public string UndoOutputFile
        {
            get
            {
                return this.undoOutputFile;
            }

            set
            {
                if (value != null && value.Trim() != string.Empty)
                {
                    this.undoOutputFile = value;
                }
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether or not transactions should be used for each script
        /// </summary>
        /// <remarks>
        ///   In my opinion, this option should not be used. Instead, put a transaction in the actual script file itself, as needed.
        /// </remarks>
        public bool UseTransactions { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Overrides tostring to get a descriptive list of all settings.
        /// </summary>
        /// <returns>
        /// A string containing a list of all settings. 
        /// </returns>
        public override string ToString()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext());
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine(string.Format("ConnectionString: {0}", this.ConnectionString));
            builder.AppendLine(string.Format("DatabaseManagementSystem: {0}", this.DatabaseManagementSystem));
            builder.AppendLine(string.Format("LastChangeToApply: {0}", this.LastChangeToApply));
            builder.AppendLine(string.Format("OutputFile: {0}", this.OutputFile));
            builder.AppendLine(string.Format("Recursive: {0}", this.Recursive));
            builder.AppendLine(string.Format("RootDirectory: {0}", this.RootDirectory));
            builder.AppendLine(string.Format("ScriptListFile: {0}", this.ScriptListFile));
            builder.AppendLine(string.Format("SearchPattern: {0}", this.SearchPattern));
            builder.AppendLine(string.Format("UndoOutputFile: {0}", this.UndoOutputFile));
            builder.AppendLine(string.Format("UseTransactions: {0}", this.UseTransactions));

            string result = builder.ToString();

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult(result));
            }

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets up the container to have the appropriate mappings for the appropriate database types.
        /// </summary>
        private void SetupDatabaseType()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext());
            }

            string rootPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), DefaultDatabasePath);

            switch (this.DatabaseManagementSystem)
            {
                case DatabaseTypesEnum.MySql:
                    Container.UnityContainer.RegisterType(typeof(IDatabaseService), typeof(MySqlDatabaseService));
                    this.DatabaseScriptPath = Path.Combine(rootPath, "mysql");
                    break;
                case DatabaseTypesEnum.Oracle:
                    Container.UnityContainer.RegisterType(typeof(IDatabaseService), typeof(OracleDatabaseService));
                    this.DatabaseScriptPath = Path.Combine(rootPath, "ora");
                    break;
                case DatabaseTypesEnum.SqlServer:
                    Container.UnityContainer.RegisterType(typeof(IDatabaseService), typeof(SqlServerDatabaseService));
                    this.DatabaseScriptPath = Path.Combine(rootPath, "mssql");
                    break;
            }

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult());
            }
        }

        #endregion
    }
}