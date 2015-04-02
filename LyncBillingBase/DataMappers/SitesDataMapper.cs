using System;
using System.Linq;
using System.Collections.Generic;

using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class SitesDataMapper : DataAccess<Site>
    {
        private static List<Site> _sites = new List<Site>();


        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public SitesDataMapper()
        {
            LoadSites();
        }


        /// <summary>
        /// INITIALIZES THE INTERNAL SITES CACHE
        /// </summary>
        public void LoadSites()
        {
            if (_sites == null || _sites.Count == 0)
            {
                lock (_sites)
                {
                    _sites = base.GetAll().ToList() ?? (new List<Site>());
                }
            }
        }


        public override Site GetById(long id, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _sites.Find(site => site.Id == id);
        }


        public override IEnumerable<Site> GetAll(string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _sites;
        }


        public override int Insert(Site dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            //
            // Null Check
            if (dataObject == null)
            {
                return -1;
            }

            //
            // Containment and existence checks
            var isContained = _sites.Contains(dataObject);
            var itExists = _sites.Exists(
                item => 
                    item.Name == dataObject.Name && 
                    item.CountryCode == dataObject.CountryCode);

            if (isContained || itExists)
            {
                return -1;
            }

            try
            {
                dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);

                lock (_sites)
                {
                    _sites.Add(dataObject);
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            return dataObject.Id;
        }


        public override bool Update(Site dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            bool status = false;

            if(dataObject == null)
            {
                return status;
            }

            //
            // Existence check
            var site = _sites.Find(item => item.Id == dataObject.Id);

            if (site != null)
            {
                try
                {
                    status = base.Update(dataObject, dataSourceName, dataSourceType);

                    if (status == true)
                    {
                        lock (_sites)
                        {
                            _sites.Remove(site);
                            _sites.Add(dataObject);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            return status;
        }


        public override bool Delete(Site dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            bool status = false;

            //
            // Null Checks
            if(dataObject == null)
            {
                return status;
            }

            //
            // Existence checks
            var site = _sites.Find(item => item.Id == dataObject.Id);

            if (site != null)
            {
                try
                {
                    status = base.Delete(dataObject, dataSourceName, dataSourceType);

                    if (status == true)
                    {
                        lock(_sites)
                        {
                            _sites.Remove(site);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            return status;
        }


        /***
         * DISABLED FUNCTIONS
         */
        [Obsolete]
        public override IEnumerable<Site> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, CCC.ORM.Globals.DataSource.Type dataSourceType = CCC.ORM.Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override IEnumerable<Site> Get(System.Linq.Expressions.Expression<Func<Site, bool>> predicate, string dataSourceName = null, CCC.ORM.Globals.DataSource.Type dataSourceType = CCC.ORM.Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override IEnumerable<Site> GetAll(string sqlQuery)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override int Insert(string sql)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override bool Update(string sql)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override bool Delete(string sql)
        {
            throw new NotImplementedException();
        }

    }

}