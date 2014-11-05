// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Console
{
    #region Usings

    using System;
    using System.ComponentModel;
    using System.Configuration;

    using CommandLine;
    using CommandLine.Text;

    using log4net;

    using Microsoft.Practices.Unity;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.Database;
    using Veracity.Utilities.DatabaseDeploy.IoC;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    #endregion

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        #region Constants and Fields

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the deployment service to use. This is injected by unity and should not be set.
        /// </summary>
        [Dependency]
        public static IConfigurationService ConfigurationService { get; set; }

        /// <summary>
        ///   Gets or sets the deployment service to use. This is injected by unity and should not be set.
        /// </summary>
        [Dependency]
        public static IDeploymentService DeploymentService { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a setting from app settings.
        /// </summary>
        /// <typeparam name="T">
        /// The type of setting we're retrieving 
        /// </typeparam>
        /// <param name="setting">
        /// The setting to retrieve 
        /// </param>
        /// <returns>
        /// A setting for the requested value or the default setting. 
        /// </returns>
        private static T GetSetting<T>(string setting)
        {
            log.DebugIfEnabled(LogUtility.GetContext(setting));

            var result = default(T);
            var value = ConfigurationManager.AppSettings[setting];
            if (value != null)
            {
                result = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value);
            }

            log.DebugIfEnabled(LogUtility.GetResult(result));

            return result;
        }

        /// <summary>
        /// The main entry point into the application.
        /// </summary>
        /// <param name="args">
        /// The arguments for this run 
        /// </param>
        private static void Main(string[] args)
        {
            log.DebugIfEnabled(LogUtility.GetContext(args));

            try
            {
                IoC.Container.UnityContainer.RegisterType<IConfigurationService>(new PerThreadLifetimeManager());

                ConfigurationService = IoC.Container.UnityContainer.Resolve<IConfigurationService>();
                DeploymentService = IoC.Container.UnityContainer.Resolve<IDeploymentService>();

                ConfigurationService.ConnectionString = GetSetting<string>("ConnectionString");

                switch (GetSetting<string>("DatabaseType"))
                {
                    case "mssql":
                        ConfigurationService.DatabaseManagementSystem = DatabaseTypesEnum.SqlServer;
                        break;
                    case "ora":
                        ConfigurationService.DatabaseManagementSystem = DatabaseTypesEnum.Oracle;
                        break;
                    case "mysql":
                        ConfigurationService.DatabaseManagementSystem = DatabaseTypesEnum.MySql;
                        break;
                    default:
                        string message = string.Format("An invalid database type of {0} was specified.  Only \"mssql\", \"ora\", and \"mysql\" are supported.", GetSetting<string>("DatabaseType"));
                        log.Fatal(message);
                        throw new ArgumentException(message);
                }

                ConfigurationService.LastChangeToApply = GetSetting<int>("LastChangeToApply");
                ConfigurationService.OutputFile = GetSetting<string>("OutputFile");
                ConfigurationService.Recursive = GetSetting<bool>("Recursive");
                ConfigurationService.RootDirectory = GetSetting<string>("RootDirectory");
                ConfigurationService.SearchPattern = GetSetting<string>("SearchPattern");
                ConfigurationService.FileNamePattern = GetSetting<string>("FileNamePattern");
                ConfigurationService.UndoOutputFile = GetSetting<string>("UndoFile");
                ConfigurationService.Schema = GetSetting<string>("Schema");
                ConfigurationService.ChangeLog = GetSetting<string>("ChangeLog");

                ParseCommandLine(args);

                DeploymentService.BuildDeploymentScript();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// Parses the command line parameters
        /// </summary>
        /// <param name="args">The arguments passed on the command line.</param>
        private static void ParseCommandLine(string[] args)
        {
            if (args.Length > 0)
            {
                var options = new Options();

                if (Parser.Default.ParseArgumentsStrict(args, options))
                {
                    ConfigurationService.ConnectionString = string.IsNullOrEmpty(options.ConnectionString)
                                                                ? ConfigurationService.ConnectionString
                                                                : options.ConnectionString;

                    switch (options.DatabaseManagementSystem)
                    {
                        case "mssql":
                            ConfigurationService.DatabaseManagementSystem = DatabaseTypesEnum.SqlServer;
                            break;
                        case "ora":
                            ConfigurationService.DatabaseManagementSystem = DatabaseTypesEnum.Oracle;
                            break;
                        case "mysql":
                            ConfigurationService.DatabaseManagementSystem = DatabaseTypesEnum.MySql;
                            break;
                    }

                    ConfigurationService.LastChangeToApply = options.LastChangeToApply != default(int)
                        ? options.LastChangeToApply
                        : ConfigurationService.LastChangeToApply;

                    ConfigurationService.OutputFile = string.IsNullOrEmpty(options.OutputFile)
                        ? ConfigurationService.OutputFile
                        : options.OutputFile;

                    ConfigurationService.Recursive = options.Recursive != default(bool)
                        ? options.Recursive
                        : ConfigurationService.Recursive;

                    ConfigurationService.RootDirectory = string.IsNullOrEmpty(options.RootDirectory)
                        ? ConfigurationService.RootDirectory
                        : options.RootDirectory;

                    ConfigurationService.SearchPattern = string.IsNullOrEmpty(options.SearchPattern)
                        ? ConfigurationService.SearchPattern
                        : options.SearchPattern;

                    ConfigurationService.FileNamePattern = string.IsNullOrEmpty(options.FileNamePattern)
                        ? ConfigurationService.FileNamePattern
                        : options.FileNamePattern;

                    ConfigurationService.UndoOutputFile = string.IsNullOrEmpty(options.UndoOutputFile)
                        ? ConfigurationService.UndoOutputFile
                        : options.UndoOutputFile;

                    ConfigurationService.Schema = string.IsNullOrEmpty(options.Schema)
                        ? ConfigurationService.Schema
                        : options.Schema;

                    ConfigurationService.ChangeLog = string.IsNullOrEmpty(options.ChangeLog)
                        ? ConfigurationService.ChangeLog
                        : options.ChangeLog;
                }
            }
        }

        #endregion
    }
}