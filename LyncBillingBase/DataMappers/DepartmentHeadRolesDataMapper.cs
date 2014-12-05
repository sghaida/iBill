using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.Helpers;
using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class DepartmentHeadRolesDataMapper : DataAccess<DepartmentHeadRole>
    {
        //
        // This instance of the SitesDepartments DataMapper is used only for data binding with the local SiteDepartment object.
        private SitesDepartmentsDataMapper _sitesDepartmentsDataMapper = new SitesDepartmentsDataMapper();


        /// <summary>
        /// Given a list of DepartmentHeadRole, return the same list of roles with complete SiteDepartments objects.
        /// We implement this functionality here because we cannot get the relations of the SiteDepartment Data Model from within the DelegateRole Data Model,
        ///     there is no nested relations feature.
        /// </summary>
        /// <param name="departmentHeadsRoles">A list of DepartmentHeadRole objects.</param>
        private void FillSiteDepartmentsData(ref IEnumerable<DepartmentHeadRole> departmentHeadsRoles)
        {
            try
            {
                // Get all sitesDepartments
                IEnumerable<SiteDepartment> allSitesDepartments = _sitesDepartmentsDataMapper.GetAll();

                //Enable parallelization of the enumerable collections
                allSitesDepartments = allSitesDepartments.AsParallel<SiteDepartment>();
                departmentHeadsRoles = departmentHeadsRoles.AsParallel<DepartmentHeadRole>();

                //Fitler, join, and project
                departmentHeadsRoles =
                    (from role in departmentHeadsRoles
                     where (role.SiteDepartmentID > 0 && (role.SiteDepartment != null && role.SiteDepartment.ID > 0))
                     join siteDepartment in allSitesDepartments on role.SiteDepartmentID equals siteDepartment.ID
                     select new DepartmentHeadRole
                     {
                         ID = role.ID,
                         SipAccount = role.SipAccount,
                         SiteDepartmentID = role.SiteDepartmentID,
                         //RELATIONS
                         User = role.User,
                         SiteDepartment = siteDepartment
                     })
                     .AsEnumerable<DepartmentHeadRole>();
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
        public List<DepartmentHeadRole> GetBySiteDepartmentID(int SiteDepartmentID)
        {
            List<DepartmentHeadRole> roles = null;
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
                    FillSiteDepartmentsData(ref temporaryList);
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
                    this.FillSiteDepartmentsData(ref roles);
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
                    this.FillSiteDepartmentsData(ref roles);
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
                roles = base.GetAll(dataSourceName, dataSource).Include(item => item.SiteDepartment, item => item.SiteDepartment.Site, item => item.SiteDepartment.Department);

                //if(roles != null && roles.Count() > 0)
                //{
                //    this.FillSiteDepartmentsData(ref roles);
                //}

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
                    this.FillSiteDepartmentsData(ref roles);
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
