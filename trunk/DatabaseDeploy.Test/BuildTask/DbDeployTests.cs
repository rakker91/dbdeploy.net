// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DbDeployTests.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Test.BuildTask
{
    using System;

    using ApprovalTests;

    using DatabaseDeploy.Core;
    using DatabaseDeploy.Core.BuildTasks;
    using DatabaseDeploy.Core.Configuration;
    using DatabaseDeploy.Core.Database;
    using DatabaseDeploy.Core.IoC;
    using DatabaseDeploy.Core.Utilities;
    using DatabaseDeploy.Test.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// Tests of the DbDeploy Build Task.
    /// </summary>
    [TestClass]
    public class DbDeployTests : TestFixtureBase
    {
        /// <summary>
        /// Tests that execute sets up a run correctly.
        /// </summary>
        [TestMethod]
        public void ThatExecuteSetupsCorrectly()
        {
            MockEnvironmentProvider mockEnvironmentProvider = new MockEnvironmentProvider();
            mockEnvironmentProvider.SetCurrentDirectory(@"c:\DbDeploy");
            EnvironmentProvider.Current = mockEnvironmentProvider;

            try
            {
                IConfigurationService configurationService = new ConfigurationService();
                Container.RegisterInstance(configurationService);

                Mock<IDeploymentService> deploymentServiceMock = new Mock<IDeploymentService>(MockBehavior.Strict);
                deploymentServiceMock.Setup(d => d.BuildDeploymentScript()).Verifiable();
                Container.RegisterInstance(deploymentServiceMock.Object);

                DbDeploy deploy = new DbDeploy();
                deploy.ConnectionString = "Connection String";
                deploy.DatabaseType = "mssql";
                deploy.LastChangeToApply = 500;
                deploy.OutputFile = "Output File";
                deploy.Recursive = true;
                deploy.RootDirectory = "Root Directory";
                deploy.SearchPattern = "*.sql";
                deploy.UndoFile = "Undo File";

                deploy.Execute();

                Approvals.Verify(configurationService);

                deploy.DatabaseType = "ora";
                deploy.Execute();

                Assert.AreEqual(configurationService.DatabaseManagementSystem, DatabaseTypesEnum.Oracle);

                deploy.DatabaseType = "mysql";
                deploy.Execute();

                Assert.AreEqual(configurationService.DatabaseManagementSystem, DatabaseTypesEnum.MySql);

                deploy.DatabaseType = "BadDBType";
                bool expectedExceptionFound = false;
                try
                {
                    deploy.Execute();
                }
                catch (ArgumentException)
                {
                    expectedExceptionFound = true;
                }

                Assert.IsTrue(expectedExceptionFound, "Expected to receive an argument exception for a bad db type, but did not.");

                deploymentServiceMock.Verify(d => d.BuildDeploymentScript(), Times.Exactly(3));
            }
            finally
            {
                EnvironmentProvider.ResetToDefault();
            }
        }
    }
}