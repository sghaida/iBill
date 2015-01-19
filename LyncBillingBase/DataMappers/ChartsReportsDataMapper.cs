using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class ChartsReportsDataMapper : DataAccess<ChartReport>
    {
        private static UsersCallsSummariesDataMapper _usersSummaries = new UsersCallsSummariesDataMapper();
        private static DepartmentCallsSummariesDataMapper _departmentsSummaries = new DepartmentCallsSummariesDataMapper();
        private static SitesCallsSummariesDataMapper _sitesSummaries = new SitesCallsSummariesDataMapper();
        private static GatewaysCallsSummariesDataMapper _gatewaysSummaries = new GatewaysCallsSummariesDataMapper();


        /// <summary>
        /// Given a list of ChartReports and a ChartReport object, check if the single object exists in the list,
        /// if it does, then add its values to the existing object, otherwise add it to the list.
        /// </summary>
        /// <param name="chartReportsList">List of ChartReport objects.</param>
        /// <param name="report">ChartReport object, to be added to the list.</param>
        private static void AddOrUpdateListOfChartReports(ref List<ChartReport> chartReportsList, ChartReport report)
        {
            //If there is already a summary with the same name in the list, then just add it's values to this currently computed summary (departmentSummary)
            //This happens due to multiple phonecalls tables
            var existingReport = chartReportsList.SingleOrDefault(summary => summary.Name == report.Name);

            if (existingReport != null)
            {
                //Get the existing summary's index
                int summaryIndex = chartReportsList.IndexOf(existingReport);

                //Compute an updated summary
                report.TotalCalls += existingReport.TotalCalls;
                report.TotalCost += existingReport.TotalCost;
                report.TotalDuration += existingReport.TotalDuration;

                //Replace the old summary with the newly updated version of it.
                chartReportsList[summaryIndex] = report;
            }
            else
            {
                chartReportsList.Add(report);
            }
        }


        /// <summary>
        /// Given a User's SipAccount, and possibly a datetime range (startDate, endDate), return their charts-reports for that specified range.
        /// If the Date and Time range was not specified, the function specifies a default date & time range for one year before, starting from DateTim.Now.
        /// </summary>
        /// <param name="sipAccount">User's SipAccount</param>
        /// <param name="startDate">Optional. Specifies the Starting Date Range.</param>
        /// <param name="endDate">Optional. Specifies the Ending Date Range.</param>
        /// <returns>List of ChartReport objects.</returns>
        public List<ChartReport> GetByUser(string sipAccount, DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime fromDate, toDate;
            List<ChartReport> userChartReports = new List<ChartReport>();

            // Handle the null values of DateTime
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

            // Create a list of zero-valued chart reports (Business, Personal and Unallocated)
            userChartReports.Add((new ChartReport(LyncBillingGlobals.ChartReport.Name.Business.Value())));
            userChartReports.Add((new ChartReport(LyncBillingGlobals.ChartReport.Name.Personal.Value())));
            userChartReports.Add((new ChartReport(LyncBillingGlobals.ChartReport.Name.Unallocated.Value())));


            try
            {
                var summaries = _usersSummaries.GetBySipAccount(sipAccount, fromDate, toDate);

                if(summaries.Any())
                {
                    summaries.ForEach((summary) => {
                        var businessReport = new ChartReport(LyncBillingGlobals.ChartReport.Name.Business.Value());
                        businessReport.TotalCalls = summary.BusinessCallsCount;
                        businessReport.TotalCost = summary.BusinessCallsCost;
                        businessReport.TotalDuration = summary.BusinessCallsDuration;

                        var personalReport = new ChartReport(LyncBillingGlobals.ChartReport.Name.Personal.Value());
                        personalReport.TotalCalls = summary.PersonalCallsCount;
                        personalReport.TotalCost = summary.PersonalCallsCost;
                        personalReport.TotalDuration = summary.PersonalCallsDuration;

                        var unallocatedReport = new ChartReport(LyncBillingGlobals.ChartReport.Name.Unallocated.Value());
                        unallocatedReport.TotalCalls = summary.UnallocatedCallsCount;
                        unallocatedReport.TotalCost = summary.UnallocatedCallsCost;
                        unallocatedReport.TotalDuration = summary.UnallocatedCallsDuration;

                        AddOrUpdateListOfChartReports(ref userChartReports, businessReport);
                        AddOrUpdateListOfChartReports(ref userChartReports, personalReport);
                        AddOrUpdateListOfChartReports(ref userChartReports, unallocatedReport);
                    });
                }

                return userChartReports;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Site Name and a Department Name, return the charts-reports for that Department for every month in the specified date & time range.
        /// If the date and time range was not specified, a default date and time range will be constructed for one year before, starting from DateTime.Now.
        /// </summary>
        /// <param name="siteName">Department's Site Name.</param>
        /// <param name="departmentName">Department Name.</param>
        /// <param name="startDate">Optional. Specifies the Starting Date Range.</param>
        /// <param name="endDate">Optional. specifies the Ending Date Range.</param>
        /// <returns>List of ChartReport objects.</returns>
        public List<ChartReport> GetByDepartment(string siteName, string departmentName, DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime fromDate, toDate;
            List<ChartReport> departmentChartReports = new List<ChartReport>();

            // Handle the null values of DateTime
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

            // Create a list of zero-valued chart reports (Business, Personal and Unallocated)
            departmentChartReports.Add((new ChartReport(LyncBillingGlobals.ChartReport.Name.Business.Value())));
            departmentChartReports.Add((new ChartReport(LyncBillingGlobals.ChartReport.Name.Personal.Value())));
            departmentChartReports.Add((new ChartReport(LyncBillingGlobals.ChartReport.Name.Unallocated.Value())));


            try
            {
                var summaries = _departmentsSummaries.GetByDepartment(siteName, departmentName, fromDate, toDate);

                if (summaries.Any())
                {
                    summaries.ForEach((summary) =>
                    {
                        var businessReport = new ChartReport(LyncBillingGlobals.ChartReport.Name.Business.Value());
                        businessReport.TotalCalls = summary.BusinessCallsCount;
                        businessReport.TotalCost = summary.BusinessCallsCost;
                        businessReport.TotalDuration = summary.BusinessCallsDuration;

                        var personalReport = new ChartReport(LyncBillingGlobals.ChartReport.Name.Personal.Value());
                        personalReport.TotalCalls = summary.PersonalCallsCount;
                        personalReport.TotalCost = summary.PersonalCallsCost;
                        personalReport.TotalDuration = summary.PersonalCallsDuration;

                        var unallocatedReport = new ChartReport(LyncBillingGlobals.ChartReport.Name.Unallocated.Value());
                        unallocatedReport.TotalCalls = summary.UnallocatedCallsCount;
                        unallocatedReport.TotalCost = summary.UnallocatedCallsCost;
                        unallocatedReport.TotalDuration = summary.UnallocatedCallsDuration;

                        AddOrUpdateListOfChartReports(ref departmentChartReports, businessReport);
                        AddOrUpdateListOfChartReports(ref departmentChartReports, personalReport);
                        AddOrUpdateListOfChartReports(ref departmentChartReports, unallocatedReport);
                    });
                }

                return departmentChartReports;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Site Name, and possibly a date and time range, return its charts-reports.
        /// If the date and time range was not specified, a default range will be constructed for one year before, starting from DateTime.Now.
        /// </summary>
        /// <param name="siteName">Site Name.</param>
        /// <param name="startDate">Optional. Specifies the Starting Date Range.</param>
        /// <param name="endDate">Optional. Specifies the Ending Date Range.</param>
        /// <returns>List of ChartReport objects.</returns>
        public List<ChartReport> GetBySite(string siteName, DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime fromDate, toDate;
            List<ChartReport> siteChartReports = new List<ChartReport>();

            // Handle the null values of DateTime
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

            // Create a list of zero-valued chart reports (Business, Personal and Unallocated)
            siteChartReports.Add((new ChartReport(LyncBillingGlobals.ChartReport.Name.Business.Value())));
            siteChartReports.Add((new ChartReport(LyncBillingGlobals.ChartReport.Name.Personal.Value())));
            siteChartReports.Add((new ChartReport(LyncBillingGlobals.ChartReport.Name.Unallocated.Value())));


            try
            {
                var summaries = _sitesSummaries.GetBySite(siteName, fromDate, toDate);

                if (summaries.Any())
                {
                    summaries.ForEach((summary) =>
                    {
                        var businessReport = new ChartReport(LyncBillingGlobals.ChartReport.Name.Business.Value());
                        businessReport.TotalCalls = summary.BusinessCallsCount;
                        businessReport.TotalCost = summary.BusinessCallsCost;
                        businessReport.TotalDuration = summary.BusinessCallsDuration;

                        var personalReport = new ChartReport(LyncBillingGlobals.ChartReport.Name.Personal.Value());
                        personalReport.TotalCalls = summary.PersonalCallsCount;
                        personalReport.TotalCost = summary.PersonalCallsCost;
                        personalReport.TotalDuration = summary.PersonalCallsDuration;

                        var unallocatedReport = new ChartReport(LyncBillingGlobals.ChartReport.Name.Unallocated.Value());
                        unallocatedReport.TotalCalls = summary.UnallocatedCallsCount;
                        unallocatedReport.TotalCost = summary.UnallocatedCallsCost;
                        unallocatedReport.TotalDuration = summary.UnallocatedCallsDuration;

                        AddOrUpdateListOfChartReports(ref siteChartReports, businessReport);
                        AddOrUpdateListOfChartReports(ref siteChartReports, personalReport);
                        AddOrUpdateListOfChartReports(ref siteChartReports, unallocatedReport);
                    });
                }

                return siteChartReports;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        // <summary>
        /// Given a Site Name, a Gateway Name, and possibly a date and time range, return its charts-reports.
        /// If a date and time range was not specified, a default date and time range will be constructed with a one year before, starting from DateTime.Now.
        /// </summary>
        /// <param name="siteName">Site Name</param>
        /// <param name="gatewayName">The Gateway Name</param>
        /// <param name="startingDate">Optional. The Starting Date Range.</param>
        /// <param name="endingDate">Optional. The Ending Date Range.</param>
        /// <returns>List of ChartReport objects.</returns>
        public List<ChartReport> GetByGateway(string siteName, string gatewayName, DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime fromDate, toDate;
            List<ChartReport> gatewayChartReports = new List<ChartReport>();

            // Handle the null values of DateTime
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

            // Create a list of zero-valued chart reports (Business, Personal and Unallocated)
            gatewayChartReports.Add((new ChartReport(LyncBillingGlobals.ChartReport.Name.Business.Value())));
            gatewayChartReports.Add((new ChartReport(LyncBillingGlobals.ChartReport.Name.Personal.Value())));
            gatewayChartReports.Add((new ChartReport(LyncBillingGlobals.ChartReport.Name.Unallocated.Value())));


            try
            {
                var summaries = _gatewaysSummaries.GetBySiteAndGateway(siteName, gatewayName, fromDate, toDate, Globals.CallsSummaryForGateway.GroupBy.GatewayNameOnly);

                if (summaries.Any())
                {
                    summaries.ForEach((summary) =>
                    {
                        var businessReport = new ChartReport(LyncBillingGlobals.ChartReport.Name.Business.Value());
                        businessReport.TotalCalls = summary.BusinessCallsCount;
                        businessReport.TotalCost = summary.BusinessCallsCost;
                        businessReport.TotalDuration = summary.BusinessCallsDuration;

                        var personalReport = new ChartReport(LyncBillingGlobals.ChartReport.Name.Personal.Value());
                        personalReport.TotalCalls = summary.PersonalCallsCount;
                        personalReport.TotalCost = summary.PersonalCallsCost;
                        personalReport.TotalDuration = summary.PersonalCallsDuration;

                        var unallocatedReport = new ChartReport(LyncBillingGlobals.ChartReport.Name.Unallocated.Value());
                        unallocatedReport.TotalCalls = summary.UnallocatedCallsCount;
                        unallocatedReport.TotalCost = summary.UnallocatedCallsCost;
                        unallocatedReport.TotalDuration = summary.UnallocatedCallsDuration;

                        AddOrUpdateListOfChartReports(ref gatewayChartReports, businessReport);
                        AddOrUpdateListOfChartReports(ref gatewayChartReports, personalReport);
                        AddOrUpdateListOfChartReports(ref gatewayChartReports, unallocatedReport);
                    });
                }

                return gatewayChartReports;
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
        public new virtual ChartReport GetById(long id, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<ChartReport> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<ChartReport> Get(Expression<Func<ChartReport, bool>> predicate, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<ChartReport> GetAll(string sqlQuery)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<ChartReport> GetAll(string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual int Insert(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual int Insert(ChartReport dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Update(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Update(ChartReport dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Delete(string sql)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual bool Delete(ChartReport dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }
    }
}
