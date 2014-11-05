// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoProxy.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.FileManagement
{
    using System.IO;

    /// <summary>
    /// This class is purely a proxy for calls into the file system for FileService.
    /// </summary>
    public class IoProxy : IIoProxy
    {
        /// <summary>
        /// Executes a Dictionary.GetFiles call
        /// </summary>
        /// <param name="rootDirectory">The root directory for the search/</param>
        /// <param name="searchPattern">The search pattern to use.</param>
        /// <param name="searchOption">The search options to use</param>
        /// <returns>A string array of files that were found.</returns>
        public string[] GetFiles(string rootDirectory, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(rootDirectory, searchPattern, searchOption);
        }

        /// <summary>
        /// Returns a stream writer with the given filename
        /// </summary>
        /// <param name="fileName">The filename to open.</param>
        /// <returns>A stream for the given filename.</returns>
        public StreamWriter GetStreamWriter(string fileName)
        {
            return new StreamWriter(fileName);
        }

        /// <summary>
        /// Gets a stream reader for the given filename
        /// </summary>
        /// <param name="fileName">The filename to open.</param>
        /// <returns>A stream reader for the filename</returns>
        public StreamReader GetStreamReader(string fileName)
        {
            return new StreamReader(fileName);
        }

        /// <summary>
        /// Checks to see if a file exists
        /// </summary>
        /// <param name="fileName">The file to check.</param>
        /// <returns>True if it exists, false otherwise.</returns>
        public bool Exists(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// Read all lines from the file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string[] ReadAllLines(string fileName)
        {
            return File.ReadAllLines(fileName);
        }

        /// <summary>
        /// Read all of the text from a file 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string ReadAllText(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}