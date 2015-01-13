using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class DiDsDataMapper : DataAccess<Did>
    {
        private static List<Did> _diDs = new List<Did>();

        public DiDsDataMapper()
        {
            LoadDiDs();
        }

        private void LoadDiDs()
        {
            if (_diDs == null || _diDs.Count == 0)
            {
                _diDs = base.GetAll().ToList();
            }
        }

        public override IEnumerable<Did> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _diDs;
        }

        public override int Insert(Did dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _diDs.Contains(dataObject);
            var itExists =
                _diDs.Exists(
                    item =>
                        item.Regex == dataObject.Regex ||
                        (item.Regex == dataObject.Regex && item.SiteId == dataObject.SiteId));

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);
            _diDs.Add(dataObject);

            return dataObject.Id;
        }

        public override bool Update(Did dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var did = _diDs.Find(item => item.Id == dataObject.Id);

            if (did != null)
            {
                _diDs.Remove(did);
                _diDs.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(Did dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var did = _diDs.Find(item => item.Id == dataObject.Id);

            if (did != null)
            {
                _diDs.Remove(did);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}