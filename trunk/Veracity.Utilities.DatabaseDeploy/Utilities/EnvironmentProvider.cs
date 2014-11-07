using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veracity.Utilities.DatabaseDeploy.Utilities
{
    /// <summary>
    /// Abstract class which represents something that provides the current time
    /// </summary>
    public abstract class EnvironmentProvider
    {
        private static EnvironmentProvider current;

        static EnvironmentProvider()
        {
            current = new DefaultEnvironmentProvider();
        }

        /// <summary>
        /// The current TimeProvider
        /// </summary>
        public static EnvironmentProvider Current
        {
            get { return current; }
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
        /// The current directory
        /// </summary>
        public abstract string CurrentDirectory { get; }

        /// <summary>
        /// The current username
        /// </summary>
        public abstract string UserName { get; }

        /// <summary>
        /// The currently executing assembly name
        /// </summary>
        public abstract string ExecutingAssemblyDirectory { get; }

        /// <summary>
        /// Reset to the default environment provider
        /// </summary>
        public static void ResetToDefault()
        {
            current = new DefaultEnvironmentProvider();
        }
    }
}
