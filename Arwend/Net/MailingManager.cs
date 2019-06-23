using System.Net;
using System.Net.Mail;

namespace Arwend.Net
{
    public static class MailingManager
    {
        public static void SendMail(string[] recipients, string subject, string body, string attachmentFilePath = "")
        {
            if (!ConfigurationManager.MailingEnabled) return;

            var smtpClient = new SmtpClient();
            if (ConfigurationManager.SmtpRequiresAuthentication && !string.IsNullOrEmpty(ConfigurationManager.SmtpServer) && !string.IsNullOrEmpty(ConfigurationManager.SmtpUser))
            {
                var credential = new NetworkCredential
                {
                    UserName = ConfigurationManager.SmtpUser,
                    Password = ConfigurationManager.SmtpPassword
                };
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = credential;
            }
            var signature = $"<br /><center><font size=\"2\">Bu e-posta {ConfigurationManager.DomainUrl?.Host.Replace("www.", "")} tarafından gönderilmiştir.</font></center>";

            var mail = new MailMessage();
            if (!string.IsNullOrEmpty(attachmentFilePath))
                mail.Attachments.Add(new Attachment(attachmentFilePath, "text/plain"));
            try
            {
                var fromAddress = new MailAddress(ConfigurationManager.MailFrom, "Business Management System");

                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.Never;
                mail.From = fromAddress;

                if (!string.IsNullOrEmpty(ConfigurationManager.MailRecipients))
                {
                    var defaultRecipients = ConfigurationManager.MailRecipients.Split(';');
                    if (defaultRecipients.Length > 0)
                        foreach (var toAddress in defaultRecipients)
                            mail.To.Add(toAddress);
                }

                if (recipients != null && recipients.Length > 0)
                    foreach (var toAddress in recipients)
                        mail.To.Add(toAddress);

                if (mail.To.Count == 0)
                    mail.To.Add(ConfigurationManager.MailFrom);

                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body + signature;
                mail.Priority = MailPriority.High;
                mail.Headers.Add("Reply-To", ConfigurationManager.MailFrom);
                smtpClient.Host = ConfigurationManager.SmtpServer;
                smtpClient.Port = ConfigurationManager.SmtpPort;
                smtpClient.EnableSsl = ConfigurationManager.SmtpRequiresSsl;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mail);
            }
            catch
            {
                // ignored
            }
        }
    }
}