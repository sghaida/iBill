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
        private readonly List<string> _dbTables = new List<string>();

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
        /// <param name="SiteName"></param>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public List<CallsSummaryForSite> GetBySite(string SiteName)
        {
            try
            {
                DateTime StartDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                DateTime EndDate = DateTime.Now;

                return this.GetBySite(SiteName, StartDate, EndDate);
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
        /// <param name="EndDate"></param>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public List<CallsSummaryForSite> GetBySite(string SiteName, DateTime StartDate, DateTime EndDate)
        {
            List<CallsSummaryForSite> summaries;
            CallsSummaryForSiteComparer summariesComparer = new CallsSummaryForSiteComparer();

            try
            {
                var sqlQuery = _summariesSqlQueries.GetCallsSummariesForSite(
                    SiteName,
                    StartDate.ConvertDate(true),
                    EndDate.ConvertDate(true),
                    _dbTables);

                summaries = base.GetAll(sqlQuery).ToList();

                if (summaries != null && summaries.Count > 0)
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
        public new virtual CallsSummaryForSite GetById(long id, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForSite> Get(Dictionary<string, object> whereConditions,
            int limit = 25, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForSite> Get(Expression<Func<CallsSummaryForSite, bool>> predicate,
            string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForSite> GetAll(string sqlQuery)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForSite> GetAll(string dataSourceName = null,
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
        public new virtual int Insert(CallsSummaryForSite dataObject, string dataSourceName = null,
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
        public new virtual bool Update(CallsSummaryForSite dataObject, string dataSourceName = null,
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
        public new virtual bool Delete(CallsSummaryForSite dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }
    }


    public class CallsSummaryForSiteComparer : IComparer<CallsSummaryForSite>
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
