// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenEnum.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Utilities
{
    /// <summary>
    /// Represents tokens that are available for replacement.
    /// </summary>
    public class TokenEnum
    {
        #region Constants and Fields

        /// <summary>
        ///   Used for CurrentDateTime replacement.
        /// </summary>
        public const string CurrentDateTimeToken = "$(CurrentDateTime)";

        /// <summary>
        ///   Used for CurrentUser replacement.
        /// </summary>
        public const string CurrentUserToken = "$(CurrentUser)";

        /// <summary>
        ///   Used for CurrentVersion replacement.
        /// </summary>
        public const string CurrentVersionToken = "$(CurrentVersion)";

        /// <summary>
        ///   Used for ScriptDescription replacement.
        /// </summary>
        public const string ScriptDescriptionToken = "$(ScriptDescription)";

        /// <summary>
        ///   Used for ScriptId replacement.
        /// </summary>
        public const string ScriptIdToken = "$(ScriptId)";

        /// <summary>
        ///   Used for ScriptName replacement.
        /// </summary>
        public const string ScriptNameToken = "$(ScriptName)";

        #endregion
    }
}