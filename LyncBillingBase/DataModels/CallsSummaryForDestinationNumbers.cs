using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MonitoringServersInfo", Type = Globals.DataSource.Type.DbTable, AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class CallsSummaryForDestinationNumbers : DataModel
    {
        [IsIdField]
        [DbColumn("PhoneNumber")]
        public string PhoneNumber { set; get; }

        [DbColumn("Country")]
        public string Country { get; set; }

        [DbColumn("CallsCount")]
        public long CallsCount { set; get; }

        [DbColumn("CallsCost")]
        public decimal CallsCost { set; get; }

        [DbColumn("CallsDuration")]
        public long CallsDuration { set; get; }

        public string DestinationContactName { get; set; }
    }
}
