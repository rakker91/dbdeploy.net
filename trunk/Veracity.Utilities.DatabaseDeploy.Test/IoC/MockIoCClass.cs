// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockIoCClass.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Test.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Veracity.Utilities.DatabaseDeploy.IoC;

    /// <summary>
    /// A MockClass for IoC tests
    /// </summary>
    [IoCMapping(LifetimePolicyEnum.PerThreadLifetimeManager)]
    public class MockIoCClass : IMockIoCClass
    {
    }
}
