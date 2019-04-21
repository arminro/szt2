// <copyright file="DBCallProcessor.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace DatabaseOperations
{
    using DatabaseOperations.Interfaces;

    /// <summary>
    /// Handles db calls towards the main repo
    /// </summary>
    public class DBCallProcessor : RepositoryBase<ElectroDBEntities>, IElectroStoreRepository
    {
    }
}
