// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MockEnvironmentProvider.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Test.Utilities
{
    using System;
    using System.IO;
    using System.Reflection;

    using DatabaseDeploy.Core.Utilities;

    /// <summary>
    ///     Class MockEnvironmentProvider.
    /// </summary>
    internal class MockEnvironmentProvider : EnvironmentProvider
    {
        /// <summary>
        ///     The current directory
        /// </summary>
        private string currentDirectory;

        /// <summary>
        ///     The current executing assembly directory
        /// </summary>
        private string currentExecutingAssemblyDirectory;

        /// <summary>
        ///     The user name
        /// </summary>
        private string userName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MockEnvironmentProvider" /> class.
        /// </summary>
        public MockEnvironmentProvider()
        {
            this.userName = Environment.UserName;
            this.currentDirectory = Environment.CurrentDirectory;
            this.currentExecutingAssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        ///     Gets the current directory.
        /// </summary>
        /// <value>The current directory.</value>
        public override string CurrentDirectory
        {
            get
            {
                return this.currentDirectory;
            }
        }

        /// <summary>
        ///     Gets the executing assembly directory.
        /// </summary>
        /// <value>The executing assembly directory.</value>
        public override string ExecutingAssemblyDirectory
        {
            get
            {
                return this.currentExecutingAssemblyDirectory;
            }
        }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public override string UserName
        {
            get
            {
                return this.userName;
            }
        }

        /// <summary>
        ///     Sets the current directory.
        /// </summary>
        /// <param name="currentDir">The current dir.</param>
        public void SetCurrentDirectory(string currentDir)
        {
            this.currentDirectory = currentDir;
        }

        /// <summary>
        ///     Sets the current executing assembly directory.
        /// </summary>
        /// <param name="dir">The dir.</param>
        public void SetCurrentExecutingAssemblyDirectory(string dir)
        {
            this.currentExecutingAssemblyDirectory = dir;
        }

        /// <summary>
        ///     Sets the name of the user.
        /// </summary>
        /// <param name="username">The username.</param>
        public void SetUserName(string username)
        {
            this.userName = username;
        }
    }
}