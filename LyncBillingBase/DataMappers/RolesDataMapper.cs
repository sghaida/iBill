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
    public class RolesDataMapper : DataAccess<Role>
    {
        public List<Role> AllRoles = new List<Role>();

        public RolesDataMapper()
        {
            AllRoles = GetAll().ToList<Role>();
        }
    }
}
