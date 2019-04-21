// <copyright file="Product_Extension.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace DatabaseOperations
{
    using DatabaseOperations.Interfaces;

    /// <summary>
    /// Extends <see cref="PRODUCT"/> to implement interfaces
    /// </summary>
    public partial class PRODUCT : IDBEntry, IKeyProvider
    {
    }
}
