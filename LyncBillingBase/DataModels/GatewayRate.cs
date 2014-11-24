using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "GatewaysRates", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSoyurceAccessType.SingleSource)]
    public class GatewayRate
    {
        [IsIDField]
        [DbColumn("GatewaysRatesID")]
        public int GatewaysRatesID { set; get; }

        [DbColumn("GatewayID")]
        public int GatewayID { set; get; }

        [AllowNull]
        [DbColumn("RatesTableName")]
        public string RatesTableName { set; get; }

        [AllowNull]
        [DbColumn("NgnRatesTableName")]
        public string NgnRatesTableName { set; get; }

        [AllowNull]
        [DbColumn("StartingDate")]
        public DateTime StartingDate { set; get; }

        [AllowNull]
        [DbColumn("EndingDate")]
        public DateTime EndingDate { set; get; }

        [AllowNull]
        [DbColumn("ProviderName")]
        public string ProviderName { set; get; }

        [AllowNull]
        [DbColumn("CurrencyCode")]
        public string CurrencyCode { set; get; }
    }
}
