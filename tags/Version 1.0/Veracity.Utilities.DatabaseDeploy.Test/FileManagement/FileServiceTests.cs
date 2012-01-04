// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileServiceTests.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test.FileManagement
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Moq;

    using NUnit.Framework;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.FileManagement;
    using Veracity.Utilities.DatabaseDeploy.ScriptGeneration;

    /// <summary>
    /// Tests the file service class
    /// </summary>
    [TestFixture]
    public class FileServiceTests : TestFixtureBase
    {
        /// <summary>
        /// Ensures that cleanup past files works.
        /// </summary>
        [Test]
        public void ThatCleanupPastRunsRemovesFiles()
        {
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            Assert.That(File.Exists(config.OutputFile), Is.True);
            Assert.That(File.Exists(config.ScriptListFile), Is.True);
            Assert.That(File.Exists(config.UndoOutputFile), Is.True);

            IFileService fileService = new FileService(config, new IoProxy());
            fileService.CleanupPastRuns();

            Assert.That(File.Exists(config.OutputFile), Is.Not.True);
            Assert.That(File.Exists(config.ScriptListFile), Is.Not.True);
            Assert.That(File.Exists(config.UndoOutputFile), Is.Not.True);
        }

        /// <summary>
        /// Ensures that non-existant files don't cause errors
        /// </summary>
        [Test]
        public void ThatNonExistantFilesDontCauseFailure()
        {
            IConfigurationService config = new ConfigurationService();
            IFileService fileService = new FileService(config, new IoProxy());

            Assert.DoesNotThrow(fileService.CleanupPastRuns);
        }

        /// <summary>
        /// Ensures that file contents are retrieved without a cache.
        /// </summary>
        [Test]
        public void ThatGetFileContentsWorkWithoutCache()
        {
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            IFileService fileService = new FileService(config, new IoProxy());
            string fileContents = fileService.GetFileContents(config.OutputFile, false);
            Assert.That(fileContents, Is.EqualTo("OutputFile"));

            fileService.CleanupPastRuns();
        }

        /// <summary>
        /// Ensures that file contents are retreived with the cache.  Since the cache is a dictionary, there's not a good way to test that
        /// the contents came from the cache.
        /// </summary>
        [Test]
        public void ThatGetFileContentsWorkWithCache()
        {
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            IFileService fileService = new FileService(config, new IoProxy());
            string fileContents = fileService.GetFileContents(config.OutputFile, true);
            Assert.That(fileContents, Is.EqualTo("OutputFile"));

            fileContents = fileService.GetFileContents(config.OutputFile, true);
            Assert.That(fileContents, Is.EqualTo("OutputFile"));

            fileService.CleanupPastRuns();
        }

        /// <summary>
        /// Ensures that if a file is in use, it'll fail.
        /// </summary>
        [Test]
        public void ThatGetFileContentsFailsWhenInUse()
        {
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            IFileService fileService = new FileService(config, new IoProxy());

            using (StreamWriter writer = new StreamWriter(config.OutputFile))
            {
                Assert.Throws<IOException>(() => fileService.GetFileContents(config.OutputFile, false));
            }

            fileService.CleanupPastRuns();
        }

        /// <summary>
        /// Ensures that a non existant file throws an error
        /// </summary>
        [Test]
        public void ThatNonExistentFileThrowsErrorOnGetFileContents()
        {
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            IFileService fileService = new FileService(config, new IoProxy());
            fileService.CleanupPastRuns();

            Assert.Throws<FileNotFoundException>(() => fileService.GetFileContents(config.OutputFile, false));
        }

        /// <summary>
        /// Ensures that get script files returns the expected file list.
        /// </summary>
        [Test]
        public void ThatGetScriptFilesReturnsFileList()
        {
            var mock = new Mock<IIoProxy>(MockBehavior.Strict);
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            mock.Setup(i => i.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns(this.GetFiles());

            IFileService fileService = new FileService(config, mock.Object);

            IDictionary<int, IScriptFile> scripts = fileService.GetScriptFiles();

            Assert.That(scripts.Count, Is.EqualTo(10));
            Assert.That(scripts[8].Id, Is.EqualTo(8));
        }

        /// <summary>
        /// Ensures that duplicates are detected in the script list
        /// </summary>
        [Test]
        public void ThatDuplicateScriptsAreDetected()
        {
            var mock = new Mock<IIoProxy>(MockBehavior.Strict);
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            mock.Setup(i => i.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns(this.GetFilesWithDuplicate());

            IFileService fileService = new FileService(config, mock.Object);

            Assert.Throws<System.Exception>(() => fileService.GetScriptFiles());
        }

        /// <summary>
        /// Ensures that WriteChangeScript writes to the expected location and with the expected contents
        /// </summary>
        [Test]
        public void ThatWriteChangeScriptWrites()
        {
            string passedFileName = string.Empty;
            string expectedContents = "ChangeScript";
            var mock = new Mock<IIoProxy>(MockBehavior.Strict);
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);
            IFileService fileService = new FileService(config, mock.Object);

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    mock.Setup(i => i.GetStreamWriter(It.IsAny<string>())).Callback<string>(name => { passedFileName = name; }).Returns(writer);
                    fileService.WriteChangeScript(expectedContents);
                }
            }

            Assert.That(passedFileName, Is.EqualTo(config.OutputFile));

            fileService.CleanupPastRuns();
        }

        /// <summary>
        /// Ensures that the undo script will write.
        /// </summary>
        [Test]
        public void ThatUndoScriptWrites()
        {
            string passedFileName = string.Empty;
            string expectedContents = "Undo Script";
            var mock = new Mock<IIoProxy>(MockBehavior.Strict);
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);
            IFileService fileService = new FileService(config, mock.Object);

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    mock.Setup(i => i.GetStreamWriter(It.IsAny<string>())).Callback<string>(name => { passedFileName = name; }).Returns(writer);
                    fileService.WriteUndoScript(expectedContents);
                }
            }

            Assert.That(passedFileName, Is.EqualTo(config.UndoOutputFile));

            fileService.CleanupPastRuns();
        }

        /// <summary>
        /// Ensures that the script list writes as expected.
        /// </summary>
        [Test]
        public void ThatScriptListWrites()
        {
            string passedFileName = string.Empty;
            var mock = new Mock<IIoProxy>(MockBehavior.Strict);
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);
            IFileService fileService = new FileService(config, mock.Object);

            mock.Setup(i => i.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns(this.GetFiles());
            IDictionary<int, IScriptFile> scripts = fileService.GetScriptFiles();

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    mock.Setup(i => i.GetStreamWriter(It.IsAny<string>())).Callback<string>(name => { passedFileName = name; }).Returns(writer);
                    fileService.WriteScriptList(scripts);
                }
            }

            Assert.That(passedFileName, Is.EqualTo(config.ScriptListFile));

            fileService.CleanupPastRuns();
        }

        /// <summary>
        /// Setsup the configuration object
        /// </summary>
        /// <param name="configService">Setups the configuration service.</param>
        private void SetupConfiguration(IConfigurationService configService)
        {
            configService.OutputFile = Path.GetTempFileName();
            configService.UndoOutputFile = Path.GetTempFileName();
            configService.ScriptListFile = Path.GetTempFileName();

            using (StreamWriter writer = File.CreateText(configService.OutputFile))
            {
                writer.Write("OutputFile");
            }

            using (StreamWriter writer = File.CreateText(configService.UndoOutputFile))
            {
                writer.Write("UndoOutputFile");
            }

            using (StreamWriter writer = File.CreateText(configService.ScriptListFile))
            {
                writer.Write("ScriptListFile");
            }
        }

        /// <summary>
        /// Gets an array of files to use for testing.
        /// </summary>
        /// <returns>A list of files to be parsed.</returns>
        private string[] GetFiles()
        {
            List<string> filesList = new List<string>();
            this.AddFile(filesList, 1);
            this.AddFile(filesList, 2);
            this.AddFile(filesList, 3);
            this.AddFile(filesList, 4);
            this.AddFile(filesList, 5);
            this.AddFile(filesList, 6);
            this.AddFile(filesList, 7);
            this.AddFile(filesList, 8);
            this.AddFile(filesList, 9);
            this.AddFile(filesList, 10);
            return filesList.ToArray();
        }

        /// <summary>
        /// Gets an array of files to use for testing.
        /// </summary>
        /// <returns>A string array with a duplicate file in the file list.</returns>
        private string[] GetFilesWithDuplicate()
        {
            List<string> filesList = new List<string>();
            this.AddFile(filesList, 1);
            this.AddFile(filesList, 2);
            this.AddFile(filesList, 3);
            this.AddFile(filesList, 4);
            this.AddFile(filesList, 5);
            this.AddFile(filesList, 6);
            this.AddFile(filesList, 7);
            this.AddFile(filesList, 8);
            this.AddFile(filesList, 8);
            this.AddFile(filesList, 9);
            return filesList.ToArray();
        }

        /// <summary>
        /// Adds a single number to a list
        /// </summary>
        /// <param name="list">The list ot add to</param>
        /// <param name="changeNumber">The change number ot add</param>
        private void AddFile(List<string> list, int changeNumber)
        {
            list.Add(string.Format("{0} {0}.sql", changeNumber));
        }
    }
}