// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DatabaseServiceBaseTests.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Test.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    using DatabaseDeploy.Core.Configuration;
    using DatabaseDeploy.Core.Database;
    using DatabaseDeploy.Core.Database.DatabaseInstances;
    using DatabaseDeploy.Core.FileManagement;
    using DatabaseDeploy.Core.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    ///     Tests the database service.
    /// </summary>
    [TestClass]
    public class DatabaseServiceBaseTests : TestFixtureBase
    {
        /// <summary>
        ///     Tests the getting of Applied Changes
        /// </summary>
        [TestMethod]
        public void ThatGetAppliedChangesReturnsChangelog()
        {
            ConfigurationService configurationService = new ConfigurationService();
            configurationService.SetupDatabaseType();
            Mock<IFileService> fileServiceMock = new Mock<IFileService>(MockBehavior.Strict);

            string fileContents = "File Contents";

            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(fileContents)
                .Verifiable();

            BogusDatabaseMock databaseService = new BogusDatabaseMock(
                configurationService,
                fileServiceMock.Object,
                new TokenReplacer(configurationService));

            databaseService.DataSetToReturn = this.GetDataset();

            IDictionary<decimal, IChangeLog> result = databaseService.GetAppliedChanges();

            fileServiceMock.Verify(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>()), Times.Exactly(2));

            Assert.IsTrue(databaseService.ScriptsRunNames.Contains(DatabaseScriptEnum.EnsureChangeLogExists));
            Assert.IsTrue(databaseService.ScriptsRunNames.Contains(DatabaseScriptEnum.GetChangeLog));
            Assert.AreEqual(result.Count, 5);
        }

        /// <summary>
        ///     Tests the get script from file functionality
        /// </summary>
        [TestMethod]
        public void ThatGetScriptFromFileReturnsString()
        {
            BogusDatabaseMock constructorTest = new BogusDatabaseMock();

            ConfigurationService configurationService = new ConfigurationService();
            configurationService.SetupDatabaseType();
            Mock<IFileService> fileServiceMock = new Mock<IFileService>(MockBehavior.Strict);

            string scriptFileName = "MyScript.sql";
            string fileContents = "File Contents";
            string filePath = Path.Combine(configurationService.DatabaseScriptPath, scriptFileName);
            string passedFile = string.Empty;
            bool useCache = false;

            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback(
                    (string f, bool c) =>
                        {
                            passedFile = f;
                            useCache = c;
                        }).Returns(fileContents);

            IDatabaseService databaseService = new BogusDatabaseMock(
                configurationService,
                fileServiceMock.Object,
                new TokenReplacer());

            string result = databaseService.GetScriptFromFile(scriptFileName);

            Assert.AreEqual(fileContents, result);
            Assert.AreEqual(passedFile, filePath);
            Assert.IsTrue(useCache);
        }

        /// <summary>
        ///     Adds a new data row
        /// </summary>
        /// <param name="ds">The dataset to add rows to</param>
        /// <param name="changeNumber">The change number ot insert</param>
        private void AddDataRow(DataSet ds, int changeNumber)
        {
            ds.Tables["Table1"].Rows.Add(
                changeNumber,
                "main",
                new DateTime(2012, 1, 1),
                new DateTime(2012, 1, 1),
                "me",
                string.Format("Script ", changeNumber));
        }

        /// <summary>
        ///     Gets a dataset for the tests.
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
    }
}