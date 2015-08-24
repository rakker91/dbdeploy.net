// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IDeploymentService.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core
{
    using DatabaseDeploy.Core.Configuration;

    /// <summary>
    ///     The i deployment service.
    /// </summary>
    public interface IDeploymentService
    {
        /// <summary>
        ///     Gets the current configuration service that is being used.
        /// </summary>
        /// <value>The configuration service.</value>
        IConfigurationService ConfigurationService { get; }

        /// <summary>
        ///     Builds the deployment scripts based on the information that is known at the time.
        /// </summary>
        void BuildDeploymentScript();
    }
}