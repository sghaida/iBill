using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "GatewaysRates", Type = Globals.DataSource.Type.DBTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class GatewayRate : DataModel
    {
        [IsIdField]
        [DbColumn("GatewaysRatesID")]
        public int Id { set; get; }

        [DbColumn("GatewayID")]
        public int GatewayId { set; get; }

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
        [DataRelation(WithDataModel = typeof (Gateway), OnDataModelKey = "Id", ThisKey = "GatewayId")]
        public Gateway Gateway { get; set; }
    }
}