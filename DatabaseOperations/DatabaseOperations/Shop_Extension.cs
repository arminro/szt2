// <copyright file="Shop_Extension.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace DatabaseOperations
{
    using System.Collections.Generic;
    using DatabaseOperations.Interfaces;

    /// <summary>
    /// Extends <see cref="SHOP"/> to implement interfaces
    /// </summary>
    public partial class SHOP : IDBEntry, IKeyProvider
    {
        /// <summary>
        /// Gets the products that are actually available for purchase, while  PRODUCTs holds all the products ever added, this collection returns those that are still active
        /// </summary>
        public ICollection<PRODUCT> ACTUAL_PRODUCTs
        {
            get
            {
                var temp = this.PRODUCTs.GetEnumerator();
                List<PRODUCT> prods = new List<PRODUCT>();
                while (temp.MoveNext())
                {
                    if (temp.Current.Dbstate != 0)
                    {
                        prods.Add(temp.Current);
                    }
                }

                return prods;
            }
        }
    }
}
