using Shared.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace Shared.Helpers
{
    public class EmailHelper
    {
        public static bool SendEmail(EmailConfig emailConfig, string toUserEmail, string subject, string message)
        {
            using var smpt = new SmtpClient
            {
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailConfig.UserName, emailConfig.Password)
            };

            var from = new MailAddress(emailConfig.FromUser);
            var to = new MailAddress(toUserEmail);
            var mailMessage = new MailMessage(from, to)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = false
            };

            try
            {
                smpt.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
