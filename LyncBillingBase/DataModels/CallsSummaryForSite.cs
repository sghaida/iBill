using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MonitoringServersInfo", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class CallsSummaryForSite : CallsSummary
    {
        [DbColumn("SiteName")]
        public string SiteName { get; set; }


        //
        // Relations
        [DataRelation(WithDataModel = typeof(Site), OnDataModelKey = "Name", ThisKey = "SiteName")]
        public Site Site { get; set; }
    }

}
