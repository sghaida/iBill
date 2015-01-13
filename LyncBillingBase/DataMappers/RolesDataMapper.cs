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

        public RolesDataMapper()
        {
            LoadRoles();
        }

        private void LoadRoles()
        {
            if (_roles == null || _roles.Count == 0)
            {
                _roles = base.GetAll().ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Role GetByRoleId(int roleId)
        {
            try
            {
                return _roles.FirstOrDefault(item => item.RoleId == roleId);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<Role> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _roles;
        }

        public override int Insert(Role dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _roles.Contains(dataObject);
            var itExists =
                _roles.Exists(item => item.RoleId == dataObject.RoleId && item.RoleName == dataObject.RoleName);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);
            _roles.Add(dataObject);

            return dataObject.Id;
        }

        public override bool Update(Role dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var role = _roles.Find(item => item.Id == dataObject.Id);

            if (role != null)
            {
                _roles.Remove(role);
                _roles.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(Role dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var role = _roles.Find(item => item.Id == dataObject.Id);

            if (role != null)
            {
                _roles.Remove(role);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}