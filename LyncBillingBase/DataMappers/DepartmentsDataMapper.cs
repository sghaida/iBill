using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class DepartmentsDataMapper : DataAccess<Department>
    {
        private static List<Department> _Departments = new List<Department>();

        public DepartmentsDataMapper()
        {
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            if (_Departments == null || _Departments.Count == 0)
            {
                _Departments = base.GetAll().ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        public Department GetByName(string departmentName)
        {
            try
            {
                return _Departments.FirstOrDefault(item => item.Name == departmentName);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<Department> GetAll(string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _Departments;
        }

        public override int Insert(Department dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var isContained = _Departments.Contains(dataObject);
            var itExists = _Departments.Exists(item => item.Name == dataObject.Name);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
            _Departments.Add(dataObject);

            return dataObject.ID;
        }

        public override bool Update(Department dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var department = _Departments.Find(item => item.ID == dataObject.ID);

            if (department != null)
            {
                _Departments.Remove(department);
                _Departments.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(Department dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var department = _Departments.Find(item => item.ID == dataObject.ID);

            if (department != null)
            {
                _Departments.Remove(department);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}