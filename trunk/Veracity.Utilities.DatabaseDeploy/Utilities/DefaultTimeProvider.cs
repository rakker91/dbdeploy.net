using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veracity.Utilities.DatabaseDeploy.Utilities
{
    /// <summary>
    /// The default time provider which uses DateTime to do its work
    /// </summary>
    public class DefaultTimeProvider : TimeProvider
    {
        /// <summary>
        /// The current UTC time
        /// </summary>
        public override DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

        /// <summary>
        /// The current local time
        /// </summary>
        public override DateTime Now
        {
            get { return DateTime.Now; }
        }
        /// <summary>
        /// The current Date in local time
        /// </summary>
        public override DateTime Today
        {
            get { return DateTime.Today; }
        }
    }
}
