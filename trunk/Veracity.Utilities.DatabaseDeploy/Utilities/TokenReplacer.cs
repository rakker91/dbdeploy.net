// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenReplacer.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Utilities
{
    using System;
    using System.Globalization;

    using Veracity.Utilities.DatabaseDeploy.ScriptGeneration;

    using log4net;

    /// <summary>
    /// Replaces Tokens in strings.
    /// </summary>
    public class TokenReplacer : ITokenReplacer
    {
        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(TokenReplacer));

        /// <summary>
        /// Initializes a new instance of the TokenReplacer class
        /// </summary>
        public TokenReplacer()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext());
            }

            this.Script = new ScriptFile();
        }

        /// <summary>
        /// Gets or sets the current version.
        /// </summary>
        public int CurrentVersion { get; set; }

        /// <summary>
        /// Gets or sets the current script being worked on.
        /// </summary>
        public IScriptFile Script { get; set; }

        /// <summary>
        /// Performs a replacement on the string that is passed in.  This assumes that any properties, such as current version, that are needed have been set.
        /// </summary>
        /// <param name="stringToParse">The string to parse.</param>
        /// <returns>A string that has been fully replaced.</returns>
        public string Replace(string stringToParse)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(stringToParse));
            }

            // There are far more elegant ways to do this (tokenization and such), but brute force will work fine here.
            string result = stringToParse;

            result = result.ReplaceEx(TokenEnum.CurrentDateTimeToken, DateTime.Now.ToString("g"));
            result = result.ReplaceEx(TokenEnum.CurrentUserToken, Environment.UserName);
            result = result.ReplaceEx(TokenEnum.CurrentVersionToken, this.CurrentVersion.ToString(CultureInfo.InvariantCulture));
            result = result.ReplaceEx(TokenEnum.ScriptIdToken, this.Script.Id.ToString(CultureInfo.InvariantCulture));
            result = result.ReplaceEx(TokenEnum.ScriptNameToken, this.Script.FileName);
            result = result.ReplaceEx(TokenEnum.ScriptDescriptionToken, this.Script.Description);

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult(result));
            }

            return result;
        }
    }
}