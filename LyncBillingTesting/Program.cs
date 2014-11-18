using LyncBillingBase.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;
using LyncBillingBase.Repository;
using System.Linq.Expressions;

namespace LyncBillingTesting
{
    class Program
    {
        static void Main(string[] args)
        {
           // List<PhoneCalls> phoneCalls = PhoneCalls.GetPhoneCalls("sghaida@ccc.gr");

            var dbContext = new Repository<Announcements>();

            Announcements ann = new Announcements();
            ann.Announcement = "tezi tezi";
            ann.ForRole = 2;
            ann.ForSite = 2;

            ann.PublishOn = DateTime.Now;

            ann.ID = dbContext.Insert(ann);
            
            dbContext.Delete(ann);

            Expression<Func<Announcements, bool>> func1 = (item) => ((item.ForRole == 2) && (item.ForSite == 2) && (item.Announcement == "tezi tezi") || (item.ID==1));
            dbContext.Delete(ann, func1);

            


        }
    }
}
