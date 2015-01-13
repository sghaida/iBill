using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class PoolsDataMapper : DataAccess<Pool>
    {
        private static List<Pool> _pools = new List<Pool>();

        public PoolsDataMapper()
        {
            LoadPools();
        }

        private void LoadPools()
        {
            if (_pools == null || _pools.Count == 0)
            {
                _pools = base.GetAll().ToList();
            }
        }

        public override IEnumerable<Pool> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _pools;
        }

        public override int Insert(Pool dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _pools.Contains(dataObject);
            var itExists = _pools.Exists(item => item.Fqdn == dataObject.Fqdn);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);
            _pools.Add(dataObject);

            return dataObject.Id;
        }

        public override bool Update(Pool dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var pool = _pools.Find(item => item.Id == dataObject.Id);

            if (pool != null)
            {
                _pools.Remove(pool);
                _pools.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(Pool dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var pool = _pools.Find(item => item.Id == dataObject.Id);

            if (pool != null)
            {
                _pools.Remove(pool);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}