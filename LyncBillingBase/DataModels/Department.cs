using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Departments", Type = Globals.DataSource.Type.DBTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class Department : DataModel
    {
        [IsIdField]
        [DbColumn("ID")]
        public int Id { get; set; }

        [DbColumn("DepartmentName")]
        public string Name { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { get; set; }
    }
}