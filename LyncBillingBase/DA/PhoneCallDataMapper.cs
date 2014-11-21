using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.DAL;
using LyncBillingBase.LookupTables;

namespace LyncBillingBase.DA
{
    public class PhoneCallDataMapper : DistributedDataAccess<PhoneCall>
    {
        DataAccess<MonitoringServerInfo> servers;

        public PhoneCallDataMapper() : base()
        {
            servers = new DataAccess<MonitoringServerInfo>();
        }

        public override List<string> GetTablesList()
        {
            //List<string> tables = new List<string>();

            //tables.Add("PhoneCalls2010");
            //tables.Add("PhoneCalls2013");

            //return tables;

            var serversData = servers.GetAll().ToList();
            var phoneCallsTables = serversData.Select<MonitoringServerInfo, string>(server => server.PhoneCallsTable).ToList<string>();

            return phoneCallsTables;
        }
    
    }

}
