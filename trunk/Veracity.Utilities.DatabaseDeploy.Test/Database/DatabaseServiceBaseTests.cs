// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseServiceBaseTests.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    using Moq;

    using NUnit.Framework;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.Database;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances;
    using Veracity.Utilities.DatabaseDeploy.FileManagement;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    /// <summary>
    /// Tests the database service.
    /// </summary>
    [TestFixture]
    public class DatabaseServiceBaseTests : TestFixtureBase
    {
        /// <summary>
        /// Tests the get script from file functionality
        /// </summary>
        [Test]
        public void ThatGetScriptFromFileReturnsString()
        {
            BogusDatabaseMock constructorTest = new BogusDatabaseMock();

            IConfigurationService configurationService = new ConfigurationService();
            var fileServiceMock = new Mock<IFileService>(MockBehavior.Strict);

            string scriptFileName = "MyScript.sql";
            string fileContents = "File Contents";
            string filePath = Path.Combine(configurationService.DatabaseScriptPath, scriptFileName);
            string passedFile = string.Empty;
            bool useCache = false;

            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Callback(
                (string f, bool c) =>
                    {
                        passedFile = f;
                        useCache = c;
                    }).Returns(fileContents);

            IDatabaseService databaseService = new BogusDatabaseMock(configurationService, fileServiceMock.Object, new TokenReplacer());

            string result = databaseService.GetScriptFromFile(scriptFileName);

            Assert.That(fileContents, Is.EqualTo(result));
            Assert.That(passedFile, Is.EqualTo(filePath));
            Assert.That(useCache, Is.EqualTo(true));
        }

        /// <summary>
        /// Tests the getting of Applied Changes
        /// </summary>
        [Test]
        public void ThatGetAppliedChangesReturnsChangelog()
        {
            IConfigurationService configurationService = new ConfigurationService();
            var fileServiceMock = new Mock<IFileService>(MockBehavior.Strict);

            string fileContents = "File Contents";

            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns(fileContents).Verifiable();

            BogusDatabaseMock databaseService = new BogusDatabaseMock(configurationService, fileServiceMock.Object, new TokenReplacer());

            databaseService.DataSetToReturn = this.GetDataset();

            IDictionary<int, IChangeLog> result = databaseService.GetAppliedChanges();

            fileServiceMock.Verify(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>()), Times.Exactly(2));

            Assert.That(databaseService.ScriptsRunNames.Contains(DatabaseScriptEnum.EnsureChangeLogExists), Is.True);
            Assert.That(databaseService.ScriptsRunNames.Contains(DatabaseScriptEnum.GetChangeLog), Is.True);
            Assert.That(result.Count, Is.EqualTo(5));
        }

        /// <summary>
        /// Gets a dataset for the tests.
        /// </summary>
        /// <returns>A dataset containg a few rows for the test.</returns>
        private DataSet GetDataset()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("Table1");
            ds.Tables["Table1"].Columns.Add("change_number", typeof(int));
            ds.Tables["Table1"].Columns.Add("delta_set", typeof(string));
            ds.Tables["Table1"].Columns.Add("start_dt", typeof(DateTime));
            ds.Tables["Table1"].Columns.Add("complete_dt", typeof(DateTime));
            ds.Tables["Table1"].Columns.Add("applied_by", typeof(string));
            ds.Tables["Table1"].Columns.Add("description", typeof(string));

            this.AddDataRow(ds, 1);
            this.AddDataRow(ds, 2);
            this.AddDataRow(ds, 3);
            this.AddDataRow(ds, 4);
            this.AddDataRow(ds, 5);

            return ds;
        }

        /// <summary>
        /// Adds a new data row
        /// </summary>
        /// <param name="ds">The dataset to add rows to</param>
        /// <param name="changeNumber">The change number ot insert</param>
        private void AddDataRow(DataSet ds, int changeNumber)
        {
            ds.Tables["Table1"].Rows.Add(changeNumber, "main", new DateTime(2012, 1, 1), new DateTime(2012, 1, 1), "me", string.Format("Script ", changeNumber));
        }
    }
}