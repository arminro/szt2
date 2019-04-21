// <copyright file="IRepository.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace DatabaseOperations.Interfaces
{
    using System.Linq;

    /// <summary>
    /// The base interface for repositories
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Creates an entry for an element in the database
        /// </summary>
        /// <typeparam name="T">The type of model class to create</typeparam>
        /// <param name="element">The model class object to create an entry for in the database</param>
        void Create<T>(T element)
            where T : class, IDBEntry, IKeyProvider;

        /// <summary>
        /// Updates an entry in the database
        /// </summary>
        /// <typeparam name="T">The type of model class to update</typeparam>
        /// <param name="element">The model class object to update an entry for in the database</param>
        void Update<T>(T element)
              where T : class, IKeyProvider;

        /// <summary>
        /// Logically deletes an entry in the database
        /// </summary>
        /// <typeparam name="T">The type of model class to delete</typeparam>
        /// <param name="element">The model class object to delete an entry for in the database</param>
        void Delete<T>(T element)
            where T : class, IDBEntry, IKeyProvider;

        /// <summary>
        /// Gets a specific collection of elements from the database
        /// </summary>
        /// <typeparam name="T">The type of modelclass entries the caller wants to access</typeparam>
        /// <returns>A queryable lazy collection of elements</returns>
        IQueryable<T> GetElements<T>()
            where T : class, IDBEntry;

        /// <summary>
        /// Provides functionality to initiate a database save externally
        /// </summary>
        void SaveToDB();
    }
}
