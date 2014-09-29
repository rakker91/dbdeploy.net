// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Veracity.Utilities.DatabaseDeploy.Utilities
{
    using log4net;

    /// <summary>
    /// Replaces strings for text in sql server.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(StringExtensions));

        /// <summary>
        /// Performs a string replacement on the given string.
        /// </summary>
        /// <param name="value">
        /// The value that will have replacement done against it. 
        /// </param>
        /// <param name="oldValue">
        /// What to search for in the string. 
        /// </param>
        /// <param name="newValue">
        /// What to replace the string with. 
        /// </param>
        /// <returns>
        /// A string that has had tokens replaced and bad charachters replacesd. 
        /// </returns>
        public static string ReplaceEx(this string value, string oldValue, string newValue)
        {
                log.DebugIfEnabled(LogUtility.GetContext(value, oldValue, newValue));

            if (newValue == null)
            {
                newValue = string.Empty;
            }

            string result = value.Replace(oldValue, newValue.Replace("'", "''").Replace(';', '_'));

                log.DebugIfEnabled(LogUtility.GetResult(result));

            return result;
        }

        /// <summary>
        /// Replace multiple spaces with a single space
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static string Canonicalize(this string script)
        {
            return Regex.Replace(script, @"\s+", " ");
        }

        /// <summary>
        /// Combine multiple lines into 1
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static string Combine(this IEnumerable<string> lines)
        {
            return lines.Aggregate("", (subTotal, next) => subTotal + next);
        }

        /// <summary>
        /// String all of the -- comments
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static IEnumerable<string> StripSingleLineComments(this IEnumerable<string> lines)
        {
            return lines.Select(l =>
            {
                int lastIndexOf = l.LastIndexOf("--");
                if (lastIndexOf == -1)
                    return l;
                return l.Substring(0, lastIndexOf);
            });
        }
    }
}