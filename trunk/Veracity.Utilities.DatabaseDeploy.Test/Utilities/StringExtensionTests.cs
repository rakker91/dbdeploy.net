// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StringExtensionTests.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Test.Utilities
{
    using System;

    using ApprovalTests;

    using DatabaseDeploy.Core.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Tests that the string replace extension works as expected.
    /// </summary>
    [TestClass]
    public class StringExtensionTests : TestFixtureBase
    {
        /// <summary>
        ///     Make sure comments are stripped
        /// </summary>
        [TestMethod]
        public void TestStripComments()
        {
            string[] lines =
                {
                    "line which has no comment", "--line which is nothing but comment",
                    "a line which has stuff --at the front"
                };

            Approvals.VerifyAll(lines.StripSingleLineComments(), "nocomments");
        }

        /// <summary>
        ///     Ensures that if an empty string is passed in for replacement, an exception is thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThatEmptyOldStringThrowsException()
        {
            string firstString = "This is the first string.";
            firstString.ReplaceEx(string.Empty, "second");
        }

        /// <summary>
        ///     Tests that the extension replaces bad characters.
        /// </summary>
        [TestMethod]
        public void ThatExtensionReplacesBadCharacters()
        {
            string badString = "This is my $(adjective) string.  ' and ; aren't allowed.";
            string goodString = "This is my g''o_od string.  ' and ; aren't allowed.";
            string result = badString.ReplaceEx("$(adjective)", "g'o;od");

            Assert.AreEqual(result, goodString);
        }

        /// <summary>
        ///     Ensures that if a null string is requested to have a replacement done, null is returned.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ThatNullBaseStringThrowsException()
        {
            string firstString = null;
            firstString.ReplaceEx("string 1", "second");
        }

        /// <summary>
        ///     Ensures that if a null value is passed in as the replacement value, all instances of first value are removed.
        /// </summary>
        [TestMethod]
        public void ThatNullNewStringDoesntFail()
        {
            string firstString = "This is the first string.";
            string secondString = "This is the  string.";

            string result = firstString.ReplaceEx("first", null);

            Assert.AreEqual(result, secondString);
        }

        /// <summary>
        ///     Ensures that if a null value is passed in for replacement, an exception is thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThatNullOldStringThrowsException()
        {
            string firstString = "This is the first string.";
            firstString.ReplaceEx(null, "second");
        }

        /// <summary>
        ///     Tests that replace does what is expected with no bad characters
        /// </summary>
        [TestMethod]
        public void ThatReplaceWithNoBadCharactersWorks()
        {
            string firstString = "This is the first string.";
            string secondString = "This is the second string.";

            string result = firstString.ReplaceEx("first", "second");

            Assert.AreEqual(result, secondString);
        }
    }
}