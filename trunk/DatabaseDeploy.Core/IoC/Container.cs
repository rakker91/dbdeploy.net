// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Container.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.IoC
{
    using Microsoft.Practices.Unity;

    /// <summary>
    ///     Provides a static container wrapper for Unity
    /// </summary>
    public static class Container
    {
        /// <summary>
        ///     Used for locking
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        ///     The container to use for IoC
        /// </summary>
        private static IUnityContainer container;

        /// <summary>
        ///     Gets the container to use for IoC
        /// </summary>
        /// <value>The unity container.</value>
        public static IUnityContainer UnityContainer
        {
            get
            {
                if (container == null)
                {
                    lock (LockObject)
                    {
                        ContainerFactory factory = new ContainerFactory();
                        container = factory.BuildUnityContainer();
                    }
                }

                return container;
            }
        }

        /// <summary>
        ///     Registers an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type that we're registering for.</typeparam>
        /// <param name="instance">The instance we're registering.</param>
        public static void RegisterInstance<T>(T instance)
        {
            UnityContainer.RegisterInstance(typeof(T), instance);
        }

        /// <summary>
        ///     Resets the container by setting it to null, which will cause it to be recreated on the next call.
        /// </summary>
        public static void Reset()
        {
            container = null;
        }

        /// <summary>
        ///     Sets the lifetime manager for a registration. If the type isn't yet registered, will add a lifetime manager to the
        ///     registration. If the type is already registered, it will do nothing.
        /// </summary>
        /// <typeparam name="T">The type that will be resolved.</typeparam>
        /// <param name="lifetimeManager">The lifetime manager to use.</param>
        public static void SetLifetimeManager<T>(LifetimeManager lifetimeManager)
        {
            if (!UnityContainer.IsRegistered<T>())
            {
                UnityContainer.RegisterType<T>(lifetimeManager);
            }
        }
    }
}