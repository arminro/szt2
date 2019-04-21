// <copyright file="IClerk.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Utils.CommonInterfaces
{
    using System;
    using System.Collections.Generic;
    using DatabaseOperations;

    /// <summary>
    /// Provides clerk functionality
    /// </summary>
    public interface IClerk : IElectroLogicProvider
    {
        /// <summary>
        /// Provides a way to signal to the UI
        /// </summary>
        event EventHandler<TextEventArgs> SystemMessage;

        /// <summary>
        /// Gets list of products
        /// </summary>
        IEnumerable<PRODUCT> Products { get;  }

        /// <summary>
        /// Gets list of RegularCustomers
        /// </summary>
        IEnumerable<REGULARCUSTOMER> RegularCustomers { get;  }

        /// <summary>
        /// Gets the list of Transctions
        /// </summary>
        IEnumerable<TRANSACT> Transactions { get; }

        /// <summary>
        /// Adds a RegularCustomer
        /// </summary>
        /// <param name="customer">Customer to add</param>
        /// <param name="tr">Transaction to add if there is one</param>
        void AddRegularCustomer(REGULARCUSTOMER customer, TRANSACT tr);

        /// <summary>
        /// Deletes a regular customer
        /// </summary>
        /// <param name="customer">Customer to be deleted</param>
        void DeleteRegularCustomer(REGULARCUSTOMER customer);

        /// <summary>
        /// Issues a purchase
        /// </summary>
        /// <param name="products">List of purchased products</param>
        /// <param name="customer">A regular customer associated with the purchase if there is one</param>
        void Purchase(ICollection<PRODUCT> products, REGULARCUSTOMER customer);

        /// <summary>
        /// Sends newsletter
        /// </summary>
        /// <param name="message">Message of the newsletter</param>
        /// <param name="subject">Subject of the newsletter</param>
        void SendNewsLetter(string message, string subject);

        /// <summary>
        /// Updates a regualr customer
        /// </summary>
        /// <param name="customer">Customer ot be updated</param>
        /// <param name="tr">The transaction to update the customer with</param>
        void UpdateRegularCustomer(REGULARCUSTOMER customer, TRANSACT tr);
    }
}
