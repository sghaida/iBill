using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class MonitoringServersDataMapper : DataAccess<MonitoringServerInfo>
    {
        private static List<MonitoringServerInfo> _monitoringServers = new List<MonitoringServerInfo>();

        public MonitoringServersDataMapper()
        {
            LoadMonitoringServersInfo();
        }

        private void LoadMonitoringServersInfo()
        {
            if (_monitoringServers == null || _monitoringServers.Count == 0)
            {
                _monitoringServers = base.GetAll().ToList();
            }
        }

        public string CreateConnectionString(MonitoringServerInfo monInfo)
        {
            string connectionString = null;

            if (monInfo.InstanceName != null)
            {
                connectionString =
                    String.Format(
                        "Provider=SQLOLEDB.1;Data Source={0}\\{1};Persist Security Info=True;User ID={2};Password='{3}';Initial Catalog={4}",
                        monInfo.InstanceHostName,
                        monInfo.InstanceName,
                        monInfo.Username,
                        monInfo.Password,
                        monInfo.DatabaseName);
            }
            else
            {
                connectionString =
                    String.Format(
                        "Provider=SQLOLEDB.1;Data Source={0};Persist Security Info=True;User ID={1};Password='{2}';Initial Catalog={3}",
                        monInfo.InstanceHostName,
                        monInfo.Username,
                        monInfo.Password,
                        monInfo.DatabaseName);
            }

            return connectionString;
        }

        /// <summary>
        ///     Return a dictionary containing all the monitoring server's info, accessible by the TelephonySolutionName field of
        ///     each MonitoringServer.
        /// </summary>
        /// <returns>Dictionary<string, MonitoringServerInfo></returns>
        public Dictionary<string, MonitoringServerInfo> GetMonitoringServersInfoMap()
        {
            Dictionary<string, MonitoringServerInfo> monitoringServersInfoMap = null;

            try
            {
                if (_monitoringServers.Count > 0)
                {
                    monitoringServersInfoMap = new Dictionary<string, MonitoringServerInfo>();

                    Parallel.ForEach(
                        _monitoringServers,
                        server =>
                        {
                            lock (monitoringServersInfoMap)
                            {
                                if (server != null && !string.IsNullOrEmpty(server.TelephonySolutionName))
                                {
                                    if (false == monitoringServersInfoMap.Keys.Contains(server.TelephonySolutionName))
                                    {
                                        monitoringServersInfoMap.Add(server.TelephonySolutionName, server);
                                    }
                                    else
                                    {
                                        monitoringServersInfoMap[server.TelephonySolutionName] = server;
                                    }
                                }
                            }
                        });
                    //end-of-parallel-foreach
                }

                return monitoringServersInfoMap;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<MonitoringServerInfo> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _monitoringServers;
        }

        public override int Insert(MonitoringServerInfo dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _monitoringServers.Contains(dataObject);
            var itExists = _monitoringServers.Exists(
                item =>
                    item.InstanceHostName == dataObject.InstanceHostName &&
                    item.InstanceName == dataObject.InstanceName &&
                    item.DatabaseName == dataObject.DatabaseName &&
                    item.PhoneCallsTable == dataObject.PhoneCallsTable
                );


            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);
            _monitoringServers.Add(dataObject);

            return dataObject.Id;
        }

        public override bool Update(MonitoringServerInfo dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var server = _monitoringServers.Find(item => item.Id == dataObject.Id);

            if (server != null)
            {
                _monitoringServers.Remove(server);
                _monitoringServers.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(MonitoringServerInfo dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var server = _monitoringServers.Find(item => item.Id == dataObject.Id);

            if (server != null)
            {
                _monitoringServers.Remove(server);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}