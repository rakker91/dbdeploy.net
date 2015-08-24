// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TokenEnum.cs" company="Database Deploy 2">
//    Copyright (c) 2015 Database Deploy 2.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
//  </copyright>
//   <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DatabaseDeploy.Core.Utilities
{
    /// <summary>
    ///     Represents tokens that are available for replacement.
    /// </summary>
    public class TokenEnum
    {
        /// <summary>
        ///     Used for CurrentDateTime replacement.
        /// </summary>
        public const string CurrentDateTimeToken = "$(CurrentDateTime)";

        /// <summary>
        ///     Used for CurrentUser replacement.
        /// </summary>
        public const string CurrentUserToken = "$(CurrentUser)";

        /// <summary>
        ///     Used for CurrentVersion replacement.
        /// </summary>
        public const string CurrentVersionToken = "$(CurrentVersion)";

        /// <summary>
        ///     Used for ScriptDescription replacement.
        /// </summary>
        public const string ScriptDescriptionToken = "$(ScriptDescription)";

        /// <summary>
        ///     Used for ScriptId replacement.
        /// </summary>
        public const string ScriptIdToken = "$(ScriptId)";

        /// <summary>
        ///     Used for ScriptName replacement.
        /// </summary>
        public const string ScriptNameToken = "$(ScriptName)";

        /// <summary>
        ///     Used for schema token replacement
        /// </summary>
        public const string SchemaToken = "$(Schema)";

        /// <summary>
        ///     Used for change log token replacement.
        /// </summary>
        public const string ChangeLogToken = "$(ChangeLog)";
    }
}