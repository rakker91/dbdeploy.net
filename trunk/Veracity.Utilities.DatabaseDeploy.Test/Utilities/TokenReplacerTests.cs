// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TokenReplacerTests.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Test.Utilities
{
    using System;
    using System.Globalization;

    using DatabaseDeploy.Core.Configuration;
    using DatabaseDeploy.Core.ScriptGeneration;
    using DatabaseDeploy.Core.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Tests the Token Replacer.
    /// </summary>
    [TestClass]
    public class TokenReplacerTests : TestFixtureBase
    {
        /// <summary>
        ///     Ensures that datetime replacement works
        /// </summary>
        [TestMethod]
        public void ThatCurrentDateTimeReplaces()
        {
            DateTime dateTime = new DateTime(2014, 09, 17, 17, 42, 55);
            TimeProvider.Current = new MockTimeProvider(dateTime);
            ITokenReplacer tp = new TokenReplacer(new ConfigurationService());
            string token = TokenEnum.CurrentDateTimeToken;

            string result = tp.Replace(token);

            Assert.AreEqual(result, dateTime.ToString("g"));
        }

        /// <summary>
        ///     Ensures thatcurrent user replacement works
        /// </summary>
        [TestMethod]
        public void ThatCurrentUserReplaces()
        {
            ITokenReplacer tp = new TokenReplacer(new ConfigurationService());
            string token = TokenEnum.CurrentUserToken;

            string result = tp.Replace(token);

            Assert.AreEqual(result, EnvironmentProvider.Current.UserName);
        }

        /// <summary>
        ///     Ensures that current version doesn't fail when no version is set
        /// </summary>
        [TestMethod]
        public void ThatCurrentVersionDoesntFailWithoutVersion()
        {
            ITokenReplacer tp = new TokenReplacer(new ConfigurationService());
            string token = TokenEnum.CurrentVersionToken;

            string result = tp.Replace(token);

            Assert.AreEqual(result, 0.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Ensures that current version replacement works.
        /// </summary>
        [TestMethod]
        public void ThatCurrentVersionReplaces()
        {
            ITokenReplacer tp = new TokenReplacer(new ConfigurationService());
            tp.CurrentVersion = 500;
            string token = TokenEnum.CurrentVersionToken;

            string result = tp.Replace(token);

            Assert.AreEqual(result, 500.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Ensures that script description replaces
        /// </summary>
        [TestMethod]
        public void ThatScriptDescriptionReplaces()
        {
            ITokenReplacer tp = new TokenReplacer(new ConfigurationService());
            tp.Script = new ScriptFile { Id = 1, Description = "1", FileName = "1.sql" };

            string token = TokenEnum.ScriptDescriptionToken;

            string result = tp.Replace(token);

            Assert.AreEqual(result, "1");
        }

        /// <summary>
        ///     Ensures that script description works as expected.
        /// </summary>
        [TestMethod]
        public void ThatScriptDescriptionReplacesWithoutScript()
        {
            ITokenReplacer tp = new TokenReplacer(new ConfigurationService());
            string token = TokenEnum.ScriptDescriptionToken;

            string result = tp.Replace(token);

            Assert.AreEqual(result, string.Empty);
        }

        /// <summary>
        ///     Ensures that script ID replaces.
        /// </summary>
        [TestMethod]
        public void ThatScriptIdReplaces()
        {
            ITokenReplacer tp = new TokenReplacer(new ConfigurationService());
            tp.Script = new ScriptFile { Id = 1, Description = "1", FileName = "1.sql" };
            string token = TokenEnum.ScriptIdToken;

            string result = tp.Replace(token);

            Assert.AreEqual(result, 1.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Ensures that script id repalces without a script
        /// </summary>
        [TestMethod]
        public void ThatScriptIdReplacesWithoutScript()
        {
            ITokenReplacer tp = new TokenReplacer(new ConfigurationService());
            string token = TokenEnum.ScriptIdToken;

            string result = tp.Replace(token);

            Assert.AreEqual(result, 0.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Ensures that script naem replaces.
        /// </summary>
        [TestMethod]
        public void ThatScriptNameReplaces()
        {
            ITokenReplacer tp = new TokenReplacer(new ConfigurationService());

            tp.Script = new ScriptFile { Id = 1, Description = "1", FileName = "1.sql" };

            string token = TokenEnum.ScriptNameToken;

            string result = tp.Replace(token);

            Assert.AreEqual(result, "1.sql");
        }

        /// <summary>
        ///     Ensures that script name replaces without a script.
        /// </summary>
        [TestMethod]
        public void ThatScriptNameReplacesWithoutScript()
        {
            ITokenReplacer tp = new TokenReplacer(new ConfigurationService());

            string token = TokenEnum.ScriptNameToken;

            string result = tp.Replace(token);

            Assert.AreEqual(result, string.Empty);
        }
    }
}