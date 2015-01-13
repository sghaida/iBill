using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class DIDsDataMapper : DataAccess<DID>
    {
        private static List<DID> _DIDs = new List<DID>();

        public DIDsDataMapper()
        {
            LoadDIDs();
        }

        private void LoadDIDs()
        {
            if (_DIDs == null || _DIDs.Count == 0)
            {
                _DIDs = base.GetAll().ToList();
            }
        }

        public override IEnumerable<DID> GetAll(string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _DIDs;
        }

        public override int Insert(DID dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var isContained = _DIDs.Contains(dataObject);
            var itExists =
                _DIDs.Exists(
                    item =>
                        item.Regex == dataObject.Regex ||
                        (item.Regex == dataObject.Regex && item.SiteID == dataObject.SiteID));

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
            _DIDs.Add(dataObject);

            return dataObject.ID;
        }

        public override bool Update(DID dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var did = _DIDs.Find(item => item.ID == dataObject.ID);

            if (did != null)
            {
                _DIDs.Remove(did);
                _DIDs.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(DID dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var did = _DIDs.Find(item => item.ID == dataObject.ID);

            if (did != null)
            {
                _DIDs.Remove(did);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}