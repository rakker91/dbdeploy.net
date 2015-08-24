// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ContainerTests.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Test.IoC
{
    using DatabaseDeploy.Core;
    using DatabaseDeploy.Core.Configuration;
    using DatabaseDeploy.Core.Database;
    using DatabaseDeploy.Core.Database.DatabaseInstances;
    using DatabaseDeploy.Core.IoC;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Tests registrations in the container
    /// </summary>
    [TestClass]
    public class ContainerTests : TestFixtureBase
    {
        /// <summary>
        ///     The that appropriate database registration is used.
        /// </summary>
        [TestMethod]
        public void ThatAppropriateDatabaseRegistrationIsUsed()
        {
            Container.Reset();

            Container.UnityContainer.RegisterType<IConfigurationService>(new PerThreadLifetimeManager());

            IConfigurationService config = Container.UnityContainer.Resolve<IConfigurationService>();
            config.DatabaseManagementSystem = DatabaseTypesEnum.MySql;

            IDatabaseService db = Container.UnityContainer.Resolve<IDatabaseService>();

            Assert.AreEqual(db.DatabaseType, "mysql");

            config.DatabaseManagementSystem = DatabaseTypesEnum.SqlServer;

            db = Container.UnityContainer.Resolve<IDatabaseService>();

            Assert.AreEqual(db.DatabaseType, "mssql");

            Container.Reset();
        }
    }
}