using LyncBillingBase.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Rates", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSoyurceAccessType.Distributed)]
    public class Rates_International
    {
        public int RateID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public decimal FixedLineRate { get; set; }
        public decimal MobileLineRate { get; set; }
    }
}
