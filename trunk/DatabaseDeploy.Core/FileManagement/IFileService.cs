// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IFileService.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.FileManagement
{
    using System.Collections.Generic;

    using DatabaseDeploy.Core.ScriptGeneration;

    /// <summary>
    /// Represents service that control file oprations for this run
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Removes previous files and leftovers to ensure a clean run.
        /// </summary>
        void CleanupPastRuns();

        /// <summary>
        /// Reads the contents of a file from disc
        /// </summary>
        /// <param name="fileName">The name of the file to read.</param>
        /// <param name="useCache">Indicates whether or not the cache should be used for getting file contents.</param>
        /// <returns>A string containing the contents of the file.</returns>
        string GetFileContents(string fileName, bool useCache);

        /// <summary>
        /// Reads the lines of a file from disc
        /// </summary>
        /// <param name="fileName">The name of the file to read.</param>
        /// <returns>A string containing the contents of the file.</returns>
        string[] GetLinesFromFile(string fileName);

        /// <summary>
        /// Gets a list of script files from disk
        /// </summary>
        /// <returns>A dictionary of script files that are on disc.</returns>
        IDictionary<decimal, IScriptFile> GetScriptFiles();

        /// <summary>
        /// Writes the change script file to disk
        /// </summary>
        /// <param name="changeScript">The change script contents</param>
        void WriteChangeScript(string changeScript);

        /// <summary>
        /// Writes the list of scripts found to a text file in the order in which they were found.
        /// </summary>
        /// <param name="scripts">The scripts that were found.</param>
        void WriteScriptList(IDictionary<decimal, IScriptFile> scripts);

        /// <summary>
        /// Writes the undo script file to disk
        /// </summary>
        /// <param name="undoScript">The undo disk</param>
        void WriteUndoScript(string undoScript);
    }
}