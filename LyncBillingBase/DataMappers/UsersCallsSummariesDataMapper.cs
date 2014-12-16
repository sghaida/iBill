using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.ComponentModel;

using LyncBillingBase.Helpers;
using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class UsersCallsSummariesDataMapper : DataAccess<CallsSummaryForUser>
    {
        /***
         * DB Tables, to get calculate the summaries from.
         */
        private List<string> DBTables = new List<string>();

        /***
         * Predefined SQL Queries Store.
         */
        private SQLQueries.CallsSummariesSQL SUMMARIES_SQL_QUERIES = new SQLQueries.CallsSummariesSQL();

        /***
         * Get the phone calls tables list from the MonitoringServersInfo table
         */
        private DataAccess<MonitoringServerInfo> _monitoringServersInfoDataMapper = new DataAccess<MonitoringServerInfo>();


        public UsersCallsSummariesDataMapper()
        {
            DBTables = _monitoringServersInfoDataMapper.GetAll().Select<MonitoringServerInfo, string>(item => item.PhoneCallsTable).ToList<string>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SipAccount"></param>
        /// <returns></returns>
        public List<CallsSummaryForUser> GetBySipAccount(string SipAccount)
        {
            List<CallsSummaryForUser> summaries = null;

            try
            {
                string StartingDate = HelperFunctions.ConvertDate((new DateTime(DateTime.Now.Year, 1, 1)), excludeHoursAndMinutes: true);
                string EndingDate = HelperFunctions.ConvertDate(DateTime.Now, excludeHoursAndMinutes: true);

                string SQL_QUERY = SUMMARIES_SQL_QUERIES.GetCallsSummariesForUser(SipAccount, StartingDate, EndingDate, DBTables);

                summaries = base.GetAll(SQL_QUERY).ToList<CallsSummaryForUser>();

                return summaries;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SipAccount"></param>
        /// <param name="StartingDate"></param>
        /// <param name="EndingDate"></param>
        /// <returns></returns>
        public List<CallsSummaryForUser> GetBySipAccount(string SipAccount, DateTime StartingDate, DateTime EndingDate)
        {
            List<CallsSummaryForUser> summaries = null;

            try
            {
                string SQL_QUERY = SUMMARIES_SQL_QUERIES.GetCallsSummariesForUser(
                    SipAccount, 
                    HelperFunctions.ConvertDate(StartingDate, excludeHoursAndMinutes: true), 
                    HelperFunctions.ConvertDate(EndingDate, excludeHoursAndMinutes: true), 
                    DBTables);

                summaries = base.GetAll(SQL_QUERY).ToList<CallsSummaryForUser>();

                return summaries;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }



        /***
         * DISABLED FUNCTIONS
         */
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual new CallsSummaryForUser GetById(long id, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default) { throw new NotSupportedException(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual new IEnumerable<CallsSummaryForUser> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default) { throw new NotSupportedException(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual new IEnumerable<CallsSummaryForUser> Get(Expression<Func<CallsSummaryForUser, bool>> predicate, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default) { throw new NotSupportedException(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual new IEnumerable<CallsSummaryForUser> GetAll(string SQL_QUERY) { throw new NotSupportedException(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual new IEnumerable<CallsSummaryForUser> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default) { throw new NotSupportedException(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual new int Insert(string sql) { throw new NotSupportedException(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual new int Insert(CallsSummaryForUser dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default) { throw new NotSupportedException(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual new bool Update(string sql) { throw new NotSupportedException(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual new bool Update(CallsSummaryForUser dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default) { throw new NotSupportedException(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual new bool Delete(string sql) { throw new NotSupportedException(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual new bool Delete(CallsSummaryForUser dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default) { throw new NotSupportedException(); }

    }
}
