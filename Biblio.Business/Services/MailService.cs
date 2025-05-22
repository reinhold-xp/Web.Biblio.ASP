using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Configuration;

namespace Biblio.Business.Services
{
    public static class MailService
    {
        public static void SendMail(string userEmail, string message)
        {
            var apiKey = ConfigurationManager.AppSettings["MailjetApiKey"];
            var secretKey = ConfigurationManager.AppSettings["MailjetSecretKey"];    
            var client = new SmtpClient("in-v3.mailjet.com", 587)
            {
                Credentials = new NetworkCredential(apiKey, secretKey),
                EnableSsl = true
            };

            var mail = new MailMessage();
            mail.From = new MailAddress(ConfigurationManager.AppSettings["MailjetFrom"]);
            mail.To.Add(ConfigurationManager.AppSettings["MailjetTo"]);
            mail.Subject = "Message Biblio";
            mail.Body = $"Message de : {userEmail}\n\n{message}";
            mail.IsBodyHtml = false;

            client.Send(mail);
        }
    }
}