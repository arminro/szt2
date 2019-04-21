// <copyright file="Employee_Extension.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace DatabaseOperations
{
    using DatabaseOperations.Interfaces;

    /// <summary>
    /// Extends <see cref="EMPLOYEE"/> to implement interfaces
    /// </summary>
    public partial class EMPLOYEE : IDBEntry, IKeyProvider
    {
    }
}
