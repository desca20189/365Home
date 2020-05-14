using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace _365Home.Ultility
{
    public class EmailSender : IEmailSender
    {
        // Credentials:
        string _sender = "desca20189@outlook.com";
        string _password = "Test123456789*";

        public async Task SendEmailAsync(string recipient, string subject, string message)
        {
            // Configure the client:
            SmtpClient client =
                new System.Net.Mail.SmtpClient("smtp-mail.outlook.com");

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            System.Net.NetworkCredential credentials =
               new System.Net.NetworkCredential(_sender, _password);

            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail =
                new MailMessage(_sender.Trim(), recipient.Trim());

            mail.Subject = subject;
            mail.Body = message;

            // Send:
            await client.SendMailAsync(mail);
        }
    }
}
