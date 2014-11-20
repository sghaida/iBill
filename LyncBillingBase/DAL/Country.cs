using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.Helpers;

namespace LyncBillingBase.DAL
{
    [DataSource(Name = "Countries", SourceType = Enums.DataSources.DBTable)]
    public class Country
    {
        //TODO: ADD ID COLUMN AND CLASS PROPERTY

        [DbColumn("CountryName")]
        public string CountryName { get; set; }

        [DbColumn("CountryCodeISO2")]
        public string CountryCodeISO2 { get; set; }

        [DbColumn("CountryCodeISO3")]
        public string CountryCodeISO3 { get; set; }

        [DbColumn("CurrencyName")]
        public string CurrencyName { get; set; }

        [DbColumn("CurrencyISOName")]
        public string CurrencyISOName { get; set; }
    }
}
