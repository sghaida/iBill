using CCC.ORM;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MonitoringServersInfo", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class CallsSummaryForUser : CallsSummary
    {
        [DbColumn("UserId")]
        public int UserId { get; set; }

        [DbColumn("ChargingParty")]
        public string UserSipAccount { get; set; }

        [DbColumn("UserName")]
        public string UserName { get; set; }

        [DbColumn("UserDepartment")]
        public string UserDepartment { get; set; }

        [DbColumn("IsInvoiced")]
        public string IsInvoiced { get; set; }
    }

}