// <copyright file="UserFunctions.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using DatabaseOperations;
    using DatabaseOperations.Interfaces;
    using Newtonsoft.Json;
    using Utils;
    using Utils.CommonInterfaces;
    using static DatabaseOperations.Enums;

    /// <summary>
    /// The class implementing User logic
    /// </summary>
    public class UserFunctions : IClerk, IMainClerk, IAdmin
    {
        private IElectroStoreRepository db;
        private ISession session;
        private IMessenger messenger;
        private IAdministratorRepository adb;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFunctions"/> class.
        /// </summary>
        /// <param name="db">A reference to a class instance implementing <see cref="IAdministratorRepository"/></param>
        /// <param name="adb">A reference to a class instance implementing <see cref="IElectroStoreRepository"/></param>
        /// <param name="session">A reference to a class instance implementing <see cref="ISession"/></param>
        /// <param name="messenger">A reference to a class instance implementing <see cref="IMessenger"/></param>
        public UserFunctions(IElectroStoreRepository db, IAdministratorRepository adb, ISession session, IMessenger messenger)
        {
            this.db = db;
            this.session = session;
            this.messenger = messenger;
            this.adb = adb;
        }

        /// <inheritdoc/>
        public event EventHandler<TextEventArgs> SystemMessage;

        /// <inheritdoc/>
        public IEnumerable<PRODUCT> Products => this.db.GetElements<PRODUCT>();

        /// <inheritdoc/>
        public IEnumerable<REGULARCUSTOMER> RegularCustomers => this.db.GetElements<REGULARCUSTOMER>();

        /// <inheritdoc/>
        public IEnumerable<TRANSACT> Transactions => this.db.GetElements<TRANSACT>();

        // Getting every transaction that happened today, the income will be the sum of all the prices of products in all transactions

        /// <inheritdoc/>
        public int DailyIncome => this.ComputeDailyIncome();

        /// <inheritdoc/>
        public IEnumerable<TRANSACT> DailyPurchase => this.db.GetElements<TRANSACT>()
            .Where(t => t.DATEOFTRANSACT == DateTime.Today);

        /// <inheritdoc/>
        public IEnumerable<EMPLOYEE> Employees => this.db.GetElements<EMPLOYEE>();

        /// <inheritdoc/>
        public IEnumerable<SHOP> Shops => this.db.GetElements<SHOP>();

        /// <inheritdoc/>
        public int TotlaIncome => this.ComputeTotalIncome();

        /// <inheritdoc/>
        public IEnumerable<LOGIN> Logins => this.adb.GetElements<LOGIN>();

        /// <summary>
        /// Reads the saves from the default save path
        /// </summary>
        /// <returns>A string[] with the paths of all the .xml elements in the default save folder</returns>
        public IEnumerable<string> ReadSaves()
        {
            return Directory.GetFiles(this.GetSaveDirectory()).Where(p => p.EndsWith("json")).ToList();
        }

        /// <summary>
        /// Restores the original prices of prducts to the latest restore point
        /// </summary>
        public void RestoreOriginalPrices()
        {
            IDictionary<int, int> originalPrices;

            // deserializing data
            // XmlSerializer xs = new XmlSerializer(typeof(IReadOnlyDictionary<int, int>));
            // using (StreamReader rd = new StreamReader(GetSaveDirectory()+pathToResotreFile))
            // {
            //   originalPrices = xs.Deserialize(rd) as IDictionary<int, int>;
            // }
            // using (StreamReader r = new StreamReader(pathToResotreFile))
            // {
            //    originalPrices = JsonConvert.DeserializeObject<IDictionary<int, int>>(r.ReadLine());
            // }
            //// recreating and updated data
            // foreach (KeyValuePair<int, int> kvp in originalPrices)
            // {
            //    PRODUCT pr = Products.First(p => p.ID == kvp.Key);
            //    pr.PRICE = kvp.Value;
            //    // saving updated element
            //
            // }

            // getting the latest date
            DateTime latest = this.adb.GetElements<ORIGINALPRICE>().Max(o => o.DATE);

            // getting  restore data
            ORIGINALPRICE restore = this.adb.GetElements<ORIGINALPRICE>().Where(o => o.DATE == latest).SingleOrDefault();
            if (restore != null)
            {
                // getting data out of restore data
                originalPrices = JsonConvert.DeserializeObject<IDictionary<int, int>>(restore.ORIGINALPRICEDATA);

                // restoring products prices
                foreach (KeyValuePair<int, int> kvp in originalPrices)
                {
                    PRODUCT pr = this.Products.First(p => p.Id == kvp.Key);
                    pr.PRICE = kvp.Value;
                }

                // saving products to the database
                this.db.SaveToDB();
            }
            else
            {
                throw new ApplicationException($"There are no restore files yet");
            }
        }

        /// <summary>
        /// Adds a regular customer to the database
        /// </summary>
        /// <param name="customer">The model class holding data to add</param>
        /// <param name="tr">The transaction to add to the regualr customer</param>
        public void AddRegularCustomer(REGULARCUSTOMER customer, TRANSACT tr = null)
        {
            // should any error occur, we catch it and ask for the UI to display the message
            // try
            // {
            //    //if the customer wishes to add his/her latest pruchase to his account
            //    if(tr != null)
            //    {
            //        TRANSACT trInDB = GetItem<TRANSACT>(tr.ID);
            //        customer.TRANSACTs.Add(trInDB);
            //        //trInDB.REGULARCUSTOMER = customer;
            //        //trInDB.REGULARCUSTOMERID = customer.ID;
            //
            //    }
            //    db.Create(customer);
            //
            // }
            // catch (Exception e)
            // {
            //    SendErrorMessage(e.Message, "An error occurred during the creation process, saving the specified regular customer was terminated");
            // }
            if (tr != null)
            {
                this.Add(tr);
                this.Add(customer, tr);
            }
            else
            {
                this.Add(customer);
            }
        }

        /// <summary>
        /// Deletes a regular customer from the db identified by the instance passed as a param
        /// </summary>
        /// <param name="customer">The regular customer to delete</param>
        public void DeleteRegularCustomer(REGULARCUSTOMER customer)
        {
            this.Delete(customer);
        }

        /// <summary>
        /// Initiates the purchase of a number of products
        /// </summary>
        /// <param name="products">The purchased products</param>
        /// <param name="customer">The regular customer, if the purchase is associated to one</param>
        public void Purchase(ICollection<PRODUCT> products, REGULARCUSTOMER customer = null)
        {
            try
            {
                // creating new tr instance and setting its poperties
                TRANSACT tr = ModelBuilder.CreateTransact(this.session);
                tr.ROLE = (int)TransactionRoles.Purchase;

                this.AddTo(tr, products);

                // saving transaction to the db

                // if the transaction is associated with a regcust, we add it
                if (customer != null)
                {
                    // if we have a valid regular customer that is in the db, we update it with the new transaction
                    REGULARCUSTOMER customerInDB = this.GetItem<REGULARCUSTOMER>(customer.Id);
                    if (customerInDB != null)
                    {
                        // tr.REGULARCUSTOMER = customerInDB;
                        customerInDB.TRANSACTs.Add(tr);

                        // updating regualr customer in the db
                        this.db.Update(customerInDB);
                    }
                }

                this.db.Create(tr);

                // deleting purchased goods
                this.DeleteElements(products);

                // removing purchased products from their respective shops
                // foreach (PRODUCT p in products)
                // {
                //     this.DeletePurchasedProductFromItsShop(p);
                // }
            }
            catch (Exception e)
            {
                this.SendErrorMessage("An error occurred during registering the purchase, transaction was terminated", e, null);
            }
        }

        /// <summary>
        /// Sends out a newsletter to all the regular customers to whom it is relevant
        /// </summary>
        /// <param name="message">The message body</param>
        /// <param name="subject">The subject of the message</param>
        public void SendNewsLetter(string message, string subject)
        {
            try
            {
                if (this.adb.GetElements<DISCOUNT>().FirstOrDefault() != null)
                {
                    this.messenger.SendNewsletter(message, subject, this.RegularCustomers.Select(r => r.EMAIL).ToArray());
                }
                else
                {
                    throw new DiscountException("There are no discounts issued by a MainClerk");
                }
            }
            catch (Exception e)
            {
                this.SendErrorMessage("An error occured during the process of sending the newsletter", e, new List<System.Type>() { typeof(DiscountException) });
            }
        }

        /// <summary>
        /// Deletes a product from the db
        /// </summary>
        /// <param name="product">The product to delete</param>
        public void DeleteProduct(PRODUCT product)
        {
            this.Delete(product);
        }

        /// <inheritdoc/>
        public void DeleteTransaction(TRANSACT tr)
        {
            this.Delete(tr);
        }

        /// <inheritdoc/>
        public void RegisterShipment(IEnumerable<PRODUCT> products)
        {
            foreach (PRODUCT p in products)
            {
                p.Dbstate = (int)DBState.Active;
            }

            this.AddTo<SHOP, PRODUCT>(this.session.ShopOfCurrentSession, products);
        }

        /// <summary>
        /// Transfers a collection of products from 1 shop to the other
        /// </summary>
        /// <param name="fromShop">The shop the products are coming from</param>
        /// <param name="toShop">The shop the prducts are going to</param>
        /// <param name="productsTransfered">The list of products to transafer</param>
        public void Transfer(SHOP fromShop, SHOP toShop, IEnumerable<PRODUCT> productsTransfered)
        {
            try
            {
                SHOP origin = this.GetItem<SHOP>(fromShop.Id);
                if (origin == null)
                {
                    throw new DatabaseException("The origin of the shipment is not in the database");
                }

                SHOP destination = this.GetItem<SHOP>(toShop.Id);
                if (destination == null)
                {
                    throw new DatabaseException("The destination of the shipment is not in the database");
                }

                // removing products from origin and adding them to the destination
                foreach (PRODUCT p in productsTransfered)
                {
                    origin.PRODUCTs.Remove(p);
                    destination.PRODUCTs.Add(p);
                }

                this.db.Update(origin);
                this.db.Update(destination);
            }
            catch (Exception e)
            {
                this.SendErrorMessage($"An error occured during the process of registering the specified transfer", e, new List<System.Type>() { typeof(DatabaseException) });
            }
        }

        /// <summary>
        /// Updates a transaction with the data in the param
        /// </summary>
        /// <param name="tr">The model class holding info about changes</param>
        public void UpdateTransaction(TRANSACT tr)
        {
            this.Update(tr);
        }

        /// <summary>
        /// Adds an employee to the db, we are expecting employee entites that already have a SECURE pw
        /// </summary>
        /// <param name="emp">The employee to add</param>
        public void AddEmployee(EMPLOYEE emp)
        {
            this.Add(emp);
        }

        /// <inheritdoc/>
        public void AddShop(SHOP s)
        {
            this.Add(s);
        }

        /// <inheritdoc/>
        public void DeleteEmployee(EMPLOYEE e)
        {
            this.Delete(e);
        }

        /// <inheritdoc/>
        public void DeleteShop(SHOP s)
        {
            this.Delete(s);
        }

        /// <summary>
        /// Updates an employee with the data in the param
        /// </summary>
        /// <param name="e">The employee data</param>
        public void UpdateEmployee(EMPLOYEE e)
        {
            this.Update(e);
        }

        /// <summary>
        /// Updates the prduct with the data in the param
        /// </summary>
        /// <param name="p">The product data</param>
        public void UpdateProduct(PRODUCT p)
        {
            this.Update(p);
        }

        /// <summary>
        /// Updates a shop with the data in the param
        /// </summary>
        /// <param name="s">The shop data</param>
        public void UpdateShop(SHOP s)
        {
            this.Update(s);
        }

        /// <inheritdoc/>
        public void IssueDiscount(IDictionary<Utils.Type, int> discountPercentile)
        {
            try
            {
                if (this.adb.GetElements<DISCOUNT>().FirstOrDefault() != null)
                {
                    throw new DiscountException("There is a discount already active, please revoke it before issuing a new one");
                }
                else
                {
                    // saving original prices
                    this.SavePreviousPrices(this.GetPricesByProductId(discountPercentile.Select(d => d.Key)));

                    // updating prices to discounted ones
                    foreach (KeyValuePair<Utils.Type, int> kvp in discountPercentile)
                    {
                        var prodsOfType = this.Products.Where(p => p.TYPE == (int)kvp.Key);
                        foreach (PRODUCT p in prodsOfType)
                        {
                            var temp = (int)(p.PRICE * ((float)kvp.Value / 100));
                            p.PRICE -= temp;
                        }
                    }

                    // saving discount to the db
                    this.SaveDiscount(discountPercentile);

                    // updating products in the db
                    this.db.SaveToDB();
                }
            }
            catch (Exception e)
            {
                this.SendErrorMessage("The discount could not be issued", e, new List<System.Type>() { typeof(SerializationException), typeof(DiscountException) });
            }
        }

        /// <summary>
        /// Revokes a discount, this method assumes that only 1 discount is valid at a time
        /// </summary>
        public void RevokeDiscount()
        {
            /*We set the dbsate of discounts to deleted*/
            // we expect to have only 1 discount active
            DISCOUNT d = this.adb.GetElements<DISCOUNT>().FirstOrDefault();
            if (d != null)
            {
                d.Dbstate = (int)DBState.Deleted;
                this.adb.Update(d);
            }
            else
            {
                throw new ApplicationException("There are no discounts in the system to revoke");
            }
        }

        /// <summary>
        /// Maps discount percentiles to discounted producttypes, the types and percentages must match in order
        /// </summary>
        /// <param name="discountedTypes">Types that are to be discounted</param>
        /// <param name="discountPercentages">Percentages by which appropriate products are discounted</param>
        /// <returns>A mapping of discounted types to discount percentiles</returns>
        public IDictionary<Utils.Type, int> BuildDiscountData(IEnumerable<Utils.Type> discountedTypes, IEnumerable<int> discountPercentages)
        {
            IDictionary<Utils.Type, int> discount = null;
            try
            {
                discount = new Dictionary<Utils.Type, int>();

                for (int i = 0; i < discountedTypes.Count(); i++)
                {
                    discount.Add(discountedTypes.ElementAt(i), discountPercentages.ElementAt(i));
                }

                return discount;
            }
            catch (Exception e)
            {
                this.SendErrorMessage("Could not create discount mapping", e, null);
            }

            return discount;
        }

        /// <summary>
        /// Updates a regular customer
        /// </summary>
        /// <param name="customer">The regular customer to update</param>
        /// <param name="tr">The transaction to add if the update is concerned with one</param>
        public void UpdateRegularCustomer(REGULARCUSTOMER customer, TRANSACT tr = null)
        {
            // REGULARCUSTOMER custInDB = GetItem<REGULARCUSTOMER>(customer.ID);
            //
            // var props = customer.GetType().GetProperties();
            // foreach (PropertyInfo p in props)
            // {
            //    // we dont want to modify any collection
            //    if (p.PropertyType.Name != "ICollection`1")
            //    {
            //        custInDB.GetType().GetProperty(p.Name).SetValue(custInDB, customer.GetType().GetProperty(p.Name).GetValue(customer));
            //    }
            // }
            //// adding the transaction
            // custInDB.TRANSACTs.Add(tr);
            // db.Update(customer);
            if (tr != null)
            {
                this.Update(customer, tr);
            }
            else
            {
                this.Update(customer);
            }
        }

        private void AddTo<T, C>(T element, IEnumerable<C> elementsToAdd)
        {
            var parentCol = element.GetType().GetProperty(typeof(C).Name + "s").GetValue(element) as ICollection<C>;
            foreach (C c in elementsToAdd)
            {
                parentCol.Add(c);
            }

            this.db.SaveToDB();
        }

        private void DeleteElements<C>(ICollection<C> elementsToBeDeleted)
            where C : class, IKeyProvider, IDBEntry
        {
            foreach (C c in elementsToBeDeleted)
            {
                this.db.Delete(c);
            }
        }

        /// <summary>
        /// Sends a message to the UI in the folowing manner: in debug mode, sends the error message,
        /// in release mode, if the exception type if of the specified,
        /// sends the release message with the error message attached, otherwise, sends the release message only
        /// </summary>
        /// <param name="releaseMessage">The message shown in release mode</param>
        /// <param name="caughtException">The exception caught</param>
        /// <param name="exceptionsToShow">A collection of exception that we want to show to the User even in release mode</param>
        private void SendErrorMessage(string releaseMessage, Exception caughtException, IEnumerable<System.Type> exceptionsToShow)
        {
#if DEBUG
            this.SystemMessage?.Invoke(this, new TextEventArgs(caughtException.Message));
#else

            if (exceptionsToShow != null && exceptionsToShow.Contains(caughtException.GetType()))
            {
                StringBuilder error = new StringBuilder(releaseMessage);
                error.Append($": {caughtException.Message}");
                    this.SystemMessage?.Invoke(this, new TextEventArgs(error.ToString())); 
            }
            else
            {
                this.SystemMessage?.Invoke(this, new TextEventArgs(releaseMessage));
            }
#endif

        }

        private int ComputeDailyIncome()
        {
            // getting all the transactions that happened today so far
            var transactsToday = this.db.GetElements<TRANSACT>().Where(t => t.DATEOFTRANSACT == DateTime.Today);

            // if there are any elements in it, we add all the prices of every product of every transaction
            if (transactsToday.Any())
            {
                return (int)transactsToday.Sum(trans => trans.PRODUCTs.Sum(p => p.PRICE));
            }

            return 0;
        }

        private int ComputeTotalIncome()
        {
            // getting all the purchases
            var alltransacts = this.db.GetElements<TRANSACT>().Where(t => t.ROLE == (int)TransactionRoles.Purchase);
            if (alltransacts.Any())
            {
                // if there are any elements in it, we add all the prices of every product of every transaction
                return (int)alltransacts.Sum(trans => trans.PRODUCTs.Sum(p => p.PRICE));
            }

            return 0;
        }

        private void Add<T>(T element)
             where T : class, IKeyProvider, IDBEntry
        {
            try
            {
                this.db.Create(element);
            }
            catch (Exception e)
            {
                this.SendErrorMessage($"An error occurred during the creation process, saving the specified {typeof(T).Name} element was terminated", e, null);
            }
        }

        // private void DeletePurchasedProductFromItsShop(PRODUCT p)
        // {
        //     SHOP motherShop = this.db.GetElements<SHOP>().AsEnumerable().FirstOrDefault(s => s.PRODUCTs.Contains(p));
        //     motherShop.PRODUCTs.Remove(p);
        //     this.db.SaveToDB();
        // }
        private void Add<T, C>(T element, C child)
            where T : class, IKeyProvider, IDBEntry
            where C : class, IKeyProvider, IDBEntry
        {
            try
            {
                if (child != null)
                {
                    // getting the element in the DB
                    var inDb = this.GetItem<C>(child.Id);

                    // getting the collection
                    var parentCol = (ICollection<C>)element.GetType().GetProperty(typeof(C).Name + "s").GetValue(element);
                    parentCol.Add(inDb);
                }

                this.db.Create(element);
            }
            catch (Exception e)
            {
                this.SendErrorMessage($"An error occurred during the creation process, saving the specified {typeof(T).Name} element was terminated", e, null);
            }
        }

        private void Delete<T>(T element)
            where T : class, IKeyProvider, IDBEntry
        {
            try
            {
                this.db.Delete(element);
            }
            catch (Exception e)
            {
                this.SendErrorMessage("An error occurred during delete, saving the specified regular customer was terminated", e, null);
            }
        }

        private E GetItem<E>(int id)
            where E : class, IKeyProvider, IDBEntry
        {
            return this.db.GetElements<E>().Where(e => e.Id == id).First();
        }

        private string GetSaveDirectory()
        {
            StringBuilder saveFolder = new StringBuilder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            saveFolder.Append("\\");
            saveFolder.Append("ElectroStore");
            return saveFolder.ToString();
        }

        private IDictionary<int, int> GetPricesByProductId(IEnumerable<Utils.Type> typesToDiscount)
        {
            IDictionary<int, int> pricesByProductKeys = new Dictionary<int, int>();

            foreach (PRODUCT p in this.Products.ToList())
            {
                if (typesToDiscount.Contains((Utils.Type)p.TYPE))
                {
                    pricesByProductKeys.Add(p.Id, (int)p.PRICE);
                }
            }

            return pricesByProductKeys;
        }

        // todo get originalprice dates
        private void SaveDiscount(IDictionary<Utils.Type, int> discounts)
        {
            string discount = JsonConvert.SerializeObject(discounts);

            if (discount != null)
            {
                DISCOUNT d = new DISCOUNT
                {
                    DATE = DateTime.Now,
                    DISCOUNTDATA = discount
                };
                this.adb.Create(d);
            }
            else
            {
                throw new SerializationException("The list of discounts was invalid");
            }
        }

        private void SavePreviousPrices(IDictionary<int, int> originalPrices)
        {
            // serializing data
            string prices = JsonConvert.SerializeObject(originalPrices);

            ORIGINALPRICE original = new ORIGINALPRICE
            {
                DATE = DateTime.Now,
                ORIGINALPRICEDATA = prices
            };

            // saving it to the database
            this.adb.Create(original);
        }

        private void Update<T, C>(T element, C child)
             where T : class, IKeyProvider, IDBEntry
            where C : class, IKeyProvider, IDBEntry
        {
            try
            {
                T inDB = this.GetItem<T>(element.Id);
                if (inDB != null)
                {
                    // getting the properties of the elemenet to add
                    PropertyInfo[] properties = element.GetType().GetProperties();

                    // copying the new elements
                    foreach (PropertyInfo p in properties)
                    {
                        // we separate collections, becuase they have to be added children to
                        if (p.PropertyType.Name != "ICollection`1")
                        {
                            inDB.GetType().GetProperty(p.Name).SetValue(inDB, element.GetType().GetProperty(p.Name).GetValue(element));
                        }
                    }

                    var parentCol = element.GetType().GetProperty(typeof(C).Name + "s").GetValue(element) as ICollection<C>;
                    parentCol.Add(child);
                    this.db.Update(element);
                }
                else
                {
                    throw new DatabaseException($"The specified {typeof(T).Name} element is not in the database");
                }
            }
            catch (Exception e)
            {
                this.SendErrorMessage($"An error occured during the process of Updating the specified {typeof(T).Name} element", e, new List<System.Type>() { typeof(DatabaseException) });
            }
        }

        private void Update<T>(T element)
            where T : class, IKeyProvider, IDBEntry
        {
            try
            {
                T inDB = this.GetItem<T>(element.Id);
                if (inDB != null)
                {
                    PropertyInfo[] properties = element.GetType().GetProperties();
                    foreach (PropertyInfo p in properties)
                    {
                        inDB.GetType().GetProperty(p.Name).SetValue(inDB, element.GetType().GetProperty(p.Name).GetValue(element));
                    }

                    this.db.Update(inDB);
                }
                else
                {
                    throw new DatabaseException($"The specified {typeof(T).Name} element is not in the database");
                }
            }
            catch (Exception e)
            {
                this.SendErrorMessage($"An error occured during the process of Updating the specified {typeof(T).Name} element", e, new List<System.Type>() { typeof(DatabaseException) });
            }
        }
    }
}
