using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Reports
{
    public class DetailedReport
    {
        public int BusinessCallsCount { get; set; }
        public decimal BusinessCallsCost { get; set; }
        public int BusinessCallsDuration { get; set; }
        public int PersonalCallsCount { get; set; }
        public int PersonalCallsDuration { get; set; }
        public decimal PersonalCallsCost { get; set; }
        public int UnmarkedCallsCount { get; set; }
        public int UnmarkedCallsDuration { get; set; }
        public decimal UnmarkedCallsCost { get; set; }
        public int NumberOfDisputedCalls { get; set; }

        public decimal TotalCallsCost { get; set; }
        public long TotalCallsDuration { get; set; }
        public long TotalCallsCount { get; set; }

        public DateTime Date { set; get; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
