// <copyright file="Class1.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Utils
{
    using System.ComponentModel;

    /// <summary>
    /// Roles of the employees
    /// </summary>
    public enum EmployeeRole
    {
        /// <summary>
        /// Role of the administrator
        /// </summary>
        Admin = 1,

        /// <summary>
        /// The role of the leading shop clerk
        /// </summary>
        MainClerk = 3,

        /// <summary>
        /// Role of ordinary shop clerk
        /// </summary>
        Clerk = 2
    }

    /// <summary>
    /// Roles of transactions
    /// </summary>
    public enum TransactionRoles
    {
        /// <summary>
        /// The transaction is a purchase
        /// </summary>
        Purchase = 1,

        /// <summary>
        /// The transaction is an addition of goods
        /// </summary>
        Supply = 2,

        /// <summary>
        /// The transaction is a transfer of products from a store to another
        /// </summary>
        Transfer = 3
    }

    /// <summary>
    /// The type of products
    /// </summary>
    public enum Type
    {
        /// <summary>
        /// Central Porcessing Unit
        /// </summary>
        [Description("Processzorok")]
        Cpu = 0,

        /// <summary>
        /// Secure Digital card
        /// </summary>
        [Description("SD kártyák")]
        SDCard = 1,

        /// <summary>
        /// Portable devices
        /// </summary>
        [Description("Hordozható eszközök")]
        Portable = 2,

        /// <summary>
        /// Accessory devices to consumer electronics
        /// </summary>
        [Description("Elektronikai kiegészítők")]
        Accessories = 3,

        /// <summary>
        /// Power supply unit
        /// </summary>
        [Description("Tápegységek")]
        Psu = 4,

        /// <summary>
        /// Solid State Card
        /// </summary>
        [Description("SSD-kártyák")]
        Ssd = 7,

        /// <summary>
        /// Random Access Memory
        /// </summary>
        [Description("RAM-ok")]
        Ram = 6,

        /// <summary>
        /// Computer hardware monitor
        /// </summary>
        [Description("Monitorok")]
        Monitor = 5,

        /// <summary>
        /// Computer hardware mouse
        /// </summary>
        [Description("Egerek")]
        Mouse = 8,

        /// <summary>
        /// Computer hardware keyboard
        /// </summary>
        [Description("Billentyűzetek")]
        Keyboard = 9,

        /// <summary>
        /// Graphical Processing Unit
        /// </summary>
        [Description("Videokártyák")]
        Gpu = 10
    }
}
