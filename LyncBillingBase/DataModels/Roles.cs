using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Roles", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class Role : DataModel
    {
        [DbColumn("ID")]
        public int Id { get; set; }

        [IsIdField]
        [DbColumn("RoleID")]
        public int RoleId { get; set; }

        [DbColumn("RoleName")]
        public string RoleName { get; set; }

        [DbColumn("RoleDescription")]
        public string RoleDescription { get; set; }
    }
}