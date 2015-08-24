// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TokenReplacer.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Utilities
{
    using System.Globalization;

    using DatabaseDeploy.Core.Configuration;
    using DatabaseDeploy.Core.ScriptGeneration;

    /// <summary>
    ///     Replaces Tokens in strings.
    /// </summary>
    public class TokenReplacer : ITokenReplacer
    {
        /// <summary>
        ///     The configuration service to use.
        /// </summary>
        private readonly IConfigurationService configurationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenReplacer"/> class.
        /// </summary>
        public TokenReplacer()
        {
            this.Script = new ScriptFile();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenReplacer"/> class.
        /// </summary>
        /// <param name="configurationService">The configuration service to use for this instance</param>
        public TokenReplacer(IConfigurationService configurationService)
            : this()
        {
            this.configurationService = configurationService;
        }

        /// <summary>
        ///     Gets or sets the current version.
        /// </summary>
        /// <value>The current version.</value>
        public decimal CurrentVersion { get; set; }

        /// <summary>
        ///     Gets or sets the current script being worked on.
        /// </summary>
        /// <value>The script.</value>
        public IScriptFile Script { get; set; }

        /// <summary>
        ///     Performs a replacement on the string that is passed in. This assumes that any properties, such as current version,
        ///     that are needed have been set.
        /// </summary>
        /// <param name="stringToParse">The string to parse.</param>
        /// <returns>A string that has been fully replaced.</returns>
        public string Replace(string stringToParse)
        {
            // There are far more elegant ways to do this (tokenization and such), but brute force will work fine here.
            string result = stringToParse;

            result = result.ReplaceEx(TokenEnum.CurrentDateTimeToken, TimeProvider.Current.Now.ToString("g"));
            result = result.ReplaceEx(TokenEnum.CurrentUserToken, EnvironmentProvider.Current.UserName);
            result = result.ReplaceEx(
                TokenEnum.CurrentVersionToken,
                this.CurrentVersion.ToString(CultureInfo.InvariantCulture));
            result = result.ReplaceEx(TokenEnum.ScriptIdToken, this.Script.Id.ToString(CultureInfo.InvariantCulture));
            result = result.ReplaceEx(TokenEnum.ScriptNameToken, this.Script.FileName);
            result = result.ReplaceEx(TokenEnum.ScriptDescriptionToken, this.Script.Description);
            result = result.ReplaceEx(TokenEnum.SchemaToken, this.configurationService.Schema);
            result = result.ReplaceEx(TokenEnum.ChangeLogToken, this.configurationService.ChangeLog);

            return result;
        }
    }
}