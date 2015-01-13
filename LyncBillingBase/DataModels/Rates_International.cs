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
