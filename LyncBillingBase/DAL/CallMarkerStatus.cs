using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DAL
{
    [DataSource(Name = "CallMarkerStatus", SourceType = Enums.DataSources.DBTable)]
    public class CallMarkerStatus
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
