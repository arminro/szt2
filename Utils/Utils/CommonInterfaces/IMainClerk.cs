// <copyright file="IMainClerk.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Utils.CommonInterfaces
{
    using System.Collections.Generic;
    using DatabaseOperations;

    /// <summary>
    /// Provides MainClerk functionality
    /// </summary>
    public interface IMainClerk : IClerk, IElectroLogicProvider
    {
        /// <summary>
        /// Gets the daily income of the shop of the sessions
        /// </summary>
         int DailyIncome { get;  }

        /// <summary>
        /// Gets a list of daily purchases
        /// </summary>
         IEnumerable<TRANSACT> DailyPurchase { get;  }

        /// <summary>
        /// Gets a list of employees
        /// </summary>
         IEnumerable<EMPLOYEE> Employees { get;  }

        /// <summary>
        /// Gets the list of shops
        /// </summary>
         IEnumerable<SHOP> Shops { get; }

        /// <summary>
        /// Gets the total income
        /// </summary>
         int TotlaIncome { get;  }

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="product">The product to be deleted</param>
        void DeleteProduct(PRODUCT product);

        /// <summary>
        /// Deletes a transaction
        /// </summary>
        /// <param name="tr">Transaction to be deleted</param>
        void DeleteTransaction(TRANSACT tr);

        /// <summary>
        /// Registers a shipment of products
        /// </summary>
        /// <param name="products">Products that are shipped from one shop to another</param>
        void RegisterShipment(IEnumerable<PRODUCT> products);

        /// <summary>
        /// Tranfers a list of goods from one shop to another
        /// </summary>
        /// <param name="fromShop">The source of the goods</param>
        /// <param name="toShop">The destination of the goods</param>
        /// <param name="productsTransfered">The goods to be transfered</param>
        void Transfer(SHOP fromShop, SHOP toShop, IEnumerable<PRODUCT> productsTransfered);

        /// <summary>
        /// Updates a transaction
        /// </summary>
        /// <param name="tr">The transaction to be updated</param>
        void UpdateTransaction(TRANSACT tr);

        /// <summary>
        /// Issues a new discount
        /// </summary>
        /// <param name="discountPercentile">The data reagarding percintiles of each type to be discounted</param>
        void IssueDiscount(IDictionary<Utils.Type, int> discountPercentile);

        /// <summary>
        /// Restores the original price of products
        /// </summary>
        void RestoreOriginalPrices();

        /// <summary>
        /// Revokes a discount so that a new one can be issued
        /// </summary>
        void RevokeDiscount();

        /// <summary>
        /// Maps discount percentages to producttypes, it is important that types and percentages are in the correct order, otherwise, the algorithm  will not make the appropriate pairs
        /// </summary>
        /// <param name="discountedTypes">Producttypes that are discounted</param>
        /// <param name="discountPercentages">Percentages by which producttypes are discounted</param>
        /// <returns>A mapping of producttypes and discount percentages</returns>
        IDictionary<Utils.Type, int> BuildDiscountData(IEnumerable<Utils.Type> discountedTypes, IEnumerable<int> discountPercentages);
    }
}
