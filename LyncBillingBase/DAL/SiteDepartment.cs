using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DAL
{
    [DataSource(DataSourceName="Sites_Departments",DataSource=Enums.DataSources.DBTable)]
    public class SiteDepartment
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("SiteID")]
        public int SiteID { get; set; }

        [DbColumn("DepartmentID")]
        public int DepartmentID { get; set; }
    }
}
