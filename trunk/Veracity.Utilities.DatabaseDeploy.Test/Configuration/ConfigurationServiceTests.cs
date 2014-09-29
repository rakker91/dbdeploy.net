// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationServiceTests.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test.Configuration
{
    #region Usings

    using System;
    using System.IO;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.Database;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances.MySql;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances.Oracle;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances.SqlServer;
    using Veracity.Utilities.DatabaseDeploy.IoC;

    #endregion

    /// <summary>
    /// Tests the configuration service
    /// </summary>
    [TestFixture]
    public class ConfigurationServiceTests : TestFixtureBase
    {
        /// <summary>
        /// Tests that database management setup updates the config for databases
        /// </summary>
        [Test]
        public void ThatDatabaseManagementSetUpdatesConfig()
        {
            IConfigurationService config = new ConfigurationService();

            config.DatabaseManagementSystem = DatabaseTypesEnum.MySql;
            IDatabaseService db = Container.UnityContainer.Resolve<IDatabaseService>();

            Assert.That(db, Is.TypeOf<MySqlDatabaseService>());
            Assert.That(config.DatabaseScriptPath.Contains("mysql"));

            config.DatabaseManagementSystem = DatabaseTypesEnum.SqlServer;
            db = Container.UnityContainer.Resolve<IDatabaseService>();

            Assert.That(db, Is.TypeOf<SqlServerDatabaseService>());
            Assert.That(config.DatabaseScriptPath.Contains("mssql"));

            config.DatabaseManagementSystem = DatabaseTypesEnum.Oracle;
            db = Container.UnityContainer.Resolve<IDatabaseService>();

            Assert.That(db, Is.TypeOf<OracleDatabaseService>());
            Assert.That(config.DatabaseScriptPath.Contains("ora"));
        }

        /// <summary>
        /// Tests to ensure a default database setup is in place.
        /// </summary>
        [Test]
        public void ThatDefaultDatabaseSetupExists()
        {
            var config = new ConfigurationService();
            config.SetupDatabaseType();
            IDatabaseService db = Container.UnityContainer.Resolve<IDatabaseService>();

            Assert.That(db, Is.TypeOf<SqlServerDatabaseService>());
            Assert.That(config.DatabaseScriptPath.Contains("mssql"));
        }

        /// <summary>
        /// Ensures that a basic config object has the appropriate defaults.
        /// </summary>
        [Test]
        public void ThatConfigHasDefaults()
        {
            IConfigurationService config = new ConfigurationService();

            Assert.That(config.LastChangeToApply, Is.EqualTo(int.MaxValue));
            Assert.That(config.OutputFile, Is.Not.Null);
            Assert.That(config.OutputFile.Trim(), Is.Not.Empty);
            Assert.That(config.RootDirectory, Is.Not.Null);
            Assert.That(config.RootDirectory.Trim(), Is.Not.Empty);
            Assert.That(config.ScriptListFile, Is.Not.Null);
            Assert.That(config.ScriptListFile.Trim(), Is.Not.Empty);
            Assert.That(config.SearchPattern, Is.EqualTo("*.sql"));
            Assert.That(config.UndoOutputFile, Is.Not.Null);
            Assert.That(config.UndoOutputFile.Trim(), Is.Not.Empty);
        }

        /// <summary>
        /// Ensures that if the defaults of values are passed or null is passed, the settings aren't altered.
        /// </summary>
        [Test]
        public void ThatEmptyValuesDontChangeSettings()
        {
            IConfigurationService config = new ConfigurationService();
            config.OutputFile = "My Value";
            config.OutputFile = string.Empty;
            Assert.That(config.OutputFile, Is.Not.Empty);

            config.RootDirectory = "My Value";
            config.RootDirectory = string.Empty;
            Assert.That(config.RootDirectory, Is.Not.Empty);

            config.ScriptListFile = "My Value";
            config.ScriptListFile = string.Empty;
            Assert.That(config.ScriptListFile, Is.Not.Empty);

            config.SearchPattern = "My Value";
            config.SearchPattern = string.Empty;
            Assert.That(config.SearchPattern, Is.Not.Empty);

            config.UndoOutputFile = "My Value";
            config.UndoOutputFile = string.Empty;
            Assert.That(config.UndoOutputFile, Is.Not.Empty);
        }

        /// <summary>
        /// Ensures that a value of 0 will apply all changes.
        /// </summary>
        [Test]
        public void That0EqualsIntMaxValueForLastChangeToApply()
        {
            IConfigurationService config = new ConfigurationService();
            config.LastChangeToApply = 0;

            Assert.That(config.LastChangeToApply, Is.EqualTo(int.MaxValue));

            config.LastChangeToApply = -155;
            Assert.That(config.LastChangeToApply, Is.EqualTo(int.MaxValue));

            config.LastChangeToApply = 500;
            Assert.That(config.LastChangeToApply, Is.EqualTo(500));
        }

        /// <summary>
        /// Tests that a relative path for a root directory resolves to the correct physical path.
        /// </summary>
        [Test]
        public void ThatRelativeRootDirectoryResolves()
        {
            IConfigurationService config = new ConfigurationService();
            config.RootDirectory = "TestDirectory";

            Assert.That(config.RootDirectory, Is.EqualTo(Path.Combine(Environment.CurrentDirectory, "TestDirectory")));
        }

        /// <summary>
        /// Ensures that a rooted root directory doesn't get changed into a relative root directory.
        /// </summary>
        [Test]
        public void ThatRootedRootDirectoryIsntRelocated()
        {
            IConfigurationService config = new ConfigurationService();
            config.RootDirectory = @"c:\MyDirectory";

            Assert.That(config.RootDirectory, Is.EqualTo(@"c:\MyDirectory"));
        }

        /// <summary>
        /// Ensures that the location of the script list file will be in the root directory.
        /// </summary>
        [Test]
        public void ThatScriptListFileIsInRootDirectory()
        {
            IConfigurationService config = new ConfigurationService();
            string directory = Path.GetDirectoryName(config.ScriptListFile);
            Assert.That(directory, Is.EqualTo(config.RootDirectory));
        }

        /// <summary>
        /// Ensures that tostring actually returns the details fo rthe config.  Note that we're not doing a deep test here since the value is low.
        /// </summary>
        [Test]
        public void ThatToStringReturnsSettings()
        {
            IConfigurationService config = new ConfigurationService();
            string result = config.ToString();
            Assert.That(result, Is.Not.EqualTo(typeof(ConfigurationService).FullName));
            Assert.That(result, Is.Not.EqualTo(typeof(IConfigurationService).FullName));
        }
    }
}