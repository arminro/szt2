// <copyright file="AdminDBCallProcessor.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace DatabaseOperations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DatabaseOperations.Interfaces;

    /// <summary>
    /// Handles db calls targeting the administratorial database
    /// </summary>
    public class AdminDBCallProcessor : RepositoryBase<AdminDBEntities>, IAdministratorRepository
    {
    }
}
