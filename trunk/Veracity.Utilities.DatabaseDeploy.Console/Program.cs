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
    using System.Configuration;

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
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(setting));
            }

            AppSettingsReader reader = new AppSettingsReader();
            T result;
            try
            {
                result = (T)reader.GetValue(setting, typeof(T));
            }
            catch (Exception)
            {
                result = default(T);
            }

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult(result));
            }

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
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(args));
            }

            try
            {
                Container.UnityContainer.RegisterType<IConfigurationService>(new PerThreadLifetimeManager());

                ConfigurationService = Container.UnityContainer.Resolve<IConfigurationService>();
                DeploymentService = Container.UnityContainer.Resolve<IDeploymentService>();

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
                        string message = string.Format("An invalid database type of {0} was specified.  Only \"mssql\", \"ora\", and \"mysql\" are supported (and only mssql will work at this time).", GetSetting<string>("DatabaseType"));
                        log.Fatal(message);
                        throw new ArgumentException(message);
                }

                ConfigurationService.LastChangeToApply = GetSetting<int>("LastChangeToApply");
                ConfigurationService.OutputFile = GetSetting<string>("OutputFile");
                ConfigurationService.Recursive = GetSetting<bool>("Recursive");
                ConfigurationService.RootDirectory = GetSetting<string>("RootDirectory");
                ConfigurationService.SearchPattern = GetSetting<string>("SearchPattern");
                ConfigurationService.UndoOutputFile = GetSetting<string>("UndoFile");

                DeploymentService.BuildDeploymentScript();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion
    }
}