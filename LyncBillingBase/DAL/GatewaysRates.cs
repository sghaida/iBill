using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class GatewaysRates
    {
        public int GatewaysRatesID { set; get; }
        public int GatewayID { set; get; }
        public string RatesTableName { set; get; }
        public string NgnRatesTableName { set; get; }
        public DateTime StartingDate { set; get; }
        public DateTime EndingDate { set; get; }
        public string ProviderName { set; get; }
        public string CurrencyCode { set; get; }
    }
}
