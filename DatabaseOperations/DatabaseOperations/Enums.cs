// <copyright file="Enums.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace DatabaseOperations
{
    /// <summary>
    /// Placeholder class for storing Enums for the database
    /// </summary>
    public sealed class Enums
    {
        private Enums()
        {
        }

        /// <summary>
        /// Allowed states in the database
        /// </summary>
        public enum DBState
        {
            /// <summary>
            /// Active in the database
            /// </summary>
            Active = 1,

            /// <summary>
            /// Logically deleted in the database
            /// </summary>
            Deleted = 0
        }
    }
}
