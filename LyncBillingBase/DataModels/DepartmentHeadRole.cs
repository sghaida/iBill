using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Roles_DepartmentsHeads", Type = Globals.DataSource.Type.DBTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class DepartmentHeadRole : DataModel
    {
        [IsIdField]
        [DbColumn("ID")]
        public int Id { get; set; }

        [DbColumn("SiteDepartmentID")]
        public int SiteDepartmentId { get; set; }

        [DbColumn("SipAccount")]
        public string SipAccount { get; set; }

        //
        // Relations
        [DataRelation(WithDataModel = typeof (User), OnDataModelKey = "SipAccount", ThisKey = "SipAccount")]
        public User User { get; set; }

        [DataRelation(WithDataModel = typeof (SiteDepartment), OnDataModelKey = "ID", ThisKey = "SiteDepartmentID")]
        public SiteDepartment SiteDepartment { get; set; }
    }
}