using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




using CCC.ORM;


namespace LyncBillingBase.DataModels
{
    public class DialingPrefixRate
    {
        public Int64 RateID { set; get; }
        public Int64 DialingPrefix { set; get; }
        public decimal CountryRate { set; get; }
        public string CountryName { get; set; }
        public string TwoDigitsCountryCode { get; set; }
        public string ThreeDigitsCountryCode { get; set; }
        public string City { get; set; }
        public string Provider { get; set; }
        public string TypeOfService { get; set; }
    }
}
