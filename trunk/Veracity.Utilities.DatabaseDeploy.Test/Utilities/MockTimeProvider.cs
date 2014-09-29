using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veracity.Utilities.DatabaseDeploy.Utilities;

namespace Veracity.Utilities.DatabaseDeploy.Test.Utilities
{
    /// <summary>
    /// Time provider which provides a fixed date and time
    /// </summary>
    public class MockTimeProvider : TimeProvider
    {
        private readonly DateTime _now;

        /// <summary>
        /// Constructor which sets the time to a specific date and time
        /// </summary>
        /// <param name="now"></param>
        public MockTimeProvider(DateTime now)
        {
            _now = now;
        }

        /// <summary>
        /// The current time in UTC
        /// </summary>
        public override DateTime UtcNow
        {
            get { return _now.ToUniversalTime(); }
        }
        /// <summary>
        /// The current time in local time
        /// </summary>
        public override DateTime Now
        {
            get { return _now; }
        }
        /// <summary>
        /// The current date in local time
        /// </summary>
        public override DateTime Today
        {
            get { return _now.Date; }
        }
    }
}
