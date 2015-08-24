// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringReplaceExtensionTests.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test.Utilities
{
    using System;

    using NUnit.Framework;

    using Veracity.Utilities.DatabaseDeploy.Utilities;

    /// <summary>
    /// Tests that the string replace extension works as expected.
    /// </summary>
    [TestFixture]
    public class StringReplaceExtensionTests : TestFixtureBase
    {
        /// <summary>
        /// Tests that the extension replaces bad characters.
        /// </summary>
        [Test]
        public void ThatExtensionReplacesBadCharacters()
        {
            string badString = "This is my $(adjective) string.  ' and ; aren't allowed.";
            string goodString = "This is my g''o_od string.  ' and ; aren't allowed.";
            string result = badString.ReplaceEx("$(adjective)", "g'o;od");

            Assert.That(result, Is.EqualTo(goodString));
        }

        /// <summary>
        /// Tests that replace does what is expected with no bad characters
        /// </summary>
        [Test]
        public void ThatReplaceWithNoBadCharactersWorks()
        {
            string firstString = "This is the first string.";
            string secondString = "This is the second string.";

            string result = firstString.ReplaceEx("first", "second");

            Assert.That(result, Is.EqualTo(secondString));
        }

        /// <summary>
        /// Ensures that if a null value is passed in for replacement, an exception is thrown
        /// </summary>
        [Test]
        public void ThatNullOldStringThrowsException()
        {
            string firstString = "This is the first string.";

            Assert.Throws<ArgumentNullException>(() => firstString.ReplaceEx(null, "second"));
        }

        /// <summary>
        /// Ensures that if an empty string is passed in for replacement, an exception is thrown.
        /// </summary>
        [Test]
        public void ThatEmptyOldStringThrowsException()
        {
            string firstString = "This is the first string.";

            Assert.Throws<ArgumentException>(() => firstString.ReplaceEx(string.Empty, "second"));
        }

        /// <summary>
        /// Ensures that if a null value is passed in as the replacement value, all instances of first value are removed.
        /// </summary>
        [Test]
        public void ThatNullNewStringDoesntFail()
        {
            string firstString = "This is the first string.";
            string secondString = "This is the  string.";

            string result = firstString.ReplaceEx("first", null);

            Assert.That(result, Is.EqualTo(secondString));            
        }

        /// <summary>
        /// Ensures that if a null string is requested to have a replacement done, null is returned.
        /// </summary>
        [Test]
        public void ThatNullBaseStringThrowsException()
        {
            string firstString = null;

            Assert.Throws<NullReferenceException>(() => firstString.ReplaceEx("string 1", "second"));
        }
    }
}