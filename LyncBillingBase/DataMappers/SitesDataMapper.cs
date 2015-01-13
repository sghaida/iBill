using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;





using CCC.ORM;
using CCC.ORM.DataAccess;

using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class SitesDataMapper : DataAccess<Site>
    {
        private static List<Site> _Sites = new List<Site>();


        public SitesDataMapper()
        {
            LoadSites();
        }


        public void LoadSites()
        {
            if(_Sites == null || _Sites.Count == 0)
            {
                _Sites = base.GetAll().ToList();
            }
        }


        public override IEnumerable<Site> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _Sites;
        }


        public override int Insert(Site dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            bool isContained = _Sites.Contains(dataObject);
            bool itExists = _Sites.Exists(item => item.Name == dataObject.Name && item.CountryCode == dataObject.CountryCode);

            if (isContained || itExists)
            {
                return -1;
            }
            else
            {
                dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
                _Sites.Add(dataObject);

                return dataObject.ID;
            }
        }


        public override bool Update(Site dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var site = _Sites.Find(item => item.ID == dataObject.ID);

            if (site != null)
            {
                _Sites.Remove(site);
                _Sites.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }


        public override bool Delete(Site dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var site = _Sites.Find(item => item.ID == dataObject.ID);

            if (site != null)
            {
                _Sites.Remove(site);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }

    }

}
