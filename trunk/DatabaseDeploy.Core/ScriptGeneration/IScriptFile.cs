// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IScriptFile.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.ScriptGeneration
{
    using System.IO;

    using DatabaseDeploy.Core.FileManagement;

    /// <summary>
    ///     Represents a script file in the system.
    /// </summary>
    public interface IScriptFile
    {
        /// <summary>
        ///     Gets or sets the contents of the script read in from the files
        /// </summary>
        /// <value>The contents.</value>
        string Contents { get; set; }

        /// <summary>
        ///     Gets or sets the description for this file.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        ///     Gets or sets the file info for this script
        /// </summary>
        /// <value>The file information.</value>
        FileInfo FileInfo { get; set; }

        /// <summary>
        ///     Gets or sets the full file name for this script
        /// </summary>
        /// <value>The name of the file.</value>
        string FileName { get; set; }

        /// <summary>
        ///     Gets or sets the script Id. These must be unique
        /// </summary>
        /// <value>The identifier.</value>
        decimal Id { get; set; }

        /// <summary>
        ///     Parses a file.
        /// </summary>
        /// <param name="fileService">The service that interacts with files</param>
        /// <param name="filePath">The file that is going to be parsed.</param>
        void Parse(IFileService fileService, string filePath);
    }
}