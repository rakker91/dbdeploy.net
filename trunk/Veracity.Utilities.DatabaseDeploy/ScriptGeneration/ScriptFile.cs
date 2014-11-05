// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptFile.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;
using Veracity.Utilities.DatabaseDeploy.FileManagement;

namespace Veracity.Utilities.DatabaseDeploy.ScriptGeneration
{
    #region Usings

    using System;
    using System.IO;

    using log4net;

    using Veracity.Utilities.DatabaseDeploy.Utilities;

    #endregion

    /// <summary>
    /// Represents a script in the system.
    /// </summary>
    public class ScriptFile : IScriptFile
    {
        #region Constants and Fields

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(ScriptFile));

        private static Regex regexFileName = new Regex(@"(\d+)(\s+)?(.+)?");
        #endregion

        #region Public Properties

        /// <summary>
        /// Pattern used to parse the file names
        /// </summary>
        public static string FileNamePattern
        {
            get { return regexFileName.ToString(); }
            set { regexFileName = new Regex(value); }
        }

        /// <summary>
        ///   Gets or sets the description for this file.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the contents of the script
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        ///   Gets or sets the file info for this script
        /// </summary>
        public FileInfo FileInfo { get; set; }

        /// <summary>
        ///   Gets or sets the full file name for this script
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///   Gets or sets the script Id. These must be unique
        /// </summary>
        public decimal Id { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses a file info for the information needed in the script
        /// </summary>
        /// <param name="fileService">
        ///     Service to deal with files
        /// </param>
        /// <param name="filePath">
        ///     The file info to parse 
        /// </param>
        public void Parse(IFileService fileService, string filePath)
        {
            log.DebugIfEnabled(LogUtility.GetContext(fileService, filePath));

            this.FileName = filePath.Trim();
            this.FileInfo = new FileInfo(this.FileName);
            this.GetIdAndDescription();
            this.ReadContents(fileService, filePath);
        }

        private void ReadContents(IFileService fileService, string filePath)
        {
            string fileContents = fileService.GetFileContents(filePath, false);
            Contents = fileContents.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the Id and description from the file name
        /// </summary>
        private void GetIdAndDescription()
        {
            log.DebugIfEnabled(LogUtility.GetContext());

            Match m = regexFileName.Match(Path.GetFileNameWithoutExtension(FileInfo.Name));
            var success = m.Success && m.Groups.Count > 1;
            decimal id = 0;
            if (success)
            {
                success = decimal.TryParse(m.Groups[1].Value, out id);
            }
            if (!success)
            {
                string message = string.Format(
                    "The file {0} is formed incorrectly.  The file must match this format: {1}",
                    this.FileInfo.FullName, FileNamePattern);
                log.Error(message);
                throw new Exception(message);
            }

            this.Id = id;
            if (m.Groups.Count > 3)
            {
                this.Description = m.Groups[3].Value;
            }

            log.DebugIfEnabled(LogUtility.GetResult());
        }

        #endregion
    }
}