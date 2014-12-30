using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "GatewaysRates", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.DistributedSource)]
    public class RateForNGN : DataModel
    {
        [IsIDField]
        [DbColumn("RateID")]
        public int ID { get; set; }

        [DbColumn("DialingCodeID")]
        public Int64 DialingCodeID { get; set; }

        [DbColumn("Rate")]
        public decimal Rate { get; set; }


        //
        // Relations
        [DataRelation(WithDataModel = typeof(NumberingPlanForNGN), OnDataModelKey = "ID", ThisKey = "DialingCodeID")]
        public NumberingPlanForNGN NumberingPlanForNGN { get; set; }
    }
}
