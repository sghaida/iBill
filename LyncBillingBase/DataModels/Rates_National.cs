using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "GatewaysRates", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class RatesNational : DataModel
    {
        [IsIdField]
        [DbColumn("Rate_ID")]
        public Int64 RateId { get; set; }

        [DbColumn("ISO3CountryCode")]
        public string CountryCode { get; set; }

        [DbColumn("CountryName")]
        public string CountryName { get; set; }

        [DbColumn("DialingCode")]
        public Int64 DialingCode { get; set; }

        [DbColumn("TypeOfService")]
        public string TypeOfService { get; set; }

        [DbColumn("Rate")]
        public decimal Rate { get; set; }
    }
}