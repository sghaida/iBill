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


        public override int Insert(PhoneCall phoneCallObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            string finalDataSourceName = string.Empty;

            // NULL object check
            if(null == phoneCallObject)
            {
                throw new Exception("PhoneCalls#Insert: Cannot insert NULL phone call objects.");
            }

            // NULL check on the DataSource Name
            if (false == string.IsNullOrEmpty(dataSourceName))
            {
                finalDataSourceName = dataSourceName;
            }
            else
            {
                throw new Exception("PhoneCalls#Insert: Empty DataSource name. Couldn't insert phone call object.");
            }

            // Perform data insert
            try
            {
                return base.Insert(phoneCallObject, finalDataSourceName, dataSource);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override bool Update(PhoneCall phoneCallObject, Dictionary<string, object> whereConditions = null, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            string finalDataSourceName = string.Empty;

            // NULL object check
            if (phoneCallObject == null)
            {
                throw new Exception("PhoneCalls#Update: Cannot update NULL phone call objects.");
            }

            // Decide on the value of the DataSource name
            if (false == string.IsNullOrEmpty(dataSourceName))
            {
                finalDataSourceName = dataSourceName;
            }
            else if (false == string.IsNullOrEmpty(phoneCallObject.PhoneCallsTableName))
            {
                finalDataSourceName = phoneCallObject.PhoneCallsTableName;
            }
            else
            {
                throw new Exception("PhoneCalls#Update: Both the DataSource name and the phoneCallObject.PhoneCallsTableName are NULL.");
            }

            // Perform data update 
            try
            { 
                return base.Update(phoneCallObject, whereConditions, finalDataSourceName, dataSource);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override bool Delete(PhoneCall phoneCallObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            string finalDataSourceName = string.Empty;

            // NULL object check
            if(null == phoneCallObject)
            {
                throw new Exception("PhoneCalls#Delete: Cannot delete NULL phone call objects.");
            }

            // Decide on the value of the DataSource name
            if(false == string.IsNullOrEmpty(dataSourceName))
            {
                finalDataSourceName = dataSourceName;
            }
            else if(false == string.IsNullOrEmpty(phoneCallObject.PhoneCallsTableName))
            {
                finalDataSourceName = phoneCallObject.PhoneCallsTableName;
            }
            else
            {
                throw new Exception("PhoneCalls#Delete: Both the DataSource name and the phoneCallObject.PhoneCallsTableName are NULL.");
            }

            // Perform data delete
            try
            { 
                return base.Delete(phoneCallObject, dataSourceName, dataSource);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
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


        public override PhoneCall GetById(long id, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            throw new NotImplementedException();
        }


        public override IEnumerable<PhoneCall> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            //return base.GetAll(dataSourceName, dataSourceType, IncludeDataRelations);
            throw new NotImplementedException();
        }
    }

}
