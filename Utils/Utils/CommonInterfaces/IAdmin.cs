// <copyright file="IAdmin.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Utils.CommonInterfaces
{
    using System.Collections.Generic;
    using DatabaseOperations;

    /// <summary>
    /// Provides Admin functionality
    /// </summary>
    public interface IAdmin : IMainClerk, IElectroLogicProvider
    {
        /// <summary>
        /// Gets list of logged in users
        /// </summary>
        IEnumerable<LOGIN> Logins { get; }

        /// <summary>
        /// Adds an Employee
        /// </summary>
        /// <param name="emp">Employee to be added</param>
        void AddEmployee(EMPLOYEE emp);

        /// <summary>
        /// Adds a shop
        /// </summary>
        /// <param name="s">Shop to be added</param>
        void AddShop(SHOP s);

        /// <summary>
        /// Deletes and emplyoee
        /// </summary>
        /// <param name="e">Employee to be deleted</param>
        void DeleteEmployee(EMPLOYEE e);

        /// <summary>
        /// Deletes a shop
        /// </summary>
        /// <param name="s">Shop to be deleted</param>
        void DeleteShop(SHOP s);

        /// <summary>
        /// Updates and employee
        /// </summary>
        /// <param name="e">Employee to be updated</param>
        void UpdateEmployee(EMPLOYEE e);

        /// <summary>
        /// Updates a product
        /// </summary>
        /// <param name="p">Product to be updated</param>
        void UpdateProduct(PRODUCT p);

        /// <summary>
        /// Updates a shop
        /// </summary>
        /// <param name="s">Shop to be updated</param>
        void UpdateShop(SHOP s);
    }
}
