using LyncBillingBase.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Sites", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
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

        [DbColumn("CountryId")]
        public string CountryId { get; set; }

        [DataRelation(Name = "Site.CountryID_Country.ID", SourceDataModel = typeof(Country), SourceKeyName = "ID", LocalKeyName = "CountryId")]
        public Country SiteCountry { get; set; }
    }
}
