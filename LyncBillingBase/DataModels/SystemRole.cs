using LyncBillingBase.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name="Roles_System", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class SystemRole : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { set; get; }

        [DbColumn("SipAccount")]
        public string SipAccount { get; set; }

        [DbColumn("RoleID")]
        public int RoleID { get; set; }

        [AllowNull]
        [DbColumn("SiteID")]
        public int SiteID { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { get; set; }


        //
        // Relations
        [DataRelation(WithDataModel = typeof(Site), OnDataModelKey = "ID", ThisKey = "SiteID")]
        public Site Site { get; set; }

        [DataRelation(WithDataModel = typeof(User), OnDataModelKey = "SipAccount", ThisKey = "SipAccount")]
        public User User { get; set; }
    }
}
