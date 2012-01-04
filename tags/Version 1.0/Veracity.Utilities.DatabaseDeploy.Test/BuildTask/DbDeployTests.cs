// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbDeployTests.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test.BuildTask
{
    using System;
    using System.IO;

    using Microsoft.Practices.Unity;

    using Moq;

    using NUnit.Framework;

    using Veracity.Utilities.DatabaseDeploy.BuildTasks;
    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.Database;
    using Veracity.Utilities.DatabaseDeploy.IoC;

    /// <summary>
    /// Tests of the DbDeploy Build Task.
    /// </summary>
    [TestFixture]
    public class DbDeployTests : TestFixtureBase
    {
        /// <summary>
        /// Tests that execute sets up a run correctly.
        /// </summary>
        [Test]
        public void ThatExecuteSetupsCorrectly()
        {
            IConfigurationService configurationService = new ConfigurationService();
            var deploymentServiceMock = new Mock<IDeploymentService>(MockBehavior.Strict);

            deploymentServiceMock.Setup(d => d.BuildDeploymentScript()).Verifiable();

            Container.RegisterInstance(configurationService);
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

            Assert.That(configurationService, Is.EqualTo(deploy.ConfigurationService));

            Assert.That(configurationService.ConnectionString, Is.EqualTo("Connection String"));
            Assert.That(configurationService.DatabaseManagementSystem, Is.EqualTo(DatabaseTypesEnum.SqlServer));
            Assert.That(configurationService.LastChangeToApply, Is.EqualTo(500));
            Assert.That(configurationService.OutputFile, Is.EqualTo("Output File"));
            Assert.That(configurationService.UndoOutputFile, Is.EqualTo("Undo File"));
            Assert.That(configurationService.Recursive, Is.True);
            Assert.That(configurationService.RootDirectory, Is.EqualTo(Path.Combine(Environment.CurrentDirectory, "Root Directory")));
            Assert.That(configurationService.SearchPattern, Is.EqualTo("*.sql"));

            deploy.DatabaseType = "ora";
            deploy.Execute();

            Assert.That(configurationService.DatabaseManagementSystem, Is.EqualTo(DatabaseTypesEnum.Oracle));

            deploy.DatabaseType = "mysql";
            deploy.Execute();

            Assert.That(configurationService.DatabaseManagementSystem, Is.EqualTo(DatabaseTypesEnum.MySql));

            deploy.DatabaseType = "BadDBType";
            Assert.Throws<ArgumentException>(() => deploy.Execute());

            deploymentServiceMock.Verify(d => d.BuildDeploymentScript(), Times.Exactly(3));
        }
    }
}