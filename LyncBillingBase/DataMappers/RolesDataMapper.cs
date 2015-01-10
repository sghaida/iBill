using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using DALDotNet;
using DALDotNet.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class RolesDataMapper : DataAccess<Role>
    {
        private static List<Role> _Roles = new List<Role>();


        public RolesDataMapper()
        {
            LoadRoles();
        }


        private void LoadRoles()
        {
            if(_Roles == null || _Roles.Count == 0)
            {
                _Roles = base.GetAll().ToList();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public Role GetByRoleID(int RoleID)
        {
            try
            {
                return _Roles.FirstOrDefault(item => item.RoleID == RoleID);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<Role> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _Roles;
        }


        public override int Insert(Role dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            bool isContained = _Roles.Contains(dataObject);
            bool itExists = _Roles.Exists(item => item.RoleID == dataObject.RoleID && item.RoleName == dataObject.RoleName);

            if (isContained || itExists)
            {
                return -1;
            }
            else
            {
                dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
                _Roles.Add(dataObject);

                return dataObject.ID;
            }
        }


        public override bool Update(Role dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var role = _Roles.Find(item => item.ID == dataObject.ID);

            if (role != null)
            {
                _Roles.Remove(role);
                _Roles.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }


        public override bool Delete(Role dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var role = _Roles.Find(item => item.ID == dataObject.ID);

            if (role != null)
            {
                _Roles.Remove(role);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }

    }

}
