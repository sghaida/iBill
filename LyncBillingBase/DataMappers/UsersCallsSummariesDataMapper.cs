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
using CCC.UTILS.Libs;

namespace LyncBillingBase.DataMappers
{
    public class UsersCallsSummariesDataMapper : DataAccess<CallsSummaryForUser>
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
        private readonly CallsSummariesForUsersSql _summariesSqlQueries = new CallsSummariesForUsersSql();

        public UsersCallsSummariesDataMapper()
        {
            _dbTables = _monitoringServersInfoDataMapper.GetAll().Select(item => item.PhoneCallsTable).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="summaries"></param>
        private void GroupByUserOnly(ref IEnumerable<CallsSummaryForUser> summaries)
        {
            summaries = summaries.AsParallel();

            summaries = (
                from summary in summaries
                group summary by new {summary.SipAccount}
                into result
                select new CallsSummaryForUser
                {
                    SipAccount = result.Key.SipAccount,
                    BusinessCallsCost = result.Sum(item => item.BusinessCallsCost),
                    BusinessCallsDuration = result.Sum(item => item.BusinessCallsDuration),
                    BusinessCallsCount = result.Sum(item => item.BusinessCallsCount),
                    PersonalCallsCost = result.Sum(item => item.PersonalCallsCost),
                    PersonalCallsDuration = result.Sum(item => item.PersonalCallsDuration),
                    PersonalCallsCount = result.Sum(item => item.PersonalCallsCount),
                    UnmarkedCallsCost = result.Sum(item => item.UnmarkedCallsCost),
                    UnmarkedCallsDuration = result.Sum(item => item.UnmarkedCallsDuration),
                    UnmarkedCallsCount = result.Sum(item => item.UnmarkedCallsCount)
                }
                ).ToList<CallsSummaryForUser>();
        }

        /// <summary>
        /// </summary>
        /// <param name="summaries"></param>
        private void GroupByUserAndInvoiceFlag(ref IEnumerable<CallsSummaryForUser> summaries)
        {
            summaries = summaries.AsParallel();

            summaries = (
                from summary in summaries
                group summary by new {summary.SipAccount, summary.IsInvoiced}
                into result
                select new CallsSummaryForUser
                {
                    SipAccount = result.Key.SipAccount,
                    IsInvoiced = result.Key.IsInvoiced,
                    BusinessCallsCost = result.Sum(item => item.BusinessCallsCost),
                    BusinessCallsDuration = result.Sum(item => item.BusinessCallsDuration),
                    BusinessCallsCount = result.Sum(item => item.BusinessCallsCount),
                    PersonalCallsCost = result.Sum(item => item.PersonalCallsCost),
                    PersonalCallsDuration = result.Sum(item => item.PersonalCallsDuration),
                    PersonalCallsCount = result.Sum(item => item.PersonalCallsCount),
                    UnmarkedCallsCost = result.Sum(item => item.UnmarkedCallsCost),
                    UnmarkedCallsDuration = result.Sum(item => item.UnmarkedCallsDuration),
                    UnmarkedCallsCount = result.Sum(item => item.UnmarkedCallsCount)
                }
                ).ToList<CallsSummaryForUser>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sipAccount"></param>
        /// <returns></returns>
        public List<SpecialDateTime> GetYearsBySipAccount(string sipAccount)
        {
            List<SpecialDateTime> yearsList = new List<SpecialDateTime>();

            var summaries = GetBySipAccount(sipAccount, DateTime.MinValue, DateTime.Now);

            if(summaries != null && summaries.Count > 0)
            {
                yearsList = summaries
                    .Select(item => item.Year)
                    .Distinct()
                    .Select(item => new SpecialDateTime
                    {
                        YearAsNumber = item,
                        YearAsText = item.ToString()
                    })
                    .OrderBy(item => item.YearAsNumber)
                    .ToList();
            }

            return yearsList;
        }

        /// <summary>
        /// </summary>
        /// <param name="sipAccount"></param>
        /// <returns></returns>
        public List<CallsSummaryForUser> GetBySipAccount(string sipAccount)
        {
            try
            {
                var startingDate = (new DateTime(DateTime.Now.Year, 1, 1)).ConvertDate(true);
                var endingDate = DateTime.Now.ConvertDate(true);

                var sqlQuery = _summariesSqlQueries.GetCallsSummariesForUser(sipAccount, startingDate, endingDate,
                    _dbTables);

                return base.GetAll(sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sipAccount"></param>
        /// <param name="startingDate"></param>
        /// <param name="endingDate"></param>
        /// <returns></returns>
        public List<CallsSummaryForUser> GetBySipAccount(string sipAccount, DateTime startingDate, DateTime endingDate)
        {
            try
            {
                var sqlQuery = _summariesSqlQueries.GetCallsSummariesForUser(
                    sipAccount,
                    startingDate.ConvertDate(true),
                    endingDate.ConvertDate(true),
                    _dbTables);

                return base.GetAll(sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public List<CallsSummaryForUser> GetBySite(string siteName,
            Globals.CallsSummary.GroupBy groupBy = Globals.CallsSummary.GroupBy.DontGroup)
        {
            try
            {
                var startingDate = new DateTime(DateTime.Now.Year, 1, 1);
                var endingDate = DateTime.Now;

                return this.GetBySite(siteName, startingDate, endingDate, groupBy);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="startingDate"></param>
        /// <param name="endingDate"></param>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public List<CallsSummaryForUser> GetBySite(string siteName, DateTime startingDate, DateTime endingDate,
            Globals.CallsSummary.GroupBy groupBy = Globals.CallsSummary.GroupBy.DontGroup)
        {
            try
            {
                string sqlQuery = _summariesSqlQueries.GetCallsSummariesForUsersInSite(
                    siteName,
                    startingDate.ConvertDate(true),
                    endingDate.ConvertDate(true),
                    _dbTables);

                var summaries = base.GetAll(sqlQuery);

                if(summaries != null && summaries.Any())
                {
                    if (groupBy == Globals.CallsSummary.GroupBy.UserOnly)
                    {
                        GroupByUserOnly(ref summaries);
                    }
                    else if (groupBy == Globals.CallsSummary.GroupBy.UserAndInvoiceFlag)
                    {
                        GroupByUserAndInvoiceFlag(ref summaries);
                    }
                }

                return summaries.ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="sipAccountsList"></param>
        /// <param name="startingDate"></param>
        /// <param name="endingDate"></param>
        /// <param name="invoiceStatus"></param>
        /// <returns></returns>
        public Dictionary<string, CallsSummaryForUser> GetBySite(string siteName, List<string> sipAccountsList, 
            DateTime startingDate, DateTime endingDate, string invoiceStatus = "NO")
        {
            if (string.IsNullOrEmpty(invoiceStatus)) invoiceStatus = "NO";
            const Globals.CallsSummary.GroupBy groupBy = Globals.CallsSummary.GroupBy.UserAndInvoiceFlag;

            List<CallsSummaryForUser> listOfUsersSummaries = GetBySite(siteName, startingDate, endingDate, groupBy)
                .Where(summary => !string.IsNullOrEmpty(summary.IsInvoiced) && summary.IsInvoiced == invoiceStatus)
                .ToList();

            Dictionary<string, CallsSummaryForUser> usersSummaryList = listOfUsersSummaries
                .Where(summary => sipAccountsList.Contains(summary.SipAccount))
                .ToDictionary(summary => summary.SipAccount);

            return usersSummaryList;
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

}