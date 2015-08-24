// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TimeProvider.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Utilities
{
    using System;

    /// <summary>
    ///     Abstract class which represents something that provides the current time
    /// </summary>
    public abstract class TimeProvider
    {
        /// <summary>
        ///     The current
        /// </summary>
        private static TimeProvider current;

        /// <summary>
        ///     Initializes static members of the <see cref="TimeProvider" /> class.
        /// </summary>
        static TimeProvider()
        {
            current = new DefaultTimeProvider();
        }

        /// <summary>
        ///     The current TimeProvider
        /// </summary>
        /// <value>The current.</value>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static TimeProvider Current
        {
            get
            {
                return current;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                current = value;
            }
        }

        /// <summary>
        ///     The current local time
        /// </summary>
        /// <value>The now.</value>
        public abstract DateTime Now { get; }

        /// <summary>
        ///     The current date in local time
        /// </summary>
        /// <value>The today.</value>
        public abstract DateTime Today { get; }

        /// <summary>
        ///     The current UTC time
        /// </summary>
        /// <value>The UTC now.</value>
        public abstract DateTime UtcNow { get; }

        /// <summary>
        ///     Reset to the default time provider
        /// </summary>
        public static void ResetToDefault()
        {
            current = new DefaultTimeProvider();
        }
    }
}