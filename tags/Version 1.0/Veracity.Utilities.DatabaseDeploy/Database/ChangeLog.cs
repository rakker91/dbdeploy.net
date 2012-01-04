// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeLog.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Database
{
    #region Usings

    using System;
    using System.Data;

    using log4net;

    using Veracity.Utilities.DatabaseDeploy.Utilities;

    #endregion

    /// <summary>
    /// A record of a changelog
    /// </summary>
    public class ChangeLog : IChangeLog
    {
        #region Constants and Fields

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(ChangeLog));

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the application end date
        /// </summary>
        public DateTime ApplicationEndDate { get; set; }

        /// <summary>
        ///   Gets or sets the person who applied the change set
        /// </summary>
        public string AppliedBy { get; set; }

        /// <summary>
        ///   Gets or sets the change number Id
        /// </summary>
        public int ChangeNumber { get; set; }

        /// <summary>
        ///   Gets or sets the script description
        /// </summary>
        public string Description { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses a data row to extract the information for this changelog record
        /// </summary>
        /// <param name="row">
        /// The row to parse 
        /// </param>
        public void Parse(DataRow row)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(LogUtility.GetContext(row));
            }

            this.ApplicationEndDate = (DateTime)row["complete_dt"];
            this.AppliedBy = (string)row["applied_by"];
            this.ChangeNumber = (int)row["change_number"];
            this.Description = (string)row["description"];
        }

        #endregion
    }
}