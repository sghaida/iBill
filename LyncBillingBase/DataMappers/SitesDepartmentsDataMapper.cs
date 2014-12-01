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
        //Get all site-departments for a site
        public List<SiteDepartment> GetDepartmentsForSite(long siteID)
        {
            Dictionary<string, object> condition = new Dictionary<string,object>();
            condition.Add("SiteID", siteID);

            try
            {
                var sitesDepartments = Get(whereConditions: condition, limit: 0).ToList<SiteDepartment>();
                return sitesDepartments;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }

}
