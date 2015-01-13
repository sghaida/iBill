using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "GatewaysRates", Type = Globals.DataSource.Type.DBTable,
        AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class RateForNgn : DataModel
    {
        [IsIdField]
        [DbColumn("RateID")]
        public int Id { get; set; }

        [DbColumn("DialingCodeID")]
        public Int64 DialingCodeId { get; set; }

        [DbColumn("Rate")]
        public decimal Rate { get; set; }

        //
        // Relations
        [DataRelation(WithDataModel = typeof (NumberingPlanForNgn), OnDataModelKey = "ID", ThisKey = "DialingCodeID")]
        public NumberingPlanForNgn NumberingPlanForNgn { get; set; }
    }
}