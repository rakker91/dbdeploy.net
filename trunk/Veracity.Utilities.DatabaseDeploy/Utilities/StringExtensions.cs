// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StringExtensions.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Replaces strings for text in sql server.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Replace multiple spaces with a single space
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns>An object of type System.String.</returns>
        public static string Canonicalize(this string script)
        {
            return Regex.Replace(script, @"\s+", " ");
        }

        /// <summary>
        ///     Combine multiple lines into 1
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <returns>An object of type System.String.</returns>
        public static string Combine(this IEnumerable<string> lines)
        {
            return
                lines.Select(l => l.Replace("\r\n", "\n").Replace("\n", Environment.NewLine))
                    .Aggregate(
                        new StringBuilder(),
                        (subtotal, next) => subtotal.Append(next),
                        subtotal => subtotal.ToString());
        }

        /// <summary>
        ///     Performs a string replacement on the given string.
        /// </summary>
        /// <param name="value">The value that will have replacement done against it.</param>
        /// <param name="oldValue">What to search for in the string.</param>
        /// <param name="newValue">What to replace the string with.</param>
        /// <returns>A string that has had tokens replaced and bad charachters replacesd.</returns>
        public static string ReplaceEx(this string value, string oldValue, string newValue)
        {
            if (newValue == null)
            {
                newValue = string.Empty;
            }

            string result = value.Replace(oldValue, newValue.Replace("'", "''").Replace(';', '_'));

            return result;
        }

        /// <summary>
        ///     String all of the -- comments
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <returns>An object of type IEnumerable&lt;System.String&gt;.</returns>
        public static IEnumerable<string> StripSingleLineComments(this IEnumerable<string> lines)
        {
            return lines.Select(
                l =>
                    {
                        int lastIndexOf = l.LastIndexOf("--");
                        if (lastIndexOf == -1)
                        {
                            return l;
                        }
                        return l.Substring(0, lastIndexOf);
                    });
        }
    }
}