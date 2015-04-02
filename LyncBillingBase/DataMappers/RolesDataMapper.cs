using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class RolesDataMapper : DataAccess<Role>
    {
        private static List<Role> _roles = new List<Role>();
        
        public int DeveloperRoleID { get; set; }
        public int SystemAdminRoleID { get; set; }
        public int SiteAdminRoleID { get; set; }
        public int SiteAccountantRoleID { get; set; }
        public int DepartmentHeadRoleID { get; set; }
        public int SiteDelegeeRoleID { get; set; }
        public int DepartmentDelegeeRoleID { get; set; }
        public int UserDelegeeRoleID { get; set; }
        public int UserRoleID { get; set; }

        private void LoadRoles()
        {
            if (_roles == null || _roles.Count == 0)
            {
                lock (_roles)
                {
                    _roles = base.GetAll().ToList();

                    // Developer Role
                    var developer = _roles.Find(item => item.RoleName.ToLower() == "developer");
                    DeveloperRoleID = (developer != null ? developer.RoleId : -1);

                    // System Admin Role
                    var systemAdmin = _roles.Find(item => item.RoleName.ToLower() == "sysadmin");
                    SystemAdminRoleID = (systemAdmin != null ? systemAdmin.RoleId : -1);

                    // Site Admin Role
                    var siteAdmin = _roles.Find(item => item.RoleName.ToLower() == "admin");
                    SiteAdminRoleID = (siteAdmin != null ? siteAdmin.RoleId : -1);

                    // Site Accountant Role
                    var siteAccountant = _roles.Find(item => item.RoleName.ToLower() == "accounting");
                    SiteAccountantRoleID = (siteAccountant != null ? siteAccountant.RoleId : -1);

                    // Department Head Role
                    var departmentHead = _roles.Find(item => item.RoleName.ToLower() == "dephead");
                    DepartmentHeadRoleID = (departmentHead != null ? departmentHead.RoleId : -1);

                    // Site Delegee Role
                    var siteDelegee = _roles.Find(item => item.RoleName.ToLower() == "sitedelegee");
                    SiteDelegeeRoleID = (siteDelegee != null ? siteDelegee.RoleId : -1);

                    // Department Delegee Role
                    var departmentDelegee = _roles.Find(item => item.RoleName.ToLower() == "depdelegee");
                    DepartmentDelegeeRoleID = (departmentDelegee != null ? departmentDelegee.RoleId : -1);

                    // User Delegee Role
                    var userDelegee = _roles.Find(item => item.RoleName.ToLower() == "userdelegee");
                    UserDelegeeRoleID = (userDelegee != null ? userDelegee.RoleId : -1);

                    // Normal User Role
                    var user = _roles.Find(item => item.RoleName.ToLower() == "user");
                    UserRoleID = (user != null ? user.RoleId : -1);

                }//end-lock
            }//end-if
        }


        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public RolesDataMapper()
        {
            LoadRoles();
        }


        /// <summary>
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Role GetByRoleId(int roleId)
        {
            try
            {
                return _roles.Find(item => item.RoleId == roleId);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetRoleNameById(int roleId)
        {
            string roleName = null;

            try
            {
                var role = _roles.Find(item => item.RoleId == roleId);

                if(role != null)
                {
                    roleName = role.RoleName;
                }

                return roleName;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<Role> GetAll(string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _roles;
        }


        public override int Insert(Role dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            //
            // Null Checks
            if(dataObject == null)
            {
                return -1;
            }

            //
            // Containment and existence checks
            var isContained = _roles.Contains(dataObject);
            var itExists = _roles.Exists(item => item.RoleId == dataObject.RoleId && item.RoleName == dataObject.RoleName);

            if (isContained || itExists)
            {
                return -1;
            }

            //
            // Insert process
            try
            {
                dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);
                
                lock(_roles)
                {
                    _roles.Add(dataObject);
                }
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }

            return dataObject.Id;
        }


        public override bool Update(Role dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            bool status = false;

            //
            // Null check
            if(dataObject == null)
            {
                return status;
            }

            //
            // Existence check
            var role = _roles.Find(item => item.Id == dataObject.Id);

            if (role != null)
            {
                try
                {
                    status = base.Update(dataObject, dataSourceName, dataSourceType);

                    if (status == true)
                    {
                        lock (_roles)
                        {
                            _roles.Remove(role);
                            _roles.Add(dataObject);
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            return status;
        }


        public override bool Delete(Role dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            bool status = false;

            //
            // Null check
            if(dataObject == null)
            {
                return status;
            }

            //
            // Existence check
            var role = _roles.Find(item => item.Id == dataObject.Id);

            if (role != null)
            {
                try
                {
                    status = base.Delete(dataObject, dataSourceName, dataSourceType);

                    if(status == true)
                    {
                        lock (_roles)
                        {
                            _roles.Remove(role);
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw ex.InnerException;
                }
            }
            return false;
        }


        /***
         * DISABLED FUNCTIONS
         */
        [Obsolete]
        public override IEnumerable<Role> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, CCC.ORM.Globals.DataSource.Type dataSourceType = CCC.ORM.Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override IEnumerable<Role> Get(System.Linq.Expressions.Expression<Func<Role, bool>> predicate, string dataSourceName = null, CCC.ORM.Globals.DataSource.Type dataSourceType = CCC.ORM.Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override IEnumerable<Role> GetAll(string sqlQuery)
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