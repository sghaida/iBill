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
        private readonly List<string> _dbTables = new List<string>();

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
        /// <param name="Summaries"></param>
        private void GroupByUserOnly(ref IEnumerable<CallsSummaryForUser> Summaries)
        {
            Summaries = Summaries.AsParallel();

            Summaries = (
                from summary in Summaries
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
        /// <param name="Summaries"></param>
        private void GroupByUserAndInvoiceFlag(ref IEnumerable<CallsSummaryForUser> Summaries)
        {
            Summaries = Summaries.AsParallel();

            Summaries = (
                from summary in Summaries
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
        /// <param name="SipAccount"></param>
        /// <returns></returns>
        public List<SpecialDateTime> GetYearsBySipAccount(string SipAccount)
        {
            List<SpecialDateTime> yearsList = new List<SpecialDateTime>();

            var summaries = GetBySipAccount(SipAccount, DateTime.MinValue, DateTime.Now);

            if(summaries != null && summaries.Count > 0)
            {
                yearsList = summaries
                    .Select<CallsSummaryForUser, int>(item => item.Year)
                    .Distinct()
                    .Select<int, SpecialDateTime>(item => new SpecialDateTime
                    {
                        YearAsNumber = item,
                        YearAsText = item.ToString()
                    })
                    .OrderBy(item => item.YearAsNumber)
                    .ToList<SpecialDateTime>();
            }

            return yearsList;
        }

        /// <summary>
        /// </summary>
        /// <param name="SipAccount"></param>
        /// <returns></returns>
        public List<CallsSummaryForUser> GetBySipAccount(string SipAccount)
        {
            try
            {
                var startingDate = (new DateTime(DateTime.Now.Year, 1, 1)).ConvertDate(true);
                var endingDate = DateTime.Now.ConvertDate(true);

                var sqlQuery = _summariesSqlQueries.GetCallsSummariesForUser(SipAccount, startingDate, endingDate,
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
        /// <param name="SipAccount"></param>
        /// <param name="StartingDate"></param>
        /// <param name="EndingDate"></param>
        /// <returns></returns>
        public List<CallsSummaryForUser> GetBySipAccount(string SipAccount, DateTime StartingDate, DateTime EndingDate)
        {
            try
            {
                var sqlQuery = _summariesSqlQueries.GetCallsSummariesForUser(
                    SipAccount,
                    StartingDate.ConvertDate(true),
                    EndingDate.ConvertDate(true),
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
        /// <param name="SiteName"></param>
        /// <param name="GroupBy"></param>
        /// <returns></returns>
        public List<CallsSummaryForUser> GetBySite(string SiteName,
            Globals.CallsSummary.GroupBy GroupBy = Globals.CallsSummary.GroupBy.DontGroup)
        {
            try
            {
                var startingDate = new DateTime(DateTime.Now.Year, 1, 1);
                var endingDate = DateTime.Now;

                return this.GetBySite(SiteName, startingDate, endingDate, GroupBy);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="SiteName"></param>
        /// <param name="StartingDate"></param>
        /// <param name="EndingDate"></param>
        /// <param name="GroupBy"></param>
        /// <returns></returns>
        public List<CallsSummaryForUser> GetBySite(string SiteName, DateTime StartingDate, DateTime EndingDate,
            Globals.CallsSummary.GroupBy GroupBy = Globals.CallsSummary.GroupBy.DontGroup)
        {
            IEnumerable<CallsSummaryForUser> summaries = null;

            try
            {
                string sqlQuery = _summariesSqlQueries.GetCallsSummariesForUsersInSite(
                    SiteName,
                    StartingDate.ConvertDate(true),
                    EndingDate.ConvertDate(true),
                    _dbTables);

                summaries = base.GetAll(sqlQuery);

                if(summaries != null && summaries.Count() > 0)
                {
                    if (GroupBy == Globals.CallsSummary.GroupBy.UserOnly)
                    {
                        GroupByUserOnly(ref summaries);
                    }
                    else if (GroupBy == Globals.CallsSummary.GroupBy.UserAndInvoiceFlag)
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
        /// <param name="SiteName"></param>
        /// <param name="SipAccountsList"></param>
        /// <param name="StartingDate"></param>
        /// <param name="endingDate"></param>
        /// <param name="invoiceStatus"></param>
        /// <returns></returns>
        public Dictionary<string, CallsSummaryForUser> GetBySite(string SiteName, List<string> SipAccountsList, 
            DateTime StartingDate, DateTime EndingDate, string InvoiceStatus = "NO")
        {
            if (string.IsNullOrEmpty(InvoiceStatus)) InvoiceStatus = "NO";
            Globals.CallsSummary.GroupBy GroupBy = Globals.CallsSummary.GroupBy.UserAndInvoiceFlag;

            List<CallsSummaryForUser> ListOfUsersSummaries = GetBySite(SiteName, StartingDate, EndingDate, GroupBy)
                .Where(summary => !string.IsNullOrEmpty(summary.IsInvoiced) && summary.IsInvoiced == InvoiceStatus)
                .ToList();

            Dictionary<string, CallsSummaryForUser> usersSummaryList = ListOfUsersSummaries
                .Where(summary => SipAccountsList.Contains(summary.SipAccount))
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