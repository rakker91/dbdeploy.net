// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseTypesEnum.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Database
{
    /// <summary>
    /// An enum for the allowed database types in the system
    /// </summary>
    public enum DatabaseTypesEnum
    {
        /// <summary>
        ///   Microsoft Sql Server
        /// </summary>
        SqlServer, 

        /// <summary>
        ///   Oracle Database
        /// </summary>
        Oracle, 

        /// <summary>
        ///   MySQL database
        /// </summary>
        MySql
    }
}