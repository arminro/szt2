// <copyright file="NotificationManager.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace Messaging
{
    using System.Net;
    using System.Net.Mail;
    using Utils.CommonInterfaces;

    /// <summary>
    /// A class implementing <see cref="IMessenger"/>
    /// </summary>
    public class NotificationManager : IMessenger
    {
        private ISession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationManager"/> class.
        /// </summary>
        /// <param name="session">An instane of <see cref="ISession"/></param>
        public NotificationManager(ISession session)
        {
            this.session = session;
        }

        /// <inheritdoc/>
        public void SendNewsletter(string message, string subject, string[] recipients)
        {
            SendEmail(this.session.CompanyEmailAddres, this.session.ClerkOfCurrentSession.USERNAME, recipients, subject, message);
        }

        private static void SendEmail(string source, string nameOfMainClerk, string[] destinations, string subject, string emailBody)
        {
            /*c# SmtpClient uses TLS-/STARTTLS which is 587 in google mail,
             * see: https://support.google.com/mail/answer/7126229?hl=hu */

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(source, "szofttech123")
            };

            /*BEWARE: SmtpClient seems to either send the message to everyone, or nobody,
            so if any of the mail addresses are incorrect, nobody will get the email*/
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(source, nameOfMainClerk);
                mail.Subject = subject;
                mail.Body = emailBody;

                // adding the recipients to the email pool
                foreach (string s in destinations)
                {
                    mail.To.Add(s);
                }

                client.Send(mail);
            }
        }
    }
}
