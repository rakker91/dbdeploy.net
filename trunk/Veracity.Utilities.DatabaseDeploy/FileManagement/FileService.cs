// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileService.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.FileManagement
{
    #region Usings

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;

    using log4net;

    using Veracity.Utilities.DatabaseDeploy.Configuration;
    using Veracity.Utilities.DatabaseDeploy.ScriptGeneration;
    using Veracity.Utilities.DatabaseDeploy.Utilities;

    #endregion

    /// <summary>
    /// Manages file intractions for the generation
    /// </summary>
    public class FileService : IFileService
    {
        #region Constants and Fields

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(FileService));

        /// <summary>
        ///   The configuration service to use for this file manager
        /// </summary>
        private readonly IConfigurationService configurationService;

        /// <summary>
        /// A proxy for certain IO functions to make this class more testable.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private readonly IIoProxy ioProxy;

        /// <summary>
        ///   Caches file contents, upon request to speed up retrieval of the contents.
        /// </summary>
        private Dictionary<string, string> fileCache = new Dictionary<string, string>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileService"/> class.
        /// </summary>
        /// <param name="configurationService">
        /// The configuration service. 
        /// </param>
        /// <param name="ioProxy">Represents a proxy for certain file system functions</param>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public FileService(IConfigurationService configurationService, IIoProxy ioProxy)
        {
            log.DebugIfEnabled(LogUtility.GetContext(configurationService, ioProxy));

            this.configurationService = configurationService;
            this.ioProxy = ioProxy;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Removes previous files and leftovers to ensure a clean run.
        /// </summary>
        public void CleanupPastRuns()
        {
            log.DebugIfEnabled(LogUtility.GetContext());

            var output = new FileInfo(this.configurationService.OutputFile);
            if (output.Exists)
            {
                output.Delete();
            }

            var undo = new FileInfo(this.configurationService.UndoOutputFile);
            if (undo.Exists)
            {
                undo.Delete();
            }

            var scriptList = new FileInfo(this.configurationService.ScriptListFile);
            if (scriptList.Exists)
            {
                scriptList.Delete();
            }

            log.DebugIfEnabled(LogUtility.GetResult());
        }

        /// <summary>
        /// Reads the contents of a file from disc
        /// </summary>
        /// <param name="fileName">
        /// The name of the file to read. 
        /// </param>
        /// <param name="useCache">
        /// Indicates whether or not the cache should be used for getting file contents. 
        /// </param>
        /// <returns>
        /// A string containing the contents of the file. 
        /// </returns>
        public string GetFileContents(string fileName, bool useCache)
        {
            log.DebugIfEnabled(LogUtility.GetContext(fileName, useCache));

            string result;

            if (useCache && this.fileCache.ContainsKey(fileName))
            {
                result = this.fileCache[fileName];
            }
            else
            {
                if (this.ioProxy.Exists(fileName))
                {
                    try
                    {
                        result = File.ReadAllText(fileName);

                        if (useCache)
                        {
                            this.fileCache.Add(fileName, result);
                        }
                    }
                    catch (IOException ex)
                    {
                        log.Error(ex);
                        throw;
                    }
                }
                else
                {
                    string message = string.Format("The file \"{0}\" does not exist.", fileName);
                    log.Error(message);
                    throw new FileNotFoundException(message, fileName);
                }
            }

            log.DebugIfEnabled(LogUtility.GetResult(result));

            return result;
        }

        /// <summary>
        /// Gets a list of script files from disk
        /// </summary>
        /// <returns>
        /// A dictionary of script files that are on disc. 
        /// </returns>
        public IDictionary<int, IScriptFile> GetScriptFiles()
        {
            log.DebugIfEnabled(LogUtility.GetContext());

            IDictionary<int, IScriptFile> result = new ConcurrentDictionary<int, IScriptFile>();

            string[] files = this.ioProxy.GetFiles(this.configurationService.RootDirectory, this.configurationService.SearchPattern, this.configurationService.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            ScriptFile.FileNamePattern = this.configurationService.FileNamePattern;
            foreach (string file in files)
            {
                IScriptFile scriptFile = new ScriptFile();
                scriptFile.Parse(this, file);

                if (result.ContainsKey(scriptFile.Id))
                {
                    string message = string.Format("File {0} is has a duplicate script id ({1}) with file {2}", scriptFile.FileName, scriptFile.Id, result[scriptFile.Id].FileName);
                    log.Error(message);
                    throw new Exception(message);
                }

                result.Add(scriptFile.Id, scriptFile);
            }

            log.DebugIfEnabled(LogUtility.GetResult(result));

            return result;
        }

        /// <summary>
        /// Writes the change script file to disk
        /// </summary>
        /// <param name="changeScript">
        /// The change script contents 
        /// </param>
        public void WriteChangeScript(string changeScript)
        {
            log.DebugIfEnabled(LogUtility.GetContext(changeScript));

            this.WriteStringToFile(this.configurationService.OutputFile, changeScript);

            log.DebugIfEnabled(LogUtility.GetResult());
        }

        /// <summary>
        /// Writes the list of scripts found to a text file in the order in which they were found.
        /// </summary>
        /// <param name="scripts">
        /// The scripts that were found. 
        /// </param>
        public void WriteScriptList(IDictionary<int, IScriptFile> scripts)
        {
            log.DebugIfEnabled(LogUtility.GetContext(scripts));

            StringBuilder textList = new StringBuilder();
            string root = this.configurationService.RootDirectory;

            int[] sortedKeys = scripts.Keys.OrderBy(k => k).ToArray();

            textList.AppendLine("This file is autogenerated and will be replaced on each run of DatabaseDeploy.  You should not change the contents.");
            textList.AppendLine("This file reflects all of the scripts that were found in this directory and if recursion is on, child directories.  These are the scripts that will be included in the deployment script.");
            textList.AppendLine("The order in which the scripts will be applied is the order in which they appear in this file.");
            textList.AppendLine("----------------------------------------------------------------------------------------------");

            foreach (int key in sortedKeys)
            {
                textList.AppendLine(scripts[key].FileInfo.FullName.Replace(root, string.Empty));
            }

            this.WriteStringToFile(this.configurationService.ScriptListFile, textList.ToString());

            log.DebugIfEnabled(LogUtility.GetResult());
        }

        /// <summary>
        /// Writes the undo script file to disk
        /// </summary>
        /// <param name="undoScript">
        /// The undo disk 
        /// </param>
        public void WriteUndoScript(string undoScript)
        {
            log.DebugIfEnabled(LogUtility.GetContext(undoScript));

            this.WriteStringToFile(this.configurationService.UndoOutputFile, undoScript);

            log.DebugIfEnabled(LogUtility.GetResult());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Writes the contents of a string to the specified filename.
        /// </summary>
        /// <param name="fileName">
        /// The filename to which the content should be written 
        /// </param>
        /// <param name="content">
        /// The content to write 
        /// </param>
        private void WriteStringToFile(string fileName, string content)
        {
            log.DebugIfEnabled(LogUtility.GetContext(fileName, content));

            using (StreamWriter writer = this.ioProxy.GetStreamWriter(fileName))
            {
                writer.Write(content);
            }

            log.DebugIfEnabled(LogUtility.GetResult());
        }

        #endregion


        /// <summary>
        /// Gets the lines from a file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string[] GetLinesFromFile(string fileName)
        {
            log.DebugIfEnabled(LogUtility.GetContext(fileName));

            string[] result;

            if (this.ioProxy.Exists(fileName))
            {
                try
                {
                    result = ioProxy.ReadAllLines(fileName);
                }
                catch (IOException ex)
                {
                    log.Error(ex);
                    throw;
                }
            }
            else
            {
                string message = string.Format("The file \"{0}\" does not exist.", fileName);
                log.Error(message);
                throw new FileNotFoundException(message, fileName);
            }

            log.DebugIfEnabled(LogUtility.GetResult(result));

            return result;
        }
    }
}