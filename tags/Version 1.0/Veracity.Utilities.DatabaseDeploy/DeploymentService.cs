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
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(databaseService, configurationService, scriptService, fileService, scriptMessageFormatter));
            }

            this.databaseService = databaseService;
            this.configurationService = configurationService;
            this.scriptService = scriptService;
            this.fileService = fileService;
            this.scriptMessageFormatter = scriptMessageFormatter;

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult());
            }
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
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext());
            }

            if (log.IsInfoEnabled)
            {
                log.Info(string.Format("Starting dbdeploy run with settings:\n{0}", this.configurationService));
            }

            if (log.IsInfoEnabled)
            {
                log.Info("Cleaning up past runs.");
            }

            this.fileService.CleanupPastRuns();

            if (log.IsInfoEnabled)
            {
                log.Info("Getting available script files.");
            }

            IDictionary<int, IScriptFile> scripts = this.fileService.GetScriptFiles();

            if (scripts.Any())
            {
                if (log.IsInfoEnabled)
                {
                    log.Info(string.Format("Writing found file list into {0}", this.configurationService.ScriptListFile));
                }

                this.fileService.WriteScriptList(scripts);

                if (log.IsInfoEnabled)
                {
                    log.Info(string.Format("Found scripts {0}.  Getting applied changes.", this.scriptMessageFormatter.FormatCollection(scripts.Keys)));
                }

                IDictionary<int, IChangeLog> changes = this.databaseService.GetAppliedChanges();

                if (log.IsInfoEnabled)
                {
                    log.Info(string.Format("Found scripts {0}.  Getting scripts to apply.", this.scriptMessageFormatter.FormatCollection(changes.Keys)));
                }

                IDictionary<int, IScriptFile> scriptsToApply = this.GetScriptsToApply(scripts, changes);

                if (scriptsToApply.Any())
                {
                    if (log.IsInfoEnabled)
                    {
                        log.Info(string.Format("Scripts {0} need to be applied.  Building change script.", this.scriptMessageFormatter.FormatCollection(scriptsToApply.Keys)));
                    }

                    string changeScript = this.scriptService.BuildChangeScript(scriptsToApply);

                    if (log.IsInfoEnabled)
                    {
                        log.Info(string.Format("Building undo script for scripts {0}.", this.scriptMessageFormatter.FormatCollection(changes.Keys)));
                    }

                    string undoScript = this.scriptService.BuildUndoScript(scriptsToApply);
                    if (log.IsInfoEnabled)
                    {
                        log.Info(string.Format("Writing change script to {0}", this.configurationService.OutputFile));
                    }

                    this.fileService.WriteChangeScript(changeScript);

                    if (log.IsInfoEnabled)
                    {
                        log.Info(string.Format("Writing undo change script to {0}", this.configurationService.UndoOutputFile));
                    }

                    this.fileService.WriteUndoScript(undoScript);
                }
                else
                {
                    if (log.IsInfoEnabled)
                    {
                        log.Info(string.Format("No changes need to be applied.  Skipping script generation."));
                    }
                }
            }
            else
            {
                if (log.IsInfoEnabled)
                {
                    log.Info(string.Format("No scripts found at {0}.  Skipping script generation.", this.configurationService.RootDirectory));
                }                
            }

            if (log.IsInfoEnabled)
            {
                log.Info("Deployment generation complete.");
            }

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult());
            }
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
        private IDictionary<int, IScriptFile> GetScriptsToApply(IDictionary<int, IScriptFile> availableScripts, IDictionary<int, IChangeLog> appliedChanges)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(availableScripts, appliedChanges));
            }

            IDictionary<int, IScriptFile> scriptsToApply = new Dictionary<int, IScriptFile>();
            int[] sortedKeys = availableScripts.Keys.OrderBy(k => k).ToArray();

            foreach (int key in sortedKeys.Where(key => !appliedChanges.ContainsKey(key) && !scriptsToApply.ContainsKey(key)))
            {
                if (key > this.configurationService.LastChangeToApply)
                {
                    if (log.IsInfoEnabled)
                    {
                        log.Info(string.Format("LastChangeToApply == {0}.  Skipping remaining scripts.", this.configurationService.LastChangeToApply));
                    }

                    break;
                }

                scriptsToApply.Add(key, availableScripts[key]);
            }

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult(scriptsToApply));
            }

            return scriptsToApply;
        }

        #endregion
    }
}