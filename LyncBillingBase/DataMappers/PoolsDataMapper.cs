using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class PoolsDataMapper : DataAccess<Pool>
    {
        private static List<Pool> _Pools = new List<Pool>();

        public PoolsDataMapper()
        {
            LoadPools();
        }

        private void LoadPools()
        {
            if (_Pools == null || _Pools.Count == 0)
            {
                _Pools = base.GetAll().ToList();
            }
        }

        public override IEnumerable<Pool> GetAll(string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _Pools;
        }

        public override int Insert(Pool dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var isContained = _Pools.Contains(dataObject);
            var itExists = _Pools.Exists(item => item.FQDN == dataObject.FQDN);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
            _Pools.Add(dataObject);

            return dataObject.ID;
        }

        public override bool Update(Pool dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var pool = _Pools.Find(item => item.ID == dataObject.ID);

            if (pool != null)
            {
                _Pools.Remove(pool);
                _Pools.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(Pool dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var pool = _Pools.Find(item => item.ID == dataObject.ID);

            if (pool != null)
            {
                _Pools.Remove(pool);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}