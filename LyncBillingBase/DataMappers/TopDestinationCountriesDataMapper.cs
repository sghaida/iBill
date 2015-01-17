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
    public class TopDestinationCountriesDataMapper : DataAccess<CallsSummaryForDestinationCountries>
    {
         /***
        * Get the phone calls tables list from the MonitoringServersInfo table
        */
        private readonly DataAccess<MonitoringServerInfo> _monitoringServersInfoDataMapper = new DataAccess<MonitoringServerInfo>();

        /***
         * DB Tables, to get calculate the summaries from.
         */
        private readonly List<string> _dbTables;

        /***
         * Predefined SQL Queries Store.
         */
        private readonly CallsSummariesForDestinationCountriesSQL _summariesSqlQueries = new CallsSummariesForDestinationCountriesSQL();

        /// <summary>
        /// Given an enumerable collection of CallsSummaryForDestinationCountries objects, group them by country name and calculate their
        /// totals. The totals of Calls Counts, Costs, and Durations.
        /// </summary>
        /// <param name="topDestinationCountries"></param>
        private static void GroupByCountry(ref IEnumerable<CallsSummaryForDestinationCountries> topDestinationCountries)
        {
            if(topDestinationCountries.Any())
            {
                topDestinationCountries = (
                    from summary in topDestinationCountries.AsEnumerable<CallsSummaryForDestinationCountries>()
                    group summary by new { summary.CountryName } into result
                    select new CallsSummaryForDestinationCountries
                    {
                        CountryName = result.Key.CountryName ?? "N/A",
                        CallsCost = result.Sum(x => x.CallsCost),
                        CallsCount = result.Sum(x => x.CallsCount),
                        CallsDuration = result.Sum(x => x.CallsDuration)
                    })
                    .OrderByDescending(summary => summary.CallsCount);
            }
        }

        public TopDestinationCountriesDataMapper()
        {
            _dbTables = _monitoringServersInfoDataMapper.GetAll().Select(item => item.PhoneCallsTable).ToList();
        }

        /// <summary>
        /// Given a SipAccount, a limit value and possibly a date and time range, return the most called countries for this user. The limit value controls the number of objects returned.
        /// If the date and time range wasn't specified, a default range is constructed for one year before, starting from DateTime.Now.
        /// </summary>
        /// <param name="sipAccount">The User's SipAccount</param>
        /// <param name="limit">Optional. By default it is 5. The limit of Destination Countries to return</param>
        /// <param name="startingDate">Optional. The starting date range.</param>
        /// <param name="endingDate">Optional. The ending date range.</param>
        /// <returns>List of CallsSummaryForDestinationCountries objects</returns>
        public List<CallsSummaryForDestinationCountries> GetByUser(string sipAccount, int limit = 5, DateTime? startingDate = null, DateTime? endingDate = null)
        {
            DateTime fromDate, toDate;
            IEnumerable<CallsSummaryForDestinationCountries> topDestinationCountries;

            if (startingDate == null || endingDate == null)
            {
                fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                toDate = DateTime.Now;
            }
            else
            {
                //Assign the beginning of date. Month to the startingDate and the end of it to the endingDate 
                fromDate = (DateTime)startingDate;
                toDate = (DateTime)endingDate;
            }

            try
            {
                var sql = _summariesSqlQueries.GetTopDestinationCountriesForUser(
                    sipAccount,
                    fromDate.ConvertDate(true),
                    toDate.ConvertDate(true),
                    limit,
                    _dbTables);

                topDestinationCountries = base.GetAll(sql) ?? (new List<CallsSummaryForDestinationCountries>() as IEnumerable<CallsSummaryForDestinationCountries>);

                if(topDestinationCountries.Any())
                {
                    GroupByCountry(ref topDestinationCountries);
                }

                return topDestinationCountries.ToList();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Given a Site Name, a Department Name, a limit value and possibly a date and time range, return the most called countries for this site-department.
        /// The limit value controls the number of objects returned.
        /// If the date and time range wasn't specified, a default range is constructed for one year before, starting from DateTime.Now.
        /// </summary>
        /// <param name="siteName">The Department's Site Name.</param>
        /// <param name="departmentName">The Department Name.</param>
        /// <param name="limit">Optional. By default it is 5. The limit of Destination Countries to return</param>
        /// <param name="startingDate">Optional. The starting date range.</param>
        /// <param name="endingDate">Optional. The ending date range.</param>
        /// <returns>List of CallsSummaryForDestinationCountries objects</returns>
        public List<CallsSummaryForDestinationCountries> GetByDepartment(string siteName, string departmentName, int limit = 5, DateTime? startingDate = null, DateTime? endingDate = null)
        {
            DateTime fromDate, toDate;
            IEnumerable<CallsSummaryForDestinationCountries> topDestinationCountries;

            if (startingDate == null || endingDate == null)
            {
                fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                toDate = DateTime.Now;
            }
            else
            {
                //Assign the beginning of date. Month to the startingDate and the end of it to the endingDate 
                fromDate = (DateTime)startingDate;
                toDate = (DateTime)endingDate;
            }

            try
            {
                var sql = _summariesSqlQueries.GetTopDestinationCountriesForSiteDepartment(
                    siteName,
                    departmentName,
                    fromDate.ConvertDate(true),
                    toDate.ConvertDate(true),
                    limit,
                    _dbTables);

                topDestinationCountries = base.GetAll(sql) ?? (new List<CallsSummaryForDestinationCountries>() as IEnumerable<CallsSummaryForDestinationCountries>);

                if (topDestinationCountries.Any())
                {
                    GroupByCountry(ref topDestinationCountries);
                }

                return topDestinationCountries.ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Given a Site Name, a limit value and possibly a date and time range, return the most called countries for this site.
        /// The limit value controls the number of objects returned.
        /// If the date and time range wasn't specified, a default range is constructed for one year before, starting from DateTime.Now.
        /// </summary>
        /// <param name="siteName">The Site Name.</param
        /// <param name="limit">Optional. By default it is 5. The limit of Destination Countries to return</param>
        /// <param name="startingDate">Optional. The starting date range.</param>
        /// <param name="endingDate">Optional. The ending date range.</param>
        /// <returns>List of CallsSummaryForDestinationCountries objects</returns>
        public List<CallsSummaryForDestinationCountries> GetBySite(string siteName, int limit = 5, DateTime? startingDate = null, DateTime? endingDate = null)
        {
            DateTime fromDate, toDate;
            IEnumerable<CallsSummaryForDestinationCountries> topDestinationCountries;

            if (startingDate == null || endingDate == null)
            {
                fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                toDate = DateTime.Now;
            }
            else
            {
                //Assign the beginning of date. Month to the startingDate and the end of it to the endingDate 
                fromDate = (DateTime)startingDate;
                toDate = (DateTime)endingDate;
            }

            try
            {
                var sql = _summariesSqlQueries.GetTopDestinationCountriesForSite(
                    siteName,
                    fromDate.ConvertDate(true),
                    toDate.ConvertDate(true),
                    limit,
                    _dbTables);

                topDestinationCountries = base.GetAll(sql) ?? (new List<CallsSummaryForDestinationCountries>() as IEnumerable<CallsSummaryForDestinationCountries>);

                if (topDestinationCountries.Any())
                {
                    GroupByCountry(ref topDestinationCountries);
                }

                return topDestinationCountries.ToList();
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
        public new virtual CallsSummaryForDestinationCountries GetById(long id, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForDestinationCountries> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForDestinationCountries> Get(Expression<Func<CallsSummaryForDestinationCountries, bool>> predicate, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForDestinationCountries> GetAll(string sqlQuery)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForDestinationCountries> GetAll(string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual int Insert(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual int Insert(CallsSummaryForDestinationCountries dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Update(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Update(CallsSummaryForDestinationCountries dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Delete(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Delete(CallsSummaryForDestinationCountries dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

    }

}
