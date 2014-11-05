// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptMessageFormatterTests.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace Veracity.Utilities.DatabaseDeploy.Test.Utilities
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Veracity.Utilities.DatabaseDeploy.Utilities;

    /// <summary>
    /// Tests the script message formatter
    /// </summary>
    [TestFixture]
    public class ScriptMessageFormatterTests : TestFixtureBase
    {
        /// <summary>
        /// Ensures that format collection returns expected string
        /// </summary>
        [Test]
        public void ThatFormatCollectionReturnsExpectedString()
        {
            string expectedResult = "1 to 10";
            IScriptMessageFormatter formatter = new ScriptMessageFormatter();
            IList<int> numbers = new List<int>();
            numbers.Add(1);
            numbers.Add(2);
            numbers.Add(3);
            numbers.Add(4);
            numbers.Add(5);
            numbers.Add(6);
            numbers.Add(7);
            numbers.Add(8);
            numbers.Add(9);
            numbers.Add(10);

            string result = formatter.FormatCollection(numbers);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Ensures that an empty string won't fail.
        /// </summary>
        [Test]
        public void ThatEmptyCollectionDoesntFail()
        {
            string expectedResult = "No scripts found.";
            IScriptMessageFormatter formatter = new ScriptMessageFormatter();
            IList<int> numbers = new List<int>();

            string result = formatter.FormatCollection(numbers);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Ensures that a null collection doesn't fail but returns the expected string.
        /// </summary>
        [Test]
        public void ThatNullIntCollectionReturnsCorrectString()
        {
            string expectedResult = "No scripts found.";
            IScriptMessageFormatter formatter = new ScriptMessageFormatter();

            string result = formatter.FormatCollection(new Collection<int>());

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Ensures that a null collection doesn't fail but returns the expected string.
        /// </summary>
        [Test]
        public void ThatNullDecimalCollectionReturnsCorrectString()
        {
            string expectedResult = "No scripts found.";
            IScriptMessageFormatter formatter = new ScriptMessageFormatter();

            string result = formatter.FormatCollection(new Collection<decimal>());

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Ensures that skipped numbers return the right values
        /// </summary>
        [Test]
        public void ThatSkippedNumbersWorkCorrectly()
        {
            string expectedResult = "1, 3, 5, 9 to 12, 20, 21, 30";
            IScriptMessageFormatter formatter = new ScriptMessageFormatter();
            IList<int> numbers = new List<int>();
            numbers.Add(1);
            numbers.Add(3);
            numbers.Add(5);
            numbers.Add(9);
            numbers.Add(10);
            numbers.Add(11);
            numbers.Add(12);
            numbers.Add(20);
            numbers.Add(21);
            numbers.Add(30);

            string result = formatter.FormatCollection(numbers);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Ensures that a single skipped number works correctly.
        /// </summary>
        [Test]
        public void ThatSingleSkippedNumberWorksCorrectly()
        {
            string expectedResult = "1 to 4, 6 to 11";
            IScriptMessageFormatter formatter = new ScriptMessageFormatter();
            IList<int> numbers = new List<int>();
            numbers.Add(1);
            numbers.Add(2);
            numbers.Add(3);
            numbers.Add(4);
            numbers.Add(6);
            numbers.Add(7);
            numbers.Add(8);
            numbers.Add(9);
            numbers.Add(10);
            numbers.Add(11);

            string result = formatter.FormatCollection(numbers);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Ensures that unordered numbers are sorted correctly.
        /// </summary>
        [Test]
        public void ThatUnorderedNumbersAreSorted()
        {
            string expectedResult = "1, 3, 5, 9 to 12, 20, 21, 30";
            IScriptMessageFormatter formatter = new ScriptMessageFormatter();
            IList<int> numbers = new List<int>();
            numbers.Add(3);
            numbers.Add(11);
            numbers.Add(5);
            numbers.Add(20);
            numbers.Add(9);
            numbers.Add(30);
            numbers.Add(1);
            numbers.Add(10);
            numbers.Add(12);
            numbers.Add(21);

            string result = formatter.FormatCollection(numbers);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}