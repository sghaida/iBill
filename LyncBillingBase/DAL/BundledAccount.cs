using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DAL
{
    [DataSource(Name = "BundledAccounts", SourceType = Enums.DataSourceType.DBTable)]
    public class BundledAccount
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("PrimarySipAccount")]
        public string PrimarySipAccount { get; set; }

        [DbColumn("AssociatedSipAccount")]
        public List<string> AssociatedSipAccounts { get; set; }

        public User PrimaryUserAccount { get; set; }



        /***
         * Custom Functions
         */
        public static List<string> GetAssociatedSipAccounts(string primarySipAccount)
        {
            throw new NotImplementedException();
        }

    }
}
