// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LogExtensions.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Utilities
{
    using log4net;

    /// <summary>
    ///     Extensions that allow single line log calls
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        ///     Log only if Debug level is enabled
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void DebugIfEnabled(this ILog log, string format, params object[] args)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(args != null && args.Length > 0 ? string.Format(format, args) : format);
            }
        }

        /// <summary>
        ///     Log only if Info level is enabled
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void InfoIfEnabled(this ILog log, string format, params object[] args)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(args != null && args.Length > 0 ? string.Format(format, args) : format);
            }
        }
    }
}