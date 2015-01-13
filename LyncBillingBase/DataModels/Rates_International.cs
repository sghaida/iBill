using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "GatewaysRates", Type = GLOBALS.DataSource.Type.DBTable,
        AccessMethod = GLOBALS.DataSource.AccessMethod.DistributedSource)]
    public class Rates_International : DataModel
    {
        [IsIDField]
        [DbColumn("ISO3CountryCode")]
        public string ISO3CountryCode { get; set; }

        [DbColumn("ISO2CountryCode")]
        public string ISO2CountryCode { get; set; }

        [DbColumn("CountryName")]
        public string CountryName { get; set; }

        [DbColumn("FixedLineRate")]
        public decimal FixedLineRate { get; set; }

        [DbColumn("MobileLineRate")]
        public decimal MobileLineRate { get; set; }
    }
}