using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Pools", Type = GLOBALS.DataSource.Type.DBTable,
        AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class Pool : DataModel
    {
        [IsIDField]
        [DbColumn("PoolID")]
        public int ID { set; get; }

        [DbColumn("PoolFQDN")]
        public string FQDN { set; get; }
    }
}