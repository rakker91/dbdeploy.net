// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FileServiceTests.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Test.FileManagement
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using DatabaseDeploy.Core.Configuration;
    using DatabaseDeploy.Core.FileManagement;
    using DatabaseDeploy.Core.ScriptGeneration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    ///     Tests the file service class
    /// </summary>
    [TestClass]
    public class FileServiceTests : TestFixtureBase
    {
        /// <summary>
        ///     Ensures that cleanup past files works.
        /// </summary>
        [TestMethod]
        public void ThatCleanupPastRunsRemovesFiles()
        {
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            Assert.IsTrue(File.Exists(config.OutputFile));
            Assert.IsTrue(File.Exists(config.ScriptListFile));
            Assert.IsTrue(File.Exists(config.UndoOutputFile));

            IFileService fileService = new FileService(config, new IoProxy());
            fileService.CleanupPastRuns();

            Assert.IsFalse(File.Exists(config.OutputFile));
            Assert.IsFalse(File.Exists(config.ScriptListFile));
            Assert.IsFalse(File.Exists(config.UndoOutputFile));
        }

        /// <summary>
        ///     Ensures that duplicates are detected in the script list
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ThatDuplicateScriptsAreDetected()
        {
            Mock<IIoProxy> mock = new Mock<IIoProxy>(MockBehavior.Strict);
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            mock.Setup(i => i.Exists(It.IsAny<string>())).Returns(true);
            mock.Setup(i => i.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
                .Returns(this.GetFilesWithDuplicate());
            mock.Setup(i => i.ReadAllText(It.IsAny<string>())).Returns(string.Empty);

            IFileService fileService = new FileService(config, mock.Object);
            fileService.GetScriptFiles();
        }

        /// <summary>
        ///     Ensures that if a file is in use, it'll fail.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void ThatGetFileContentsFailsWhenInUse()
        {
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            IFileService fileService = new FileService(config, new IoProxy());

            try
            {
                using (StreamWriter writer = new StreamWriter(config.OutputFile))
                {
                    fileService.GetFileContents(config.OutputFile, false);
                }
            }
            finally
            {
                fileService.CleanupPastRuns();
            }
        }

        /// <summary>
        ///     Ensures that file contents are retreived with the cache.  Since the cache is a dictionary, there's not a good way
        ///     to test that
        ///     the contents came from the cache.
        /// </summary>
        [TestMethod]
        public void ThatGetFileContentsWorkWithCache()
        {
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            IFileService fileService = new FileService(config, new IoProxy());
            string fileContents = fileService.GetFileContents(config.OutputFile, true);
            Assert.AreEqual(fileContents, "OutputFile");

            fileContents = fileService.GetFileContents(config.OutputFile, true);
            Assert.AreEqual(fileContents, "OutputFile");

            fileService.CleanupPastRuns();
        }

        /// <summary>
        ///     Ensures that file contents are retrieved without a cache.
        /// </summary>
        [TestMethod]
        public void ThatGetFileContentsWorkWithoutCache()
        {
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            IFileService fileService = new FileService(config, new IoProxy());
            string fileContents = fileService.GetFileContents(config.OutputFile, false);
            Assert.AreEqual(fileContents, "OutputFile");

            fileService.CleanupPastRuns();
        }

        /// <summary>
        ///     Ensures that get script files returns the expected file list.
        /// </summary>
        [TestMethod]
        public void ThatGetScriptFilesReturnsFileList()
        {
            Mock<IIoProxy> mock = new Mock<IIoProxy>(MockBehavior.Strict);
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            mock.Setup(i => i.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
                .Returns(this.GetFiles());
            mock.Setup(i => i.Exists(It.IsAny<string>())).Returns(true);
            mock.Setup(i => i.ReadAllText(It.IsAny<string>())).Returns(string.Empty);

            IFileService fileService = new FileService(config, mock.Object);

            IDictionary<decimal, IScriptFile> scripts = fileService.GetScriptFiles();

            Assert.AreEqual(scripts.Count, 10);
            Assert.AreEqual(scripts[8].Id, 8);
        }

        /// <summary>
        ///     Ensures that non-existant files don't cause errors
        /// </summary>
        [TestMethod]
        public void ThatNonExistantFilesDontCauseFailure()
        {
            IConfigurationService config = new ConfigurationService();
            IFileService fileService = new FileService(config, new IoProxy());

            try
            {
                fileService.CleanupPastRuns();
            }
            catch (Exception ex)
            {
                Assert.Fail("Clean Up Previous caused exception.  Message was: " + ex.Message);
            }
        }

        /// <summary>
        ///     Ensures that a non existant file throws an error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ThatNonExistentFileThrowsErrorOnGetFileContents()
        {
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);

            IFileService fileService = new FileService(config, new IoProxy());
            fileService.CleanupPastRuns();
            fileService.GetFileContents(config.OutputFile, false);
        }

        /// <summary>
        ///     Ensures that the script list writes as expected.
        /// </summary>
        [TestMethod]
        public void ThatScriptListWrites()
        {
            string passedFileName = string.Empty;
            Mock<IIoProxy> mock = new Mock<IIoProxy>(MockBehavior.Strict);
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);
            IFileService fileService = new FileService(config, mock.Object);

            mock.Setup(i => i.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
                .Returns(this.GetFiles());
            mock.Setup(i => i.Exists(It.IsAny<string>())).Returns(true);
            mock.Setup(i => i.ReadAllText(It.IsAny<string>())).Returns(string.Empty);
            IDictionary<decimal, IScriptFile> scripts = fileService.GetScriptFiles();

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    mock.Setup(i => i.GetStreamWriter(It.IsAny<string>()))
                        .Callback<string>(name => { passedFileName = name; })
                        .Returns(writer);
                    fileService.WriteScriptList(scripts);
                }
            }

            Assert.AreEqual(passedFileName, config.ScriptListFile);

            fileService.CleanupPastRuns();
        }

        /// <summary>
        ///     Ensures that the undo script will write.
        /// </summary>
        [TestMethod]
        public void ThatUndoScriptWrites()
        {
            string passedFileName = string.Empty;
            string expectedContents = "Undo Script";
            Mock<IIoProxy> mock = new Mock<IIoProxy>(MockBehavior.Strict);
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);
            IFileService fileService = new FileService(config, mock.Object);

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    mock.Setup(i => i.GetStreamWriter(It.IsAny<string>()))
                        .Callback<string>(name => { passedFileName = name; })
                        .Returns(writer);
                    fileService.WriteUndoScript(expectedContents);
                }
            }

            Assert.AreEqual(passedFileName, config.UndoOutputFile);

            fileService.CleanupPastRuns();
        }

        /// <summary>
        ///     Ensures that WriteChangeScript writes to the expected location and with the expected contents
        /// </summary>
        [TestMethod]
        public void ThatWriteChangeScriptWrites()
        {
            string passedFileName = string.Empty;
            string expectedContents = "ChangeScript";
            Mock<IIoProxy> mock = new Mock<IIoProxy>(MockBehavior.Strict);
            IConfigurationService config = new ConfigurationService();
            this.SetupConfiguration(config);
            IFileService fileService = new FileService(config, mock.Object);

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    mock.Setup(i => i.GetStreamWriter(It.IsAny<string>()))
                        .Callback<string>(name => { passedFileName = name; })
                        .Returns(writer);
                    fileService.WriteChangeScript(expectedContents);
                }
            }

            Assert.AreEqual(passedFileName, config.OutputFile);

            fileService.CleanupPastRuns();
        }

        /// <summary>
        ///     Adds a single number to a list
        /// </summary>
        /// <param name="list">The list ot add to</param>
        /// <param name="changeNumber">The change number ot add</param>
        private void AddFile(List<string> list, int changeNumber)
        {
            list.Add(string.Format("{0} {0}.sql", changeNumber));
        }

        /// <summary>
        ///     Gets an array of files to use for testing.
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
        ///     Gets an array of files to use for testing.
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
        ///     Setsup the configuration object
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
    }
}