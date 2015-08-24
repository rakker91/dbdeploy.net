// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LogAspect.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Utilities
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    using log4net;

    using PostSharp.Aspects;

    /// <summary>
    ///     Logging advice for a logging aspect
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public sealed class LogAspect : OnMethodBoundaryAspect
    {
        /// <summary>
        ///     The namespace to pay attention to.
        /// </summary>
        private const string Namespace = "DatabaseDeploy";

        /// <summary>
        ///     cached method name
        /// </summary>
        private string cachedMethodName;

        /// <summary>
        ///     the last exception that was logged.
        /// </summary>
        private Exception lastException = new Exception();

        /// <summary>
        ///     The logger used
        /// </summary>
        [NonSerialized]
        private ILog logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LogAspect" /> class
        /// </summary>
        public LogAspect()
        {
            this.AspectPriority = 100;
            this.ApplyToStateMachine = true;
        }

        /// <summary>
        ///     Cache the method name during compilation
        /// </summary>
        /// <param name="method">The methodbase reflection info</param>
        /// <param name="aspectInfo">Reserved for later PostSharp use</param>
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            this.cachedMethodName = method.Name;
        }

        /// <summary>
        ///     Log at method entry pointcut
        /// </summary>
        /// <param name="args">The MethodExecutionArgs</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            if (this.logger.IsDebugEnabled)
            {
                if (args.Method.IsConstructor
                    || (args.Instance != null && args.Instance.GetType().Namespace.StartsWith(Namespace)))
                {
                    this.logger.Debug(LogUtility.GetContext(args.Method, args.Arguments.ToArray()));
                }
            }
        }

        /// <summary>
        ///     Log when a method exception occurs
        /// </summary>
        /// <param name="args">The MethodExecutionArgs which includes the exception</param>
        public override void OnException(MethodExecutionArgs args)
        {
            if (this.logger.IsErrorEnabled && !ReferenceEquals(args.Exception, this.lastException))
            {
                if (args.Method.IsConstructor
                    || (args.Instance != null && args.Instance.GetType().Namespace.StartsWith(Namespace)))
                {
                    this.lastException = args.Exception;
                    string message = string.Format(
                        "EXCEPTION in Method: {0} - Exception: {1}",
                        this.cachedMethodName,
                        args.Exception);

                    this.logger.Error(message, args.Exception);
                }
            }
        }

        /// <summary>
        ///     Log at method exist pointcut
        /// </summary>
        /// <param name="args">The MethodExecutionArgs</param>
        public override void OnExit(MethodExecutionArgs args)
        {
            if (this.logger.IsDebugEnabled)
            {
                if (args.Method.IsConstructor
                    || (args.Instance != null && args.Instance.GetType().Namespace.StartsWith(Namespace)))
                {
                    this.logger.Debug(LogUtility.GetResult(args.Method, args.ReturnValue));
                }
            }
        }

        /// <summary>
        ///     Apply runtime processing for the aspect
        /// </summary>
        /// <param name="method">The methodbase information</param>
        public override void RuntimeInitialize(MethodBase method)
        {
            this.logger = LogManager.GetLogger(method.DeclaringType);
        }
    }
}