namespace LyncBillingBase.Reports
{
    public class UserCallsSummary : DetailedReport
    {
        public string EmployeeId { get; set; }
        public string FullName { get; set; }
        public string SipAccount { get; set; }
        public string SiteName { get; set; }
        public string AcIsInvoiced { get; set; }
        public long Duration { get; set; }
    }
}