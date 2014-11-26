using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Sites_Departments", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class SiteDepartment
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("SiteID")]
        [DataRelation(Name = "SiteID_Site.ID", SourceDataModel = typeof(Site), SourceKeyName = "ID", LocalKeyName = "SiteID", IncludeProperties = "Name")]
        public int SiteID { get; set; }

        [DbColumn("DepartmentID")]
        [DataRelation(Name = "DepartmentID_Department.ID", SourceDataModel = typeof(Department), SourceKeyName = "ID", LocalKeyName = "DepartmentID", IncludeProperties = "Name")]
        public int DepartmentID { get; set; }

        [DataMapper(RelationName = "SiteID_Site.ID", SourceDataModel = typeof(Site), SourceDataAttribute = "Name")]
        public string SiteName { get; set; }

        [DataMapper(RelationName = "DepartmentID_Department.ID", SourceDataModel = typeof(Department), SourceDataAttribute = "Name")]
        public string DepartmentName { get; set; }
    }
}
