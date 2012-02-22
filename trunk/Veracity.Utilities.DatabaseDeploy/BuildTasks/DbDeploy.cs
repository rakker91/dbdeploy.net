// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbDeploy.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.BuildTasks
{
    #region Usings

    using System;
    using System.IO;
    using System.Reflection;

    using log4net;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Microsoft.Practices.Unity;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.Database;
    using Veracity.Utilities.DatabaseDeploy.IoC;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    #endregion

    /// <summary>
    /// Represents an MSBuild task for performing db deploy's inside of msbuild
    /// </summary>
    public class DbDeploy : Task
    {
        #region Constants and Fields

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(DbDeploy));

        /// <summary>
        ///   Represents the last changes that can be applied.
        /// </summary>
        private int lastChangeToApply = int.MaxValue;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the deployment service to use. This is injected by unity and should not be set.
        /// </summary>
        [Dependency]
        public IConfigurationService ConfigurationService { get; set; }

        /// <summary>
        ///   Gets or sets a connection string to be used by the system. There is no default for this value.
        /// </summary>
        [Required]
        public string ConnectionString { get; set; }

        /// <summary>
        ///   Gets or sets the database type to be used. Currently, only mssql is supported. mysql and ora will be recognized but will fail because they aren't implemented. The default is mssql
        /// </summary>
        public string DatabaseType { get; set; }

        /// <summary>
        ///   Gets or sets the deployment service to use. This is injected by unity and should not be set.
        /// </summary>
        [Dependency]
        public IDeploymentService DeploymentService { get; set; }

        /// <summary>
        /// Gets or sets the schema to use as a prefix for the change log table.  Defaults to dbo.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Gets or sets the name of the change log table.  Defaults to changelog
        /// </summary>
        public string ChangeLog { get; set; }

        /// <summary>
        ///   Gets or sets the last change to apply to the database. The default is int.MaxValue. The max is int.MaxValue.
        /// </summary>
        public int LastChangeToApply
        {
            get
            {
                return this.lastChangeToApply;
            }

            set
            {
                this.lastChangeToApply = value;
            }
        }

        /// <summary>
        ///   Gets or sets the output file where the generated script will be placed. The default is DbDeploy.sql in the same directory where dbdeploy is run.
        /// </summary>
        public string OutputFile { get; set; }

        /// <summary>
        /// Gets or sets the undo file that will be used for this run.  The default is DbDeployUndo.sql in the same directory where dbdeploy is run.
        /// </summary>
        public string UndoFile { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether or not the script search should be recursive. If true, all scripts matching the search criteria below the RootDirectory will be included in the script run.
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        ///   Gets or sets the root directory where dbdeploy begins its search for scripts
        /// </summary>
        [Required]
        public string RootDirectory { get; set; }

        /// <summary>
        ///   Gets or sets the search pattern to use for file parsing. The default is "*.sql"
        /// </summary>
        public string SearchPattern { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns>
        /// A boolean indicating whether or not the execution was successful. 
        /// </returns>
        public override bool Execute()
        {
            FileInfo log4netConfig = new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log4net.config"));
            log4net.Config.XmlConfigurator.ConfigureAndWatch(log4netConfig);

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext());
            }

            bool result = false;
            try
            {
                Container.SetLifetimeManager<IConfigurationService>(new PerThreadLifetimeManager());   

                Container.UnityContainer.BuildUp(this);

                this.ConfigurationService.ConnectionString = this.ConnectionString;

                switch (this.DatabaseType)
                {
                    case "mssql":
                        this.ConfigurationService.DatabaseManagementSystem = DatabaseTypesEnum.SqlServer;
                        break;
                    case "ora":
                        this.ConfigurationService.DatabaseManagementSystem = DatabaseTypesEnum.Oracle;
                        break;
                    case "mysql":
                        this.ConfigurationService.DatabaseManagementSystem = DatabaseTypesEnum.MySql;
                        break;
                    default:
                        string message = string.Format("An invalid database type of {0} was specified.  Only \"mssql\", \"ora\", and \"mysql\" are supported (and only mssql will work at this time).", this.DatabaseType);
                        log.Fatal(message);
                        throw new ArgumentException(message);
                }

                this.ConfigurationService.LastChangeToApply = this.LastChangeToApply;
                this.ConfigurationService.OutputFile = this.OutputFile;
                this.ConfigurationService.Recursive = this.Recursive;
                this.ConfigurationService.RootDirectory = this.RootDirectory;
                this.ConfigurationService.SearchPattern = this.SearchPattern;
                this.ConfigurationService.UndoOutputFile = this.UndoFile;
                this.ConfigurationService.Schema = this.Schema;
                this.ConfigurationService.ChangeLog = this.ChangeLog;

                this.DeploymentService.BuildDeploymentScript();

                result = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

            return result;
        }

        #endregion
    }
}