using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Rates", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSoyurceAccessType.Distributed)]
    public class Rate
    {
        public int RateID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public decimal FixedLineRate { get; set; }
        public decimal MobileLineRate { get; set; }
    }
}
