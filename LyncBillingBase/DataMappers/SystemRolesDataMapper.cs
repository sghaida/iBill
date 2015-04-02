using System;
using System.Collections.Generic;
using System.Linq;

using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class SystemRolesDataMapper : DataAccess<SystemRole>
    {
        private static SitesDataMapper _sitesDataMapper = new SitesDataMapper();
        private static RolesDataMapper _rolesDataMapper = new RolesDataMapper();

        private static List<SystemRole> _allSystemRoles = new List<SystemRole>();


        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public SystemRolesDataMapper()
        {
            LoadSystemRoles();
        }


        /// <summary>
        /// Initializes the _allSystemRoles list
        /// </summary>
        private void LoadSystemRoles()
        {
            if (_allSystemRoles == null || _allSystemRoles.Count == 0)
            {
                lock (_allSystemRoles)
                {
                    _allSystemRoles = (new List<SystemRole>()).GetWithRelations<SystemRole>(item => item.User, item => item.Site).ToList<SystemRole>() ?? (new List<SystemRole>());
                }
            }
        }


        /// <summary>
        ///     Given a User SipAccount, return the list of System Roles.
        /// </summary>
        /// <param name="sipAccount"></param>
        /// <returns>List of SystemRole objects</returns>
        public List<SystemRole> GetBySipAccount(string sipAccount)
        {
            try
            {
                return _allSystemRoles.Where(roles => roles.SipAccount == sipAccount).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        ///     Given a User SipAccount and Role's ID, return whether this user has this role (true or false).
        /// </summary>
        /// <param name="roleId">Role.ID (int)</param>
        /// <param name="sipAccount">User.SipAccount (string)</param>
        /// <returns>boolean</returns>
        public bool ValidateRoleForUser(int roleId, string sipAccount)
        {
            try
            {
                return _allSystemRoles.Any(role => role.RoleId == roleId && role.SipAccount == sipAccount);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        ///     Given a User SipAccount and Role object, return whether this user has this role (true or false).
        /// </summary>
        /// <param name="userRole">Role (object)</param>
        /// <param name="sipAccount">User.SipAccount (string)</param>
        /// <returns>boolean</returns>
        public bool ValidateRoleForUser(Role userRole, string sipAccount)
        {
            try
            {
                if (userRole != null)
                    return ValidateRoleForUser(userRole.RoleId, sipAccount);
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a list of user roles for a specific user, his SipAccount and his granted user-roles, return a list of the AllSites on which he was granted elevated access.
        /// </summary>
        /// <param name="userRoles">A list of the user's roles, taken from the session.</param>
        /// <param name="sipAccount">This is the user's SipAccount, taken from the session.</param>
        /// <param name="allowedRoleName">This is parameter of the type Backend.Enum.ValidRoles.</param>
        /// <returns>The list of AllSites on which the user was granted an elevated-access, such as: SiteAdmin, SiteAccountant. Developer is a universal access-role.</returns>
        public static List<Site> GetSitesByRoles(List<SystemRole> userRoles, string allowedRoleName)
        {
            List<Site> sites = new List<Site>();
            List<int> tmpUserSites = new List<int>();

            string developerRoleName = _rolesDataMapper.GetRoleNameById(_rolesDataMapper.DeveloperRoleID);
            string systemAdminRoleName = _rolesDataMapper.GetRoleNameById(_rolesDataMapper.SystemAdminRoleID);
            string siteAdminRoleName = _rolesDataMapper.GetRoleNameById(_rolesDataMapper.SiteAdminRoleID);
            string siteAccountantRoleName = _rolesDataMapper.GetRoleNameById(_rolesDataMapper.SiteAccountantRoleID);

            SystemRole developerRole = userRoles.Find(role => role.RoleId == _rolesDataMapper.DeveloperRoleID);

            if (developerRole != null || allowedRoleName == systemAdminRoleName)
            {
                return _sitesDataMapper.GetAll().ToList<Site>();
            }
            else if (allowedRoleName == siteAdminRoleName)
            {
                sites = userRoles
                    .Where(role => role.RoleId == _rolesDataMapper.SiteAdminRoleID)
                    .Select(role => role.Site)
                    .ToList<Site>();
            }
            else if (allowedRoleName == siteAccountantRoleName)
            {
                sites = userRoles
                    .Where(role => role.RoleId == _rolesDataMapper.SiteAccountantRoleID)
                    .Select(role => role.Site)
                    .ToList<Site>();
            }

            return sites.OrderBy(item => item.Name).ToList();
        }


        public override SystemRole GetById(long id, string dataSourceName = null, CCC.ORM.Globals.DataSource.Type dataSourceType = CCC.ORM.Globals.DataSource.Type.Default)
        {
            return _allSystemRoles.Find(role => role.Id == id);
        }


        public override IEnumerable<SystemRole> GetAll(string dataSourceName = null, CCC.ORM.Globals.DataSource.Type dataSourceType = CCC.ORM.Globals.DataSource.Type.Default)
        {
            return _allSystemRoles;
        }


        public override int Insert(SystemRole dataObject, string dataSourceName = null, CCC.ORM.Globals.DataSource.Type dataSourceType = CCC.ORM.Globals.DataSource.Type.Default)
        {
            //
            // Null check
            if (dataObject == null)
                return -1;

            //
            // Containment and existence checks
            var isContained = _allSystemRoles.Contains(dataObject);

            var itExists = _allSystemRoles.Exists(
                item =>
                    item.RoleId == dataObject.RoleId && 
                    item.SiteId == dataObject.SiteId && 
                    item.SipAccount == dataObject.SipAccount);

            if (isContained || itExists)
            {
                return -1;
            }

            //
            // Inserting process
            try
            {
                dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);

                lock (_allSystemRoles)
                {
                    _allSystemRoles.Add(dataObject);
                }
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }

            return dataObject.Id;
        }


        public override bool Update(SystemRole dataObject, string dataSourceName = null, CCC.ORM.Globals.DataSource.Type dataSourceType = CCC.ORM.Globals.DataSource.Type.Default)
        {
            bool status = false;

            //
            // Null Check
            if (dataObject == null)
                return status;

            //
            // Existence check
            var role = _allSystemRoles.Find(item => item.Id == dataObject.Id);

            //
            // In case the dataObject's Id was modified!
            if(role == null)
            {
                role = _allSystemRoles.Find(item => item.RoleId == dataObject.RoleId && item.SipAccount == dataObject.SipAccount && item.SiteId == dataObject.SiteId);
            }

            //
            // Only perform update if the role object is not null
            if (role != null)
            {
                try
                {
                    status = base.Update(dataObject, dataSourceName, dataSourceType);

                    if (status == true)
                    {
                        lock(_allSystemRoles)
                        { 
                        _allSystemRoles.Remove(role);
                        _allSystemRoles.Add(dataObject);
                            }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            return status;
        }


        public override bool Delete(SystemRole dataObject, string dataSourceName = null, CCC.ORM.Globals.DataSource.Type dataSourceType = CCC.ORM.Globals.DataSource.Type.Default)
        {
            bool status = false;

            if (dataObject == null)
                return status;

            //
            // Existence check
            var role = _allSystemRoles.Find(item => item.Id == dataObject.Id);

            //
            // In case the dataObject's Id was modified!
            if(role == null)
            {
                role = _allSystemRoles.Find(item => item.RoleId == dataObject.RoleId && item.SipAccount == dataObject.SipAccount && item.SiteId == dataObject.SiteId);
            }

            //
            // Only perform update if the role object is not null
            if (role != null)
            {
                try
                {
                    status = base.Delete(dataObject, dataSourceName, dataSourceType);

                    if (status == true)
                    {
                        lock (_allSystemRoles)
                        {
                            _allSystemRoles.Remove(role);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            return status;
        }


        /***
         * DISABLED FUNCTIONS
         */
        [Obsolete]
        public override IEnumerable<SystemRole> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, CCC.ORM.Globals.DataSource.Type dataSourceType = CCC.ORM.Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override IEnumerable<SystemRole> Get(System.Linq.Expressions.Expression<Func<SystemRole, bool>> predicate, string dataSourceName = null, CCC.ORM.Globals.DataSource.Type dataSourceType = CCC.ORM.Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override IEnumerable<SystemRole> GetAll(string sqlQuery)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override int Insert(string sql)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override bool Update(string sql)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override bool Delete(string sql)
        {
            throw new NotImplementedException();
        }

    }

}