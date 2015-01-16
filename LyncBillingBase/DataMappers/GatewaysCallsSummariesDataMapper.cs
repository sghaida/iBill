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

        public GatewaysCallsSummariesDataMapper()
        {
            _dbTables = _monitoringServersInfoDataMapper.GetAll().Select(item => item.PhoneCallsTable).ToList();
        }


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


        public List<CallsSummaryForGateway> GetBySite(string siteName, DateTime? startingDate = null, DateTime? endingDate = null)
        {
            //DateTime fromDate, toDate;

            //if (startingDate == null || endingDate == null)
            //{
            //    fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
            //    toDate = DateTime.Now;
            //}
            //else
            //{
            //    //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
            //    fromDate = (DateTime)startingDate;
            //    toDate = (DateTime)endingDate;
            //}

            throw new NotImplementedException();
        }


        public List<CallsSummaryForGateway> GetBySiteAndGateway(string siteName, string gatewayName, DateTime? startingDate = null, DateTime? endingDate = null)
        {
            throw new NotImplementedException();
        }


        public List<CallsSummaryForGateway> GetUsage(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }


        public List<CallsSummaryForGateway> GetGatewaysStatisticsResults(List<CallsSummaryForGateway> gatewaysUsage)
        {
            throw new NotImplementedException();
        }


        public List<CallsSummaryForGateway> SetGatewaysUsagePercentagesPerCallsCount(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }


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
