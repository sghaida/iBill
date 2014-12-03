using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Gateways", SourceType = GLOBALS.DataSourceType.DBTable, AccessType = GLOBALS.DataSourceAccessType.SingleSource)]
    public class Gateway : DataModel
    {
        [IsIDField]
        [DbColumn("GatewayId")]
        public int ID { get; set; }

        [DbColumn("Gateway")]
        public string Name { get; set; }
    }
}
