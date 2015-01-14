using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "GatewaysRates", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class RatesInternational : DataModel
    {
        [IsIdField]
        [DbColumn("ISO3CountryCode")]
        public string Iso3CountryCode { get; set; }

        [DbColumn("ISO2CountryCode")]
        public string Iso2CountryCode { get; set; }

        [DbColumn("CountryName")]
        public string CountryName { get; set; }

        [DbColumn("FixedLineRate")]
        public decimal FixedLineRate { get; set; }

        [DbColumn("MobileLineRate")]
        public decimal MobileLineRate { get; set; }
    }
}