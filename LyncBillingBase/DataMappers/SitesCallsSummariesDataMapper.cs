using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataMappers.SQLQueries;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class SitesCallsSummariesDataMapper : DataAccess<CallsSummaryForSite>
    {
        /***
        * Get the phone calls tables list from the MonitoringServersInfo table
        */
        private readonly DataAccess<MonitoringServerInfo> _monitoringServersInfoDataMapper =
            new DataAccess<MonitoringServerInfo>();

        /***
         * DB Tables, to get calculate the summaries from.
         */
        private readonly List<string> _dbTables;

        /***
         * Predefined SQL Queries Store.
         */
        private readonly CallsSummariesForSitesSql _summariesSqlQueries = new CallsSummariesForSitesSql();

        public SitesCallsSummariesDataMapper()
        {
            _dbTables = _monitoringServersInfoDataMapper.GetAll().Select(item => item.PhoneCallsTable).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public List<CallsSummaryForSite> GetBySite(string siteName)
        {
            var summariesComparer = new CallsSummaryForSiteComparer();

            try
            {
                var startingDate = (new DateTime(DateTime.Now.Year, 1, 1)).ConvertDate(true);
                var endingDate = DateTime.Now.ConvertDate(true);

                var sqlQuery = _summariesSqlQueries.GetCallsSummariesForSite(siteName, startingDate, endingDate,
                    _dbTables);

                var summaries = base.GetAll(sqlQuery).ToList();
                
                if ( summaries.Count > 0)
                {
                    summaries.Sort(summariesComparer);
                }

                return summaries;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="startingDate"></param>
        /// <param name="endingDate"></param>
        /// <returns></returns>
        public List<CallsSummaryForSite> GetBySite(string siteName, DateTime startingDate, DateTime endingDate)
        {
            CallsSummaryForSiteComparer summariesComparer = new CallsSummaryForSiteComparer();

            try
            {
                var sqlQuery = _summariesSqlQueries.GetCallsSummariesForSite(
                    siteName,
                    startingDate.ConvertDate(true),
                    endingDate.ConvertDate(true),
                    _dbTables);

                var summaries = base.GetAll(sqlQuery).ToList();

                if (summaries.Count > 0)
                {
                    summaries.Sort(summariesComparer);
                }

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
        public new virtual CallsSummaryForUser GetById(long id, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForUser> Get(Dictionary<string, object> whereConditions,
            int limit = 25, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForUser> Get(Expression<Func<CallsSummaryForUser, bool>> predicate,
            string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForUser> GetAll(string sqlQuery)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForUser> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual int Insert(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual int Insert(CallsSummaryForUser dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Update(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Update(CallsSummaryForUser dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Delete(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Delete(CallsSummaryForUser dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }
    }


    internal sealed class CallsSummaryForSiteComparer : IComparer<CallsSummaryForSite>
    {
        public int Compare(CallsSummaryForSite x, CallsSummaryForSite y)
        {
            //less than
            if (x.Year < y.Year)
                return -1;
            //equals
            else if (x.Year == y.Year)
            {
                if (x.Month < y.Month)
                    return -1;
                else if (x.Month == y.Month)
                    return 0;
                else
                    return 1;
            }
            //greater than
            else
                return 1;
        }
    }

}
