// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestFixtureBase.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test
{
    using System.Collections.Generic;

    using log4net.Appender;

    /// <summary>
    /// A base class for all test fixtures.
    /// </summary>
    public class TestFixtureBase
    {
        /// <summary>
        /// Initializes a new instance of the TestFixtureBase class
        /// </summary>
        public TestFixtureBase()
        {
            this.TurnOnLogging();
        }

        /// <summary>
        /// Turns on all logging for the system.
        /// </summary>
        private void TurnOnLogging()
        {
            log4net.Repository.ILoggerRepository[] repositories = log4net.LogManager.GetAllRepositories();
            IList<IAppender> toRemove;

            // Configure all loggers to be at the debug level.
            foreach (log4net.Repository.ILoggerRepository repository in repositories)
            {
                repository.Threshold = repository.LevelMap["DEBUG"];
                log4net.Repository.Hierarchy.Hierarchy hier = (log4net.Repository.Hierarchy.Hierarchy)repository;
                log4net.Core.ILogger[] loggers = hier.GetCurrentLoggers();
                foreach (log4net.Core.ILogger logger in loggers)
                {
                    log4net.Repository.Hierarchy.Logger log = (log4net.Repository.Hierarchy.Logger)logger;
                    log.Level = hier.LevelMap["DEBUG"];
                    log.AddAppender(new NullAppender());

                    toRemove = new List<IAppender>();
                    foreach (var appender in log.Appenders)
                    {
                        if (appender.GetType() != typeof(NullAppender))
                        {
                            toRemove.Add(appender);
                        }
                    }

                    foreach (IAppender appender in toRemove)
                    {
                        log.RemoveAppender(appender);
                    }
                }
            }

            // Configure the root logger.
            log4net.Repository.Hierarchy.Hierarchy h = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();
            log4net.Repository.Hierarchy.Logger rootLogger = h.Root;
            rootLogger.Level = h.LevelMap["DEBUG"];
            rootLogger.AddAppender(new NullAppender());

            toRemove = new List<IAppender>();
            foreach (var appender in rootLogger.Appenders)
            {
                if (appender.GetType() != typeof(NullAppender))
                {
                    toRemove.Add(appender);
                }
            }

            foreach (IAppender appender in toRemove)
            {
                rootLogger.RemoveAppender(appender);
            }
        }
    }
}