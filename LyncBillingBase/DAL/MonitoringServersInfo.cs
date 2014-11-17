using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class MonitoringServersInfo
    {
        public int Id { get; set; }
        public string InstanceHostName { get; set; }
        public string InstanceName { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TelephonySolutionName { get; set; }
        public string PhoneCallsTable { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
