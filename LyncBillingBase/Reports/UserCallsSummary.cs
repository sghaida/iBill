using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Reports
{
    public class UserCallsSummary : DetailedReport
    {
        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public string SipAccount { get; set; }
        public string SiteName { get; set; }
        public string AC_IsInvoiced { get; set; }

        public long Duration { get; set; }
    }
}
