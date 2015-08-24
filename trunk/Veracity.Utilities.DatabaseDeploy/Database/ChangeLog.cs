// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ChangeLog.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Database
{
    using System;
    using System.Data;

    using DatabaseDeploy.Core.Utilities;

    using log4net;

    /// <summary>
    /// A record of a changelog
    /// </summary>
    public class ChangeLog : IChangeLog
    {
        /// <summary>
        /// Creates the default logger
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(ChangeLog));

        /// <summary>
        /// Gets or sets the application end date
        /// </summary>
        /// <value>The application end date.</value>
        public DateTime ApplicationEndDate { get; set; }

        /// <summary>
        /// Gets or sets the person who applied the change set
        /// </summary>
        /// <value>The applied by.</value>
        public string AppliedBy { get; set; }

        /// <summary>
        /// Gets or sets the change number Id
        /// </summary>
        /// <value>The change number.</value>
        public int ChangeNumber { get; set; }

        /// <summary>
        /// Gets or sets the script description
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Parses a data row to extract the information for this changelog record
        /// </summary>
        /// <param name="row">The row to parse</param>
        public void Parse(DataRow row)
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug(LogUtility.GetContext(row));
            }

            this.ApplicationEndDate = (DateTime)row["complete_dt"];
            this.AppliedBy = (string)row["applied_by"];
            this.ChangeNumber = (int)row["change_number"];
            this.Description = (string)row["description"];
        }
    }
}