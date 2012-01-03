// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoResolverStrategy.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.IoC
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;

    #endregion

    /// <summary>
    /// Represents a strategy for performing mappings when the given type isn't already mapped.
    /// </summary>
    public class AutoResolverStrategy : BuilderStrategy
    {
        #region Constants and Fields

        /// <summary>
        ///   An object to use for locking.
        /// </summary>
        private static readonly object lockObject = new object();

        /// <summary>
        ///   A cache of type mappings that we've looked up already.
        /// </summary>
        private static readonly Dictionary<string, Type> typeCache = new Dictionary<string, Type>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Looks for an existing mapping. if not found, attempts a mapping based on the name of the resolve request.
        /// </summary>
        /// <param name="context">
        /// The current context for the buildup. 
        /// </param>
        public override void PreBuildUp(IBuilderContext context)
        {
            // Note that this is the same functionality as the default BuildKeyMappingPolicy.  The difference is that if we have no policy, we infer one.
            var policy = context.Policies.Get<IBuildKeyMappingPolicy>(context.BuildKey);

            if (policy != null)
            {
                context.BuildKey = policy.Map(context.BuildKey, context);
            }
            else
            {
                if (context.BuildKey.Type.IsInterface)
                {
                    string interFaceName = context.BuildKey.Type.Name;
                    string interfaceNamespace = context.BuildKey.Type.Namespace;

                    // By convention, all of our interfaces start with I, and by convention, the concrete type that they implement is the same name without the I.
                    if (interFaceName.StartsWith("I"))
                    {
                        string newName = interFaceName.Substring(1, interFaceName.Length - 1);
                        string fullName = interfaceNamespace + "." + newName;

                        Type concreteType = null;

                        if (typeCache.ContainsKey(fullName))
                        {
                            concreteType = typeCache[fullName];
                        }
                        else
                        {
                            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                            foreach (Assembly assembly in assemblies.Where(assembly => assembly.FullName.StartsWith("Veracity") || assembly.FullName.StartsWith("Veracity")))
                            {
                                concreteType = assembly.GetType(fullName);
                                if (concreteType != null)
                                {
                                    lock (lockObject)
                                    {
                                        if (!typeCache.ContainsKey(fullName))
                                        {
                                            typeCache.Add(fullName, concreteType);
                                        }
                                    }

                                    break;
                                }
                            }
                        }

                        if (concreteType != null)
                        {
                            NamedTypeBuildKey oldKey = context.BuildKey;
                            NamedTypeBuildKey newBuildKey = new NamedTypeBuildKey(concreteType, null);
                            context.BuildKey = newBuildKey;

                            // look for persistant policies for this interface name
                            ILifetimePolicy lifetime = context.PersistentPolicies.Get<ILifetimePolicy>(oldKey);
                            if (lifetime != null)
                            {
                                context.PersistentPolicies.Set(lifetime, newBuildKey);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}