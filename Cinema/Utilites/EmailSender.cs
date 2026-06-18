using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Cinema.Utilities
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mohamedkamel11988@gmail.com", "jwka arle ftdm lreh")
            };

            return client.SendMailAsync(
                new MailMessage(from: "mohamedkamel11988@gmail.com",
                                to: email,
                                subject,
                                htmlMessage
                                )
                {
                    IsBodyHtml = true
                }
                );

        }
    }
}
