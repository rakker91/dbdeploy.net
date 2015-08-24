// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MockTimeProvider.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Test.Utilities
{
    using System;

    using DatabaseDeploy.Core.Utilities;

    /// <summary>
    ///     Time provider which provides a fixed date and time
    /// </summary>
    public class MockTimeProvider : TimeProvider
    {
        /// <summary>
        ///     The now
        /// </summary>
        private readonly DateTime now;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MockTimeProvider" /> class.
        /// </summary>
        /// <param name="now">The now.</param>
        public MockTimeProvider(DateTime now)
        {
            this.now = now;
        }

        /// <summary>
        ///     The current time in local time
        /// </summary>
        /// <value>The now.</value>
        public override DateTime Now
        {
            get
            {
                return this.now;
            }
        }

        /// <summary>
        ///     The current date in local time
        /// </summary>
        /// <value>The today.</value>
        public override DateTime Today
        {
            get
            {
                return this.now.Date;
            }
        }

        /// <summary>
        ///     The current time in UTC
        /// </summary>
        /// <value>The UTC now.</value>
        public override DateTime UtcNow
        {
            get
            {
                return this.now.ToUniversalTime();
            }
        }
    }
}