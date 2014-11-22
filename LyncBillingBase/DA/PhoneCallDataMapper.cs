using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.DAL;
using LyncBillingBase.LookupTables;
using System.Linq.Expressions;
using LyncBillingBase.DAL.SQL;

namespace LyncBillingBase.DA
{
    public class PhoneCallDataMapper : DataAccess<PhoneCall>
    {
        private DataAccess<MonitoringServerInfo> monInfoDA = new DataAccess<MonitoringServerInfo>();
        //private DataAccess<PhoneCall> phoneCallDA = new DataAccess<PhoneCall>();

        private PhoneCallsSQL sqlAccessor = new PhoneCallsSQL();

        private List<string> dbTables = new List<string>();

        public PhoneCallDataMapper() : base()
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

        public IQueryable<PhoneCall> Get(Dictionary<string, object> where, string dataSourceName, int limit = 25) 
        {
            return base.Get(where, limit, dataSourceName);
        }

        public IQueryable<PhoneCall> Get(Expression<Func<PhoneCall, bool>> predicate, string dataSourceName)
        {
            return base.Get(predicate, dataSourceName);
        }

        public bool Delete(PhoneCall dataObject, string dataSourceName)
        {
            return base.Delete(dataObject);
        }

        public IQueryable<PhoneCall> GetAll(string dataSourceName) 
        {
            return base.GetAll(dataSourceName);
        }

        public IQueryable<PhoneCall> GetChargableCallsPerUser(string sipAccount) 
        {
            string sqlStatemnet = sqlAccessor.ChargableCallsPerUser(dbTables, sipAccount);

            return base.GetAll(sqlStatemnet);
        }

        public IQueryable<PhoneCall> GetChargeableCallsForSite(string siteName) 
        {
            string sqlStatemnet = sqlAccessor.ChargeableCallsForSite(dbTables, siteName);

            return base.GetAll(sqlStatemnet);
        }


        
    
    }

}
