// <copyright file="SerializationException.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Core
{
    using System;

    /// <summary>
    /// An exception denoting any error during serialization
    /// </summary>
    [Serializable]
    public class SerializationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationException"/> class.
        /// </summary>
        /// <param name="msg">The message of the exception</param>
        public SerializationException(string msg)
            : base(msg)
        {
        }
    }
}
