using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class Rates_National
    {
        public int RateID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string DialingCode { get; set; }
        public string TypeOfService { get; set; }
        public decimal Rate { get; set; }
    }
}
