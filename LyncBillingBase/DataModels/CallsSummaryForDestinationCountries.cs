using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MonitoringServersInfo", Type = Globals.DataSource.Type.DbTable, AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class CallsSummaryForDestinationCountries : DataModel
    {
        [IsIdField]
        [DbColumn("Country")]
        public string CountryName { set; get; }

        [DbColumn("CallsCount")]
        public long CallsCount { set; get; }

        [DbColumn("CallsCost")]
        public decimal CallsCost { set; get; }

        [DbColumn("CallsDuration")]
        public long CallsDuration { set; get; }
    }
}
