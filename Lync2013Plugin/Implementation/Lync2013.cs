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
        private static OleDbConnection SourceDBConnector;
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
                SourceDBConnector = new OleDbConnection(ConstructConnectionString());
                DestinationDBConnector = new OleDbConnection(DBLib.ConnectionString_Lync);
            }
            catch (Exception e) { throw e.InnerException; }
        }

        public void ProcessPhoneCalls()
        {
            PhoneCallsImpl phoneCallsFunc = new PhoneCallsImpl();

            OleDbDataReader dataReader = null;
           
            PhoneCall phoneCallObj = new PhoneCall();
            Dictionary<string, object> phoneCallDic = null;

            var exceptions = new ConcurrentQueue<Exception>();
          
            string column = string.Empty;

            DateTime lastImportedPhoneCallDate = DateTime.MinValue;
            
            //OPEN CONNECTIONS
            SourceDBConnector.Open();
            DestinationDBConnector.Open();

            dataReader = DBRoutines.EXECUTEREADER(SQLs.GetLastImportedPhonecallDate(PhoneCallsTableName,isRemote:false), DestinationDBConnector);

            if (dataReader.Read() && !dataReader.IsDBNull(0))
            {
                lastImportedPhoneCallDate = dataReader.GetDateTime(dataReader.GetOrdinal("SessionIdTime"));
                lastImportedPhoneCallDate = lastImportedPhoneCallDate.AddDays(+1);
            }
            else
            {
                //Table is empty in this case we need to read from the source that we will import the data from
                dataReader = DBRoutines.EXECUTEREADER(SQLs.GetLastImportedPhonecallDate("DialogsView",isRemote:true), SourceDBConnector);

                if (dataReader.Read() && !dataReader.IsDBNull(0))
                {
                    lastImportedPhoneCallDate = dataReader.GetDateTime(dataReader.GetOrdinal("SessionIdTime"));
                }
            }


            while (lastImportedPhoneCallDate <= DateTime.Now)
            {
                //Construct CREATE_IMPORT_PHONE_CALLS_QUERY
                string SQL = SQLs.CreateImportCallsQueryLync2013(lastImportedPhoneCallDate);

                dataReader = DBRoutines.EXECUTEREADER(SQL, SourceDBConnector);

                if (lastImportedPhoneCallDate > DateTime.MinValue)
                    Console.WriteLine("Importing PhoneCalls from " + PhoneCallsTableName + " since " + lastImportedPhoneCallDate);
                else
                    Console.WriteLine("Importing PhoneCalls from " + PhoneCallsTableName + " since the begining");

                DataTable dt = new DataTable();

                //Load data into Datatable
                dt.Load(dataReader);

                if (dt.Rows.Count > 0)
                {
                    //Convert Datatable to list of objects
                    List<PhoneCall> phoneCalls = dt.ConvertToList<PhoneCall>();

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

                    });

                    //phoneCalls.ConvertToDataTable<PhoneCall>().BulkInsert(PhoneCallsTableName);
                    //toBeInserted.BulkInsert(PhoneCallsTableName);

                    //var tempDT = phoneCalls.ConvertToDataTable();


                    foreach (PhoneCall phoneCall in phoneCalls)
                    {
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

                    Console.WriteLine("   [+] Imported: " + phoneCalls.Count + " phone calls.");
                }


                // Increment the datetime object by 1 day.
                lastImportedPhoneCallDate = lastImportedPhoneCallDate.AddDays(+1);


                //GarbageCollect the datatable
                dt.Dispose();
                dataReader.Close();
                dataReader.Dispose();
                GC.Collect();
            }

            //Close All Connection and DataReaders
            SourceDBConnector.Close();
            DestinationDBConnector.Close();

            if (dataReader.IsClosed == false) { dataReader.Close(); }

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
