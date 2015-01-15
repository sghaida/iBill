using CCC.ORM;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MonitoringServersInfo", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class CallsSummaryForSite : CallsSummary
    {
        [DbColumn("SiteName")]
        public string SiteName { get; set; }
    }
}
