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
        /// <summary>
        /// Given a User's SipAccount, return the list of Sites-Departments that he is assigned on as a Department Head.
        /// </summary>
        /// <param name="UserSipAccount">The Department Head SipAccount (string).</param>
        /// <returns>List of SiteDepartment objects.</returns>
        public List<SiteDepartment> GetSiteDepartmentsForUser(string UserSipAccount)
        {
            List<DepartmentHeadRole> roles = null;
            List<SiteDepartment> sitesDepartments = null;
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("SipAccount", UserSipAccount);

            try
            {
                roles = Get(whereConditions: condition, limit: 0).ToList<DepartmentHeadRole>();

                if(roles != null && roles.Count > 0)
                {
                    sitesDepartments = roles.Select<DepartmentHeadRole, SiteDepartment>(role => role.SiteDepartment).ToList<SiteDepartment>();
                }

                return sitesDepartments;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a User's SipAccount, return whether this User has the role of a Department Head.
        /// </summary>
        /// <param name="UserSipAccount">User.SipAccount (string).</param>
        /// <returns>boolean.</returns>
        public bool IsDepartmentHead(string UserSipAccount)
        {
            List<DepartmentHeadRole> roles = null;
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("SipAccount", UserSipAccount);

            try
            {
                roles = Get(whereConditions: condition, limit: 1).ToList<DepartmentHeadRole>();

                if (roles != null && roles.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Site-Deparment's ID, return all the associated Department-Head User Roles.
        /// </summary>
        /// <param name="SiteDepartmentID">SiteDepartment.ID (int).</param>
        /// <returns>List of DepartmentHeadRole objects.</returns>
        public List<DepartmentHeadRole> GetDepartmentHeads(int SiteDepartmentID)
        {
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("SiteDepartmentID", SiteDepartmentID);

            try
            {
                return Get(whereConditions: condition, limit: 0).ToList<DepartmentHeadRole>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }

}
