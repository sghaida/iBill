using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Sites_Departments", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class SiteDepartment : DataModel
    {
        [IsIdField]
        [DbColumn("ID")]
        public int Id { get; set; }

        [IsForeignKey]
        [DbColumn("SiteID")]
        public int SiteId { get; set; }

        [IsForeignKey]
        [DbColumn("DepartmentID")]
        public int DepartmentId { get; set; }

        [DataRelation(WithDataModel = typeof (Site), OnDataModelKey = "Id", ThisKey = "SiteId")]
        public Site Site { get; set; }

        [DataRelation(WithDataModel = typeof (Department), OnDataModelKey = "Id", ThisKey = "DepartmentId")]
        public Department Department { get; set; }
    }
}