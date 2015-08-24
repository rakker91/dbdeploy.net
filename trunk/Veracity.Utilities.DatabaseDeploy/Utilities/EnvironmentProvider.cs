// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EnvironmentProvider.cs" company="Database Deploy 2">
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
    public abstract class EnvironmentProvider
    {
        /// <summary>
        ///     The current
        /// </summary>
        private static EnvironmentProvider current;

        /// <summary>
        ///     Initializes static members of the <see cref="EnvironmentProvider" /> class.
        /// </summary>
        static EnvironmentProvider()
        {
            current = new DefaultEnvironmentProvider();
        }

        /// <summary>
        ///     The current TimeProvider
        /// </summary>
        /// <value>The current.</value>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static EnvironmentProvider Current
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
        ///     The current directory
        /// </summary>
        /// <value>The current directory.</value>
        public abstract string CurrentDirectory { get; }

        /// <summary>
        ///     The currently executing assembly name
        /// </summary>
        /// <value>The executing assembly directory.</value>
        public abstract string ExecutingAssemblyDirectory { get; }

        /// <summary>
        ///     The current username
        /// </summary>
        /// <value>The name of the user.</value>
        public abstract string UserName { get; }

        /// <summary>
        ///     Reset to the default environment provider
        /// </summary>
        public static void ResetToDefault()
        {
            current = new DefaultEnvironmentProvider();
        }
    }
}