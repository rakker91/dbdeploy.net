// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeploymentService.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy
{
    #region Usings

    using System.Collections.Generic;
    using System.Linq;

    using log4net;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.Database;
    using Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances;
    using Veracity.Utilities.DatabaseDeploy.FileManagement;
    using Veracity.Utilities.DatabaseDeploy.ScriptGeneration;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    #endregion

    /// <summary>
    /// Manages the overall database deployment.
    /// </summary>
    public class DeploymentService : IDeploymentService
    {
        #region Constants and Fields

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(DeploymentService));

        /// <summary>
        ///   The configuration service to use for the build
        /// </summary>
        private readonly IConfigurationService configurationService;

        /// <summary>
        ///   The database service to use for building
        /// </summary>
        private readonly IDatabaseService databaseService;

        /// <summary>
        ///   The file service to use for the build
        /// </summary>
        private readonly IFileService fileService;

        /// <summary>
        ///   The formatter for formatting script concats
        /// </summary>
        private readonly IScriptMessageFormatter scriptMessageFormatter;

        /// <summary>
        ///   The script service to use for the build
        /// </summary>
        private readonly IScriptService scriptService;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeploymentService"/> class.
        /// </summary>
        /// <param name="databaseService">
        /// The database service to use for database calls 
        /// </param>
        /// <param name="configurationService">
        /// The configuration Service. 
        /// </param>
        /// <param name="scriptService">
        /// The script Service. 
        /// </param>
        /// <param name="fileService">
        /// The file Service. 
        /// </param>
        /// <param name="scriptMessageFormatter">
        /// The script formatter for messages 
        /// </param>
        public DeploymentService(IDatabaseService databaseService, IConfigurationService configurationService, IScriptService scriptService, IFileService fileService, IScriptMessageFormatter scriptMessageFormatter)
        {
            log.DebugIfEnabled(LogUtility.GetContext(databaseService, configurationService, scriptService, fileService, scriptMessageFormatter));

            this.databaseService = databaseService;
            this.configurationService = configurationService;
            this.scriptService = scriptService;
            this.fileService = fileService;
            this.scriptMessageFormatter = scriptMessageFormatter;

            log.DebugIfEnabled(LogUtility.GetResult());
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the current configuration service that is being used.
        /// </summary>
        public IConfigurationService ConfigurationService
        {
            get
            {
                return this.configurationService;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Builds the deployment scripts based on the information that is known at the time.
        /// </summary>
        public void BuildDeploymentScript()
        {
            log.DebugIfEnabled(LogUtility.GetContext());

            log.InfoIfEnabled("Starting dbdeploy run with settings:\n{0}", this.configurationService);

            log.InfoIfEnabled("Cleaning up past runs.");

            this.fileService.CleanupPastRuns();

            log.InfoIfEnabled("Getting available script files.");

            IDictionary<decimal, IScriptFile> scripts = this.fileService.GetScriptFiles();

            if (scripts.Any())
            {
                log.InfoIfEnabled("Writing found file list into {0}", this.configurationService.ScriptListFile);
                this.fileService.WriteScriptList(scripts);
                log.InfoIfEnabled("Found scripts {0}.", this.scriptMessageFormatter.FormatCollection(scripts.Keys));

                log.InfoIfEnabled("Getting applied changes.");
                IDictionary<decimal, IChangeLog> changes = this.databaseService.GetAppliedChanges();
                log.InfoIfEnabled("Found scripts {0}.", this.scriptMessageFormatter.FormatCollection(changes.Keys));

                log.InfoIfEnabled("Getting scripts to apply.");
                IDictionary<decimal, IScriptFile> scriptsToApply = this.GetScriptsToApply(scripts, changes);

                if (scriptsToApply.Any())
                {
                    log.InfoIfEnabled("Scripts {0} need to be applied.",
                        this.scriptMessageFormatter.FormatCollection(scriptsToApply.Keys));

                    log.InfoIfEnabled("Building change script.");
                    string changeScript = this.scriptService.BuildChangeScript(scriptsToApply);

                    log.InfoIfEnabled("Building undo script for scripts {0}.",
                        this.scriptMessageFormatter.FormatCollection(changes.Keys));

                    string undoScript = this.scriptService.BuildUndoScript(scriptsToApply);
                    log.InfoIfEnabled("Writing change script to {0}", this.configurationService.OutputFile);

                    this.fileService.WriteChangeScript(changeScript);

                    log.InfoIfEnabled("Writing undo change script to {0}", this.configurationService.UndoOutputFile);

                    this.fileService.WriteUndoScript(undoScript);
                }
                else
                {
                    log.InfoIfEnabled("No changes need to be applied.  Skipping script generation.");
                }
            }
            else
            {
                log.InfoIfEnabled("No scripts found at {0}.  Skipping script generation.", this.configurationService.RootDirectory);
            }

            log.InfoIfEnabled("Deployment generation complete.");

            log.DebugIfEnabled(LogUtility.GetResult());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compares the applied scripts to the available scripts and determines which scripts to apply.
        /// </summary>
        /// <param name="availableScripts">
        /// The scripts that are available to be applied to the database.
        /// </param>
        /// <param name="appliedChanges">
        /// The scripts that have already been applied to the database
        /// </param>
        /// <returns>
        /// A dictionary containing the scripts that need to be applied to the current database instance 
        /// </returns>
        private IDictionary<decimal, IScriptFile> GetScriptsToApply(IDictionary<decimal, IScriptFile> availableScripts, IDictionary<decimal, IChangeLog> appliedChanges)
        {
            log.DebugIfEnabled(LogUtility.GetContext(availableScripts, appliedChanges));

            IDictionary<decimal, IScriptFile> scriptsToApply = new Dictionary<decimal, IScriptFile>();
            decimal[] sortedKeys = availableScripts.Keys.OrderBy(k => k).ToArray();

            foreach (decimal key in sortedKeys.Where(key => !appliedChanges.ContainsKey(key) && !scriptsToApply.ContainsKey(key)))
            {
                if (key > this.configurationService.LastChangeToApply)
                {
                    log.InfoIfEnabled("LastChangeToApply == {0}.  Skipping remaining scripts.", this.configurationService.LastChangeToApply);

                    break;
                }

                scriptsToApply.Add(key, availableScripts[key]);
            }

            log.DebugIfEnabled(LogUtility.GetResult(scriptsToApply));

            return scriptsToApply;
        }

        #endregion
    }
}