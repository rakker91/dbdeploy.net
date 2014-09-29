using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Veracity.Utilities.DatabaseDeploy.Utilities
{
    /// <summary>
    /// Extensions that allow single line log calls
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        /// Log only if Debug level is enabled
        /// </summary>
        /// <param name="log"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void DebugIfEnabled(this ILog log, string format, params object[] args)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(args != null && args.Length > 0 ? string.Format(format, args) : format);
            }
        }

        /// <summary>
        /// Log only if Info level is enabled
        /// </summary>
        /// <param name="log"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void InfoIfEnabled(this ILog log, string format, params object[] args)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(args != null && args.Length > 0 ? string.Format(format, args) : format);
            }
        }
    }
}
