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
    public class Rates_National : DataModel
    {
        [IsIDField]
        [DbColumn("Rate_ID")]
        public Int64 RateID { get; set; }

        [DbColumn("ISO3CountryCode")]
        public string CountryCode { get; set; }

        [DbColumn("CountryName")]
        public string CountryName { get; set; }

        [DbColumn("DialingCode")]
        public Int64 DialingCode { get; set; }

        [DbColumn("TypeOfService")]
        public string TypeOfService { get; set; }

        [DbColumn("Rate")]
        public decimal Rate { get; set; }
    }
}
