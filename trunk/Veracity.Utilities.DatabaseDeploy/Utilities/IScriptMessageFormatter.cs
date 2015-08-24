// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IScriptMessageFormatter.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Utilities
{
    using System.Collections.Generic;

    /// <summary>
    ///     Takes an IDictionary and parses it into a pretty format
    /// </summary>
    public interface IScriptMessageFormatter
    {
        /// <summary>
        ///     Formats a collection of int values int a pretty string.
        /// </summary>
        /// <param name="values">The values to format</param>
        /// <returns>A string containing the pretty values (for example "1 to 5, 10 to 15, 20, 40, 60")</returns>
        string FormatCollection(ICollection<int> values);

        /// <summary>
        ///     Concatenates values with commas
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>An object of type System.String.</returns>
        string FormatCollection(ICollection<decimal> values);
    }
}