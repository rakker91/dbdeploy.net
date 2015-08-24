// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScriptMessageFormatterTests.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Test.Utilities
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using DatabaseDeploy.Core.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Tests the script message formatter
    /// </summary>
    [TestClass]
    public class ScriptMessageFormatterTests : TestFixtureBase
    {
        /// <summary>
        ///     Ensures that an empty string won't fail.
        /// </summary>
        [TestMethod]
        public void ThatEmptyCollectionDoesntFail()
        {
            string expectedResult = "No scripts found.";
            IScriptMessageFormatter formatter = new ScriptMessageFormatter();
            IList<int> numbers = new List<int>();

            string result = formatter.FormatCollection(numbers);

            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        ///     Ensures that format collection returns expected string
        /// </summary>
        [TestMethod]
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

            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        ///     Ensures that a null collection doesn't fail but returns the expected string.
        /// </summary>
        [TestMethod]
        public void ThatNullDecimalCollectionReturnsCorrectString()
        {
            string expectedResult = "No scripts found.";
            IScriptMessageFormatter formatter = new ScriptMessageFormatter();

            string result = formatter.FormatCollection(new Collection<decimal>());

            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        ///     Ensures that a null collection doesn't fail but returns the expected string.
        /// </summary>
        [TestMethod]
        public void ThatNullIntCollectionReturnsCorrectString()
        {
            string expectedResult = "No scripts found.";
            IScriptMessageFormatter formatter = new ScriptMessageFormatter();

            string result = formatter.FormatCollection(new Collection<int>());

            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        ///     Ensures that a single skipped number works correctly.
        /// </summary>
        [TestMethod]
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

            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        ///     Ensures that skipped numbers return the right values
        /// </summary>
        [TestMethod]
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

            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        ///     Ensures that unordered numbers are sorted correctly.
        /// </summary>
        [TestMethod]
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

            Assert.AreEqual(result, expectedResult);
        }
    }
}