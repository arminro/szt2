// <copyright file="IDBEntry.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace DatabaseOperations.Interfaces
{
    /// <summary>
    /// Provides a way to get or set the Dbstate of entities
    /// </summary>
    public interface IDBEntry
    {
        // THIS HAS TO BE EXACTLY THE SAME AS THE TYPE IN THE MODEL CLASSES

        /// <summary>
        /// Gets or sets the dbstate of entities
        /// </summary>
        int Dbstate { get; set; }
    }
}
