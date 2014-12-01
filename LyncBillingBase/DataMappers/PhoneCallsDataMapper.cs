using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class PhoneCallsDataMapper : DataAccess<PhoneCall>
    {
        /***
         * Get the phone calls tables list from the MonitoringServersInfo table
         */
        private DataAccess<MonitoringServerInfo> monInfoDA = new DataAccess<MonitoringServerInfo>();

        /**
         * The SQL Queries Generator
         */
        private SQLQueries.PhoneCallsSQL sqlAccessor = new SQLQueries.PhoneCallsSQL();

        /***
         * The list of phone calls tables
         */
        private List<string> dbTables = new List<string>();


        public PhoneCallsDataMapper() : base()
        {
            dbTables = monInfoDA.GetAll().Select(item => item.PhoneCallsTable).ToList<string>();
   
        }

        public int Insert(PhoneCall dataObject, string dataSourceName = null) 
        {
            return base.Insert(dataObject,dataSourceName);
        }

        public bool Update(PhoneCall dataObject, string dataSourceName = null) 
        {
            return base.Update(dataObject,dataSourceName);
        }

        public PhoneCall GetById(long id, string dataSourceName) 
        {
            return base.GetById(id, dataSourceName);
        }

        public IEnumerable<PhoneCall> Get(Dictionary<string, object> where, string dataSourceName, int limit = 25) 
        {
            return base.Get(where, limit, dataSourceName);
        }

        public IEnumerable<PhoneCall> Get(Expression<Func<PhoneCall, bool>> predicate, string dataSourceName)
        {
            return base.Get(predicate, dataSourceName);
        }

        public bool Delete(PhoneCall dataObject, string dataSourceName)
        {
            return base.Delete(dataObject);
        }

        public IEnumerable<PhoneCall> GetAll(string dataSourceName) 
        {
            return base.GetAll(dataSourceName);
        }

        public IEnumerable<PhoneCall> GetChargableCallsPerUser(string sipAccount) 
        {
            string sqlStatemnet = sqlAccessor.ChargableCallsPerUser(dbTables, sipAccount);

            return base.GetAll(sqlStatemnet);
        }

        public IEnumerable<PhoneCall> GetChargeableCallsForSite(string siteName) 
        {
            string sqlStatemnet = sqlAccessor.ChargeableCallsForSite(dbTables, siteName);

            return base.GetAll(sqlStatemnet);
        }

    }

}
