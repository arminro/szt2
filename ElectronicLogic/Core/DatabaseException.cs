// <copyright file="DatabaseException.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Core
{
    using System;

    /// <summary>
    /// An exception used to give special attention to exceptions during db operations
    /// </summary>
    [Serializable]
    public class DatabaseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseException"/> class.
        /// </summary>
        /// <param name="message">The message of the exception</param>
        public DatabaseException(string message)
            : base(message)
        {
        }
    }
}
