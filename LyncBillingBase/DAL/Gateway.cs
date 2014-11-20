using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DAL
{
    [DataSource(DataSourceName = "Gateways", DataSource = Enums.DataSources.DBTable)]
    public class Gateway
    {
        [IsIDField]
        [DbColumn("GatewayId")]
        public int ID { get; set; }

        [DbColumn("Gateway")]
        public string Name { get; set; }
    }
}
