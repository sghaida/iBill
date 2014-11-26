using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class DepartmentsDataMapper : DataAccess<Department>
    {
        //Identity Map
        private List<IdentityMapItem<Department>> IdentityMap = new List<IdentityMapItem<Department>>();

        //Identity Map Updater functions
        private void UpdateIdentityMapItem(Department department)
        {
            var now = DateTime.Now;
            var cachedVersion = IdentityMap.Find(item => item.DataObject.ID == department.ID);

            if (cachedVersion != null)
            {
                int index = IdentityMap.IndexOf(cachedVersion);

                lock (IdentityMap[index].MutexLock)
                {
                    IdentityMap[index].DataObject.Name = department.Name;
                    IdentityMap[index].DataObject.Description = department.Description;

                    IdentityMap[index].Updated = true;
                    IdentityMap[index].DBSynced = false;
                }
            }
        }

        private void UpdateIdentityMap(ref List<Department> sites)
        {
            var now = DateTime.Now;

            IdentityMap = sites
                .Select(item => new IdentityMapItem<Department>
                {
                    DataObject = item,
                    AddedOn = now,
                    Updated = false,
                    DBSynced = true
                })
                .ToList<IdentityMapItem<Department>>();
        }


        //Get all sites
        public IEnumerable<Department> GetAll()
        {
            List<Department> departments = new List<Department>();

            if (IdentityMap.Count > 0)
            {
                departments = IdentityMap.Select(item => item.DataObject).ToList<Department>();
            }
            else
            {
                departments = base.GetAll().ToList<Department>();
                UpdateIdentityMap(ref departments);
            }

            return departments;
        }

        //Get a site by it's ID
        public Department GetById(long id)
        {
            Department department;

            if (IdentityMap.Count > 0)
            {
                var cachedSite = IdentityMap.Find(item => item.DataObject.ID == id);

                if (cachedSite != null)
                {
                    department = cachedSite.DataObject;
                }
                else
                {
                    department = base.GetById(id);
                }
            }
            else
            {
                department = base.GetById(id);
            }

            return department;
        }

        //Get a site that matches a specific set of conditions.
        public IEnumerable<Department> Get(Dictionary<string, object> whereCondition, int limit = 25)
        {
            return base.Get(whereCondition, limit);
        }

        //Get a site that matches a specific search predicate
        public IEnumerable<Department> Get(Expression<Func<Department, bool>> predicate)
        {
            return base.Get(predicate);
        }

    }
}
