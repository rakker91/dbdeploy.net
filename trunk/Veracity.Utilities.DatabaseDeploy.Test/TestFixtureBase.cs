// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TestFixtureBase.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Test
{
    using System.Collections.Generic;

    using log4net;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Repository;
    using log4net.Repository.Hierarchy;

    /// <summary>
    ///     A base class for all test fixtures.
    /// </summary>
    public class TestFixtureBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TestFixtureBase" /> class.
        /// </summary>
        public TestFixtureBase()
        {
            this.TurnOnLogging();
        }

        /// <summary>
        ///     Turns on all logging for the system.
        /// </summary>
        private void TurnOnLogging()
        {
            ILoggerRepository[] repositories = LogManager.GetAllRepositories();
            IList<IAppender> toRemove;

            // Configure all loggers to be at the debug level.
            foreach (ILoggerRepository repository in repositories)
            {
                repository.Threshold = repository.LevelMap["DEBUG"];
                Hierarchy hier = (Hierarchy)repository;
                ILogger[] loggers = hier.GetCurrentLoggers();
                foreach (ILogger logger in loggers)
                {
                    Logger log = (Logger)logger;
                    log.Level = hier.LevelMap["DEBUG"];
                    log.AddAppender(new NullAppender());

                    toRemove = new List<IAppender>();
                    foreach (IAppender appender in log.Appenders)
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
            Hierarchy h = (Hierarchy)LogManager.GetRepository();
            Logger rootLogger = h.Root;
            rootLogger.Level = h.LevelMap["DEBUG"];
            rootLogger.AddAppender(new NullAppender());

            toRemove = new List<IAppender>();
            foreach (IAppender appender in rootLogger.Appenders)
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