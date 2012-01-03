// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenReplacerTests.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test.Utilities
{
    using System;
    using System.Globalization;

    using NUnit.Framework;

    using Veracity.Utilities.DatabaseDeploy.ScriptGeneration;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    /// <summary>
    /// Tests the Token Replacer.
    /// </summary>
    [TestFixture]
    public class TokenReplacerTests : TestFixtureBase
    {
        /// <summary>
        /// Ensures that datetime replacement works
        /// </summary>
        [Test]
        public void ThatCurrentDateTimeReplaces()
        {
            ITokenReplacer tp = new TokenReplacer();
            string token = TokenEnum.CurrentDateTimeToken;

            string result = tp.Replace(token);

            Assert.That(result, Is.EqualTo(DateTime.Now.ToString("g")));
        }

        /// <summary>
        /// Ensures thatcurrent user replacement works
        /// </summary>
        [Test]
        public void ThatCurrentUserReplaces()
        {
            ITokenReplacer tp = new TokenReplacer();
            string token = TokenEnum.CurrentUserToken;

            string result = tp.Replace(token);

            Assert.That(result, Is.EqualTo(Environment.UserName));
        }

        /// <summary>
        /// Ensures that current version replacement works.
        /// </summary>
        [Test]
        public void ThatCurrentVersionReplaces()
        {
            ITokenReplacer tp = new TokenReplacer();
            tp.CurrentVersion = 500;
            string token = TokenEnum.CurrentVersionToken;

            string result = tp.Replace(token);

            Assert.That(result, Is.EqualTo(500.ToString(CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Ensures that current version doesn't fail when no version is set
        /// </summary>
        [Test]
        public void ThatCurrentVersionDoesntFailWithoutVersion()
        {
            ITokenReplacer tp = new TokenReplacer();
            string token = TokenEnum.CurrentVersionToken;

            string result = tp.Replace(token);

            Assert.That(result, Is.EqualTo(0.ToString(CultureInfo.InvariantCulture)));
        }
        
        /// <summary>
        /// Ensures that script ID replaces.
        /// </summary>
        [Test]
        public void ThatScriptIdReplaces()
        {
            ITokenReplacer tp = new TokenReplacer();
            tp.Script = new ScriptFile() { Id = 1, Description = "1", FileName = "1.sql" };
            string token = TokenEnum.ScriptIdToken;

            string result = tp.Replace(token);

            Assert.That(result, Is.EqualTo(1.ToString(CultureInfo.InvariantCulture)));

        }

        /// <summary>
        /// Ensures that script id repalces without a script
        /// </summary>
        [Test]
        public void ThatScriptIdReplacesWithoutScript()
        {
            ITokenReplacer tp = new TokenReplacer();
            string token = TokenEnum.ScriptIdToken;

            string result = tp.Replace(token);

            Assert.That(result, Is.EqualTo(0.ToString(CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Ensures that script naem replaces.
        /// </summary>
        [Test]
        public void ThatScriptNameReplaces()
        {
            ITokenReplacer tp = new TokenReplacer();

            tp.Script = new ScriptFile() { Id = 1, Description = "1", FileName = "1.sql" };
            
            string token = TokenEnum.ScriptNameToken;

            string result = tp.Replace(token);

            Assert.That(result, Is.EqualTo("1.sql"));
        }

        /// <summary>
        /// Ensures that script name replaces without a script.
        /// </summary>
        [Test]
        public void ThatScriptNameReplacesWithoutScript()
        {
            ITokenReplacer tp = new TokenReplacer();

            string token = TokenEnum.ScriptNameToken;

            string result = tp.Replace(token);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Ensures that script description replaces
        /// </summary>
        [Test]
        public void ThatScriptDescriptionReplaces()
        {
            ITokenReplacer tp = new TokenReplacer();
            tp.Script = new ScriptFile() { Id = 1, Description = "1", FileName = "1.sql" };

            string token = TokenEnum.ScriptDescriptionToken;

            string result = tp.Replace(token);

            Assert.That(result, Is.EqualTo("1"));
        }

        /// <summary>
        /// Ensures that script description works as expected.
        /// </summary>
        [Test]
        public void ThatScriptDescriptionReplacesWithoutScript()
        {
            ITokenReplacer tp = new TokenReplacer();
            string token = TokenEnum.ScriptDescriptionToken;

            string result = tp.Replace(token);

            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }
}