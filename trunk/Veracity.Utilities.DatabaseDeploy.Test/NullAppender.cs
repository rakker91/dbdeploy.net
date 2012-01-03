// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullAppender.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test
{
    using log4net.Appender;
    using log4net.Core;

    /// <summary>
    /// A null appender for log4net
    /// </summary>
    public class NullAppender : AppenderSkeleton
    {
        /// <summary>
        /// Subclasses of <see cref="T:log4net.Appender.AppenderSkeleton"/> should implement this method 
        ///             to perform actual logging.
        /// </summary>
        /// <param name="loggingEvent">The event to append.</param>
        /// <remarks>
        /// <para>
        /// A subclass must implement this method to perform
        ///             logging of the <paramref name="loggingEvent"/>.
        /// </para>
        /// <para>
        /// This method will be called by <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)"/>
        ///             if all the conditions listed for that method are met.
        /// </para>
        /// <para>
        /// To restrict the logging of events in the appender
        ///             override the <see cref="M:log4net.Appender.AppenderSkeleton.PreAppendCheck"/> method.
        /// </para>
        /// </remarks>
        protected override void Append(LoggingEvent loggingEvent)
        {
            // do nothing
        }
    }
}