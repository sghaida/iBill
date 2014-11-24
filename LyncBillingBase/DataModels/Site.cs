using LyncBillingBase.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Sites", SourceType = Enums.DataSourceType.DBTable)]
    public class Site
    {
        [IsIDField]
        [DbColumn("SiteID")]
        public int ID { get; set; }

        [DbColumn("SiteName")]
        public string Name { get; set; }

        [DbColumn("CountryCode")]
        public string CountryCode { get; set; }

        [DbColumn("Description")]
        public string Description { get; set; }

        public string CountryName { get; set; }
    }
}
