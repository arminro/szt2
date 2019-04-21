// <copyright file="AdministrationsDBCallProcessor.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace DatabaseOperations
{
    using DatabaseOperations.Interfaces;

    /// <summary>
    /// Handles database calls towards the admin repo
    /// </summary>
    public class AdministrationsDBCallProcessor : RepositoryBase<AdminDBEntities>, IAdministratorRepository
    {
    }
}
