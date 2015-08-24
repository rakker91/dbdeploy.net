// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScriptFileTests.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Test.ScriptGeneration
{
    using System;

    using DatabaseDeploy.Core.FileManagement;
    using DatabaseDeploy.Core.ScriptGeneration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    ///     Tests the script file class
    /// </summary>
    [TestClass]
    public class ScriptFileTests : TestFixtureBase
    {
        /// <summary>
        ///     Ensures that extra spaces in the name are removed.
        /// </summary>
        [TestMethod]
        public void ThatExtraSpacesAreRemoved()
        {
            Mock<IFileService> fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns(string.Empty);
            ScriptFile script = new ScriptFile();
            script.Parse(fileServiceMock.Object, "  0001   Script.sql  ");

            Assert.AreEqual(script.Id, 1);
            Assert.AreEqual(script.Description, "Script");
            Assert.AreEqual(script.FileName, "0001   Script.sql");
        }

        /// <summary>
        ///     Ensures that extra zeros are correctly interpreted.
        /// </summary>
        [TestMethod]
        public void ThatExtraZerosAreAccepted()
        {
            Mock<IFileService> fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns(string.Empty);
            ScriptFile script = new ScriptFile();
            string fileName = "00001 Script.sql";
            script.Parse(fileServiceMock.Object, fileName);

            Assert.AreEqual(script.Id, 1);
            Assert.AreEqual(script.Description, "Script");
            Assert.AreEqual(script.FileName, fileName);
        }

        /// <summary>
        ///     Ensures that a file without an ID is caught.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ThatMissingIdIsCaught()
        {
            Mock<IFileService> fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns(string.Empty);
            ScriptFile script = new ScriptFile();
            script.Parse(fileServiceMock.Object, "My Script.sql");
        }

        /// <summary>
        ///     Ensures that if they just use numbers, things don't go south.
        /// </summary>
        [TestMethod]
        public void ThatNoDescriptionWorksAsExpected()
        {
            Mock<IFileService> fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns(string.Empty);
            ScriptFile script = new ScriptFile();
            string fileName = "1.sql";
            script.Parse(fileServiceMock.Object, fileName);

            Assert.AreEqual(script.Id, 1);
            Assert.AreEqual(script.Description, string.Empty);
            Assert.AreEqual(script.FileName, fileName);
        }

        /// <summary>
        ///     Ensures that a normal parse works as expected.
        /// </summary>
        [TestMethod]
        public void ThatRegularFileNameParsesCorrectly()
        {
            Mock<IFileService> fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns(string.Empty);
            ScriptFile script = new ScriptFile();
            string fileName = "1 Script.sql";
            script.Parse(fileServiceMock.Object, fileName);

            Assert.AreEqual(script.Id, 1);
            Assert.AreEqual(script.Description, "Script");
            Assert.AreEqual(script.FileName, fileName);
        }

        /// <summary>
        ///     Ensures that a script that looks like 1 1 script file works correctly.
        /// </summary>
        [TestMethod]
        public void ThatScriptDescriptionStartingWithNumberWorks()
        {
            Mock<IFileService> fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns(string.Empty);
            ScriptFile script = new ScriptFile();
            string fileName = "1 1 Script.sql";
            script.Parse(fileServiceMock.Object, fileName);

            Assert.AreEqual(script.Id, 1);
            Assert.AreEqual(script.Description, "1 Script");
            Assert.AreEqual(script.FileName, fileName);
        }
    }
}