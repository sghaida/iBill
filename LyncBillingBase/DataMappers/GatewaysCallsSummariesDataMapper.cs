using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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

        //private readonly SitesDepartmentsDataMapper _siteDepartmentsDataMapper = SitesDepartmentsDataMapper.Instance; 

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
                        UnallocatedCallsCost = result.Sum(item => item.UnallocatedCallsCost),
                        UnallocatedCallsDuration = result.Sum(item => item.UnallocatedCallsDuration),
                        UnallocatedCallsCount = result.Sum(item => item.UnallocatedCallsCount)
                    }
                ).ToList<CallsSummaryForGateway>();
        }
        
        /// <summary>
        /// Given a list of Gateways Usage Data, return the same list with calculated Percentages for Calls Counts, Costs, and Durations.
        /// </summary>
        /// <param name="gatewaysUsageData">List of Summed CallsSummaryForGateway objects.</param>
        private static void CalculatePercentages(ref List<CallsSummaryForGateway> gatewaysUsageData)
        {
            decimal totalCostCount = 0;
            decimal totalDurationCount = 0;
            decimal totalOutGoingCallsCount = 0;

            string resolvedGatewayAddress;

            if (gatewaysUsageData.Any())
            {
                //Calculate totals
                gatewaysUsageData.ForEach(gatewayUsage =>
                {
                    totalCostCount += gatewayUsage.TotalCallsCost;
                    totalOutGoingCallsCount += gatewayUsage.TotalCallsCount;
                    totalDurationCount += gatewayUsage.TotalCallsDuration;
                });

                gatewaysUsageData.ForEach(tmpGatewayUsage =>
                {
                    //first
                    if (CCC.UTILS.Helpers.HelperFunctions.GetResolvedConnecionIpAddress(tmpGatewayUsage.GatewayName, out resolvedGatewayAddress))
                        tmpGatewayUsage.GatewayName = resolvedGatewayAddress;

                    //second
                    tmpGatewayUsage.CallsCountPercentage = tmpGatewayUsage.TotalCallsCount > 0 ? Math.Round((tmpGatewayUsage.TotalCallsCount * 100 / totalOutGoingCallsCount), 2) : 0;

                    //third
                    tmpGatewayUsage.CallsCostPercentage = tmpGatewayUsage.TotalCallsCost > 0 ? Math.Round((tmpGatewayUsage.TotalCallsCost * 100) / totalCostCount, 2) : 0;

                    //fourth
                    tmpGatewayUsage.CallsDurationPercentage = tmpGatewayUsage.TotalCallsDuration > 0 ? Math.Round((tmpGatewayUsage.TotalCallsDuration * 100 / totalDurationCount), 2) : 0;
                });
            }
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
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
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
        /// <param name="gatewayName">The Gateway Name / IP (string).</param>
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
                    summaries = summaries.Where(item => String.Equals(item.GatewayName, gatewayName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    return summaries.OrderBy(item => item.Year).ThenBy(item => item.Month).ToList();
                }

                return summaries;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Given a date and time range, return all the gateways usage per month.
        /// If the date and time range was not specified, a default date and time range is constructed for one year before, starting from DateTime.Now.
        /// </summary>
        /// <param name="startingDate">Optional. The Starting Date Range.</param>
        /// <param name="endingDate">Optional. The Ending date Range.</param>
        /// <returns>List of CallsSummaryForGateway for all gateways.</returns>
        public List<CallsSummaryForGateway> GetUsageForAllGateways(DateTime? startingDate = null, DateTime? endingDate = null)
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
                var sql = _summariesSqlQueries.GetCallsSummariesForAllSites(
                    fromDate.ConvertDate(true),
                    toDate.ConvertDate(true),
                    _dbTables
                );

                return base.GetAll(sql).ToList();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Given a list of gateways calls summaries, return the totals of every gateway for every year of it's summaries.
        /// </summary>
        /// <param name="gatewaysUsage">List of CallsSummaryForGateway objects</param>
        /// <param name="minimumCallsCount"></param>
        /// <returns>List of CallsSummaryForGateway objects.</returns>
        [SuppressMessage("ReSharper", "PossibleIntendedRethrow")]
        public List<CallsSummaryForGateway> GetGatewaysStatisticsResults(List<CallsSummaryForGateway> gatewaysUsage, int minimumCallsCount = 200)
        {
            try
            {
                var gatewaysUsageData = (
                    from data in gatewaysUsage.AsEnumerable()
                    group data by new { data.GatewayName, data.Year } into res
                    select new CallsSummaryForGateway
                    {
                        GatewayName = res.Key.GatewayName,
                        Year = res.Key.Year,
                        CallsCount = res.Sum(x => x.CallsCount),
                        CallsDuration = res.Sum(x => x.CallsDuration),
                        CallsCost = res.Sum(x => x.CallsCost),
                        BusinessCallsCost = res.Sum(item => item.BusinessCallsCost),
                        BusinessCallsCount = res.Sum(item => item.BusinessCallsCount),
                        BusinessCallsDuration = res.Sum(item => item.BusinessCallsDuration),
                        PersonalCallsCost = res.Sum(item => item.PersonalCallsCost),
                        PersonalCallsCount = res.Sum(item => item.PersonalCallsCount),
                        PersonalCallsDuration = res.Sum(item => item.PersonalCallsDuration),
                        UnallocatedCallsCost = res.Sum(item => item.UnallocatedCallsCost),
                        UnallocatedCallsCount = res.Sum(item => item.UnallocatedCallsCount),
                        UnallocatedCallsDuration = res.Sum(item => item.UnallocatedCallsDuration)
                    })
                    .Where(e => e.CallsCount > minimumCallsCount)
                    .ToList();

                return gatewaysUsageData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This function uses the GetUsageForAllGateways and GetGatewaysStatisticsResults functions to generate a report where the values:
        /// CallsDurationPercentage, CallsCostPercentage, and CallsCountPercentage are set. You can either use this right away, or use it's overloaded
        /// method which takes an already summarized list of gateway usage data and calculates the percentages on it.
        /// </summary>
        /// <param name="startingDate">Optional. The Starting Date Range.</param>
        /// <param name="endingDate">Optional. The Ending Date Range.</param>
        /// <param name="minimumCallsCount"></param>
        /// <returns>List of CallsSummaryForGateway objects.</returns>
        public List<CallsSummaryForGateway> SetGatewaysUsagePercentagesPerCallsCount(DateTime? startingDate = null, DateTime? endingDate = null, int minimumCallsCount = 200)
        {
            List<CallsSummaryForGateway> gatewaysSummaries;
            List<CallsSummaryForGateway> gatewaysUsageData;

            try
            {
                //Get all the gateways usage summaries
                gatewaysSummaries = GetUsageForAllGateways(startingDate, endingDate);

                //Map all teh records for each gateway into a total-sum-one!
                gatewaysUsageData = GetGatewaysStatisticsResults(gatewaysSummaries, minimumCallsCount);

                // Calculate percentages
                CalculatePercentages(ref gatewaysUsageData);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }

            return gatewaysUsageData;
        }

        /// <summary>
        /// This function, unlike it's overloaded version which takes a date and time range, takes an already summarized list of gateway usage data and then calculates the percentages fields for every entry in it.
        /// </summary>
        /// <returns>List of CallsSummaryForGateway objects.</returns>
        public List<CallsSummaryForGateway> SetGatewaysUsagePercentagesPerCallsCount(List<CallsSummaryForGateway> gatewaysUsageInputs, int minimumCallsCount = 200)
        {
            List<CallsSummaryForGateway> gatewaysUsageData;

            try
            {
                //Map all teh records for each gateway into a total-sum-one!
                gatewaysUsageData = GetGatewaysStatisticsResults(gatewaysUsageInputs, minimumCallsCount);

                // Calculate percentages
                CalculatePercentages(ref gatewaysUsageData);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }


            return gatewaysUsageData;
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
