// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContainerTests.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test.IoC
{
    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.Database;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances;
    using Veracity.Utilities.DatabaseDeploy.IoC;

    /// <summary>
    /// Tests registrations in the container
    /// </summary>
    [TestFixture]
    public class ContainerTests : TestFixtureBase
    {
        #region Public Methods

        /// <summary>
        /// The that appropriate database registration is used.
        /// </summary>
        [Test]
        public void ThatAppropriateDatabaseRegistrationIsUsed()
        {
            Container.Reset();

            Container.UnityContainer.RegisterType<IConfigurationService>(new PerThreadLifetimeManager());

            IConfigurationService config = Container.UnityContainer.Resolve<IConfigurationService>();
            config.DatabaseManagementSystem = DatabaseTypesEnum.MySql;

            IDatabaseService db = Container.UnityContainer.Resolve<IDatabaseService>();

            Assert.That(db.DatabaseType, Is.EqualTo("mysql"));

            config.DatabaseManagementSystem = DatabaseTypesEnum.SqlServer;

            db = Container.UnityContainer.Resolve<IDatabaseService>();

            Assert.That(db.DatabaseType, Is.EqualTo("mssql"));

            Container.Reset();
        }

        /// <summary>
        /// The that auto resolver gives same instance with per thread mapping.
        /// </summary>
        [Test]
        public void ThatAutoResolverGivesSameInstanceWithPerThreadMapping()
        {
            Container.Reset();

            Container.UnityContainer.RegisterType<IConfigurationService>(new PerThreadLifetimeManager());

            IConfigurationService service = Container.UnityContainer.Resolve<IConfigurationService>();
            service.ConnectionString = "A fake connection string.";

            IDeploymentService deployment = Container.UnityContainer.Resolve<IDeploymentService>();

            Assert.That(service, Is.EqualTo(deployment.ConfigurationService));

            Container.Reset();
        }

        #endregion
    }
}