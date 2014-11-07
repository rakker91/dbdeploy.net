// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogUtility.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Utilities
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using log4net;

    #endregion

    /// <summary>
    /// Represents a class for assisting with logging statements.
    /// </summary>
    /// <remarks>
    /// Througout this class, logging is NOT done as recommended in other classes to avoid potential recursion from calling this static class over and over.
    /// </remarks>
    public static class LogUtility
    {
        #region Constants and Fields

        /// <summary>
        ///   Limits the maximum logging depth. Basically, if you're deeper than this, you're probably recursing, so don't go any deeper
        /// </summary>
        private const int MaxDepth = 50;

        /// <summary>
        ///   Creates the default logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(LogUtility));

        /// <summary>
        ///   A counter for preventing recursive depths in logging.
        /// </summary>
        private static int currentDepth;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the context for the current method call and translates it into a debug logging statement.
        /// </summary>
        /// <param name="passedValues">
        /// The parameters of the call. 
        /// </param>
        /// <remarks>
        /// The parameters of the call are not available to reflection, so they must be passed in. If the number of parameters passed to GetContext doesn't match the number expected, and exception is thrown.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when the number of parameters passed does not match the number of parameters defined on the method.
        /// </exception>
        /// <returns>
        /// A string containing the context string. 
        /// </returns>
        public static string GetContext(params object[] passedValues)
        {
            if (!log.IsDebugEnabled)
            {
                return string.Empty;
            }
            log.DebugIfEnabled("GetContext called.");

            string result = "Unable to build context.";

            currentDepth++;

            if (currentDepth > MaxDepth)
            {
                result = string.Format("Max logging depth of {0} reached.", MaxDepth);
            }
            else
            {
                MethodBase currentMethod = GetCurrentMethod();

                if (currentMethod != null)
                {
                    ParameterInfo[] methodParameters = currentMethod.GetParameters();

                    ValidateParameters(currentMethod, methodParameters, passedValues);

                    result = BuildLoggingStatement(currentMethod, methodParameters, passedValues);
                }
            }

            currentDepth--;

            log.DebugIfEnabled("GetContext finished.  Result={0}", result);

            return result;
        }

        /// <summary>
        /// Gets the result string message for result calls.
        /// </summary>
        /// <returns>
        /// A string containing the method get result string and the result value. 
        /// </returns>
        public static string GetResult()
        {
            log.DebugIfEnabled("GetResult called.");

            return GetResult(null);
        }

        /// <summary>
        /// Gets the result string message for result calls.
        /// </summary>
        /// <param name="result">
        /// The result value to be added to the string. 
        /// </param>
        /// <returns>
        /// A string containing the method get result string and the result value. 
        /// </returns>
        public static string GetResult(object result)
        {
            log.DebugIfEnabled("GetResult(object) called.");

            MethodBase currentMethod = GetCurrentMethod();
            var logResult = new StringBuilder();

            GetMethodDeclaration(logResult, currentMethod, currentMethod.GetParameters());
            logResult.Append(" Completed.");
            if (result != null)
            {
                logResult.Append("  Result=~|");
                logResult.Append(GetObjectValue(result));
                logResult.Append("|~");
            }

            log.DebugIfEnabled("GetResult Completed.  Result={0}", logResult);

            return logResult.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds a logging statement from the given parameters and passed values.
        /// </summary>
        /// <param name="currentMethod">
        /// The current method that we're logging. 
        /// </param>
        /// <param name="methodParameters">
        /// The parameters from the method 
        /// </param>
        /// <param name="passedValues">
        /// The values that have been passed for logging. 
        /// </param>
        /// <returns>
        /// A string containing the built up logging statement. 
        /// </returns>
        private static string BuildLoggingStatement(MethodBase currentMethod, ParameterInfo[] methodParameters, object[] passedValues)
        {
            log.DebugIfEnabled("BuildLoggingStatement Called.");

            StringBuilder loggingStatement = new StringBuilder();

            GetMethodDeclaration(loggingStatement, currentMethod, methodParameters);

            if (methodParameters.Any())
            {
                loggingStatement.Append(" --> ");
            }

            GetMethodParameterValues(loggingStatement, methodParameters, passedValues);

            string result = loggingStatement.ToString();

            log.DebugIfEnabled("BuildLoggingStatement Complete.  Result = {0}", result);

            return result;
        }

        /// <summary>
        /// Gets the current method for the calls to logutility.
        /// </summary>
        /// <returns>
        /// A method base containing the current logging method. 
        /// </returns>
        private static MethodBase GetCurrentMethod()
        {
            log.DebugIfEnabled("GetCurrentMethod called.");

            MethodBase result = null;

            StackTrace trace = new StackTrace();

            StackFrame[] frames = trace.GetFrames();
            if (frames != null)
            {
                result = frames.Select(frame => frame.GetMethod()).FirstOrDefault(method => method.DeclaringType != typeof(LogUtility) && method.Name != "Wrap");
            }

            log.DebugIfEnabled("GetCurrentMethod Complete");

            return result;
        }

        /// <summary>
        /// Gets a list of the generic types for the method.
        /// </summary>
        /// <param name="loggingStatement">
        /// The logging statement being built 
        /// </param>
        /// <param name="genericTypes">
        /// Type types ot put into the statement. 
        /// </param>
        private static void GetGenericTypes(StringBuilder loggingStatement, Type[] genericTypes)
        {
            log.DebugIfEnabled("GetGenericTypes called.");

            loggingStatement.Append("<");
            bool first = true;
            foreach (Type type in genericTypes)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    loggingStatement.Append(", ");
                }

                loggingStatement.Append(type.Name);
            }

            loggingStatement.Append(">");

            log.DebugIfEnabled("GetGenericTypes complete.");
        }

        /// <summary>
        /// Gets a method declaration for the current method.
        /// </summary>
        /// <param name="loggingStatement">
        /// The logging statement we're building. 
        /// </param>
        /// <param name="currentMethod">
        /// The current method to log. 
        /// </param>
        /// <param name="methodParameters">
        /// The method paramters for the current method. 
        /// </param>
        private static void GetMethodDeclaration(StringBuilder loggingStatement, MethodBase currentMethod, ParameterInfo[] methodParameters)
        {
            log.DebugIfEnabled("GetMethodDeclaration Called");

            loggingStatement.Append(currentMethod.Name);
            if (currentMethod.IsGenericMethod)
            {
                GetGenericTypes(loggingStatement, currentMethod.GetGenericArguments());
            }

            loggingStatement.Append("(");

            bool first = true;

            foreach (ParameterInfo info in methodParameters)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    loggingStatement.Append(", ");
                }

                if (info.GetCustomAttributes(typeof(ParamArrayAttribute), true).Any())
                {
                    loggingStatement.Append("params ");
                }

                loggingStatement.Append(info.ParameterType.Name);
                if (info.ParameterType.IsGenericType)
                {
                    GetGenericTypes(loggingStatement, info.ParameterType.GetGenericArguments());
                }

                loggingStatement.Append(" ");

                loggingStatement.Append(info.Name);
            }

            loggingStatement.Append(")");

            log.DebugIfEnabled("GetMethodDeclaration Complete");
        }

        /// <summary>
        /// Gets the values for all parameters.
        /// </summary>
        /// <param name="loggingStatement">
        /// The logging statement we're building. 
        /// </param>
        /// <param name="methodParameters">
        /// The method parameters for the current method. 
        /// </param>
        /// <param name="passedValues">
        /// The values that have been passed for logging. 
        /// </param>
        private static void GetMethodParameterValues(StringBuilder loggingStatement, ParameterInfo[] methodParameters, object[] passedValues)
        {
            log.DebugIfEnabled("GetMethodParameterValues called.");

            int passedLocation = 0;
            bool first = true;
            foreach (ParameterInfo info in methodParameters)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    loggingStatement.Append(", ");
                }

                loggingStatement.Append(info.Name);
                loggingStatement.Append(" = |: ");

                if (info.GetCustomAttributes(typeof(ParamArrayAttribute), true).Count() <= 0 || methodParameters.Count() != 1)
                {
                    loggingStatement.Append(GetObjectValue(passedValues));
                }
                else
                {
                    if (info.IsOut)
                    {
                        loggingStatement.Append("Out Parameter");
                    }
                    else
                    {
                        if (passedValues.Count() > passedLocation)
                        {
                            loggingStatement.Append(GetObjectValue(passedValues[passedLocation]));
                            passedLocation++;
                        }
                        else
                        {
                            if (info.IsOptional)
                            {
                                loggingStatement.Append("Optional.  Not provided.");
                            }
                        }
                    }
                }

                loggingStatement.Append(" :|");
            }

            log.DebugIfEnabled("GetMethodParameterValues complete.");
        }

        /// <summary>
        /// Gets the value for an object.
        /// </summary>
        /// <param name="value">
        /// The value to get a string for. 
        /// </param>
        /// <returns>
        /// A string representation of the object. 
        /// </returns>
        private static string GetObjectValue(object value)
        {
            log.DebugIfEnabled("GetObjectValue(object) called.");

            return GetObjectValue(value, new Dictionary<object, string>(), new List<object>());
        }

        /// <summary>
        /// Gets the value for an object.
        /// </summary>
        /// <param name="value">
        /// The value to get a string for. 
        /// </param>
        /// <param name="appendedObjects">
        /// The objects that have already been parsed. 
        /// </param>
        /// <param name="seenObjects">
        /// Objects that have been seen. May or may not have string values created. 
        /// </param>
        /// <returns>
        /// A string representation of the object. 
        /// </returns>
        private static string GetObjectValue(object value, IDictionary<object, string> appendedObjects, IList<object> seenObjects)
        {
            log.DebugIfEnabled("GetObjectValue called.");

            StringBuilder builder = new StringBuilder();
            string result;

            if (seenObjects.Contains(value) && !appendedObjects.ContainsKey(value))
            {
                result = string.Format("Recursion detected for object of type {0}", value.GetType().Name);
            }
            else
            {
                if (value == null)
                {
                    result = "null";
                }
                else
                {
                    seenObjects.Add(value);

                    if (appendedObjects.ContainsKey(value))
                    {
                        result = appendedObjects[value];
                    }
                    else
                    {
                        if (IsArray(value))
                        {
                            IEnumerable array = value as IEnumerable;
                            builder.Append("{ ");

                            if (array != null)
                            {
                                bool first = true;
                                foreach (object item in array)
                                {
                                    if (first)
                                    {
                                        first = false;
                                    }
                                    else
                                    {
                                        builder.Append(", ");
                                    }

                                    builder.Append(GetObjectValue(item, appendedObjects, seenObjects));
                                }
                            }
                            else
                            {
                                builder.Append("null");
                            }

                            builder.Append(" }");
                        }
                        else
                        {
                            builder.Append(value);
                        }

                        result = builder.ToString();
                        appendedObjects.Add(value, result);
                    }
                }
            }

            log.DebugIfEnabled("GetObjectValue complete.  Result={0}", result);

            return result;
        }

        /// <summary>
        /// Determines whether or not the value passed is an array.
        /// </summary>
        /// <param name="valueToCheck">
        /// The value to verify. 
        /// </param>
        /// <returns>
        /// True of the valueToCheck is or contains an array, false otherwise. 
        /// </returns>
        private static bool IsArray(object valueToCheck)
        {
            log.DebugIfEnabled("IsArray Called");

            bool result = false;

            if (valueToCheck != null && valueToCheck.GetType() != typeof(string))
            {
                if (valueToCheck is IEnumerable)
                {
                    result = true;
                }
            }

            log.DebugIfEnabled("IsArray Complete.  Result ={0}", result);

            return result;
        }

        /// <summary>
        /// Verifies taht the parameters passed match the method's properties
        /// </summary>
        /// <param name="currentMethod">
        /// The current method we're testing for. 
        /// </param>
        /// <param name="methodParameters">
        /// The parameters for the method. 
        /// </param>
        /// <param name="passedValues">
        /// The values that have been passed in. 
        /// </param>
        private static void ValidateParameters(MethodBase currentMethod, ParameterInfo[] methodParameters, object[] passedValues)
        {
            log.DebugIfEnabled("ValidateParameters called.");

            if (methodParameters == null)
            {
                methodParameters = new ParameterInfo[0];
            }

            int outParameterCount = methodParameters.Count(info => info.IsOut);
            int paramsCount = methodParameters.Count(info => info.GetCustomAttributes(typeof(ParamArrayAttribute), true).Any());
            int optionalCount = methodParameters.Count(info => info.IsOptional);
            int requiredAmount = methodParameters.Count() - outParameterCount - optionalCount;

            if ((paramsCount > 0 && methodParameters.Count() == 1) || (methodParameters.Count() == 1 && methodParameters[0].ParameterType.IsArray && paramsCount == 0))
            {
                // do nothing--if a method only has the params array, the passed values may be null, they may be an array of values, etc.  In other words, we'll take whatever they send because we can't detect
                // an invalid configuration.
            }
            else
            {
                // If they passed more than we can possibly have, that's bad.
                if (passedValues.Count() > methodParameters.Count())
                {
                    throw new ArgumentException(string.Format("LogUtility: Method {0} has {1} parameters passed but only has {2} parameters.", currentMethod.Name, passedValues.Count(), methodParameters.Count()));
                }

                if (passedValues.Count() < requiredAmount)
                {
                    throw new ArgumentException(string.Format("LogUtility: Method {0} has {1} parameters passed but requires at least {2} parameters.", currentMethod.Name, passedValues.Count(), methodParameters.Count()));
                }
            }

            log.DebugIfEnabled("ValidateParameters complete.");
        }

        #endregion
    }
}