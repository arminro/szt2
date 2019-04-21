// <copyright file="SessionManager.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using DatabaseOperations;
    using DatabaseOperations.Interfaces;
    using Utils.CommonInterfaces;

    /// <summary>
    /// A class implementing <see cref="ISession"/>
    /// </summary>
    public class SessionManager : ISession
    {
        private IElectroStoreRepository db;
        private IAdministratorRepository adb;
        private string email = "szt2company@gmail.com";

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionManager"/> class.
        /// </summary>
        /// <param name="db">An instance of <see cref="IElectroStoreRepository"/></param>
        /// <param name="adb"> An instance of <see cref="IAdministratorRepository"/> </param>
        public SessionManager(IElectroStoreRepository db, IAdministratorRepository adb)
        {
            this.db = db;
            this.adb = adb;
        }

        /// <inheritdoc/>
        public EMPLOYEE ClerkOfCurrentSession { get; set; }

        /// <inheritdoc/>
        public SHOP ShopOfCurrentSession { get; set; }

        /// <inheritdoc/>
        public string CompanyEmailAddres { get => this.email; set => this.email = value; }

        /// <inheritdoc/>
        public IEnumerable<SHOP> Shops => this.db.GetElements<SHOP>();

        // public void RegisterClerkForCurrentSession(EMPLOYEE e)
        // {
        //    ClerkOfCurrentSession = e;
        // }

        /// <inheritdoc/>
        public void RegisterShopForCurrentSession(SHOP s)
        {
            this.ShopOfCurrentSession = s;
        }

        /// <inheritdoc/>
        public bool Authenticate(string username, SecureString password)
        {
            // usernames are unique, so it is enough to look for a username
            EMPLOYEE authenticatee = this.db.GetElements<EMPLOYEE>().Where(e => e.USERNAME == username).FirstOrDefault();

            if (authenticatee == null)
            {
                return false;
            }

            // converting the hashed byte[] of the given pw into a string for comparison
            if (this.GetStringFromSecure(password) == authenticatee.PASSWORD)
            {
                // if the authentication process is succesful, we set the clerk for the session
                this.ClerkOfCurrentSession = this.db.GetElements<EMPLOYEE>().Single(e => e.USERNAME == username);

                // and we register the login
                LOGIN log = new LOGIN
                {
                    EMPID = authenticatee.Id,
                    LOGINTIME = DateTime.Now
                };

                this.adb.Create(log);

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public byte[] ComputeHash(SecureString input)
        {
            byte[] hash = null;

            using (var hasher = SHA512.Create())
            {
                hash = input.Compute(hasher.ComputeHash);
            }

            return hash;
        }

        /// <inheritdoc/>
        public string GetStringFromSecure(SecureString s)
        {
            /*we compute the hash of a secure string and converting the hash in byte[] into a long string
             this does not expose the string, only converts the byte[] so it can be stored in the db as a string*/
            StringBuilder hashed = new StringBuilder();
            foreach (byte b in this.ComputeHash(s))
            {
                hashed.Append(b);
            }

            return hashed.ToString();
        }
    }
}
