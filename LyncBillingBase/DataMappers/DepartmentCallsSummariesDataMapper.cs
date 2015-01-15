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
        private readonly List<string> _dbTables = new List<string>();

        /***
         * Predefined SQL Queries Store.
         */
        private readonly CallsSummariesForDepartmentsSQL _summariesSqlQueries = new CallsSummariesForDepartmentsSQL();

        public DepartmentCallsSummariesDataMapper()
        {
            _dbTables = _monitoringServersInfoDataMapper.GetAll().Select(item => item.PhoneCallsTable).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteName"></param>
        /// <param name="DepartmentName"></param>
        /// <returns></returns>
        public List<CallsSummaryForDepartment> GetByDepartment(string SiteName, string DepartmentName)
        {
            DateTime StartDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
            DateTime EndDate = DateTime.Now;

            try
            {
                return this.GetByDepartment(SiteName, DepartmentName, StartDate, EndDate);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteName"></param>
        /// <param name="DepartmentName"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<CallsSummaryForDepartment> GetByDepartment(string SiteName, string DepartmentName, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var sql = _summariesSqlQueries.GetCallsSummariesForDepartment(
                    SiteName, 
                    DepartmentName, 
                    StartDate.ConvertDate(true),
                    EndDate.ConvertDate(true),
                    _dbTables);

                return base.GetAll(sql).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteName"></param>
        /// <param name="DepartmentName"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public CallsSummaryForDepartment GetTotalByDepartment(string SiteName, string DepartmentName, DateTime StartDate, DateTime EndDate)
        {
            CallsSummaryForDepartment departmentTotalSummary = new CallsSummaryForDepartment();

            departmentTotalSummary.SiteName = SiteName;
            departmentTotalSummary.DepartmentName = DepartmentName;

            try
            {
                var summaries = GetByDepartment(SiteName, DepartmentName, StartDate, EndDate);

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
        /// 
        /// </summary>
        /// <param name="SiteName"></param>
        /// <returns></returns>
        public Dictionary<string, CallsSummaryForDepartment> GetTotalsForEachDepartmentInSite(string SiteName)
        {
            Dictionary<string, CallsSummaryForDepartment> siteDepartmentsTotals = new Dictionary<string, CallsSummaryForDepartment>();

            DateTime StartDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
            DateTime EndDate = DateTime.Now;

            try
            {
                return this.GetTotalsForEachDepartmentInSite(SiteName);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public Dictionary<string, CallsSummaryForDepartment> GetTotalsForEachDepartmentInSite(string SiteName, DateTime StartDate, DateTime EndDate)
        {
            Dictionary<string, CallsSummaryForDepartment> siteDepartmentsTotals = new Dictionary<string, CallsSummaryForDepartment>();

            List<SiteDepartment> departments = _siteDepartmentsDataMapper.GetAll().Where(item => item.Site != null && item.Site.Name == SiteName).ToList();

            if (departments != null && departments.Count > 0)
            {
                try
                {
                    CallsSummaryForDepartment tempTotalSummary;

                    foreach (SiteDepartment department in departments)
                    {
                        string siteName = department.Site.Name;
                        string departmentName = department.Department.Name;

                        tempTotalSummary = this.GetTotalByDepartment(siteName, departmentName, StartDate, EndDate);

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
