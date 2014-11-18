using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;
using LyncBillingBase.DAL;

namespace LyncBillingBase.Repository
{
    public class DataStorage
    {
        /***
         * DataStorage Repositories
         */
        public Repository<Announcements> Announcements = new Repository<Announcements>();
        public Repository<MailTemplates> MailTemplates = new Repository<MailTemplates>();


        /***
         * Singleton instance
         */
        private static DataStorage instance;
        private DataStorage() { }

        public static DataStorage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataStorage();
                }

                return instance;
            }
        }
    }
}
