using CCC.ORM;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MonitoringServersInfo", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class CallsSummaryForUser : CallsSummary
    {
        [DbColumn("ChargingParty")]
        public string SipAccount { get; set; }

        [DbColumn("AC_IsInvoiced")]
        public string IsInvoiced { get; set; }

        
        //
        // Relations
        [DataRelation(WithDataModel = typeof (User), OnDataModelKey = "SipAccount", ThisKey = "SipAccount")]
        public User User { get; set; }
    }
}