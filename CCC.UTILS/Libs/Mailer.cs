using System.Configuration;
using System.Net.Mail;

namespace CCC.UTILS.Libs
{
    public class Mailer
    {
        private readonly SmtpClient client = new SmtpClient();
        private readonly string mailhost = ConfigurationManager.AppSettings["MailHost"];

        private readonly MailAddress notificationsEmail =
            new MailAddress(ConfigurationManager.AppSettings["NotificationsEmail"]);

        private MailAddress replyTo = new MailAddress(ConfigurationManager.AppSettings["ReplyTo"]);

        public Mailer(string emailAddress, string templateSubject, string templateBody)
        {
            var mail = new MailMessage(notificationsEmail.Address, @emailAddress);
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = mailhost;

            //mail.ReplyToList = {replyto};
            mail.IsBodyHtml = true;
            mail.Subject = templateSubject;
            mail.Body = templateBody;

            client.Send(mail);
        }
    }
}