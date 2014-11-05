// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptFileTests.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test.ScriptGeneration
{
    using NUnit.Framework;
    using Moq;
    using Veracity.Utilities.DatabaseDeploy.FileManagement;
    using Veracity.Utilities.DatabaseDeploy.ScriptGeneration;

    /// <summary>
    /// Tests the script file class
    /// </summary>
    [TestFixture]
    public class ScriptFileTests : TestFixtureBase
    {
        /// <summary>
        /// Ensures that a normal parse works as expected.
        /// </summary>
        [Test]
        public void ThatRegularFileNameParsesCorrectly()
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns("");
            var script = new ScriptFile();
            string fileName = "1 Script.sql";
            script.Parse(fileServiceMock.Object, fileName);

            Assert.That(script.Id, Is.EqualTo(1));
            Assert.That(script.Description, Is.EqualTo("Script"));
            Assert.That(script.FileName, Is.EqualTo(fileName));
        }

        /// <summary>
        /// Ensures that a file without an ID is caught.
        /// </summary>
        [Test]
        public void ThatMissingIdIsCaught()
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns("");
            var script = new ScriptFile();
            Assert.Throws<System.Exception>(() => script.Parse(fileServiceMock.Object, "My Script.sql"));
        }

        /// <summary>
        /// Ensures that extra zeros are correctly interpreted.
        /// </summary>
        [Test]
        public void ThatExtraZerosAreAccepted()
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns("");
            var script = new ScriptFile();
            string fileName = "00001 Script.sql";
            script.Parse(fileServiceMock.Object, fileName);

            Assert.That(script.Id, Is.EqualTo(1));
            Assert.That(script.Description, Is.EqualTo("Script"));
            Assert.That(script.FileName, Is.EqualTo(fileName));
        }

        /// <summary>
        /// Ensures that extra spaces in the name are removed.
        /// </summary>
        [Test]
        public void ThatExtraSpacesAreRemoved()
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns("");
            var script = new ScriptFile();
            script.Parse(fileServiceMock.Object, "  0001   Script.sql  ");

            Assert.That(script.Id, Is.EqualTo(1));
            Assert.That(script.Description, Is.EqualTo("Script"));
            Assert.That(script.FileName, Is.EqualTo("0001   Script.sql"));
        }

        /// <summary>
        /// Ensures that a script that looks like 1 1 script file works correctly.
        /// </summary>
        [Test]
        public void ThatScriptDescriptionStartingWithNumberWorks()
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns("");
            var script = new ScriptFile();
            string fileName = "1 1 Script.sql";
            script.Parse(fileServiceMock.Object, fileName);

            Assert.That(script.Id, Is.EqualTo(1));
            Assert.That(script.Description, Is.EqualTo("1 Script"));
            Assert.That(script.FileName, Is.EqualTo(fileName));
        }

        /// <summary>
        /// Ensures that if they just use numbers, things don't go south.
        /// </summary>
        [Test]
        public void ThatNoDescriptionWorksAsExpected()
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(f => f.GetFileContents(It.IsAny<string>(), It.IsAny<bool>())).Returns("");
            var script = new ScriptFile();
            string fileName = "1.sql";
            script.Parse(fileServiceMock.Object, fileName);

            Assert.That(script.Id, Is.EqualTo(1));
            Assert.That(script.Description, Is.EqualTo(string.Empty));
            Assert.That(script.FileName, Is.EqualTo(fileName));
        }

    }
}