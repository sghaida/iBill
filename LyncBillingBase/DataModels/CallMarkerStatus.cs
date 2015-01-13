using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ORM;
using ORM.DataAccess;
using ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "CallMarkerStatus", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class CallMarkerStatus : DataModel
    {
        [IsIDField]
        [DbColumn("markerId")]
        public int ID { get; set; }

        [DbColumn("phoneCallsTable")]
        public string PhoneCallsTable { get; set; }

        [DbColumn("type")]
        public string Type { get; set; }

        [DbColumn("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
