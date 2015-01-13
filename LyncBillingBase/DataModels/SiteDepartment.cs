using System;
using System.Collections.Generic;
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
    [DataSource(Name = "Sites_Departments", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class SiteDepartment : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [IsForeignKey]
        [DbColumn("SiteID")]
        public int SiteID { get; set; }

        [IsForeignKey]
        [DbColumn("DepartmentID")]
        public int DepartmentID { get; set; }

        [DataRelation(WithDataModel = typeof(Site), OnDataModelKey = "ID", ThisKey = "SiteID")]
        public Site Site { get; set; }

        [DataRelation(WithDataModel = typeof(Department), OnDataModelKey = "ID", ThisKey = "DepartmentID")]
        public Department Department { get; set; }
    }
}
