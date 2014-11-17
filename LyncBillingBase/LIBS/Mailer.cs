using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Libs
{
    public class Mailer
    {
        private SmtpClient client = new SmtpClient();
        private string mailhost = ConfigurationManager.AppSettings["MailHost"];
        private MailAddress notificationsEmail = new MailAddress(ConfigurationManager.AppSettings["NotificationsEmail"]);
        private MailAddress replyTo = new MailAddress(ConfigurationManager.AppSettings["ReplyTo"]);

        public Mailer(string emailAddress, string templateSubject, string templateBody)
        {
            MailMessage mail = new MailMessage(notificationsEmail.Address, @emailAddress);
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
