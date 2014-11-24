using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NumberingPlan", SourceType = Enums.DataSourceType.DBTable)]
    public class NumberingPlan
    {
        [IsIDField]
        [DbColumn("Dialing_prefix")]
        public Int64 DialingPrefix { get; set; }

        [DbColumn("Country_Name")]
        public string CountryName { get; set; }

        [DbColumn("Two_Digits_country_code")]
        public string TwoDigitsCountryCode { get; set; }

        [DbColumn("Three_Digits_Country_Code")]
        public string ThreeDigitsCountryCode { get; set; }

        [AllowNull]
        [DbColumn("City")]
        public string City { get; set; }

        [AllowNull]
        [DbColumn("Provider")]
        public string Provider { get; set; }

        [AllowNull]
        [DbColumn("Type_Of_Service")]
        public string TypeOfService { get; set; }
    }
}
