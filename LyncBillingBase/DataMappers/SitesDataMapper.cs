using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class SitesDataMapper : DataAccess<Site>
    {
        private static List<Site> _sites = new List<Site>();

        public SitesDataMapper()
        {
            LoadSites();
        }

        public void LoadSites()
        {
            if (_sites == null || _sites.Count == 0)
            {
                _sites = base.GetAll().ToList();
            }
        }

        public override IEnumerable<Site> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _sites;
        }

        public override int Insert(Site dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _sites.Contains(dataObject);
            var itExists =
                _sites.Exists(item => item.Name == dataObject.Name && item.CountryCode == dataObject.CountryCode);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);
            _sites.Add(dataObject);

            return dataObject.Id;
        }

        public override bool Update(Site dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var site = _sites.Find(item => item.Id == dataObject.Id);

            if (site != null)
            {
                _sites.Remove(site);
                _sites.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(Site dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var site = _sites.Find(item => item.Id == dataObject.Id);

            if (site != null)
            {
                _sites.Remove(site);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}