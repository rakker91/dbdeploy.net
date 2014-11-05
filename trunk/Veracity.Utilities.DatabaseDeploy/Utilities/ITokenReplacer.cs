// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITokenReplacer.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Utilities
{
    using Veracity.Utilities.DatabaseDeploy.ScriptGeneration;

    /// <summary>
    /// Represents a class that replaces tokens in strings.
    /// </summary>
    public interface ITokenReplacer
    {
        /// <summary>
        /// Gets or sets the current version.
        /// </summary>
        decimal CurrentVersion { get; set; }

        /// <summary>
        /// Gets or sets the current script being worked on.
        /// </summary>
        IScriptFile Script { get; set; }

        /// <summary>
        /// Performs a replacement on the string that is passed in.
        /// </summary>
        /// <param name="stringToParse">The string to parse.</param>
        /// <returns>A string that has been fully replaced.</returns>
        string Replace(string stringToParse);
    }
}