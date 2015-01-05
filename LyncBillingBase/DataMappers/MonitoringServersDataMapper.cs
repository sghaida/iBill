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
        private static List<MonitoringServerInfo> _MonitoringServers = new List<MonitoringServerInfo>();


        public MonitoringServersDataMapper()
        {
            LoadMonitoringServersInfo();
        }


        private void LoadMonitoringServersInfo()
        {
            if(_MonitoringServers == null || _MonitoringServers.Count == 0)
            { 
                _MonitoringServers = base.GetAll().ToList();
            }
        }


        public string CreateConnectionString(MonitoringServerInfo monInfo)
        {
            string ConnectionString = null;

            if (monInfo.InstanceName != null)
            {
                ConnectionString = String.Format("Provider=SQLOLEDB.1;Data Source={0}\\{1};Persist Security Info=True;User ID={2};Password='{3}';Initial Catalog={4}",
                    monInfo.InstanceHostName,
                    monInfo.InstanceName,
                    monInfo.Username,
                    monInfo.Password,
                    monInfo.DatabaseName);
            }
            else
            {
                ConnectionString = String.Format("Provider=SQLOLEDB.1;Data Source={0};Persist Security Info=True;User ID={1};Password='{2}';Initial Catalog={3}",
                    monInfo.InstanceHostName,
                    monInfo.Username,
                    monInfo.Password,
                    monInfo.DatabaseName);
            }
            
            return ConnectionString;
        }


        /// <summary>
        /// Return a dictionary containing all the monitoring server's info, accessible by the TelephonySolutionName field of each MonitoringServer.
        /// </summary>
        /// <returns>Dictionary<string, MonitoringServerInfo></returns>
        public Dictionary<string, MonitoringServerInfo> GetMonitoringServersInfoMap()
        {
            Dictionary<string, MonitoringServerInfo> monitoringServersInfoMap = null;

            try
            {
                if(_MonitoringServers.Count > 0)
                {
                    monitoringServersInfoMap = new Dictionary<string, MonitoringServerInfo>();

                    Parallel.ForEach(
                        _MonitoringServers,
                        (server) =>
                        {
                            lock(monitoringServersInfoMap)
                            {
                                if(server != null && !string .IsNullOrEmpty(server.TelephonySolutionName))
                                { 
                                    if(false == monitoringServersInfoMap.Keys.Contains(server.TelephonySolutionName))
                                    {
                                        monitoringServersInfoMap.Add(server.TelephonySolutionName, server);
                                    }
                                    else
                                    {
                                        monitoringServersInfoMap[server.TelephonySolutionName]= server;
                                    }
                                }
                            }
                        });
                    //end-of-parallel-foreach
                }

                return monitoringServersInfoMap;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<MonitoringServerInfo> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _MonitoringServers;
        }


        public override int Insert(MonitoringServerInfo dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            bool isContained = _MonitoringServers.Contains(dataObject);
            bool itExists = _MonitoringServers.Exists(
                item => 
                    item.InstanceHostName == dataObject.InstanceHostName &&
                    item.InstanceName == dataObject.InstanceName &&
                    item.DatabaseName == dataObject.DatabaseName &&
                    item.PhoneCallsTable == dataObject.PhoneCallsTable
                );


            if(isContained || itExists)
            {
                return -1;
            }
            else
            {
                dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
                _MonitoringServers.Add(dataObject);

                return dataObject.ID;
            }
        }


        public override bool Update(MonitoringServerInfo dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var server = _MonitoringServers.Find(item => item.ID == dataObject.ID);

            if(server != null)
            {
                _MonitoringServers.Remove(server);
                _MonitoringServers.Add(dataObject);
                
                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false; 
            }
        }


        public override bool Delete(MonitoringServerInfo dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var server = _MonitoringServers.Find(item => item.ID == dataObject.ID);

            if(server != null)
            {
                _MonitoringServers.Remove(server);
                
                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }

    }

}
