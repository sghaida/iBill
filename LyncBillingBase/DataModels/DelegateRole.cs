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
    [DataSource(Name = "Roles_Delegates", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class DelegateRole : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { set; get; }

        [DbColumn("DelegeeType")]
        public int DelegeeType { get; set; }

        [DbColumn("Delegee")]
        public string DelegeeSipAccount { get; set; }

        [AllowNull]
        [DbColumn("SipAccount")]
        public string SipAccount { get; set; }

        [AllowNull]
        [DbColumn("DepartmentID")]
        public int SiteDepartmentID { get; set; }

        [AllowNull]
        [DbColumn("SiteID")]
        public int SiteID { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { get; set; }


        //
        // Relations
        [DataRelation(Name = "SipAccount_User.SipAccount", WithDataModel = typeof(User), OnDataModelKey = "SipAccount", ThisKey = "SipAccount")]
        public User DelegeeUser { get; set; }

        [DataRelation(Name = "SiteID_Site.ID", WithDataModel = typeof(Site), OnDataModelKey = "ID", ThisKey = "SiteID")]
        public Site DelegeeSite { get; set; }

        [DataRelation(Name = "SiteDepartmentID_SiteDepartment.ID", WithDataModel = typeof(SiteDepartment), OnDataModelKey = "ID", ThisKey = "SiteDepartmentID")]
        public SiteDepartment DelegeeDepartment { get; set; }

    }

}
