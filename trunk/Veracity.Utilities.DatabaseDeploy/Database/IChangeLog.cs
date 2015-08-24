// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IChangeLog.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Database
{
    using System;
    using System.Data;

    /// <summary>
    /// Represents a changelog record
    /// </summary>
    public interface IChangeLog
    {
        /// <summary>
        /// Gets or sets the application end date
        /// </summary>
        /// <value>The application end date.</value>
        DateTime ApplicationEndDate { get; set; }

        /// <summary>
        /// Gets or sets the person who applied the change set
        /// </summary>
        /// <value>The applied by.</value>
        string AppliedBy { get; set; }

        /// <summary>
        /// Gets or sets the change number Id
        /// </summary>
        /// <value>The change number.</value>
        int ChangeNumber { get; set; }

        /// <summary>
        /// Gets or sets the script description
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// Parses a data row to extract the information for this changelog record
        /// </summary>
        /// <param name="row">The row to parse</param>
        void Parse(DataRow row);
    }
}