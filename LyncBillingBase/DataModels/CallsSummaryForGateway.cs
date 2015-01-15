using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MonitoringServersInfo", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class CallsSummaryForGateway : CallsSummary
    {
        [DbColumn("GatewayName")]
        public string GatewayName { get; set; }

        [DbColumn("CallsDuration")]
        public long CallsDuration { get; set; }

        [DbColumn("CallsCount")]
        public long CallsCount { get; set; }

        [DbColumn("CallsCost")]
        public decimal CallsCost { get; set; }

        [DbColumn("CallsCountPercentage")]
        public decimal CallsCountPercentage { get; set; }

        [DbColumn("CallsCostPercentage")]
        public decimal CallsCostPercentage { get; set; }

        [DbColumn("CallsDurationPercentage")]
        public decimal CallsDurationPercentage { get; set; }
    }
}
