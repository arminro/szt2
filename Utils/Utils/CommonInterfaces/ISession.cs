// <copyright file="ISession.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Utils.CommonInterfaces
{
    using System.Collections.Generic;
    using System.Security;
    using DatabaseOperations;

    /// <summary>
    /// A sessio variable storing session-wise information
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// Gets or sets the shop assistant dealing with the current session
        /// </summary>
        EMPLOYEE ClerkOfCurrentSession { get; set; }

        /// <summary>
        /// Gets or sets the shop handling the current session
        /// </summary>
        SHOP ShopOfCurrentSession { get; set; }

        /// <summary>
        /// Gets the list of shops
        /// </summary>
        IEnumerable<SHOP> Shops { get; }

        /// <summary>
        /// Gets or sets the EmailAddres of the Company
        /// </summary>
        string CompanyEmailAddres { get; set; }

        /// <summary>
        /// Registers a shop as the host of the current session
        /// </summary>
        /// <param name="s">Host of the current session</param>
        void RegisterShopForCurrentSession(SHOP s);

        /// <summary>
        /// Authenticates a user with the provided credentials
        /// </summary>
        /// <param name="username">The username to be used</param>
        /// <param name="password">The password to be authenticated</param>
        /// <returns>True if authentication succeded, False otherwise</returns>
        bool Authenticate(string username, SecureString password);

        /// <summary>
        /// Computes the hash of the input
        /// </summary>
        /// <param name="input">The password</param>
        /// <returns>The hash of the input</returns>
        byte[] ComputeHash(SecureString input);

        /// <summary>
        /// Creates a string out of a secure string
        /// </summary>
        /// <param name="s">A SecureString that we need as a string</param>
        /// <returns>The SecureString as a string</returns>
        string GetStringFromSecure(SecureString s);
    }
}
