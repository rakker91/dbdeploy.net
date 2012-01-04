// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoResolverUnityExtension.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.IoC
{
    #region Usings

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    #endregion

    /// <summary>
    /// Extends unity to automatically resolve most items based on naming convention.
    /// </summary>
    public class AutoResolverUnityExtension : UnityContainerExtension
    {
        #region Methods

        /// <summary>
        /// Initializes this extension.
        /// </summary>
        protected override void Initialize()
        {
            this.Context.Strategies.AddNew<AutoResolverStrategy>(UnityBuildStage.TypeMapping);
        }

        #endregion
    }
}