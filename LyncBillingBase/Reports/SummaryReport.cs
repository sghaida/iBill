using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Reports
{
    public interface ISummaryReport
    {
        string Name { get; set; }
        long TotalCalls { get; set; }
        long TotalDuration { get; set; }
        decimal TotalCost { get; set; }
    }

    public class SummaryReport<T> where T : class, ISummaryReport
    {
        public string Name { get; set; }
        public long TotalCalls { get; set; }
        public long TotalDuration { get; set; }
        public decimal TotalCost { get; set; }
    }
}
