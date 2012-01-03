// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContainerFactory.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.IoC
{
    #region Usings

    using Microsoft.Practices.Unity;

    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances.Oracle;

    #endregion

    /// <summary>
    /// Bootstraps the container.
    /// </summary>
    public class ContainerFactory
    {
        #region Public Methods

        /// <summary>
        /// The build unity container.
        /// </summary>
        /// <returns>
        /// An IUnityContainer that is the container for injection. 
        /// </returns>
        public IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.AddNewExtension<AutoResolverUnityExtension>();

            //// Add any additional registrations here.
            container.RegisterType(typeof(IDatabaseService), typeof(OracleDatabaseService));
            return container;
        }

        #endregion
    }
}