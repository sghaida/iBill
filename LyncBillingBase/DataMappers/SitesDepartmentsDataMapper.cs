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
        public DataAccess<Site> Sites = new DataAccess<Site>();
        public DataAccess<Department> Departments = new DataAccess<Department>();

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
                    //Parallel.ForEach(sitesDepartments,
                    //    (siteDepartment) =>
                    //    {
                    //        siteDepartment.SiteName = site.Name;
                    //        siteDepartment.DepartmentName = (
                    //            siteDepartment.DepartmentID > 0 && departments.Find(dep => dep.ID == siteDepartment.DepartmentID) != null ? 
                    //            ((Department)departments.Find(dep => dep.ID == siteDepartment.DepartmentID))?Name : 
                    //            string.Empty
                    //        );
                    //    });

                    //sitesDepartments = (from siteDep in sitesDepartments
                    //                   join dep in departments on siteDep.DepartmentID equals dep.ID
                    //                   select new SiteDepartment
                    //                   {
                    //                       ID = siteDep.ID,
                    //                       SiteID = site.ID,
                    //                       SiteName = site.Name,
                    //                       DepartmentID = dep.ID,
                    //                       DepartmentName = dep.Name
                    //                   }).ToList<SiteDepartment>();

                    return sitesDepartments.ToList<SiteDepartment>();
                }
                catch(Exception ex)
                {
                    string x = string.Empty;
                }
                
                //sitesDepartments = sitesDepartments.AsEnumerable().AsParallel()
                //    .Select(item =>
                //    {
                //        item.DepartmentName = (departments.Find(dep => dep.ID == item.DepartmentID) != null ? (departments.Find(dep => dep.ID == item.DepartmentID)).Name : string.Empty);
                //        item.SiteName = site.Name;
                //        return item;
                //    })
                //    .ToList<SiteDepartment>();
            }

            return sitesDepartments;
        }
    }
}
