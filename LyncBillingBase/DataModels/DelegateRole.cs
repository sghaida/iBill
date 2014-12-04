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
    [DataSource(Name = "NEW_Roles_Delegates", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class DelegateRole : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { set; get; }

        [DbColumn("DelegationType")]
        public int DelegationType { get; set; }

        [DbColumn("DelegeeSipAccount")]
        public string DelegeeSipAccount { get; set; }

        [AllowNull]
        [DbColumn("ManagedUserSipAccount")]
        public string ManagedUserSipAccount { get; set; }

        [AllowNull]
        [DbColumn("ManagedSiteDepartmentID")]
        public int ManagedSiteDepartmentID { get; set; }

        [AllowNull]
        [DbColumn("ManagedSiteID")]
        public int ManagedSiteID { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { get; set; }


        //
        // Relations
        [DataRelation(WithDataModel = typeof(User), OnDataModelKey = "SipAccount", ThisKey = "DelegeeSipAccount", RelationType = GLOBALS.DataRelation.Type.UNION)]
        public User DelegeeAccount { get; set; }

        [DataRelation(WithDataModel = typeof(User), OnDataModelKey = "SipAccount", ThisKey = "ManagedUserSipAccount", RelationType = GLOBALS.DataRelation.Type.UNION)]
        public User ManagedUser { get; set; }

        [DataRelation(WithDataModel = typeof(SiteDepartment), OnDataModelKey = "ID", ThisKey = "ManagedSiteDepartmentID", RelationType = GLOBALS.DataRelation.Type.UNION)]
        public SiteDepartment ManagedSiteDepartment { get; set; }

        [DataRelation(WithDataModel = typeof(Site), OnDataModelKey = "ID", ThisKey = "ManagedSiteID", RelationType = GLOBALS.DataRelation.Type.UNION)]
        public Site ManagedSite { get; set; }

    }

}
