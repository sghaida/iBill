using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class SitesDataMapper : DataAccess<Site>
    {
        private DataAccess<Department> Departments = new DataAccess<Department>();
        private DataAccess<SiteDepartment> SitesDepartments = new DataAccess<SiteDepartment>();

        //Identity Map
        private List<IdentityMapItem<Site>> IdentityMap = new List<IdentityMapItem<Site>>();

        //Identity Map Updater functions
        private void UpdateIdentityMapItem(Site site)
        {
            var now = DateTime.Now;
            var cachedVersion = IdentityMap.Find(item => item.DataObject.ID == site.ID);

            if (cachedVersion != null)
            {
                int index = IdentityMap.IndexOf(cachedVersion);

                lock (IdentityMap[index].MutexLock)
                {
                    IdentityMap[index].DataObject.Name = site.Name;
                    IdentityMap[index].DataObject.CountryCode = site.CountryCode;
                    IdentityMap[index].DataObject.CountryName = site.CountryName;
                    IdentityMap[index].DataObject.Description = site.Description;

                    IdentityMap[index].Updated = true;
                    IdentityMap[index].DBSynced = false;
                }
            }
        }

        private void UpdateIdentityMap(ref List<Site> sites)
        {
            var now = DateTime.Now;

            IdentityMap = sites
                .Select(item => new IdentityMapItem<Site>{
                    DataObject = item,
                    AddedOn = now,
                    Updated = false,
                    DBSynced = true
                })
                .ToList<IdentityMapItem<Site>>();
        }


        //Get all sites
        public IEnumerable<Site> GetAll()
        {
            List<Site> sites = new List<Site>();

            if (IdentityMap.Count > 0)
            {
                sites = IdentityMap.Select(item => item.DataObject).ToList<Site>();
            }
            else
            {
                sites = base.GetAll().ToList<Site>();
                UpdateIdentityMap(ref sites);
            }

            return sites;
        }

        //Get a site by it's ID
        public Site GetById(long id)
        {
            Site site;

            if(IdentityMap.Count > 0)
            {
                var cachedSite = IdentityMap.Find(item => item.DataObject.ID == id);

                if (cachedSite != null)
                {
                    site = cachedSite.DataObject;
                }
                else
                {
                    site = base.GetById(id);
                }
            }
            else
            {
                site = base.GetById(id);
            }

            return site;
        }

        //Get a site that matches a specific set of conditions.
        public IEnumerable<Site> Get(Dictionary<string, object> whereCondition, int limit = 25)
        {
            return base.Get(whereCondition, limit);
        }

        //Get a site that matches a specific search predicate
        public IEnumerable<Site> Get(Expression<Func<Site, bool>> predicate)
        {
            return base.Get(predicate);
        }


        ////Get a site-department by it's id
        //public SiteDepartment GetSiteDepartmentById(long siteDepartmentID)
        //{
        //    return SitesDepartments.GetById(siteDepartmentID);
        //}

        ////Get all site-departments for a site
        //public IEnumerable<SiteDepartment> GetSiteDepartments(long siteID)
        //{
        //    Expression<Func<SiteDepartment, bool>> expr = (item) => item.SiteID == siteID;

        //    return SitesDepartments.Get(expr);
        //}

        ////Get all site-departments that match a specific predicate
        //public IEnumerable<SiteDepartment> GetSiteDepartments(Expression<Func<SiteDepartment, bool>> predicate)
        //{
        //    return SitesDepartments.Get(predicate);
        //}

    }

}
