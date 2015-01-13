using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<< HEAD
using ORM;
using ORM.DataAccess;
using ORM.DataAttributes;
=======
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;
>>>>>>> 4d2825ed2d6c07fa47ef8a534e938e39e0b8f09c

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
