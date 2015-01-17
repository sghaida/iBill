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
    public class TopDestinationNumbersDataMapper : DataAccess<CallsSummaryForDestinationNumbers>
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
        private readonly CallsSummariesForDestinationNumbersSQL _summariesSqlQueries = new CallsSummariesForDestinationNumbersSQL();

        /// <summary>
        /// Given an enumerable collection of CallsSummaryForDestinationNumbers objects, group the by phone number and country, and then calculate the totals for their 
        /// Calls Count, Duration and Costs.
        /// </summary>
        /// <param name="topDestinations">List of CallsSummaryForDestinationNumbers objects</param>
        private static void GroupByPhoneNumber(ref IEnumerable<CallsSummaryForDestinationNumbers> topDestinationNumbers)
        {
            if (topDestinationNumbers.Any())
            {
                topDestinationNumbers = (
                    from summary in topDestinationNumbers
                    group summary by new { summary.PhoneNumber, summary.Country } into result
                    select new CallsSummaryForDestinationNumbers
                    {
                        Country = result.Key.Country ?? "N/A",
                        PhoneNumber = result.Key.PhoneNumber,
                        CallsCost = result.Sum(x => x.CallsCost),
                        CallsCount = result.Sum(x => x.CallsCount),
                        CallsDuration = result.Sum(x => x.CallsDuration)
                    })
                    .OrderByDescending(summary => summary.CallsCount);
            }
        }

        public TopDestinationNumbersDataMapper()
        {
            _dbTables = _monitoringServersInfoDataMapper.GetAll().Select(item => item.PhoneCallsTable).ToList();
        }

        /// <summary>
        /// Given a SipAccount, a limit value and possibly a date and time range, return the most called numbers for this user. The limit value controls the number of objects to return.
        /// If the date and time range wasn't specified, a default range is constructed for one year before, starting from DateTime.Now.
        /// </summary>
        /// <param name="sipAccount">The User's SipAccount</param>
        /// <param name="limit">Optional. By default it is 5. The limit of Destination Numbers to return</param>
        /// <param name="startingDate">Optional. The starting date range.</param>
        /// <param name="endingDate">Optional. The ending date range.</param>
        /// <returns>List of CallsSummaryForDestinationNumbers objects</returns>
        public List<CallsSummaryForDestinationNumbers> GetBySipAccount(string sipAccount, int limit = 5, DateTime? startingDate = null, DateTime? endingDate = null)
        {
            DateTime fromDate, toDate;
            IEnumerable<CallsSummaryForDestinationNumbers> topDestinationNumbers;

            if (startingDate == null || endingDate == null)
            {
                fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                toDate = DateTime.Now;
            }
            else
            {
                //Assign the beginning of date. Month to the startingDate and the end of it to the endingDate.
                fromDate = (DateTime)startingDate;
                toDate = (DateTime)endingDate;
            }

            try
            {
                var sql = _summariesSqlQueries.GetTopDestinationNumbersForUser(
                    sipAccount,
                    fromDate.ConvertDate(true),
                    toDate.ConvertDate(true),
                    limit,
                    _dbTables
                );

                topDestinationNumbers = base.GetAll(sql) ?? (new List<CallsSummaryForDestinationNumbers>() as IEnumerable<CallsSummaryForDestinationNumbers>);

                if(topDestinationNumbers.Any())
                {
                    GroupByPhoneNumber(ref topDestinationNumbers);
                }

                return topDestinationNumbers.ToList();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /***
         * DISABLED FUNCTIONS
         */

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual CallsSummaryForDestinationNumbers GetById(long id, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForDestinationNumbers> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForDestinationNumbers> Get(Expression<Func<CallsSummaryForDestinationNumbers, bool>> predicate, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForDestinationNumbers> GetAll(string sqlQuery)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForDestinationNumbers> GetAll(string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual int Insert(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual int Insert(CallsSummaryForDestinationNumbers dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Update(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Update(CallsSummaryForDestinationNumbers dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Delete(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Delete(CallsSummaryForDestinationNumbers dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

    }

}
