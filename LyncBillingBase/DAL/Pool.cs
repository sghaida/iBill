using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DAL
{
    [TableName("Pools")]
    public class Pool
    {
        [IsIDField]
        [DbColumn("PoolID")]
        public int ID { set; get; }

        [DbColumn("PoolFQDN")]
        public string FQDN { set; get; }
    }
}
