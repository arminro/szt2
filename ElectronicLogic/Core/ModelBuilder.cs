// <copyright file="ModelBuilder.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Core
{
    using System;
    using DatabaseOperations;
    using Utils.CommonInterfaces;
    using static DatabaseOperations.Enums;

    /// <summary>
    /// Creates blank instances of model classes
    /// </summary>
    public static class ModelBuilder
    {
        /// <summary>
        /// Creats a blank instance of <see cref="PRODUCT"/>
        /// </summary>
        /// <param name="session">An instance of <see cref="ISession"/></param>
        /// <returns>A blank instance of <see cref="PRODUCT"/></returns>
        public static PRODUCT CreateProduct(ISession session)
        {
            return new PRODUCT
            {
                Dbstate = (int)DBState.Active,
                SHOP = session.ShopOfCurrentSession,
                SHOPID = session.ShopOfCurrentSession.Id
            };
        }

        /// <summary>
        /// Creats a blank instance of <see cref="EMPLOYEE"/>
        /// </summary>
        /// <returns>A blank instance of <see cref="EMPLOYEE"/></returns>
        public static EMPLOYEE CreateEmployee()
        {
            return new EMPLOYEE
            {
                Dbstate = (int)DBState.Active
            };
        }

        /// <summary>
        /// Creats a blank instance of <see cref="REGULARCUSTOMER"/>
        /// </summary>
        /// <returns>A blank instance of <see cref="REGULARCUSTOMER"/></returns>
        public static REGULARCUSTOMER CreateRegularCustomer()
        {
            return new REGULARCUSTOMER()
            {
                Dbstate = (int)DBState.Active
            };
        }

        /// <summary>
        /// Creats a blank instance of <see cref="SHOP"/>
        /// </summary>
        /// <returns>A blank instance of <see cref="SHOP"/></returns>
        public static SHOP CreateShop()
        {
            return new SHOP
            {
                Dbstate = (int)DBState.Active
            };
        }

        /// <summary>
        /// Creats a blank instance of <see cref="TRANSACT"/>
        /// </summary>
        /// <param name="session">An instance of <see cref="ISession"/></param>
        /// <returns>A blank instance of <see cref="TRANSACT"/></returns>
        public static TRANSACT CreateTransact(ISession session)
        {
            DateTime now = DateTime.Now;
            return new TRANSACT()
            {
                Dbstate = (int)DBState.Active,
                DATEOFTRANSACT = new DateTime(now.Year, now.Month, now.Day),
                SHOP = session.ShopOfCurrentSession ?? throw new ApplicationException("Sessionmanager does not hold a valid reference to the current shop"),
                ORIGINSHOPID = session.ShopOfCurrentSession.Id,
                EMPLOYEE = session.ClerkOfCurrentSession,
                EMPID = session.ClerkOfCurrentSession.Id
            };
        }
    }
}
