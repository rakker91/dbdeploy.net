// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoCMappingAttribute.cs" company="Veracity Solutions, Inc.">
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

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    /// Provides a mechanism for giving IoC mapping information. We may use this later to cause different behaviors to happen when other strategies are applyed around IoC.
    /// </summary>
    public class IoCMappingAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IoCMappingAttribute"/> class
        /// </summary>
        public IoCMappingAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCMappingAttribute"/> class
        /// </summary>
        /// <param name="lifetimePolicyName">The lifetime policy to use for this object. This name will be used to create an instance of the appropriate lifetime policy manager at resolve time.</param>
        public IoCMappingAttribute(LifetimePolicyEnum lifetimePolicyName)
        {
            this.LifetimePolicyName = lifetimePolicyName;
        }
        
        /// <summary>
        /// Gets the LifetimePolicyName.  This name will be used to create an instance of the appropriate lifetime policy manager at resolve time.
        /// </summary>
        public LifetimePolicyEnum LifetimePolicyName { get; private set; }
    }
}