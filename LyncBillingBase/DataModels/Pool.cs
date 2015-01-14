using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Pools", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class Pool : DataModel
    {
        [IsIdField]
        [DbColumn("PoolID")]
        public int Id { set; get; }

        [DbColumn("PoolFQDN")]
        public string Fqdn { set; get; }
    }
}