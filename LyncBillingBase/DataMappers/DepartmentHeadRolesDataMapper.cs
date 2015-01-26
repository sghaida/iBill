using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DataMappers
{
    public class DepartmentHeadRolesDataMapper : DataAccess<DepartmentHeadRole>
    {
        /// <summary>
        ///     Given a User's SipAccount, return whether this User has the role of a Department Head.
        /// </summary>
        /// <param name="userSipAccount">User.SipAccount (string).</param>
        /// <returns>boolean.</returns>
        public bool IsDepartmentHead(string userSipAccount)
        {
            List<DepartmentHeadRole> roles = null;
            var condition = new Dictionary<string, object>();
            condition.Add("SipAccount", userSipAccount);

            try
            {
                roles = Get(condition, 1).ToList();

                if (roles != null && roles.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userSipAccount"></param>
        /// <returns></returns>
        public List<DepartmentHeadRole> GetBySipAccount(string userSipAccount)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("SipAccount", userSipAccount);

            try
            {
                return Get(condition, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Site-Deparment's ID, return all the associated Department-Head User Roles.
        /// </summary>
        /// <param name="siteDepartmentId">SiteDepartment.ID (int).</param>
        /// <returns>List of DepartmentHeadRole objects.</returns>
        public List<DepartmentHeadRole> GetBySiteDepartmentId(int siteDepartmentId)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("SiteDepartmentID", siteDepartmentId);

            try
            {
                return Get(condition, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override DepartmentHeadRole GetById(long id, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            DepartmentHeadRole role = null;

            try
            {
                role = base.GetById(id, dataSourceName, dataSource);

                if (role != null)
                {
                    var temporaryList = new List<DepartmentHeadRole> {role} as IEnumerable<DepartmentHeadRole>;
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

        public override IEnumerable<DepartmentHeadRole> Get(Dictionary<string, object> whereConditions, int limit = 25,
            string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            IEnumerable<DepartmentHeadRole> roles = null;

            try
            {
                roles = base.Get(whereConditions, limit, dataSourceName, dataSource);

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

        public override IEnumerable<DepartmentHeadRole> Get(Expression<Func<DepartmentHeadRole, bool>> predicate,
            string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
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

        public override IEnumerable<DepartmentHeadRole> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            IEnumerable<DepartmentHeadRole> roles = null;

            try
            {
                roles = base.GetAll(dataSourceName, dataSource);

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

        public override IEnumerable<DepartmentHeadRole> GetAll(string sqlQuery)
        {
            IEnumerable<DepartmentHeadRole> roles = null;

            try
            {
                roles = base.GetAll(sqlQuery);

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