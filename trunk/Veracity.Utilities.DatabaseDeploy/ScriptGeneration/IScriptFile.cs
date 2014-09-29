// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IScriptFile.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Veracity.Utilities.DatabaseDeploy.FileManagement;

namespace Veracity.Utilities.DatabaseDeploy.ScriptGeneration
{
    #region Usings

    using System.IO;

    #endregion

    /// <summary>
    /// Represents a script file in the system.
    /// </summary>
    public interface IScriptFile
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the description for this file.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        ///   Gets or sets the file info for this script
        /// </summary>
        FileInfo FileInfo { get; set; }

        /// <summary>
        ///   Gets or sets the full file name for this script
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gets or sets the contents of the script read in from the files
        /// </summary>
        string Contents { get; set; }

        /// <summary>
        ///   Gets or sets the script Id. These must be unique
        /// </summary>
        int Id { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses a file.
        /// </summary>
        /// <param name="fileService">
        ///     The service that interacts with files
        /// </param>
        /// <param name="filePath">
        ///     The file that is going to be parsed. 
        /// </param>
        void Parse(IFileService fileService, string filePath);

        #endregion
    }
}