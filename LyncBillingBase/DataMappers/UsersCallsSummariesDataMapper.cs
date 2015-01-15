﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using CCC.UTILS.Libs;

using LyncBillingBase.DataMappers.SQLQueries;
using LyncBillingBase.DataModels;

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
        private static void GroupByUserOnly(ref IEnumerable<CallsSummaryForUser> summaries)
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
        private static void GroupByUserAndInvoiceFlag(ref IEnumerable<CallsSummaryForUser> summaries)
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
        /// <param name="StartingDate"></param>
        /// <param name="EndingDate"></param>
        /// <returns></returns>
        public List<CallsSummaryForUser> GetBySipAccount(string sipAccount, DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime fromDate, toDate;

            if (startDate == null || endDate == null)
            {
                fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                toDate = DateTime.Now;
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                fromDate = (DateTime)startDate;
                toDate = (DateTime)endDate;
            }

            try
            {
                var sqlQuery = _summariesSqlQueries.GetCallsSummariesForUser(
                    sipAccount,
                    fromDate.ConvertDate(true),
                    toDate.ConvertDate(true),
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
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public List<CallsSummaryForUser> GetBySite(string siteName, DateTime? startDate = null, DateTime? endDate = null, Globals.CallsSummary.GroupBy groupBy = Globals.CallsSummary.GroupBy.DontGroup)
        {
            DateTime fromDate, toDate;

            if (startDate == null || endDate == null)
            {
                fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                toDate = DateTime.Now;
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                fromDate = (DateTime)startDate;
                toDate = (DateTime)endDate;
            }

            try
            {
                string sqlQuery = _summariesSqlQueries.GetCallsSummariesForUsersInSite(
                    siteName,
                    fromDate.ConvertDate(true),
                    toDate.ConvertDate(true),
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
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="invoiceStatus"></param>
        /// <returns></returns>
        public Dictionary<string, CallsSummaryForUser> GetBySite(string siteName, List<string> sipAccountsList, DateTime? startDate = null, DateTime? endDate = null, string invoiceStatus = "NO")
        {
            DateTime fromDate, toDate;
            const Globals.CallsSummary.GroupBy groupBy = Globals.CallsSummary.GroupBy.UserAndInvoiceFlag;

            if (string.IsNullOrEmpty(invoiceStatus)) invoiceStatus = "NO";

            if (startDate == null || endDate == null)
            {
                fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                toDate = DateTime.Now;
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                fromDate = (DateTime)startDate;
                toDate = (DateTime)endDate;
            }

            List<CallsSummaryForUser> listOfUsersSummaries = GetBySite(siteName, fromDate, toDate, groupBy)
                .Where(summary => !string.IsNullOrEmpty(summary.IsInvoiced) && summary.IsInvoiced == invoiceStatus)
                .ToList();

            Dictionary<string, CallsSummaryForUser> usersSummaryList = listOfUsersSummaries
                .Where(summary => sipAccountsList.Contains(summary.SipAccount))
                .ToDictionary(summary => summary.SipAccount);

            return usersSummaryList;
        }

        public List<CallsSummaryForUser> GetByGateway(string gatewayName, DateTime? startDate = null, DateTime? endDate = null, Globals.CallsSummary.GroupBy groupBy = Globals.CallsSummary.GroupBy.DontGroup)
        {
            DateTime fromDate, toDate;

            if (startDate == null || endDate == null)
            {
                fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                toDate = DateTime.Now;
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                fromDate = (DateTime)startDate;
                toDate = (DateTime)endDate;
            }

            try
            {
                string sqlQuery = _summariesSqlQueries.GetCallsSummariesForUsersPerGateway(
                    gatewayName,
                    fromDate.ConvertDate(true),
                    toDate.ConvertDate(true),
                    _dbTables);

                var summaries = base.GetAll(sqlQuery);

                if (summaries != null && summaries.Any())
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

        /***
         * DISABLED FUNCTIONS
         */

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual CallsSummaryForUser GetById(long id, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForUser> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForUser> Get(Expression<Func<CallsSummaryForUser, bool>> predicate, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForUser> GetAll(string sqlQuery)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForUser> GetAll(string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual int Insert(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual int Insert(CallsSummaryForUser dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Update(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Update(CallsSummaryForUser dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Delete(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Delete(CallsSummaryForUser dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

    }

}