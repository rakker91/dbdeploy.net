// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeploymentService.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy
{
    #region Usings

    using Veracity.Utilities.DatabaseDeploy.Configuration;

    #endregion

    /// <summary>
    /// The i deployment service.
    /// </summary>
    public interface IDeploymentService
    {
        #region Public Properties

        /// <summary>
        ///   Gets the current configuration service that is being used.
        /// </summary>
        IConfigurationService ConfigurationService { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Builds the deployment scripts based on the information that is known at the time.
        /// </summary>
        void BuildDeploymentScript();

        #endregion
    }
}