using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "CallMarkerStatus", Type = Globals.DataSource.Type.DBTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class CallMarkerStatus : DataModel
    {
        [IsIdField]
        [DbColumn("markerId")]
        public int Id { get; set; }

        [DbColumn("phoneCallsTable")]
        public string PhoneCallsTable { get; set; }

        [DbColumn("type")]
        public string Type { get; set; }

        [DbColumn("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}