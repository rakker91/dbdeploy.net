// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DefaultEnvironmentProvider.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Utilities
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    ///     Default Environment provider which uses the stardard .NET Environment class
    /// </summary>
    public class DefaultEnvironmentProvider : EnvironmentProvider
    {
        /// <summary>
        ///     User standard .NET environment to return current directory
        /// </summary>
        /// <value>The current directory.</value>
        public override string CurrentDirectory
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        /// <summary>
        ///     The location of the currently executing assembly
        /// </summary>
        /// <value>The executing assembly directory.</value>
        public override string ExecutingAssemblyDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        /// <summary>
        ///     User standard .NET environment to return current user name
        /// </summary>
        /// <value>The name of the user.</value>
        public override string UserName
        {
            get
            {
                return Environment.UserName;
            }
        }
    }
}