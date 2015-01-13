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