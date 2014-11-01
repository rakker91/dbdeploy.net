// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptServiceTests.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test.ScriptGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    using ApprovalTests;

    using Moq;

    using NUnit.Framework;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances;
    using Veracity.Utilities.DatabaseDeploy.FileManagement;
    using Veracity.Utilities.DatabaseDeploy.ScriptGeneration;
    using Veracity.Utilities.DatabaseDeploy.Test.Utilities;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    /// <summary>
    /// Tests the Script service
    /// </summary>
    [TestFixture]
    public class ScriptServiceTests : TestFixtureBase
    {
        /// <summary>
        /// Ensures that building a change script behaves as expected.
        /// </summary>
        [Test]
        public void ThatBuildChangeScriptWorks()
        {
            var dateTime = new DateTime(2014, 09, 17, 17, 42, 55);
            TimeProvider.Current = new MockTimeProvider(dateTime);
            var mockEnvironmentProvider = new MockEnvironmentProvider();
            mockEnvironmentProvider.SetUserName("userName");
            EnvironmentProvider.Current = mockEnvironmentProvider;
            string changeScriptHeader = "Change Script Header -- CurrentVersion = $(CurrentVersion);Current DateTime = $(CurrentDateTime);Current User = $(CurrentUser)";
            string scriptHeader = "Script Header -- Script Name = $(ScriptName);Script Id = $(ScriptId);Script Description = $(ScriptDescription)";
            string scriptFooter = "Script Footer -- String Name = $(ScriptName);Script Id = $(ScriptId)";
            string changeScriptFooter = "Change Script Footer -- Current DateTime = $(CurrentDateTime)";
            string undoToken = "--//@Undo";

            ScriptService constructorTest = new ScriptService();

            var databaseServiceMock = new Mock<IDatabaseService>(MockBehavior.Strict);
            var fileServiceMock = new Mock<IFileService>(MockBehavior.Strict);

            // Database service mock setup
            databaseServiceMock.Setup(d => d.GetScriptFromFile(DatabaseScriptEnum.ChangeScriptHeader)).Returns(changeScriptHeader);
            databaseServiceMock.Setup(d => d.GetScriptFromFile(DatabaseScriptEnum.ScriptHeader)).Returns(scriptHeader);
            databaseServiceMock.Setup(d => d.GetScriptFromFile(DatabaseScriptEnum.ScriptFooter)).Returns(scriptFooter);
            databaseServiceMock.Setup(d => d.GetScriptFromFile(DatabaseScriptEnum.ChangeScriptFooter)).Returns(changeScriptFooter);
            databaseServiceMock.Setup(d => d.GetScriptFromFile(DatabaseScriptEnum.UndoToken)).Returns(undoToken);

            // file Service mock setup
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns(
                (string s, bool b) =>
                    {
                        ScriptFile file = new ScriptFile();
                        file.Parse(fileServiceMock.Object, s);
                        if (file.Id % 2 == 0)
                        {
                            return file.Description + undoToken + "Undo text.";
                        }
                        else
                        {
                            return file.Description;
                        }
                    });

            IDictionary<int, IScriptFile> changes = this.GetChanges();

            IScriptService scriptService = new ScriptService(databaseServiceMock.Object, fileServiceMock.Object, new TokenReplacer(new ConfigurationService()), new ConfigurationService());

            string result = scriptService.BuildChangeScript(changes);

            TimeProvider.ResetToDefault();
            EnvironmentProvider.ResetToDefault();
            Approvals.Verify(result);
        }

        /// <summary>
        /// Ensures that build undo script functionality works.
        /// </summary>
        [Test]
        public void ThatBuildUndoScriptWorks()
        {
            var dateTime = new DateTime(2014, 09, 17, 17, 42, 55);
            TimeProvider.Current = new MockTimeProvider(dateTime);
            var mockEnvironmentProvider = new MockEnvironmentProvider();
            mockEnvironmentProvider.SetUserName("userName");
            EnvironmentProvider.Current = mockEnvironmentProvider;

            //string expectedResult = string.Format("Undo Script Header -- CurrentVersion = 3;Current DateTime = {1};Current User = {0}\r\nScript Header -- Script Name = 2 2.sql;Script Id = 2;Script Description = 2.sql\r\nUndo text.\r\nScript Footer -- String Name = 2 2.sql;Script Id = 2\r\nUndo Script Footer -- Current DateTime = {1}\r\n", Environment.UserName, DateTime.Now.ToString("g"));
            string undoScriptHeader = "Undo Script Header -- CurrentVersion = $(CurrentVersion);Current DateTime = $(CurrentDateTime);Current User = $(CurrentUser)";
            string undoHeader = "Script Header -- Script Name = $(ScriptName);Script Id = $(ScriptId);Script Description = $(ScriptDescription)";
            string undoFooter = "Script Footer -- String Name = $(ScriptName);Script Id = $(ScriptId)";
            string undoScriptFooter = "Undo Script Footer -- Current DateTime = $(CurrentDateTime)";
            string undoToken = "--//@Undo";

            ScriptService constructorTest = new ScriptService();

            var databaseServiceMock = new Mock<IDatabaseService>(MockBehavior.Strict);
            var fileServiceMock = new Mock<IFileService>(MockBehavior.Strict);

            // Database service mock setup
            databaseServiceMock.Setup(d => d.GetScriptFromFile(DatabaseScriptEnum.UndoScriptHeader)).Returns(undoScriptHeader);
            databaseServiceMock.Setup(d => d.GetScriptFromFile(DatabaseScriptEnum.UndoHeader)).Returns(undoHeader);
            databaseServiceMock.Setup(d => d.GetScriptFromFile(DatabaseScriptEnum.UndoFooter)).Returns(undoFooter);
            databaseServiceMock.Setup(d => d.GetScriptFromFile(DatabaseScriptEnum.UndoScriptFooter)).Returns(undoScriptFooter);
            databaseServiceMock.Setup(d => d.GetScriptFromFile(DatabaseScriptEnum.UndoToken)).Returns(undoToken);

            // file Service mock setup
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns(
                (string s, bool b) =>
                {
                    ScriptFile file = new ScriptFile();
                    file.Parse(fileServiceMock.Object, s);
                    if (file.Id % 2 == 0)
                    {
                        return file.Description + undoToken + "Undo text.";
                    }
                    else
                    {
                        return file.Description;
                    }
                });
            fileServiceMock.Setup(f => f.GetLinesFromFile(It.IsAny<string>())).Returns(new string[0]);

            IDictionary<int, IScriptFile> changes = this.GetChanges();

            IScriptService scriptService = new ScriptService(databaseServiceMock.Object, fileServiceMock.Object, new TokenReplacer(new ConfigurationService()), new ConfigurationService());

            string result = scriptService.BuildUndoScript(changes);

            TimeProvider.ResetToDefault();
            EnvironmentProvider.ResetToDefault();
            Approvals.Verify(result);
        }

        /// <summary>
        /// Gets a set of changes for testing.
        /// </summary>
        /// <returns>A dictionary with the changes to test.</returns>
        private IDictionary<int, IScriptFile> GetChanges()
        {
            IDictionary<int, IScriptFile> changes = new Dictionary<int, IScriptFile>();
            this.AddChange(changes, 1);
            this.AddChange(changes, 2);
            this.AddChange(changes, 3);

            return changes;
        }

        /// <summary>
        /// Adds a single change to the dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary to add changes to</param>
        /// <param name="change">The change number ot add.</param>
        private void AddChange(IDictionary<int, IScriptFile> dictionary, int change)
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetLinesFromFile(It.IsAny<string>())).Returns(new string[0]);
            ScriptFile script = new ScriptFile();
            script.Parse(fileServiceMock.Object, string.Format("{0} {0}.sql", change));
            dictionary.Add(change, script);
        }
    }
}