// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IScriptMessageFormatter.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Utilities
{
    #region Usings

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Takes an IDictionary and parses it into a pretty format
    /// </summary>
    public interface IScriptMessageFormatter
    {
        #region Public Methods

        /// <summary>
        /// Formats a collection of int values int a pretty string.
        /// </summary>
        /// <param name="values">
        /// The values to format 
        /// </param>
        /// <returns>
        /// A string containing the pretty values (for example "1 to 5, 10 to 15, 20, 40, 60") 
        /// </returns>
        string FormatCollection(ICollection<int> values);

        #endregion
    }
}