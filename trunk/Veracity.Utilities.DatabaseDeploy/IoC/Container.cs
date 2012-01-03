// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Container.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.IoC
{
    #region Usings

    using log4net;

    using Microsoft.Practices.Unity;

    using Veracity.Utilities.DatabaseDeploy.Utilities;

    #endregion

    /// <summary>
    /// Provides a static container wrapper for Unity
    /// </summary>
    public static class Container
    {
        #region Constants and Fields

        /// <summary>
        ///   Used for locking
        /// </summary>
        private static readonly object lockObject = new object();

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(Container));

        /// <summary>
        ///   The container to use for IoC
        /// </summary>
        private static IUnityContainer container;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the container to use for IoC
        /// </summary>
        public static IUnityContainer UnityContainer
        {
            get
            {
                if (container == null)
                {
                    lock (lockObject)
                    {
                        ContainerFactory factory = new ContainerFactory();
                        container = factory.BuildUnityContainer();
                    }
                }

                return container;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers an instance of the given type.
        /// </summary>
        /// <typeparam name="T">
        /// The type that we're registering for. 
        /// </typeparam>
        /// <param name="instance">
        /// The instance we're regsitering. 
        /// </param>
        public static void RegisterInstance<T>(T instance)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(instance));
            }

            UnityContainer.RegisterInstance(typeof(T), instance);

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult());
            }
        }

        /// <summary>
        /// Resets the container by setting it to null, which will cause it to be recreated on the next call.
        /// </summary>
        public static void Reset()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext());
            }

            container = null;

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult());
            }
        }

        /// <summary>
        /// Sets the lifetime manager for a registration. If the type isn't yet registered, will add a lifetime manager to the registration. If the type is already registered, it will do nothing.
        /// </summary>
        /// <typeparam name="T">
        /// The type that will be resolved. 
        /// </typeparam>
        /// <param name="lifetimeManager">
        /// The lifetime manager to use. 
        /// </param>
        public static void SetLifetimeManager<T>(LifetimeManager lifetimeManager)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(lifetimeManager));
            }

            if (!UnityContainer.IsRegistered<T>())
            {
                UnityContainer.RegisterType<T>(lifetimeManager);
            }

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult());
            }
        }

        #endregion
    }
}