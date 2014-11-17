using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Reports
{
    public class GatewayCallsSummary : DetailedReport
    {
        public string GatewayName { get; set; }

        public decimal CallsCountPercentage { get; set; }
        public decimal CallsCostPercentage { get; set; }
        public decimal CallsDurationPercentage { get; set; }
    }
}
