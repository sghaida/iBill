using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "GatewaysRates", Type = Globals.DataSource.Type.DBTable,
        AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class Rate : DataModel
    {
        [IsIdField]
        [DbColumn("Rate_ID")]
        public long RateId { get; set; }

        [DbColumn("country_code_dialing_prefix")]
        public long DialingCode { get; set; }

        [DbColumn("rate")]
        public decimal Price { get; set; }

        //
        // Relations
        [DataRelation(WithDataModel = typeof (NumberingPlan), OnDataModelKey = "DialingPrefix", ThisKey = "DialingCode")
        ]
        public NumberingPlan NumberingPlan { get; set; }
    }
}