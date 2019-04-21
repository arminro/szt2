// <copyright file="Transact_Extension.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace DatabaseOperations
{
    using DatabaseOperations.Interfaces;

    /// <summary>
    /// Extends <see cref="TRANSACT"/> to implement interfaces
    /// </summary>
    public partial class TRANSACT : IDBEntry, IKeyProvider
    {
    }
}
