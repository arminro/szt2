// <copyright file="IMessenger.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Utils.CommonInterfaces
{
    /// <summary>
    /// Interface providing messaging capabilities
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// Sending newsletter
        /// </summary>
        /// <param name="message">The message of the newsletter</param>
        /// <param name="subject">The subject of the message to be sent</param>
        /// <param name="recipients">The list of email addresses of recipients</param>
        void SendNewsletter(string message, string subject, string[] recipients);
    }
}
