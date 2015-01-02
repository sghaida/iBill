using Lync2013Plugin.Interfaces;
using LyncBillingBase.DataModels;
using PhoneCallsProcessor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using System.Data.OleDb;
using LyncBillingBase.DataMappers;
using LyncBillingBase;
using System.Data;
using LyncBillingBase.Helpers;
using System.Data.SqlTypes;
using System.Collections.Concurrent;


namespace Lync2013Plugin.Implementation
{
    class Lync2013 : ICallProcessor
    {
        private static DBLib DBRoutines = new DBLib();
        private static OleDbConnection sourceDBConnector;
        private static OleDbConnection DestinationDBConnector;

        private ENUMS enums = new ENUMS();
        
        private string PhoneCallsTableName = string.Empty;
        
        public string Name
        {
            get { return "Lync2013"; }
        }

        public string Description
        {
            get { return "Imports and Process Lync 2013 Logs"; }
        }

        public string Version
        {
            get { return "1.0.0"; }
        }

        public Lync2013() 
        {
            try
            {
                sourceDBConnector = new OleDbConnection(ConstructConnectionString());
                DestinationDBConnector = new OleDbConnection(DBLib.ConnectionString_Lync);
            }
            catch (Exception e) { throw e.InnerException; }
        }

        public void ProcessPhoneCalls()
        {
            PhoneCallsImpl phoneCallsFunc = new PhoneCallsImpl();

            OleDbDataReader dataReader = null;
           

            CallMarkerStatusDataMapper callMarkerStatusMapper = new CallMarkerStatusDataMapper();
            PhoneCallsDataMapper PhoneCallsMapper = new PhoneCallsDataMapper();

            PhoneCall phoneCallObj = new PhoneCall();
            Dictionary<string, object> phoneCallDic = null;

            int dataRowCounter = 0;
          
            string column = string.Empty;

            DateTime lastImportedPhoneCallDate = DateTime.MinValue;
            

            //OPEN CONNECTIONS
            sourceDBConnector.Open();
            DestinationDBConnector.Open();

            dataReader = DBRoutines.EXECUTEREADER(SQLs.GetLastImportedPhonecallDate(PhoneCallsTableName), DestinationDBConnector);


            if (dataReader.Read() && !dataReader.IsDBNull(0))
                lastImportedPhoneCallDate = dataReader.GetDateTime(dataReader.GetOrdinal("SessionIdTime"));
            else
                lastImportedPhoneCallDate = DateTime.MinValue;


            while (lastImportedPhoneCallDate <= DateTime.Now.AddDays(-1)) 
            {
                //Construct CREATE_IMPORT_PHONE_CALLS_QUERY
                string SQL = SQLs.CreateImportCallsQueryLync2013(lastImportedPhoneCallDate);

                dataReader = DBRoutines.EXECUTEREADER(SQL, sourceDBConnector);

                if (lastImportedPhoneCallDate > DateTime.MinValue)
                    Console.WriteLine("Importing PhoneCalls from " + PhoneCallsTableName + "since " + lastImportedPhoneCallDate);
                else
                    Console.WriteLine("Importing PhoneCalls from " + PhoneCallsTableName + "since the begining");

                DataTable dt = new DataTable();

                //Load data into Datatable
                dt.Load(dataReader);

                //Convert Datatable to list of objects
                List<PhoneCall> phoneCalls = dt.ConvertToList<PhoneCall>();
                List<PhoneCall> toBeInserted = new List<PhoneCall>();

                object status = new object();

                //To continue the loop even if exception was presented
                var exceptions = new ConcurrentQueue<Exception>();

                Parallel.ForEach(phoneCalls, (phoneCall) => 
                {
                    //Set Initial Charging Party Part
                    if (!string.IsNullOrEmpty(phoneCallObj.ReferredBy))
                    {
                        phoneCall.ChargingParty = phoneCall.ReferredBy;
                    }
                    else if (!string.IsNullOrEmpty(phoneCall.SourceUserUri))
                    {
                        phoneCall.ChargingParty = phoneCall.SourceUserUri;
                    }

                    phoneCallsFunc.SetCallType(phoneCall);
                    phoneCallsFunc.ApplyRate(phoneCall);
                    phoneCallsFunc.ApplyExceptions(phoneCall);

                    //Fix DateMin Casting to SQL DateMin
                    //if (phoneCall.SessionEndTime == DateTime.MinValue)
                    //    phoneCall.SessionEndTime = SqlDateTime.MinValue.Value;

                    //if (phoneCall.ResponseTime == DateTime.MinValue)
                    //    phoneCall.ResponseTime = SqlDateTime.MinValue.Value;

                    //if (phoneCall.AC_DisputeResolvedOn == DateTime.MinValue)
                    //    phoneCall.AC_DisputeResolvedOn = SqlDateTime.MinValue.Value;

                    //if (phoneCall.AC_InvoiceDate == DateTime.MinValue)
                    //    phoneCall.AC_InvoiceDate = SqlDateTime.MinValue.Value;

                    //if (phoneCall.UI_AssignedOn == DateTime.MinValue)
                    //    phoneCall.UI_AssignedOn = SqlDateTime.MinValue.Value;

                    //if (phoneCall.UI_MarkedOn == DateTime.MinValue)
                    //    phoneCall.UI_MarkedOn = SqlDateTime.MinValue.Value;

                    lock (status) 
                    {
                        //toBeInserted.Add(phoneCall);

                        //This part should be removed after fixing the bulk insert
                        phoneCallDic = Helpers.ConvertPhoneCallToDictionary(phoneCall);

                        try
                        {
                            DBRoutines.INSERT(PhoneCallsTableName, phoneCallDic);
                        }
                        catch (Exception e)
                        {
                            exceptions.Enqueue(e);
                        }
                    }
                });

                //toBeInserted.ConvertToDataTable<PhoneCall>().BulkInsert(PhoneCallsTableName);
                //toBeInserted.BulkInsert(PhoneCallsTableName);

               

                //Refresh the date
                dataReader = DBRoutines.EXECUTEREADER(SQLs.GetLastImportedPhonecallDate(PhoneCallsTableName), DestinationDBConnector);
                
                if (dataReader.Read() && !dataReader.IsDBNull(0))
                    lastImportedPhoneCallDate = dataReader.GetDateTime(dataReader.GetOrdinal("SessionIdTime"));

            }

            sourceDBConnector.Close();

            Console.WriteLine("Finish importing Calls from " + PhoneCallsTableName);

          

            //while (dataReader.Read())
            //{
            //    phoneCallObj = Helpers.FillPhoneCallFromOleDataReader(dataReader);

            //    //Set Initial Charging Party Part
            //    if (!string.IsNullOrEmpty(phoneCallObj.ReferredBy))
            //    {
            //        phoneCallObj.ChargingParty = phoneCallObj.ReferredBy;
            //    }
            //    else if (!string.IsNullOrEmpty(phoneCallObj.SourceUserUri))
            //    {
            //        phoneCallObj.ChargingParty = phoneCallObj.SourceUserUri;
            //    }

            //    phoneCallsFunc.SetCallType(phoneCallObj);
            //    phoneCallsFunc.ApplyRate(phoneCallObj);
            //    phoneCallsFunc.ApplyExceptions(phoneCallObj);

            //    lastMarkedPhoneCallDate = phoneCallObj.SessionIdTime.ToLongDateString();

            //    phoneCallDic = Helpers.ConvertPhoneCallToDictionary(phoneCallObj);

            //    try
            //    {
                    
            //        //This Solution is better because of the overhead of reflection for millions of records
            //        DBRoutines.INSERT(PhoneCallsTableName, phoneCallDic);

            //        //Update the CallMarkerStatus table fro this PhoneCall table.
            //        if (dataRowCounter % 10000 == 0)
            //        {
            //            callMarkerStatusMapper.UpdateCallMarkerStatus(PhoneCallsTableName, "Marking", lastMarkedPhoneCallDate);
            //            callMarkerStatusMapper.UpdateCallMarkerStatus(PhoneCallsTableName, "ApplyingRates", lastMarkedPhoneCallDate);
            //        }

            //        dataRowCounter += 1;
            //    }
            //    catch (Exception e)
            //    {
            //        continue;
            //    }

            //}

            sourceDBConnector.Close();

            Console.WriteLine("Finish importing Calls from " + PhoneCallsTableName);

        }

        public void PluginInfo()
        {
            Console.WriteLine("Name: {0}", Name);
            Console.WriteLine("Description: {0}", Description);
            Console.WriteLine("Version: {0}", Version);
            Console.Write("DB Connection: {0}", ConstructConnectionString());

        }

        private string ConstructConnectionString() 
        {
            MonitoringServersDataMapper msDataMapper = new MonitoringServersDataMapper();

            var info = Repo.monitoringServerInfo.Where(item => item.Key == this.GetType().Name).Select(item => (MonitoringServerInfo)item.Value).First() as MonitoringServerInfo;
            PhoneCallsTableName = info.PhoneCallsTable;

            return msDataMapper.CreateConnectionString(info);

        }



        
    }
}
