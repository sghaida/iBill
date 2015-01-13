using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Roles", Type = GLOBALS.DataSource.Type.DBTable,
        AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class Role : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("RoleID")]
        public int RoleID { get; set; }

        [DbColumn("RoleName")]
        public string RoleName { get; set; }

        [DbColumn("RoleDescription")]
        public string RoleDescription { get; set; }
    }
}