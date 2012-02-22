// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LifetimePolicyEnum.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.IoC
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Represents an enum of lifetime policies.
    /// </summary>
    public enum LifetimePolicyEnum
    {
        /// <summary>
        /// A <see cref="LifetimeManager"/> that holds onto the instance given to it.
        /// When the <see cref="ContainerControlledLifetimeManager"/> is disposed,
        /// the instance is disposed with it.
        /// </summary>
        ContainerControlledLifetimeManager,

        /// <summary>
        /// A <see cref="LifetimeManager"/> that holds a weak reference to
        /// it's managed instance.
        /// </summary>
        ExternallyControlledLifetimeManager,

        /// <summary>
        /// A special lifetime manager which works like <see cref="ContainerControlledLifetimeManager"/>,
        /// except that in the presence of child containers, each child gets it's own instance
        /// of the object, instead of sharing one in the common parent.
        /// </summary>
        HierarchicalLifetimeManager,

        /// <summary>
        /// This is a custom lifetime manager that acts like <see cref="TransientLifetimeManager"/>,
        /// but also provides a signal to the default build plan, marking the type so that
        /// instances are reused across the build up object graph.
        /// </summary>
        PerResolveLifetimeManager,

        /// <summary>
        /// A <see cref="LifetimeManager"/> that holds the instances given to it, 
        /// keeping one instance per thread.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This LifetimeManager does not dispose the instances it holds.
        /// </para>
        /// </remarks>
        PerThreadLifetimeManager,

        /// <summary>
        /// An <see cref="LifetimeManager"/> implementation that does nothing,
        /// thus ensuring that instances are created new every time.
        /// </summary>
        TransientLifetimeManager
    }
}
