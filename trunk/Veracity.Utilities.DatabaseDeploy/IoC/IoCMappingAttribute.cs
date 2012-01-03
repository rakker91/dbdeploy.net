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

    #endregion

    /// <summary>
    /// Provides a mechanism for giving IoC mapping information. We may use this later to cause different behaviors to happen when other strategies are applyed around IoC.
    /// </summary>
    public class IoCMappingAttribute : Attribute
    {
    }
}