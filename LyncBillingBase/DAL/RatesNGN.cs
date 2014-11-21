using LyncBillingBase.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    [DataSource(Name = "Rates", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSoyurceAccessType.Distributed)]
    public class RatesNGN
    {
        public int RateID { get; set; }
        public long DialingCodeID { get; set; }
        public string DialingCode { get; set; }
        public string CountryCodeISO3 { get; set; }
        public string CountryName { get; set; }
        public int TypeOfServiceID { get; set; }
        public string CallType { get; set; }
        public decimal Rate { get; set; }
        public string Description { get; set; }
    }
}
