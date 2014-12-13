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
    public class DelegateRolesDataMapper : DataAccess<DelegateRole>
    {
        //
        // This instance of the SitesDepartments DataMapper is used only for data binding with the local SiteDepartment object.
        private SitesDepartmentsDataMapper _sitesDepartmentsDataMapper = new SitesDepartmentsDataMapper();


        /// <summary>
        /// Given a list of DelegateRoles, return the list of roles with complete SiteDepartments objects.
        /// We implement this functionality here because we cannot get the relations of the SiteDepartment Data Model from within the DelegateRole Data Model,
        ///     there is no nested relations feature.
        /// </summary>
        /// <param name="delegateRoles">A list of DelegateRole objects.</param>
        private void FillSiteDepartmentsData(ref IEnumerable<DelegateRole> delegateRoles)
        {
            try
            { 
                // Get all sites departments
                IEnumerable<SiteDepartment> allSitesDepartments = _sitesDepartmentsDataMapper.GetAll().ToList<SiteDepartment>();

                // Enable parallelization on the enumerable collections
                allSitesDepartments = allSitesDepartments.AsParallel<SiteDepartment>();
                delegateRoles = delegateRoles.AsParallel<DelegateRole>();

                //Fitler, join, and project
                var sitesDelegates = delegateRoles.Where(item => item.ManagedSiteID > 0);
                var sitesDepartmentsDelegates = delegateRoles.Where(item => item.ManagedSiteDepartmentID > 0);
                var userDelegates = delegateRoles.Where(item => false == string.IsNullOrEmpty(item.ManagedUserSipAccount));

                sitesDelegates =
                    (from role in sitesDelegates
                     where (role.ManagedSiteDepartmentID > 0 && (role.ManagedSiteDepartment != null && role.ManagedSiteDepartment.ID > 0))
                     join site in allSitesDepartments on role.ManagedSiteID equals site.Site.ID
                     select new DelegateRole
                     {
                         ID = role.ID,
                         DelegeeSipAccount = role.DelegeeSipAccount,
                         DelegationType = role.DelegationType,
                         ManagedUserSipAccount = role.ManagedUserSipAccount,
                         ManagedSiteID = role.ManagedSiteID,
                         ManagedSiteDepartmentID = role.ManagedSiteDepartmentID,
                         Description = role.Description,
                         //RELATIONS
                         ManagedUser = role.ManagedUser,
                         ManagedSiteDepartment = role.ManagedSiteDepartment,
                         ManagedSite = site.Site
                     })
                     .AsEnumerable<DelegateRole>();

                sitesDepartmentsDelegates =
                    (from role in delegateRoles
                     where (role.ManagedSiteDepartmentID > 0 && (role.ManagedSiteDepartment != null && role.ManagedSiteDepartment.ID > 0))
                     join siteDepartment in allSitesDepartments on role.ManagedSiteDepartmentID equals siteDepartment.ID
                     select new DelegateRole
                     {
                         ID = role.ID,
                         DelegeeSipAccount = role.DelegeeSipAccount,
                         DelegationType = role.DelegationType,
                         ManagedUserSipAccount = role.ManagedUserSipAccount,
                         ManagedSiteID = role.ManagedSiteID,
                         ManagedSiteDepartmentID = siteDepartment.ID,
                         Description = role.Description,
                         //RELATIONS
                         ManagedUser = role.ManagedUser,
                         ManagedSiteDepartment = siteDepartment,
                         ManagedSite = role.ManagedSite
                     })
                     .AsEnumerable<DelegateRole>();

                delegateRoles = userDelegates.Concat(sitesDelegates.Concat(sitesDepartmentsDelegates).ToList()).ToList();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a User's SipAccount, return all the authorized Users, Sites-Departments and Sites that this user is a delegate on.
        /// </summary>
        /// <param name="DelegeeSipAccount">The Delegee SipAccount</param>
        /// <returns>List of DelegateRole</returns>
        public List<DelegateRole> GetByDelegeeSipAccount(string DelegeeSipAccount)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("DelegeeSipAccount", DelegeeSipAccount);

            try
            {
                return this.Get(whereConditions: conditions, limit: 0).ToList<DelegateRole>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a sip account and a delegation type ID, this function will return all the data that this user is managing for this kind of delegation...
        /// Managed Users, Managed Sites, Managed Sites-Departments
        /// </summary>
        /// <param name="DelegeeSipAccount">The Delegee SipAccount</param>
        /// <param name="DelegationType">The Delegation TypeID</param>
        public List<DelegateRole> GetByDelegeeSipAccount(string DelegeeSipAccount, int DelegationType)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("DelegeeSipAccount", DelegeeSipAccount);
            conditions.Add("DelegationType", DelegationType);

            try
            {
                return this.Get(whereConditions: conditions, limit: 0).ToList<DelegateRole>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override DelegateRole GetById(long id, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            DelegateRole role = null;

            try
            {
                role = base.GetById(id, dataSourceName, dataSource);

                if(role != null)
                {
                    var temporaryList = new List<DelegateRole>() { role } as IEnumerable<DelegateRole>;
                    FillSiteDepartmentsData(ref temporaryList);
                    role = temporaryList.First();
                }

                return role;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<DelegateRole> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            IEnumerable<DelegateRole> roles = null;

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


        public override IEnumerable<DelegateRole> Get(Expression<Func<DelegateRole, bool>> predicate, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            IEnumerable<DelegateRole> roles = null;

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


        public override IEnumerable<DelegateRole> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            IEnumerable<DelegateRole> roles = null;

            try
            {
                roles = base.GetAll(dataSourceName, dataSource);

                if (roles != null && roles.Count() > 0)
                {
                    //this.FillSiteDepartmentsData(ref roles);
                    roles = roles.Include(
                        role => role.DelegeeAccount,
                        role => role.ManagedUser, 
                        role => role.ManagedSite, 
                        role => role.ManagedSiteDepartment);
                }

                return roles;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<DelegateRole> GetAll(string sql)
        {
            IEnumerable<DelegateRole> roles = null;

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
