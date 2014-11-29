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
    public class SystemRolesDataMapper : DataAccess<SystemRole>
    {
        private SitesDataMapper SitesAccessor = new SitesDataMapper();
        private List<Role> RolesInformation = Role.GetRolesInformation();


        public List<SystemRole> GetSystemRolesForUser(string sipAccount)
        {
            throw new NotImplementedException();
        }

        public bool ValidateRoleForUser(int RoleID, string EmailAddress)
        {
            throw new NotImplementedException();
        }
    }
}
