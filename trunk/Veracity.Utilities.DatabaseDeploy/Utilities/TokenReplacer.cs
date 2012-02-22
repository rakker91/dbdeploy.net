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

    using Veracity.Utilities.DatabaseDeploy.Configuration;

    using log4net;

    using Veracity.Utilities.DatabaseDeploy.ScriptGeneration;

    /// <summary>
    /// Replaces Tokens in strings.
    /// </summary>
    public class TokenReplacer : ITokenReplacer
    {
        #region Constants and Fields

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(TokenReplacer));

        /// <summary>
        /// The configuration service to use.
        /// </summary>
        private IConfigurationService configurationService;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the TokenReplacer class
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
        /// Initializes a new instance of the TokenReplacer class
        /// </summary>
        /// <param name="configurationService">The configuration service to use for this instance</param>
        public TokenReplacer(IConfigurationService configurationService) : this()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(configurationService));
            }

            this.configurationService = configurationService;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the current version.
        /// </summary>
        public int CurrentVersion { get; set; }

        /// <summary>
        ///   Gets or sets the current script being worked on.
        /// </summary>
        public IScriptFile Script { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs a replacement on the string that is passed in. This assumes that any properties, such as current version, that are needed have been set.
        /// </summary>
        /// <param name="stringToParse">
        /// The string to parse. 
        /// </param>
        /// <returns>
        /// A string that has been fully replaced. 
        /// </returns>
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
            result = result.ReplaceEx(TokenEnum.SchemaToken, this.configurationService.Schema);
            result = result.ReplaceEx(TokenEnum.ChangeLogToken, this.configurationService.ChangeLog);

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult(result));
            }

            return result;
        }

        #endregion
    }
}