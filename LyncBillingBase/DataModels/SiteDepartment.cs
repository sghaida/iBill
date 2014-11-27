using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Sites_Departments", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class SiteDepartment : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("SiteID")]
        public int SiteID { get; set; }

        [DbColumn("DepartmentID")]
        public int DepartmentID { get; set; }

        [DataRelation(Name = "SiteID_Site.ID", WithDataModel = typeof(Site), OnDataModelKey = "ID", ThisKey = "SiteID")]
        public Site site { get; set; }

        [DataRelation(Name = "DepartmentID_Department.ID", WithDataModel = typeof(Department), OnDataModelKey = "ID", ThisKey = "DepartmentID")]
        public Department department { get; set; }
    }
}
