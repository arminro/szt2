// <copyright file="DiscountException.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Core
{
    using System;

    /// <summary>
    /// An exception thrown during issuing a discount
    /// </summary>
    [Serializable]
    internal class DiscountException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscountException"/> class.
        /// </summary>
        /// <param name="message">The message of the exception</param>
        public DiscountException(string message)
            : base(message)
        {
        }
    }
}
