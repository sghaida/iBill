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
    public class DepartmentHeadRolesDataMapper : DataAccess<DepartmentHeadRole>
    {
        public bool IsDepartmentHead(string userSipAccount)
        {
            throw new NotImplementedException();
        }

        public List<DepartmentHeadRole> GetDepartmentHeadsForSite(int siteDepartmentID)
        {
            throw new NotImplementedException();
        }

        public List<SiteDepartment> GetSiteDepartmentsForUser(string sipAccount)
        {
            throw new NotImplementedException();
        }
    }
}
