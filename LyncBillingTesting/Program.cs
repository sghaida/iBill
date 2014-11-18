using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DAL;
using LyncBillingBase.Helpers;
using LyncBillingBase.Repository;


namespace LyncBillingTesting
{
    class Program
    {
        public static void Main(string[] args)
        {
            var _dbStorage = DataStorage.Instance;
            var storageTest1 = DataStorage.Instance;
            var storageTest2 = DataStorage.Instance;

            Announcements ann = new Announcements();
            ann.Announcement = "tezi tezi";
            ann.ForRole = 2;
            ann.ForSite = 2;
            ann.PublishOn = DateTime.Now;
            ann.ID = _dbStorage.Announcements.Insert(ann);

            MailTemplates obj = new MailTemplates();
            obj.TemplateBody = string.Empty;
            obj.TemplateSubject = string.Empty;
            obj.TemplateID = _dbStorage.MailTemplates.Insert(obj);

            //Expression<Func<Announcements, bool>> func1 = (item) => ((item.ForRole == 2) && (item.ForSite == 2) && (item.Announcement == "tezi tezi") || (item.ID==1));
        }
    }
}
