// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptFile.cs" company="Veracity Solutions, Inc.">
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

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the description for this file.
        /// </summary>
        public string Description { get; set; }

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
        public int Id { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses a file info for the information needed in the script
        /// </summary>
        /// <param name="filePath">
        /// The file info to parse 
        /// </param>
        public void Parse(string filePath)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(filePath));
            }

            this.FileName = filePath.Trim();
            FileInfo info = new FileInfo(this.FileName);

            this.FileInfo = info;

            this.GetIdAndDescription();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the Id and description from the file name
        /// </summary>
        private void GetIdAndDescription()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext());
            }

            int idLocation = this.FileInfo.Name.IndexOf(' ');

            int id = 0;
            bool success = false;

            // this can happen if they don't put a description into the file.
            if (idLocation == -1)
            {
                idLocation = this.FileInfo.Name.IndexOf('.');
                if (idLocation != -1)
                {
                    success = int.TryParse(this.FileInfo.Name.Substring(0, idLocation), out id);
                    idLocation = 0;
                }
            }
            else
            {
                success = int.TryParse(this.FileInfo.Name.Substring(0, idLocation), out id);
            }

            if (!success)
            {
                string message = string.Format(
                    "The file {0} is formed incorrectly.  The file must start with a value that will convert to an int followed by a space followed by the description of the file.  For example:  0001 MyScriptFile.sql", this.FileInfo.FullName);
                log.Error(message);
                throw new Exception(message);
            }

            string description = this.FileInfo.Name.Substring(idLocation).Trim();

            this.Id = id;
            this.Description = description;

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult());
            }
        }

        #endregion
    }
}