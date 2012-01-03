// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringReplaceExtension.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Utilities
{
    using log4net;

    /// <summary>
    /// Replaces strings for text in sql server.
    /// </summary>
    public static class StringReplaceExtension
    {
        #region Constants and Fields

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(StringReplaceExtension));

        #endregion

        #region Public Methods

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
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(value, oldValue, newValue));
            }

            if (newValue == null)
            {
                newValue = string.Empty;
            }

            string result = value.Replace(oldValue, newValue.Replace("'", "''").Replace(';', '_'));

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult(result));
            }

            return result;
        }

        #endregion
    }
}