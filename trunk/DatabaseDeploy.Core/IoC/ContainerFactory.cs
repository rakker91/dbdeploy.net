// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ContainerFactory.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.IoC
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using DatabaseDeploy.Core.Configuration;
    using DatabaseDeploy.Core.Database.DatabaseInstances;
    using DatabaseDeploy.Core.Database.DatabaseInstances.SqlServer;

    using log4net;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    /// <summary>
    ///     Class ContainerFactory.
    /// </summary>
    public class ContainerFactory
    {
        /// <summary>
        ///     Creates the default logger
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(ContainerFactory));

        /// <summary>
        ///     Gets or sets the unity container.
        /// </summary>
        /// <value>The container.</value>
        private static IUnityContainer Container { get; set; }

        /// <summary>
        ///     The build unity container.
        /// </summary>
        /// <returns>An IUnityContainer that is the container for injection.</returns>
        public IUnityContainer BuildUnityContainer()
        {
            IUnityContainer container = this.BuildUnityContainer(true);

            return container;
        }

        /// <summary>
        ///     The build unity container.
        /// </summary>
        /// <param name="loadConfiguration">Whether or not the configuration from the app.config should be loaded.</param>
        /// <returns>An IUnityContainer that is the container for injection.</returns>
        public IUnityContainer BuildUnityContainer(bool loadConfiguration)
        {
            return this.BuildUnityContainer(loadConfiguration, null);
        }

        /// <summary>
        ///     Builds the unity container optionally loading the configuration and with a callback for registering additional
        ///     items.
        /// </summary>
        /// <param name="loadConfiguration">Whether or not the configuration should be loaded.</param>
        /// <param name="registrationCallback">A callback for loading additional configuration</param>
        /// <returns>A configured unity container.</returns>
        public IUnityContainer BuildUnityContainer(bool loadConfiguration, Action<IUnityContainer> registrationCallback)
        {
            if (Container == null)
            {
                Container = new UnityContainer();

                if (loadConfiguration)
                {
                    try
                    {
                        Container.LoadConfiguration();
                    }
                    catch (ArgumentNullException ex)
                    {
                        // do nothing--this is thrown if no configuration file or section was found.
                        Log.Info(
                            "Attempted to load the configuration but either no configuration file was found or the configuration section was empty.",
                            ex);
                    }
                }

                Container.RegisterInstance(typeof(IUnityContainer), Container, new ContainerControlledLifetimeManager());

                if (registrationCallback != null)
                {
                    registrationCallback.Invoke(Container);
                }

                IList<Type> types = AllClasses.FromLoadedAssemblies().Where(t => t.Namespace != null && t.Namespace.StartsWith("DatabaseDeploy")).ToList();
                Container.RegisterTypes(types, WithMappings.FromMatchingInterface, WithName.Default);
            }

            //// Add any additional registrations here or in the configuration file.
            // this is a temporary registration that is overridden at a later point.
            // Container.RegisterType(typeof(IDatabaseService), typeof(SqlServerDatabaseService));
            Container.RegisterType(typeof(IConfigurationService), typeof(ConfigurationService), new ContainerControlledLifetimeManager());
            return Container;
        }
    }
}