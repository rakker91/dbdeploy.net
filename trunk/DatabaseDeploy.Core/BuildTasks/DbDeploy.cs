// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DbDeploy.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.BuildTasks
{
    using System;
    using System.IO;

    using DatabaseDeploy.Core.Configuration;
    using DatabaseDeploy.Core.Database;
    using DatabaseDeploy.Core.IoC;
    using DatabaseDeploy.Core.Utilities;

    using log4net;
    using log4net.Config;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Represents an MSBuild task for performing db deploy's inside of msbuild
    /// </summary>
    public class DbDeploy : Task
    {
        /// <summary>
        /// Creates the default logger
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DbDeploy));

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDeploy"/> class.
        /// </summary>
        public DbDeploy()
        {
            this.LastChangeToApply = int.MaxValue;
        }

        /// <summary>
        /// Gets or sets the name of the change log table.  Defaults to changelog
        /// </summary>
        /// <value>The change log.</value>
        public string ChangeLog { get; set; }

        /// <summary>
        /// Gets or sets the deployment service to use. This is injected by unity and should not be set.
        /// </summary>
        /// <value>The configuration service.</value>
        [Dependency]
        public IConfigurationService ConfigurationService { get; set; }

        /// <summary>
        /// Gets or sets a connection string to be used by the system. There is no default for this value.
        /// </summary>
        /// <value>The connection string.</value>
        [Required]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the database type to be used. mssql, mysql, and ora
        /// </summary>
        /// <value>The type of the database.</value>
        public string DatabaseType { get; set; }

        /// <summary>
        /// Gets or sets the deployment service to use. This is injected by unity and should not be set.
        /// </summary>
        /// <value>The deployment service.</value>
        [Dependency]
        public IDeploymentService DeploymentService { get; set; }

        /// <summary>
        /// Gets or sets the pattern to use for parsing the name of the script file
        /// </summary>
        /// <value>The file name pattern.</value>
        public string FileNamePattern { get; set; }

        /// <summary>
        /// Gets or sets the last change to apply to the database. The default is int.MaxValue. The max is int.MaxValue.
        /// </summary>
        /// <value>The last change to apply.</value>
        public int LastChangeToApply { get; set; }

        /// <summary>
        /// Gets or sets the output file where the generated script will be placed. The default is DbDeploy.sql in the same
        /// directory where dbdeploy is run.
        /// </summary>
        /// <value>The output file.</value>
        public string OutputFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the script search should be recursive. If true, all scripts matching
        /// the search criteria below the RootDirectory will be included in the script run.
        /// </summary>
        /// <value><c>true</c> if recursive; otherwise, <c>false</c>.</value>
        public bool Recursive { get; set; }

        /// <summary>
        /// Gets or sets the root directory where dbdeploy begins its search for scripts
        /// </summary>
        /// <value>The root directory.</value>
        [Required]
        public string RootDirectory { get; set; }

        /// <summary>
        /// Gets or sets the schema to use as a prefix for the change log table.  Defaults to dbo.
        /// </summary>
        /// <value>The schema.</value>
        public string Schema { get; set; }

        /// <summary>
        /// Gets or sets the search pattern to use for finding script files. The default is "*.sql"
        /// </summary>
        /// <value>The search pattern.</value>
        public string SearchPattern { get; set; }

        /// <summary>
        /// Gets or sets the undo file that will be used for this run.  The default is DbDeployUndo.sql in the same directory
        /// where dbdeploy is run.
        /// </summary>
        /// <value>The undo file.</value>
        public string UndoFile { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns>A boolean indicating whether or not the execution was successful.</returns>
        /// <exception cref="System.ArgumentException"></exception>
        public override bool Execute()
        {
            FileInfo log4netConfig =
                new FileInfo(Path.Combine(EnvironmentProvider.Current.ExecutingAssemblyDirectory, "log4net.config"));
            XmlConfigurator.ConfigureAndWatch(log4netConfig);

            bool result = false;
            try
            {
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
                        string message =
                            string.Format(
                                "An invalid database type of {0} was specified.  Only \"mssql\", \"ora\", and \"mysql\" are supported.",
                                this.DatabaseType);
                        Logger.Fatal(message);
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
                Logger.Error(ex);
                throw;
            }

            return result;
        }
    }
}