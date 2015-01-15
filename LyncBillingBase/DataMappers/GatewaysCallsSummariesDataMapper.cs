using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq.Expressions;

using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;

using LyncBillingBase.DataMappers.SQLQueries;
using LyncBillingBase.DataModels;

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
         * DB Tables, to get calculate the summaries from.
         */
        private readonly List<string> _dbTables;

        /***
         * Predefined SQL Queries Store.
         */
        private readonly CallsSummariesForDepartmentsSQL _summariesSqlQueries = new CallsSummariesForDepartmentsSQL();

        public GatewaysCallsSummariesDataMapper()
        {
            _dbTables = _monitoringServersInfoDataMapper.GetAll().Select(item => item.PhoneCallsTable).ToList();
        }


        public List<CallsSummaryForGateway> GetSiteGatwaysCallsSummary(string siteName, DateTime? startingDate = null, DateTime? endingDate = null)
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

    }

}
