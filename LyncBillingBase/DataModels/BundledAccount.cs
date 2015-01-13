using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ORM;
using ORM.DataAccess;
using ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "BundledAccounts", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class BundledAccount : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("PrimarySipAccount")]
        public string PrimarySipAccount { get; set; }

        [DbColumn("AssociatedSipAccount")]
        public string AssociatedSipAccount { get; set; }


        //
        // Relations
        [DataRelation(WithDataModel = typeof(User), OnDataModelKey = "SipAccount", ThisKey = "PrimarySipAccount")]
        public User PrimaryUserAccount { get; set; }
    }

}
