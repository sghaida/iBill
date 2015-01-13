using System.Configuration;
using System.Net.Mail;

namespace CCC.UTILS.Libs
{
    public class Mailer
    {
        private readonly SmtpClient _client = new SmtpClient();
        private readonly string _mailhost = ConfigurationManager.AppSettings["MailHost"];

        private readonly MailAddress _notificationsEmail =
            new MailAddress(ConfigurationManager.AppSettings["NotificationsEmail"]);

        private MailAddress _replyTo = new MailAddress(ConfigurationManager.AppSettings["ReplyTo"]);

        public Mailer(string emailAddress, string templateSubject, string templateBody)
        {
            var mail = new MailMessage(_notificationsEmail.Address, @emailAddress);
            _client.Port = 25;
            _client.DeliveryMethod = SmtpDeliveryMethod.Network;
            _client.UseDefaultCredentials = false;
            _client.Host = _mailhost;

            //mail.ReplyToList = {replyto};
            mail.IsBodyHtml = true;
            mail.Subject = templateSubject;
            mail.Body = templateBody;

            _client.Send(mail);
        }
    }
}