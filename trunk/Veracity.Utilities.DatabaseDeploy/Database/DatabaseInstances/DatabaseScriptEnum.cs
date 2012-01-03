// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseScriptEnum.cs" company="Veracity Solutions, Inc.">
//   Copyright (c) Veracity Solutions, Inc. 2012.  This code is licensed under the Microsoft Public License (MS-PL).  http://www.opensource.org/licenses/MS-PL.
// </copyright>
//  <summary>
//   Created By: Robert J. May
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Veracity.Utilities.DatabaseDeploy.Database.DatabaseInstances
{
    /// <summary>
    /// Represents the scripts that are available in the system.
    /// </summary>
    public static class DatabaseScriptEnum
    {
        #region Constants and Fields

        /// <summary>
        ///   Represents the footer of the change script.
        /// </summary>
        public const string ChangeScriptFooter = "ChangeScriptFooter.sql";

        /// <summary>
        ///   Represents the footer of the change script.
        /// </summary>
        public const string UndoScriptFooter = "UndoScriptFooter.sql";

        /// <summary>
        ///   Contains the header of the change script
        /// </summary>
        public const string ChangeScriptHeader = "ChangeScriptHeader.sql";

        /// <summary>
        ///   Contains the header of the change script
        /// </summary>
        public const string UndoScriptHeader = "UndoScriptHeader.sql";

        /// <summary>
        ///   The script to check and see if the changelog table exists. If not, it should be created.
        /// </summary>
        public const string EnsureChangeLogExists = "EnsureChangeLogExists.sql";

        /// <summary>
        ///   Gets the change log table completely.
        /// </summary>
        public const string GetChangeLog = "GetChangeLog.sql";

        /// <summary>
        ///   Represents the footer placed right after each script.
        /// </summary>
        public const string ScriptFooter = "ScriptFooter.sql";

        /// <summary>
        ///   Represents the footer placed right after each script.
        /// </summary>
        public const string UndoFooter = "UndoFooter.sql";

        /// <summary>
        ///   Represents the header placed right before each script.
        /// </summary>
        public const string ScriptHeader = "ScriptHeader.sql";

        /// <summary>
        ///   Represents the header placed right before each script.
        /// </summary>
        public const string UndoHeader = "UndoHeader.sql";

        /// <summary>
        /// Represents the file containing the undo token.
        /// </summary>
        public const string UndoToken = "UndoToken.sql";

        #endregion
    }
}