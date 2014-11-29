using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "BundledAccounts", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class BundledAccount : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("PrimarySipAccount")]
        public string PrimarySipAccount { get; set; }

        [DbColumn("AssociatedSipAccount")]
        public List<string> AssociatedSipAccounts { get; set; }


        //
        // Relations
        [DataRelation(Name = "PrimarySipAccount_Users.SipAccount", WithDataModel = typeof(User), OnDataModelKey = "SipAccount", ThisKey = "PrimarySipAccount")]
        public User PrimaryUserAccount { get; set; }
    }

}
