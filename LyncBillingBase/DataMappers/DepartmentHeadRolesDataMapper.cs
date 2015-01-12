using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using CCC.ORM;
using CCC.ORM.Helpers;
using CCC.ORM.DataAccess;
using LyncBillingBase.Helpers;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class DepartmentHeadRolesDataMapper : DataAccess<DepartmentHeadRole>
    {
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
        public List<DepartmentHeadRole> GetBySiteDepartmentID(int SiteDepartmentID)
        {
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("SiteDepartmentID", SiteDepartmentID);

            try
            {
                return this.Get(whereConditions: condition, limit: 0).ToList<DepartmentHeadRole>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override DepartmentHeadRole GetById(long id, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            DepartmentHeadRole role = null;

            try
            {
                role = base.GetById(id, dataSourceName, dataSource);

                if (role != null)
                {
                    var temporaryList = new List<DepartmentHeadRole>() { role } as IEnumerable<DepartmentHeadRole>;
                    temporaryList = temporaryList.IncludeSiteDepartments();
                    role = temporaryList.First();
                }

                return role;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }


        public override IEnumerable<DepartmentHeadRole> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            IEnumerable<DepartmentHeadRole> roles = null;

            try
            { 
                roles = base.Get(whereConditions, limit, dataSourceName, dataSource);

                if(roles != null && roles.Count() > 0)
                {
                    roles = roles.IncludeSiteDepartments();
                }

                return roles;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<DepartmentHeadRole> Get(Expression<Func<DepartmentHeadRole, bool>> predicate, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            IEnumerable<DepartmentHeadRole> roles = null;

            try
            {
                roles = base.Get(predicate, dataSourceName, dataSource);

                if (roles != null && roles.Count() > 0)
                {
                    roles = roles.IncludeSiteDepartments();
                }

                return roles;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<DepartmentHeadRole> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            IEnumerable<DepartmentHeadRole> roles = null;

            try
            {
                roles = base.GetAll(dataSourceName, dataSource);

                if(roles != null && roles.Count() > 0)
                {
                    roles = roles.IncludeSiteDepartments();
                }

                return roles;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<DepartmentHeadRole> GetAll(string sql)
        {
            IEnumerable<DepartmentHeadRole> roles = null;

            try
            {
                roles = base.GetAll(sql);

                if (roles != null && roles.Count() > 0)
                {
                    roles = roles.IncludeSiteDepartments();
                }

                return roles;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }

}
