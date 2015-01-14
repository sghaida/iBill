using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NEW_Roles_Delegates", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class DelegateRole : DataModel
    {
        [IsIdField]
        [DbColumn("ID")]
        public int Id { set; get; }

        [DbColumn("DelegationType")]
        public int DelegationType { get; set; }

        [DbColumn("DelegeeSipAccount")]
        public string DelegeeSipAccount { get; set; }

        [AllowNull]
        [DbColumn("ManagedUserSipAccount")]
        public string ManagedUserSipAccount { get; set; }

        [AllowNull]
        [IsForeignKey]
        [DbColumn("ManagedSiteDepartmentID")]
        public int ManagedSiteDepartmentId { get; set; }

        [AllowNull]
        [IsForeignKey]
        [DbColumn("ManagedSiteID")]
        public int ManagedSiteId { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { get; set; }

        //
        // Relations
        [DataRelation(WithDataModel = typeof (User), OnDataModelKey = "SipAccount", ThisKey = "DelegeeSipAccount",
            RelationType = Globals.DataRelation.Type.Union)]
        public User DelegeeAccount { get; set; }

        [DataRelation(WithDataModel = typeof (User), OnDataModelKey = "SipAccount", ThisKey = "ManagedUserSipAccount",
            RelationType = Globals.DataRelation.Type.Union)]
        public User ManagedUser { get; set; }

        [DataRelation(WithDataModel = typeof (SiteDepartment), OnDataModelKey = "Id",
            ThisKey = "ManagedSiteDepartmentId" , RelationType = Globals.DataRelation.Type.Union )]
        public SiteDepartment ManagedSiteDepartment { get; set; }

        [DataRelation(WithDataModel = typeof (Site), OnDataModelKey = "Id", ThisKey = "ManagedSiteId",
            RelationType = Globals.DataRelation.Type.Union)]
        public Site ManagedSite { get; set; }
    }
}