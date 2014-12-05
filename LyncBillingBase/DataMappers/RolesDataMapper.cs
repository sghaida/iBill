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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public Role GetByRoleID(int RoleID)
        {
            Role role = null;

            var condition = new Dictionary<string, object>();
            condition.Add("RoleID", RoleID);

            try
            {
                var results = base.Get(whereConditions: condition, limit: 1).ToList<Role>();

                if(results != null && results.Count > 0)
                {
                    role = results.First();
                }

                return role;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }

}
