// <copyright file="IKeyProvider.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace DatabaseOperations.Interfaces
{
    /// <summary>
    /// Provides a way to find entry by their IDs
    /// </summary>
    public interface IKeyProvider
    {
        /// <summary>
        /// Gets or sets the id of a db entry through an interface
        /// </summary>
        int Id { get; set; }
    }
}
