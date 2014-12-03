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


        /// <summary>
        /// Return a dictionary containing all the monitoring server's info, accessible by the TelephonySolutionName field of each MonitoringServer.
        /// </summary>
        /// <returns>Dictionary<string, MonitoringServerInfo></returns>
        public Dictionary<string, MonitoringServerInfo> GetMonitoringServersInfoMap()
        {
            Dictionary<string, MonitoringServerInfo> monitoringServersInfoMap = null;

            try
            {
                var allServersInfo = base.GetAll().ToList<MonitoringServerInfo>();

                if(allServersInfo != null && allServersInfo.Count > 0)
                {
                    monitoringServersInfoMap = new Dictionary<string, MonitoringServerInfo>();

                    Parallel.ForEach(
                        allServersInfo,
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

    }

}
