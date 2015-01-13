using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataMappers;
using PhoneCallsProcessor.Interfaces;

namespace Lync2013Plugin.Implementation
{
    public class Lync2013 : ICallProcessor
    {
        private static readonly DbLib DbRoutines = new DbLib();
        private static OleDbConnection _sourceDbConnector;
        private static OleDbConnection _destinationDbConnector;
        private Enums _enums = new Enums();
        private string _phoneCallsTableName = string.Empty;

        public Lync2013()
        {
            try
            {
                _sourceDbConnector = new OleDbConnection(ConstructConnectionString());
                _destinationDbConnector = new OleDbConnection(DbLib.ConnectionString);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

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

        public void ProcessPhoneCalls()
        {
            var phoneCallsFunc = new PhoneCallsImpl();

            DataTable toBeInsertedDataTable;
            OleDbDataReader dataReader = null;

            var exceptions = new ConcurrentQueue<Exception>();

            var column = string.Empty;

            var lastImportedPhoneCallDate = DateTime.MinValue;

            //OPEN CONNECTIONS
            _sourceDbConnector.Open();
            _destinationDbConnector.Open();

            dataReader = DbRoutines.Executereader(SqLs.GetLastImportedPhonecallDate(_phoneCallsTableName, false),
                _destinationDbConnector);

            if (dataReader.Read() && !dataReader.IsDBNull(0))
            {
                lastImportedPhoneCallDate = dataReader.GetDateTime(dataReader.GetOrdinal("SessionIdTime"));
                lastImportedPhoneCallDate = lastImportedPhoneCallDate.AddDays(+1);

                dataReader.CloseDataReader();
            }
            else
            {
                //Table is empty in this case we need to read from the source that we will import the data from
                dataReader = DbRoutines.Executereader(SqLs.GetLastImportedPhonecallDate("DialogsView", true),
                    _sourceDbConnector);

                if (dataReader.Read() && !dataReader.IsDBNull(0))
                {
                    lastImportedPhoneCallDate = dataReader.GetDateTime(dataReader.GetOrdinal("SessionIdTime"));
                }

                dataReader.CloseDataReader();
            }

            while (lastImportedPhoneCallDate <= DateTime.Now)
            {
                //Construct CREATE_IMPORT_PHONE_CALLS_QUERY
                var sql = SqLs.CreateImportCallsQueryLync2013(lastImportedPhoneCallDate);

                if (lastImportedPhoneCallDate > DateTime.MinValue)
                    Console.WriteLine("Importing PhoneCalls from " + _phoneCallsTableName + " since " +
                                      lastImportedPhoneCallDate);
                else
                    Console.WriteLine("Importing PhoneCalls from " + _phoneCallsTableName + " since the begining");

                //Read DB and map it to List of PhoneCalls
                var phoneCalls =
                    Db.ReadSqlData(DbRoutines.Executereader(sql, _sourceDbConnector), Db.PhoneCallsSelector).ToList();

                if (phoneCalls.Count() > 0)
                {
                    var status = new object();

                    var partitionsize = Partitioner.Create(0, phoneCalls.Count());

                    Parallel.ForEach(partitionsize, (range, loopStet) =>
                    {
                        for (var i = range.Item1; i < range.Item2; i++)
                        {
                            phoneCallsFunc.ProcessPhoneCall(phoneCalls[i]);
                        }
                    });

                    // Bulk insert
                    toBeInsertedDataTable = phoneCalls.ConvertToDataTable();
                    toBeInsertedDataTable.BulkInsert(_phoneCallsTableName, _destinationDbConnector.ConnectionString);

                    toBeInsertedDataTable.Dispose();

                    Console.WriteLine("   [+] Imported: " + phoneCalls.Count + " phone calls.");
                }

                // Increment the datetime object by 1 day.
                lastImportedPhoneCallDate = lastImportedPhoneCallDate.AddDays(+1);

                GC.Collect();
            }

            //Close All Connection and DataReaders
            _sourceDbConnector.Close();
            _destinationDbConnector.Close();

            if (dataReader.IsClosed == false)
            {
                dataReader.Close();
            }

            Console.WriteLine("Finish importing Calls from " + _phoneCallsTableName);
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
            var msDataMapper = new MonitoringServersDataMapper();

            var info =
                Repo.MonitoringServerInfo.Where(item => item.Key == GetType().Name).Select(item => item.Value).First();
            _phoneCallsTableName = info.PhoneCallsTable;

            return msDataMapper.CreateConnectionString(info);
        }
    }
}