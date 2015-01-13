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
    public class Rate : DataModel
    {
        [IsIDField]
        [DbColumn("Rate_ID")]
        public long RateID { get; set; }

        [DbColumn("country_code_dialing_prefix")]
        public long DialingCode { get; set; }

        [DbColumn("rate")]
        public decimal Price { get; set; }
                

        //
        // Relations
        [DataRelation(WithDataModel = typeof(NumberingPlan), OnDataModelKey = "DialingPrefix", ThisKey = "DialingCode")]
        public NumberingPlan NumberingPlan { get; set; }
    }

}
