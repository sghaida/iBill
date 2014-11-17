using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class NumberingPlanNGN
    {
        public long ID { get; set; }
        public string DialingCode { get; set; }
        public string CountryName { get; set; }
        public string CountryCodeISO3 { get; set; }
        public string Provider { get; set; }
        public int TypeOfServiceID { get; set; }
        public string TypeOfService { get; set; }
        public string Description { get; set; }
    }
}
