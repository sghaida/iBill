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

        //public DataAccess<SiteDepartment> Departments = new DataAccess<SiteDepartment>();
        //public SitesDepartmentsDataMapper Departments = new SitesDepartmentsDataMapper();

        //Insert site
        public int Insert(Site dataObject)
        {
            return base.Insert(dataObject);
        }

        //Update site
        public bool Update(Site dataObject)
        {
            return base.Update(dataObject);
        }

        //Delete a site
        public bool Delete(Site dataObject)
        {
            return base.Delete(dataObject);
        }

        //Get a site by it's ID
        public Site GetById(long id)
        {
            return base.GetById(id);
        }

        //Get a site that matches a specific set of conditions.
        public IEnumerable<Site> Get(Dictionary<string, object> where, int limit = 25)
        {
            return base.Get(where, limit);
        }

        //Get a site that matches a specific search predicate
        public IEnumerable<Site> Get(Expression<Func<Site, bool>> predicate)
        {
            return base.Get(predicate);
        }

        //Get all sites
        public IEnumerable<Site> GetAll(string dataSourceName)
        {
            return base.GetAll(dataSourceName);
        }

        //Get a site-department by it's id
        public SiteDepartment GetSiteDepartmentById(long siteDepartmentID)
        {
            return SitesDepartments.GetById(siteDepartmentID);
        }

        //Get all site-departments for a site
        public IEnumerable<SiteDepartment> GetSiteDepartments(long siteID)
        {
            Expression<Func<SiteDepartment, bool>> expr = (item) => item.SiteID == siteID;

            return SitesDepartments.Get(expr);
        }

        //Get all site-departments that match a specific predicate
        public IEnumerable<SiteDepartment> GetSiteDepartments(Expression<Func<SiteDepartment, bool>> predicate)
        {
            return SitesDepartments.Get(predicate);
        }

    }

}
