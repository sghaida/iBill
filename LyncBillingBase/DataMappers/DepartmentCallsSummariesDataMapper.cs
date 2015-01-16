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
    public class DepartmentCallsSummariesDataMapper : DataAccess<CallsSummaryForDepartment>
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

        public DepartmentCallsSummariesDataMapper()
        {
            _dbTables = _monitoringServersInfoDataMapper.GetAll().Select(item => item.PhoneCallsTable).ToList();
        }

        /// <summary>
        /// Given a Site Name and a Department Name, return the calls summary for that Department for every month in the specified date & time range.
        /// If the date and time range was not specified, a default date and time range will be constructed for one year before, starting from DateTime.Now.
        /// </summary>
        /// <param name="siteName">Department's Site Name</param>
        /// <param name="departmentName">Department Name</param>
        /// <param name="startDate">Optional. Specifies the Starting Date Range.</param>
        /// <param name="endDate">Optional. specifies the Ending Date Range.</param>
        /// <returns>List of CallsSummaryForDepartment objects for every month.</returns>
        public List<CallsSummaryForDepartment> GetByDepartment(string siteName, string departmentName, DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime fromDate, toDate;
            List<CallsSummaryForDepartment> departmentSummaries;

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
                var sql = _summariesSqlQueries.GetCallsSummariesForDepartment(
                    siteName, 
                    departmentName,
                    fromDate.ConvertDate(true),
                    toDate.ConvertDate(true),
                    _dbTables);

                departmentSummaries = base.GetAll(sql).ToList();

                if (departmentSummaries != null && departmentSummaries.Any())
                {
                    departmentSummaries.ForEach(
                        (summary) => {
                            summary.SiteName = siteName;
                            summary.DepartmentName = departmentName;
                        });
                }

                return departmentSummaries;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Given a Site Name and a Department Name, return the Total Calls Summary for that Department for all months in the specified date & time range.
        /// If the date and time range was not specified, a default date and time range will be constructed for one year before, starting from DateTime.Now.
        /// </summary>
        /// <param name="siteName">Department's Site Name</param>
        /// <param name="departmentName">Department Name</param>
        /// <param name="startDate">Optional. Specifies the Starting Date Range.</param>
        /// <param name="endDate">Optional. specifies the Ending Date Range.</param>
        /// <returns>A CallsSummaryForDepartment object with the totals for all months.</returns>
        public CallsSummaryForDepartment GetTotalByDepartment(string siteName, string departmentName, DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime fromDate, toDate;
            CallsSummaryForDepartment departmentTotalSummary = new CallsSummaryForDepartment();

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

            departmentTotalSummary.SiteName = siteName;
            departmentTotalSummary.DepartmentName = departmentName;

            try
            {
                var summaries = GetByDepartment(siteName, departmentName, fromDate, toDate);

                foreach(var summary in summaries)
                {
                    departmentTotalSummary.BusinessCallsCost += summary.BusinessCallsCost;
                    departmentTotalSummary.BusinessCallsCount += summary.BusinessCallsCount;
                    departmentTotalSummary.BusinessCallsDuration += summary.BusinessCallsDuration;

                    departmentTotalSummary.PersonalCallsCost += summary.PersonalCallsCost;
                    departmentTotalSummary.PersonalCallsCount += summary.PersonalCallsCount;
                    departmentTotalSummary.PersonalCallsDuration += summary.PersonalCallsDuration;

                    departmentTotalSummary.UnmarkedCallsCost += summary.UnmarkedCallsCost;
                    departmentTotalSummary.UnmarkedCallsCount += summary.UnmarkedCallsCount;
                    departmentTotalSummary.UnmarkedCallsDuration += summary.UnmarkedCallsDuration;
                }

                return departmentTotalSummary;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Given a Site Name, return the Total Calls Summaries for all the Departments in that Site.
        /// If the date and time range was not specified, a default date and time range will be constructed for one year before, starting from DateTime.Now.
        /// </summary>
        /// <param name="siteName">Site Name</param>
        /// <param name="startDate">Optional. Specifies the Starting Date Range.</param>
        /// <param name="endDate">Optional. specifies the Ending Date Range.</param>
        /// <returns>Dictionary of CallsSummaryForDepartment, indexed by each Department's Name.</returns>
        public Dictionary<string, CallsSummaryForDepartment> GetTotalsForEachDepartmentInSite(string siteName, DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime fromDate, toDate;
            CallsSummaryForDepartment departmentTotalSummary = new CallsSummaryForDepartment();
            Dictionary<string, CallsSummaryForDepartment> siteDepartmentsTotals = new Dictionary<string, CallsSummaryForDepartment>();
            List<SiteDepartment> departments = _siteDepartmentsDataMapper.GetAll().Where(item => item.Site != null && item.Site.Name == siteName).ToList();

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

            if (departments != null && departments.Count > 0)
            {
                try
                {
                    CallsSummaryForDepartment tempTotalSummary;

                    foreach (SiteDepartment department in departments)
                    {
                        string departmentName = department.Department.Name;

                        tempTotalSummary = this.GetTotalByDepartment(siteName, departmentName, fromDate, endDate);

                        siteDepartmentsTotals.Add(departmentName, tempTotalSummary);
                    }
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            return siteDepartmentsTotals;
        }


        /***
         * DISABLED FUNCTIONS
         */

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual CallsSummaryForDepartment GetById(long id, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForDepartment> Get(Dictionary<string, object> whereConditions,
            int limit = 25, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForDepartment> Get(Expression<Func<CallsSummaryForDepartment, bool>> predicate,
            string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForDepartment> GetAll(string sqlQuery)
        {
            throw new NotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new virtual IEnumerable<CallsSummaryForDepartment> GetAll(string dataSourceName = null,
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
        public new virtual int Insert(CallsSummaryForDepartment dataObject, string dataSourceName = null,
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
        public new virtual bool Update(CallsSummaryForDepartment dataObject, string dataSourceName = null,
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
        public new virtual bool Delete(CallsSummaryForDepartment dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotSupportedException();
        }

    }

}
