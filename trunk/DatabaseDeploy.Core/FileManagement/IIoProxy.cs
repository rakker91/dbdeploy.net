// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IIoProxy.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.FileManagement
{
    using System.IO;

    /// <summary>
    ///     Represents a class that works as a proxy for calls into the file system.
    /// </summary>
    public interface IIoProxy
    {
        /// <summary>
        ///     Checks to see if a file exists
        /// </summary>
        /// <param name="fileName">The file to check.</param>
        /// <returns>True if it exists, false otherwise.</returns>
        bool Exists(string fileName);

        /// <summary>
        ///     Executes a Dictionary.GetFiles call
        /// </summary>
        /// <param name="rootDirectory">The root directory for the search/</param>
        /// <param name="searchPattern">The search pattern to use.</param>
        /// <param name="searchOption">The search options to use</param>
        /// <returns>A string array of files that were found.</returns>
        string[] GetFiles(string rootDirectory, string searchPattern, SearchOption searchOption);

        /// <summary>
        ///     Gets a stream reader for the given filename
        /// </summary>
        /// <param name="fileName">The filename to open.</param>
        /// <returns>A stream reader for the filename</returns>
        StreamReader GetStreamReader(string fileName);

        /// <summary>
        ///     Returns a stream writer with the given filename
        /// </summary>
        /// <param name="fileName">The filename to open.</param>
        /// <returns>A stream for the given filename.</returns>
        StreamWriter GetStreamWriter(string fileName);

        /// <summary>
        ///     Read all lines from a file
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>An object of type System.String[].</returns>
        string[] ReadAllLines(string fileName);

        /// <summary>
        ///     Read all text from a file
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>An object of type System.String.</returns>
        string ReadAllText(string fileName);
    }
}