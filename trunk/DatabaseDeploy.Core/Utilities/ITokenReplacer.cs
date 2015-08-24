// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ITokenReplacer.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Utilities
{
    using DatabaseDeploy.Core.ScriptGeneration;

    /// <summary>
    ///     Represents a class that replaces tokens in strings.
    /// </summary>
    public interface ITokenReplacer
    {
        /// <summary>
        ///     Gets or sets the current version.
        /// </summary>
        /// <value>The current version.</value>
        decimal CurrentVersion { get; set; }

        /// <summary>
        ///     Gets or sets the current script being worked on.
        /// </summary>
        /// <value>The script.</value>
        IScriptFile Script { get; set; }

        /// <summary>
        ///     Performs a replacement on the string that is passed in.
        /// </summary>
        /// <param name="stringToParse">The string to parse.</param>
        /// <returns>A string that has been fully replaced.</returns>
        string Replace(string stringToParse);
    }
}