// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptService.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.ScriptGeneration
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using log4net;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances;
    using Veracity.Utilities.DatabaseDeploy.FileManagement;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    #endregion

    /// <summary>
    /// Used to generate scripts for dbdeploy
    /// </summary>
    public class ScriptService : IScriptService
    {
        #region Constants and Fields

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(ScriptService));

        /// <summary>
        ///   The configuration service to use for processing.
        /// </summary>
        private readonly IConfigurationService configurationService;

        /// <summary>
        ///   The database service to use for generating scripts
        /// </summary>
        private readonly IDatabaseService databaseService;

        /// <summary>
        ///   The file service to use for file stuff.
        /// </summary>
        private readonly IFileService fileService;

        /// <summary>
        ///   The token replacer to use for processing.
        /// </summary>
        private readonly ITokenReplacer tokenReplacer;

        /// <summary>
        ///   Reprents the token to use for undo files.
        /// </summary>
        private string undoToken = string.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ScriptService" /> class.
        /// </summary>
        public ScriptService()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptService"/> class.
        /// </summary>
        /// <param name="databaseService">
        /// The database service. 
        /// </param>
        /// <param name="fileService">
        /// the File service to use for file requests. 
        /// </param>
        /// <param name="tokenReplacer">
        /// The token replacer to use for script processing. 
        /// </param>
        /// <param name="configurationService">
        /// The configuration service to use for processing. 
        /// </param>
        public ScriptService(IDatabaseService databaseService, IFileService fileService, ITokenReplacer tokenReplacer, IConfigurationService configurationService)
        {
            log.DebugIfEnabled(LogUtility.GetContext(databaseService, fileService, tokenReplacer, configurationService));

            this.databaseService = databaseService;
            this.fileService = fileService;
            this.tokenReplacer = tokenReplacer;
            this.configurationService = configurationService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The build change script.
        /// </summary>
        /// <param name="changes">
        /// The changes. 
        /// </param>
        /// <returns>
        /// A string containing the contents of the provided change files 
        /// </returns>
        public string BuildChangeScript(IDictionary<decimal, IScriptFile> changes)
        {
            log.DebugIfEnabled(LogUtility.GetContext(changes));

            StringBuilder changeScript = new StringBuilder();

            this.undoToken = this.databaseService.GetScriptFromFile(DatabaseScriptEnum.UndoToken);

            this.tokenReplacer.CurrentVersion = changes.Keys.Min() - 1;

            this.AppendScript(DatabaseScriptEnum.ChangeScriptHeader, changeScript);

            decimal[] sortedKeys = changes.Keys.OrderBy(k => k).ToArray();

            foreach (decimal key in sortedKeys)
            {
                var scriptFile = changes[key];
                this.tokenReplacer.Script = scriptFile;
                this.AppendScript(DatabaseScriptEnum.ScriptHeader, changeScript);
                this.AppendScriptBody(changeScript, scriptFile.Contents, false);
                this.AppendScript(DatabaseScriptEnum.ScriptFooter, changeScript);
            }

            this.AppendScript(DatabaseScriptEnum.ChangeScriptFooter, changeScript);

            log.DebugIfEnabled(LogUtility.GetResult(changeScript));

            return changeScript.ToString();
        }

        /// <summary>
        /// The build undo script.
        /// </summary>
        /// <param name="changes">
        /// The changes. 
        /// </param>
        /// <returns>
        /// A string containing the contents of the undo portion of the scripts. 
        /// </returns>
        public string BuildUndoScript(IDictionary<decimal, IScriptFile> changes)
        {
            log.DebugIfEnabled(LogUtility.GetContext(changes));

            StringBuilder undoScript = new StringBuilder();

            this.undoToken = this.databaseService.GetScriptFromFile(DatabaseScriptEnum.UndoToken);

            this.tokenReplacer.CurrentVersion = changes.Keys.Max();

            this.AppendScript(DatabaseScriptEnum.UndoScriptHeader, undoScript);

            decimal[] sortedKeys = changes.Keys.OrderByDescending(k => k).ToArray();

            foreach (decimal key in sortedKeys)
            {
                string scriptContents = this.fileService.GetFileContents(changes[key].FileName, false);

                if (scriptContents.Contains(this.undoToken))
                {
                    this.tokenReplacer.Script = changes[key];
                    this.AppendScript(DatabaseScriptEnum.UndoHeader, undoScript);
                    this.AppendScriptBody(undoScript, scriptContents, true);
                    this.AppendScript(DatabaseScriptEnum.UndoFooter, undoScript);
                }
            }

            this.AppendScript(DatabaseScriptEnum.UndoScriptFooter, undoScript);

            log.DebugIfEnabled(LogUtility.GetResult(undoScript));

            return undoScript.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Appends a script to the contents of the current building script.
        /// </summary>
        /// <param name="scriptToUse">
        /// The script to use for the append. 
        /// </param>
        /// <param name="changeScript">
        /// The overall change script 
        /// </param>
        private void AppendScript(string scriptToUse, StringBuilder changeScript)
        {
            log.DebugIfEnabled(LogUtility.GetContext(scriptToUse, changeScript));

            string scriptContents = this.databaseService.GetScriptFromFile(scriptToUse);

            scriptContents = this.tokenReplacer.Replace(scriptContents);

            changeScript.AppendLine(scriptContents);

            log.DebugIfEnabled(LogUtility.GetResult());
        }

        /// <summary>
        /// Appends a script to the output file
        /// </summary>
        /// <param name="changeScript">
        /// The overall change script 
        /// </param>
        /// <param name="scriptContents">
        /// The contents of the script file. 
        /// </param>
        /// <param name="undo">
        /// True if an undo script is being generated, false otherwise. 
        /// </param>
        private void AppendScriptBody(StringBuilder changeScript, string scriptContents, bool undo)
        {
            log.DebugIfEnabled(LogUtility.GetContext(changeScript, scriptContents, undo));

            string scriptPortion;
            string undoPortion = string.Empty;

            int tokenLocation = scriptContents.IndexOf(this.undoToken, StringComparison.Ordinal);

            if (tokenLocation == -1)
            {
                scriptPortion = scriptContents;
            }
            else
            {
                scriptPortion = scriptContents.Substring(0, tokenLocation);
                undoPortion = scriptContents.Substring(tokenLocation + this.undoToken.Length);
            }

            if (undo)
            {
                changeScript.AppendLine(undoPortion);
            }
            else
            {
                changeScript.AppendLine(scriptPortion);
            }
        }

        #endregion
    }
}