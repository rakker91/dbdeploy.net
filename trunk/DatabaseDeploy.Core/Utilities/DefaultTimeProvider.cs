// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DefaultTimeProvider.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Utilities
{
    using System;

    /// <summary>
    ///     The default time provider which uses DateTime to do its work
    /// </summary>
    public class DefaultTimeProvider : TimeProvider
    {
        /// <summary>
        ///     The current local time
        /// </summary>
        /// <value>The now.</value>
        public override DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        ///     The current Date in local time
        /// </summary>
        /// <value>The today.</value>
        public override DateTime Today
        {
            get
            {
                return DateTime.Today;
            }
        }

        /// <summary>
        ///     The current UTC time
        /// </summary>
        /// <value>The UTC now.</value>
        public override DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}