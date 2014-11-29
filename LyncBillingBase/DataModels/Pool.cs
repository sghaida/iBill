using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Pools", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class Pool : DataModel
    {
        [IsIDField]
        [DbColumn("PoolID")]
        public int ID { set; get; }

        [DbColumn("PoolFQDN")]
        public string FQDN { set; get; }
    }
}
