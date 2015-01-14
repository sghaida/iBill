using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Gateways", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class Gateway : DataModel
    {
        [IsIdField]
        [DbColumn("GatewayId")]
        public int Id { get; set; }

        [DbColumn("Gateway")]
        public string Name { get; set; }
    }
}