using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class GatewaysDetails
    {
        public int GatewayID { set; get; }
        public int SiteID { set; get; }
        public int PoolID { set; get; }
        public string Description { set; get; }
    }
}
