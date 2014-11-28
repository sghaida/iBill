using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NGN_NumberingPlan", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class NumberingPlanNGN
    {
        [IsIDField]
        [DbColumn("ID")]
        public long ID { get; set; }

        [DbColumn("DialingCode")]
        public string DialingCode { get; set; }

        [DbColumn("CountryCodeISO3")]
        public string CountryCodeISO3 { get; set; }

        [AllowNull]
        [DbColumn("Provider")]
        public string Provider { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { get; set; }

        [AllowNull]
        [DbColumn("TypeOfServiceID")]
        public int TypeOfServiceID { get; set; }

        public string CountryName { get; set; }
        public string TypeOfService { get; set; }
    }
}
