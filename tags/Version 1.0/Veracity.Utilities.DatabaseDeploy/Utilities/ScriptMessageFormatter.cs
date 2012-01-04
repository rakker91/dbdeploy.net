// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptMessageFormatter.cs" company="Veracity Solutions, Inc.">
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
    using System.Linq;
    using System.Text;

    using log4net;

    #endregion

    /// <summary>
    /// Takes an IDictionary and makes a prety script message
    /// </summary>
    public class ScriptMessageFormatter : IScriptMessageFormatter
    {
        #region Constants and Fields

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(ScriptMessageFormatter));

        #endregion

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
        public string FormatCollection(ICollection<int> values)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(values));
            }

            string result;

            if (values != null && values.Any())
            {
                StringBuilder textString = new StringBuilder();
                int lastNumber = -1;
                int rangeStart = -1;

                IOrderedEnumerable<int> orderedValues = values.OrderBy(x => x);

                foreach (int value in orderedValues)
                {
                    if (lastNumber == -1)
                    {
                        lastNumber = value;
                        rangeStart = value;
                    }
                    else if (value == lastNumber + 1)
                    {
                        lastNumber = value;
                    }
                    else
                    {
                        this.AppendRange(textString, lastNumber, rangeStart);
                        lastNumber = value;
                        rangeStart = value;
                    }
                }

                this.AppendRange(textString, lastNumber, rangeStart);
                result = textString.ToString().Trim().TrimEnd(',');
            }
            else
            {
                result = "No scripts found.";
            }

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult(result));
            }

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Appends a range of values to the stringbuilder
        /// </summary>
        /// <param name="textString">
        /// The stringbuilder 
        /// </param>
        /// <param name="lastNumber">
        /// The last number that was found. 
        /// </param>
        /// <param name="rangeStart">
        /// The start of the range. 
        /// </param>
        private void AppendRange(StringBuilder textString, int lastNumber, int rangeStart)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(textString, lastNumber, rangeStart));
            }

            if (lastNumber == rangeStart)
            {
                textString.Append(lastNumber);
                textString.Append(", ");
            }
            else if (rangeStart + 1 == lastNumber)
            {
                textString.Append(rangeStart);
                textString.Append(", ");
                textString.Append(lastNumber);
                textString.Append(", ");
            }
            else
            {
                textString.Append(rangeStart);
                textString.Append(" to ");
                textString.Append(lastNumber);
                textString.Append(", ");
            }

            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetResult());
            }
        }

        #endregion
    }
}