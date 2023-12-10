using Crawler.Shared.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace Crawler.Shared.Services
{
    public interface IEmailService
    {
        void SendEmail(string toUserEmail, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;

        public EmailService(EmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(string toUserEmail, string subject, string message)
        {
            using var smpt = new SmtpClient
            {
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password)
            };

            var from = new MailAddress(_emailConfig.FromUser);
            var to = new MailAddress(toUserEmail);
            using var mailMessage = new MailMessage(from, to)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = false
            };

            smpt.Send(mailMessage);
        }
    }
}
