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
    public class SitesDepartmentsDataMapper : DataAccess<SiteDepartment>
    {
        //public DataAccess<Site> Sites = new DataAccess<Site>();
        //public DataAccess<Department> Departments = new DataAccess<Department>();

        public SitesDataMapper Sites = new SitesDataMapper();
        public DepartmentsDataMapper Departments = new DepartmentsDataMapper();

        //Get all site-deaprtments
        public IEnumerable<SiteDepartment> GetAll()
        {
            List<Site> sites;
            List<Department> departments;
            IEnumerable<SiteDepartment> sitesDepartments;

            sites = Sites.GetAll().ToList<Site>();
            departments = Departments.GetAll().ToList<Department>();
            sitesDepartments = base.GetAll().ToList<SiteDepartment>();

            if (sites.Count() > 0 && departments.Count() > 0 && sitesDepartments.Count() > 0)
            {
                try
                {
                    sitesDepartments = (from siteDep in sitesDepartments
                                        join site in sites on siteDep.SiteID equals site.ID
                                        join dep in departments on siteDep.DepartmentID equals dep.ID
                                        select new SiteDepartment
                                        {
                                            ID = siteDep.ID,
                                            SiteID = site.ID,
                                            SiteName = site.Name,
                                            DepartmentID = dep.ID,
                                            DepartmentName = dep.Name
                                        }).ToList<SiteDepartment>();
                }
                catch (Exception ex)
                {
                    throw new Exception("A LINQ query error occurred. Couldn't join SiteDepartments with Departments.");
                }
            }

            return sitesDepartments;
        }


        //Get all site-departments for a site
        public IEnumerable<SiteDepartment> GetDepartmentsForSite(long siteID)
        {
            Site site;
            List<Department> departments;
            IEnumerable<SiteDepartment> sitesDepartments;
            Dictionary<string, object> whereConditions = new Dictionary<string,object>();

            whereConditions.Add("SiteID", siteID);

            site = Sites.GetById(siteID);
            departments = Departments.GetAll().ToList<Department>();
            sitesDepartments = base.Get(whereConditions, 0);
            
            if (site != null && sitesDepartments.Count() > 0)
            {
                try
                { 
                    sitesDepartments = (from siteDep in sitesDepartments
                                        join dep in departments on siteDep.DepartmentID equals dep.ID
                                        select new SiteDepartment
                                        {
                                            ID = siteDep.ID,
                                            SiteID = site.ID,
                                            SiteName = site.Name,
                                            DepartmentID = dep.ID,
                                            DepartmentName = dep.Name
                                        }).ToList<SiteDepartment>();
                }
                catch(Exception ex)
                {
                    throw new Exception("A LINQ query error occurred. Couldn't join SiteDepartments with Departments.");
                }
            }

            return sitesDepartments;
        }
    }
}
