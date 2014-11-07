using System;
using System.IO;
using System.Reflection;

namespace Veracity.Utilities.DatabaseDeploy.Utilities
{
    /// <summary>
    /// Default Environment provider which uses the stardard .NET Environment class
    /// </summary>
    public class DefaultEnvironmentProvider : EnvironmentProvider
    {
        /// <summary>
        /// User standard .NET environment to return current directory
        /// </summary>
        public override string CurrentDirectory
        {
            get { return Environment.CurrentDirectory; }
        }

        /// <summary>
        /// User standard .NET environment to return current user name
        /// </summary>
        public override string UserName
        {
            get { return Environment.UserName; }
        }

        /// <summary>
        /// The location of the currently executing assembly
        /// </summary>
        public override string ExecutingAssemblyDirectory
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }
    }
}