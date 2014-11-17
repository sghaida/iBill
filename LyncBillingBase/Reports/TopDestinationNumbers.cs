using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Reports
{
    public class TopDestinationNumbers
    {
        public string PhoneNumber { private set; get; }
        public string UserName { set; get; }
        public long CallsCount { private set; get; }
        public decimal CallsCost { private set; get; }
        public decimal CallsDuration { private set; get; }
    }
}
