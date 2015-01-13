using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class GatewaysDataMapper : DataAccess<Gateway>
    {
        private static List<Gateway> _Gateways = new List<Gateway>();

        public GatewaysDataMapper()
        {
            LoadGateways();
        }

        private void LoadGateways()
        {
            if (_Gateways == null || _Gateways.Count == 0)
            {
                _Gateways = base.GetAll().ToList();
            }
        }

        public override IEnumerable<Gateway> GetAll(string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _Gateways;
        }

        public override int Insert(Gateway dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var isContained = _Gateways.Contains(dataObject);
            var itExists = _Gateways.Exists(item => item.Name == dataObject.Name);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
            _Gateways.Add(dataObject);

            return dataObject.ID;
        }

        public override bool Update(Gateway dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var gateway = _Gateways.Find(item => item.ID == dataObject.ID);

            if (gateway != null)
            {
                _Gateways.Remove(gateway);
                _Gateways.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(Gateway dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var gateway = _Gateways.Find(item => item.ID == dataObject.ID);

            if (gateway != null)
            {
                _Gateways.Remove(gateway);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}