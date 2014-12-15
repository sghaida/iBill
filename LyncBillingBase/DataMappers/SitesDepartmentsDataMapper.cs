using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DataMappers
{
    public class SitesDepartmentsDataMapper : DataAccess<SiteDepartment>
    {
        /***
         * The local cache store of all sites departments
         */
        private List<SiteDepartment> _cachedData = new List<SiteDepartment>();



        /***
         * Singleton implementation with an attempted thread-safety using double-check locking
         */
        // internal datastorage singleton container
        private static SitesDepartmentsDataMapper _instance = null;

        // lock for thread-safety laziness
        private static readonly object _mutex = new object();

        // Empty private constuctor
        private SitesDepartmentsDataMapper() { }

        //The only public method, used to obtain an instance of DataStorage
        public static SitesDepartmentsDataMapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock(_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new SitesDepartmentsDataMapper();
                            _instance.InitializeCacheStore();
                        }
                    }
                }

                return _instance;
            }
        }


        /// <summary>
        /// This function is called upon the initialization of a singleton instance
        /// </summary>
        private void InitializeCacheStore()
        {
            this._cachedData = base.GetAll().Include(item => item.Site, item => item.Department).ToList<SiteDepartment>();
        }



        /***
         * The following are custom functionalities. Found to be needed by the UI project.
         */
        /// <summary>
        /// Given a Site's ID, return the list of it's Site-Departments.
        /// </summary>
        /// <param name="SiteID">Site.ID (int)</param>
        /// <returns>List of SiteDepartment objects</returns>
        public List<SiteDepartment> GetBySiteID(long SiteID)
        {
            Dictionary<string, object> condition = new Dictionary<string,object>();
            condition.Add("SiteID", SiteID);

            try
            {
                return this.Get(whereConditions: condition, limit: 0).ToList<SiteDepartment>();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Site's ID, return the list of it's Departments.
        /// </summary>
        /// <param name="SiteID">Site.ID (int)</param>
        /// <returns>List of Department objects</returns>
        public List<Department> GetDepartmentsBySiteID(long SiteID)
        {
            List<Department> departments = null;
            List<SiteDepartment> siteDepartments = null;

            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("SiteID", SiteID);

            try
            {
                siteDepartments = this.Get(whereConditions: condition, limit: 0).ToList<SiteDepartment>();

                if(siteDepartments != null && siteDepartments.Count > 0)
                {
                    departments = siteDepartments.Select<SiteDepartment, Department>(siteDep => siteDep.Department).ToList<Department>();
                }

                return departments;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }



        /***
         * Overriden implementations of DataAccess. These implementations read from the _cachedData list, and write to the DB and the list.
         */
        public override int Insert(SiteDepartment dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            int rowId = 0;

            try
            {
                rowId = base.Insert(dataObject, dataSourceName, dataSourceType);

                _cachedData.Add(dataObject);

                return rowId;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override bool Update(SiteDepartment dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            bool status = false;

            try
            { 
                status = base.Update(dataObject, dataSourceName, dataSourceType);

                if(status == true)
                {
                    var oldSiteDepartment = _cachedData.Find(item => item.ID == dataObject.ID);
                    _cachedData.Remove(oldSiteDepartment);
                    _cachedData.Add(dataObject);
                }

                return status;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override bool Delete(SiteDepartment dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            bool status = false;

            try
            {
                status = base.Delete(dataObject, dataSourceName, dataSourceType);

                if (status == true)
                {
                    var oldSiteDepartment = _cachedData.Find(item => item.ID == dataObject.ID);
                    _cachedData.Remove(oldSiteDepartment);
                }

                return status;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override SiteDepartment GetById(long id, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            try
            {
                return _cachedData.Find(item => item.ID == id);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<SiteDepartment> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _cachedData;
        }



        /***
         * The following functions will raise an exception when they are called.
         * Access to the Sites Departments data was restricted to the above methods.
         * All SQL-parameterized functions are not allowed for this data mapper.
         */
        public override int Insert(string sql) { throw new NotImplementedException(); }
        public override bool Update(string sql) { throw new NotImplementedException(); }
        public override bool Delete(string sql) { throw new NotImplementedException(); }
        public override IEnumerable<SiteDepartment> GetAll(string SQL_QUERY) { throw new NotImplementedException(); }
        public override IEnumerable<SiteDepartment> Get(Expression<Func<SiteDepartment, bool>> predicate, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default) { throw new NotImplementedException(); }
        public override IEnumerable<SiteDepartment> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default) { throw new NotImplementedException(); }
    }

}
