using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class SitesDepartmentsDataMapper : DataAccess<SiteDepartment>
    {
        /***
         * Singleton implementation with an attempted thread-safety using double-check locking
         */
        // internal datastorage singleton container

        private static SitesDepartmentsDataMapper _instance;

        // lock for thread-safety laziness
        private static readonly object Mutex = new object();
        
        //The local cache store of all sites departments
        private List<SiteDepartment> _cachedData = new List<SiteDepartment>();

        // Empty private constuctor
        private SitesDepartmentsDataMapper() { }

        //The only public method, used to obtain an instance of DataStorage
        public static SitesDepartmentsDataMapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Mutex)
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
        ///     This function is called upon the initialization of a singleton instance
        /// </summary>
        private void InitializeCacheStore()
        {
            _cachedData = base.GetAll().GetWithRelations(item => item.Site, item => item.Department).ToList() ?? (new List<SiteDepartment>());
        }


        /***
         * The following are custom functionalities. Found to be needed by the UI project.
         */

        /// <summary>
        ///     Given a Site's ID, return the list of it's Site-Departments.
        /// </summary>
        /// <param name="siteId">Site.ID (int)</param>
        /// <returns>List of SiteDepartment objects</returns>
        public List<SiteDepartment> GetBySiteId(long siteId)
        {
            try
            {
                return _cachedData.Where(item => item.SiteId == siteId).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Site's ID, return the list of it's Departments.
        /// </summary>
        /// <param name="siteId">Site.ID (int)</param>
        /// <returns>List of Department objects</returns>
        public List<Department> GetDepartmentsBySiteId(long siteId)
        {
            List<Department> departments = null;
            List<SiteDepartment> siteDepartments = null;

            try
            {
                siteDepartments = this.GetBySiteId(siteId) ?? (new List<SiteDepartment>());

                if (siteDepartments.Count > 0)
                {
                    departments = siteDepartments.Select(siteDep => siteDep.Department).ToList();
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

        public override int Insert(SiteDepartment dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var rowId = 0;

            try
            {
                rowId = base.Insert(dataObject, dataSourceName, dataSourceType);
                dataObject.Id = rowId;
                dataObject = dataObject.GetWithRelations(item => item.Site, item => item.Department);

                lock(_cachedData)
                {
                    _cachedData.Add(dataObject);
                }

                return rowId;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override bool Update(SiteDepartment dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var status = false;

            try
            {
                status = base.Update(dataObject, dataSourceName, dataSourceType);

                if (status)
                {
                    // Remove the old site department
                    var oldSiteDepartment = _cachedData.Find(item => item.Id == dataObject.Id);

                    if (oldSiteDepartment != null)
                    {
                        lock (_cachedData)
                        {
                            _cachedData.Remove(oldSiteDepartment);
                        }
                    }

                    // Get the Site and Department relations of the new site department
                    dataObject = dataObject.GetWithRelations(item => item.Site, item => item.Department);

                    lock (_cachedData)
                    {
                        _cachedData.Add(dataObject);
                    }
                }

                return status;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override bool Delete(SiteDepartment dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var status = false;

            try
            {
                status = base.Delete(dataObject, dataSourceName, dataSourceType);

                if (status)
                {
                    var oldSiteDepartment = _cachedData.Find(item => item.Id == dataObject.Id);

                    if (oldSiteDepartment != null)
                    {
                        lock (_cachedData)
                        {
                            _cachedData.Remove(oldSiteDepartment);
                        }
                    }
                }

                return status;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override SiteDepartment GetById(long id, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            try
            {
                return _cachedData.Find(item => item.Id == id);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<SiteDepartment> GetAll(string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _cachedData;
        }

        /***
         * The following functions will raise an exception when they are called.
         * Access to the Sites Departments data was restricted to the above methods.
         * All SQL-parameterized functions are not allowed for this data mapper.
         */

        public override int Insert(string sql)
        {
            throw new NotImplementedException();
        }

        public override bool Update(string sql)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(string sql)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<SiteDepartment> GetAll(string sqlQuery)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<SiteDepartment> Get(Expression<Func<SiteDepartment, bool>> predicate,
            string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<SiteDepartment> Get(Dictionary<string, object> whereConditions, int limit = 25,
            string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }
    }
}