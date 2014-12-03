using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "GatewaysRates", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class GatewayRate : DataModel
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


        //TODO: Refactor the table to include a Currency ID foreign key rather than a Currency ISO Code.
        //[AllowNull]
        //[DbColumn("CurrencyID")]
        //public int CurrencyID { get; set; }


        //
        // Relations
        [DataRelation(WithDataModel = typeof(Gateway), OnDataModelKey = "ID", ThisKey = "GatewayID")]
        public Gateway GatewayData { get; set; }

        //[DataRelation(Name = "CurrencyID_Currency.ID", WithDataModel = typeof(Currency), OnDataModelKey = "ID", ThisKey = "CurrencyID")]
        //public Currency RatesCurrency { get; set; }
    }
}
