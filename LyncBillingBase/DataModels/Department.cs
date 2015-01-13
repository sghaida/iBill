using LyncBillingBase.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<< HEAD
using ORM;
using ORM.DataAccess;
using ORM.DataAttributes;
=======
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;
>>>>>>> 4d2825ed2d6c07fa47ef8a534e938e39e0b8f09c

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Departments", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class Department : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("DepartmentName")]
        public string Name { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { get; set; }
    }
}
