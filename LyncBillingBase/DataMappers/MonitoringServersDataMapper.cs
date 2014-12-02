using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class MonitoringServersDataMapper : DataAccess<MonitoringServerInfo>
    {
        public string CreateConnectionString(MonitoringServerInfo monInfo)
        {
            throw new NotImplementedException();

            //string ConnectionString = null;

            //if (monInfo.InstanceName != null)
            //{
            //    ConnectionString = String.Format("Provider=SQLOLEDB.1;Data Source={0}\\{1};Persist Security Info=True;User ID={2};Password='{3}';Initial Catalog={4}",
            //        monInfo.InstanceHostName,
            //        monInfo.InstanceName,
            //        monInfo.Username,
            //        monInfo.Password,
            //        monInfo.DatabaseName);
            //}
            //else
            //{
            //    ConnectionString = String.Format("Provider=SQLOLEDB.1;Data Source={0};Persist Security Info=True;User ID={1};Password='{2}';Initial Catalog={3}",
            //        monInfo.InstanceHostName,
            //        monInfo.Username,
            //        monInfo.Password,
            //        monInfo.DatabaseName);
            //}

            //return ConnectionString;
        }

        public Dictionary<string, MonitoringServerInfo> GetMonitoringServersInfo()
        {
            // Sample data: 
            // * monInfo is MonitoringServerInfo object.
            // Dictionary Structure:
            // * Key = monInfo.TelephonySolutionName
            // * Value = monInfo

            throw new NotImplementedException();
        }

    }
}
