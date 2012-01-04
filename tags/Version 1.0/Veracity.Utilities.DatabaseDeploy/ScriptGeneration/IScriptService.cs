// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IScriptService.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.ScriptGeneration
{
    #region Usings

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a service for building scripts.
    /// </summary>
    public interface IScriptService
    {
        #region Public Methods

        /// <summary>
        /// Builds a change script from the given configuration.
        /// </summary>
        /// <param name="changes">
        /// The changes that are to be included. 
        /// </param>
        /// <returns>
        /// A string containing the script to be run. 
        /// </returns>
        string BuildChangeScript(IDictionary<int, IScriptFile> changes);

        /// <summary>
        /// Builds an undo script from the changes given. This is not currently used.
        /// </summary>
        /// <param name="changes">
        /// The changes that are to be used to build an undo script. 
        /// </param>
        /// <returns>
        /// A string containing the undo script. 
        /// </returns>
        string BuildUndoScript(IDictionary<int, IScriptFile> changes);

        #endregion
    }
}