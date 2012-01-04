// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChangeLog.cs" company="Veracity Solutions, Inc.">
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

    #endregion

    /// <summary>
    /// Represents a changelog record
    /// </summary>
    public interface IChangeLog
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the application end date
        /// </summary>
        DateTime ApplicationEndDate { get; set; }

        /// <summary>
        ///   Gets or sets the person who applied the change set
        /// </summary>
        string AppliedBy { get; set; }

        /// <summary>
        ///   Gets or sets the change number Id
        /// </summary>
        int ChangeNumber { get; set; }

        /// <summary>
        ///   Gets or sets the script description
        /// </summary>
        string Description { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses a data row to extract the information for this changelog record
        /// </summary>
        /// <param name="row">
        /// The row to parse 
        /// </param>
        void Parse(DataRow row);

        #endregion
    }
}