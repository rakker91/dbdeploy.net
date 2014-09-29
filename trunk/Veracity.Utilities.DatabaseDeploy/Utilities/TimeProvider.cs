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
    public abstract class TimeProvider
    {
        private static TimeProvider current;

        static TimeProvider()
        {
            current = new DefaultTimeProvider();
        }

        /// <summary>
        /// The current TimeProvider
        /// </summary>
        public static TimeProvider Current
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
        /// The current UTC time
        /// </summary>
        public abstract DateTime UtcNow { get; }
        /// <summary>
        /// The current local time
        /// </summary>
        public abstract DateTime Now { get; }

        /// <summary>
        /// The current date in local time
        /// </summary>
        public abstract DateTime Today { get; }

        /// <summary>
        /// Reset to the default time provider
        /// </summary>
        public static void ResetToDefault()
        {
            current = new DefaultTimeProvider();
        }
    }
}
