using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;






using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;


namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Gateways", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class Gateway : DataModel
    {
        [IsIDField]
        [DbColumn("GatewayId")]
        public int ID { get; set; }

        [DbColumn("Gateway")]
        public string Name { get; set; }
    }
}
