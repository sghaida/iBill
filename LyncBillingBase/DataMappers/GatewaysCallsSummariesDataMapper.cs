using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Linq.Expressions;

using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;

using LyncBillingBase.DataMappers.SQLQueries;
using LyncBillingBase.DataModels;
using CCC.UTILS.Libs;

namespace LyncBillingBase.DataMappers
{
    public class GatewaysCallsSummariesDataMapper : DataAccess<CallsSummaryForGateway>
    {
        /***
         * Get the phone calls tables list from the MonitoringServersInfo table
         */
        private readonly DataAccess<MonitoringServerInfo> _monitoringServersInfoDataMapper =
            new DataAccess<MonitoringServerInfo>();

        private readonly SitesDepartmentsDataMapper _siteDepartmentsDataMapper = SitesDepartmentsDataMapper.Instance;

        /***
         * YEARS FOR ALL GATEWAYS SUMMARIES
         */
        private static List<SpecialDateTime> _years = new List<SpecialDateTime>();

        /***
         * DB Tables, to get calculate the summaries from.
         */
        private readonly List<string> _dbTables;

        /***
         * Predefined SQL Queries Store.
         */
        private readonly CallsSummariesForGatewaySQL _summariesSqlQueries = new CallsSummariesForGatewaySQL();

        /// <summary>
        /// Given an enumerable list of CallsSummaryForGateway objects, group the objects by the GatewayName field only.
        /// </summary>
        /// <param name="summaries">Enumerable List of CallsSummaryForGateway objects.</param>
        private static void GroupByGateway(ref IEnumerable<CallsSummaryForGateway> summaries)
        {
            summaries = summaries.AsParallel();

            summaries = (
                from summary in summaries
                group summary by new { summary.GatewayName }
                    into result
                    select new CallsSummaryForGateway
                    {
                        GatewayName = result.Key.GatewayName,
                        
                        CallsCost = result.Sum(item => item.CallsCost),
                        CallsCount = result.Sum(item => item.CallsCount),
                        CallsDuration = result.Sum(item => item.CallsDuration),

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
                ).ToList<CallsSummaryForGateway>();
        }

        public GatewaysCallsSummariesDataMapper()
        {
            _dbTables = _monitoringServersInfoDataMapper.GetAll().Select(item => item.PhoneCallsTable).ToList();
        }

        /// <summary>
        /// Get the list of years of the calls summaries for all gateways.
        /// </summary>
        /// <returns>List of SpecialDateTime objects</returns>
        public List<SpecialDateTime> GetYears()
        {
            if(_years != null && _years.Any())
            {
                return _years;
            }

            try
            {
                string startDate = DateTime.MinValue.ConvertDate(true);
                string endDate = DateTime.Now.ConvertDate(true);

                string sql = _summariesSqlQueries.GetCallsSummariesYears(startDate, endDate, _dbTables);

                var summaries = base.GetAll(sql).ToList();

                if(summaries.Any())
                {
                    _years = summaries
                        .Select(item => item.Year)
                        .Distinct()
                        .Select(item => new SpecialDateTime { 
                            YearAsNumber = item,
                            YearAsText = item.ToString()
                        })
                        .ToList();
                }
                else
                {
                    _years = new List<SpecialDateTime>();
                }

                return _years;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Given a Site Name, and possibly a date and time range, return the summaires for it's gateways.
        /// If a date and time range was not specified, a default date and time range will be constructed with a one year before, starting from DateTime.Now.
        /// By default the data won't be grouped by, unless specified.
        /// </summary>
        /// <param name="siteName">Site Name</param>
        /// <param name="startingDate">Optional. The Starting Date Range.</param>
        /// <param name="endingDate">Optional. The Ending Date Range.</param>
        /// <param name="groupBy">Optional. By default it is set to DontGroup. Can be Set to any values of the same class of enums.</param>
        /// <returns>List of CallsSummaryForGateway objects for all the gateways of that site.</returns>
        public List<CallsSummaryForGateway> GetBySite(string siteName, DateTime? startingDate = null, DateTime? endingDate = null, Globals.CallsSummaryForGateway.GroupBy groupBy = Globals.CallsSummaryForGateway.GroupBy.DontGroup)
        {
            DateTime fromDate, toDate;

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
                var sql = _summariesSqlQueries.GetCallsSummariesForSite(
                    siteName, 
                    fromDate.ConvertDate(true),
                    toDate.ConvertDate(true),
                    _dbTables
                );
                
                var summaries = base.GetAll(sql) ?? (new List<CallsSummaryForGateway>());

                if(summaries.Any())
                {
                    if(groupBy == Globals.CallsSummaryForGateway.GroupBy.GatewayNameOnly)
                    {
                        GroupByGateway(ref summaries);
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
        /// Given a Site Name, a Gateway Name, and possibly a date and time range, return the summaires for that Site Gateway.
        /// If a date and time range was not specified, a default date and time range will be constructed with a one year before, starting from DateTime.Now.
        /// By default the data won't be grouped by, unless specified.
        /// </summary>
        /// <param name="siteName">Site Name</param>
        /// <param name="startingDate">Optional. The Starting Date Range.</param>
        /// <param name="endingDate">Optional. The Ending Date Range.</param>
        /// <param name="groupBy">Optional. By default it is set to DontGroup. Can be Set to any values of the same class of enums.</param>
        /// <returns>List of CallsSummaryForGateway objects for that Site Gateway.</returns>
        public List<CallsSummaryForGateway> GetBySiteAndGateway(string siteName, string gatewayName, DateTime? startingDate = null, DateTime? endingDate = null, Globals.CallsSummaryForGateway.GroupBy groupBy = Globals.CallsSummaryForGateway.GroupBy.DontGroup)
        {
            try
            {
                var summaries = GetBySite(siteName, startingDate, endingDate, groupBy);

                if(summaries.Any())
                {
                    summaries = summaries.Where(item => item.GatewayName.ToLower() == gatewayName.ToLower()).ToList();
                    summaries.OrderBy(item => item.Year).OrderBy(item => item.Year);
                }

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
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<CallsSummaryForGateway> GetUsage(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gatewaysUsage"></param>
        /// <returns></returns>
        public List<CallsSummaryForGateway> GetGatewaysStatisticsResults(List<CallsSummaryForGateway> gatewaysUsage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<CallsSummaryForGateway> SetGatewaysUsagePercentagesPerCallsCount(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gatewaysUsage"></param>
        /// <returns></returns>
        public List<CallsSummaryForGateway> SetGatewaysUsagePercentagesPerCallsCount(List<CallsSummaryForGateway> gatewaysUsage)
        {
            throw new NotImplementedException();
        }


        /***
         * DISABLED FUNCTIONS
         */

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual CallsSummaryForGateway GetById(long id, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForGateway> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForGateway> Get(Expression<Func<CallsSummaryForGateway, bool>> predicate, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForGateway> GetAll(string sqlQuery)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForGateway> GetAll(string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual int Insert(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual int Insert(CallsSummaryForGateway dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Update(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Update(CallsSummaryForGateway dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Delete(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Delete(CallsSummaryForGateway dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

    }

}
