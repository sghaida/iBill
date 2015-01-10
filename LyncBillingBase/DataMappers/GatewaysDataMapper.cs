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
    public class GatewaysDataMapper : DataAccess<Gateway>
    {
        private static List<Gateway> _Gateways = new List<Gateway>();

        private void LoadGateways()
        {
            if(_Gateways == null || _Gateways.Count == 0)
            {
                _Gateways = base.GetAll().ToList();
            }
        }


        public GatewaysDataMapper()
        {
            LoadGateways();
        }


        public override IEnumerable<Gateway> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _Gateways;
        }


        public override int Insert(Gateway dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            bool isContained = _Gateways.Contains(dataObject);
            bool itExists = _Gateways.Exists(item => item.Name == dataObject.Name);

            if (isContained || itExists)
            {
                return -1;
            }
            else
            {
                dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
                _Gateways.Add(dataObject);

                return dataObject.ID;
            }
        }


        public override bool Update(Gateway dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var gateway = _Gateways.Find(item => item.ID == dataObject.ID);

            if (gateway != null)
            {
                _Gateways.Remove(gateway);
                _Gateways.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }


        public override bool Delete(Gateway dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var gateway = _Gateways.Find(item => item.ID == dataObject.ID);

            if (gateway != null)
            {
                _Gateways.Remove(gateway);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }

    }

}
