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
        private static List<Department> _departments = new List<Department>();

        public DepartmentsDataMapper()
        {
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            if (_departments == null || _departments.Count == 0)
            {
                _departments = base.GetAll().ToList();
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
                return _departments.FirstOrDefault(item => item.Name == departmentName);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<Department> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _departments;
        }

        public override int Insert(Department dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _departments.Contains(dataObject);
            var itExists = _departments.Exists(item => item.Name == dataObject.Name);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);
            _departments.Add(dataObject);

            return dataObject.Id;
        }

        public override bool Update(Department dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var department = _departments.Find(item => item.Id == dataObject.Id);

            if (department != null)
            {
                _departments.Remove(department);
                _departments.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(Department dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var department = _departments.Find(item => item.Id == dataObject.Id);

            if (department != null)
            {
                _departments.Remove(department);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}