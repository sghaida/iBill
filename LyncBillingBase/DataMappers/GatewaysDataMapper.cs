using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class GatewaysDataMapper : DataAccess<Gateway>
    {
        private static List<Gateway> _gateways = new List<Gateway>();

        public GatewaysDataMapper()
        {
            LoadGateways();
        }

        private void LoadGateways()
        {
            if (_gateways == null || _gateways.Count == 0)
            {
                _gateways = base.GetAll().ToList();
            }
        }

        public override IEnumerable<Gateway> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _gateways;
        }

        public override int Insert(Gateway dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _gateways.Contains(dataObject);
            var itExists = _gateways.Exists(item => item.Name == dataObject.Name);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);
            _gateways.Add(dataObject);

            return dataObject.Id;
        }

        public override bool Update(Gateway dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var gateway = _gateways.Find(item => item.Id == dataObject.Id);

            if (gateway != null)
            {
                _gateways.Remove(gateway);
                _gateways.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(Gateway dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var gateway = _gateways.Find(item => item.Id == dataObject.Id);

            if (gateway != null)
            {
                _gateways.Remove(gateway);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}