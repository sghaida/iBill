using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class CountriesDataMapper : DataAccess<Country>
    {
        private static List<Country> _Countries = new List<Country>();

        public CountriesDataMapper()
        {
            LoadCountries();
        }

        private void LoadCountries()
        {
            if (_Countries == null || _Countries.Count == 0)
            {
                _Countries = _Countries.GetWithRelations(item => item.Currency).ToList();
            }
        }

        /// <summary>
        ///     Given an ISO2 Country Code, return the Country object.
        /// </summary>
        /// <param name="ISO2Code">ISO2 Code, such as: GR, US, UK, JO.</param>
        /// <returns>Country object.</returns>
        public Country GetByISO2Code(string ISO2Code)
        {
            Country country = null;

            var condition = new Dictionary<string, object>();
            condition.Add("ISO2Code", ISO2Code);

            try
            {
                var results = Get(condition, 1).ToList();

                if (results != null && results.Count() > 0)
                {
                    country = results.First();
                }

                return country;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given an ISO3 Country Code, return the Country object.
        /// </summary>
        /// <param name="ISO3Code">ISO3 Code, such as: GRC, USA, GBR, JOR, ARE.</param>
        /// <returns>Country object.</returns>
        public Country GetByISO3Code(string ISO3Code)
        {
            return _Countries.FirstOrDefault(item => item.ISO3Code == ISO3Code);
        }

        public override IEnumerable<Country> GetAll(string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _Countries;
        }

        public override int Insert(Country dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var isContained = _Countries.Contains(dataObject);
            var itExists =
                _Countries.Exists(
                    item =>
                        item.ISO3Code == dataObject.ISO3Code || item.ISO2Code == dataObject.ISO2Code ||
                        item.Name == dataObject.Name);


            if (isContained || itExists)
            {
                return -1;
            }
            var rowID = base.Insert(dataObject, dataSourceName, dataSourceType);

            if (rowID > 0)
            {
                dataObject.ID = rowID;
                dataObject = dataObject.GetWithRelations(item => item.Currency);
                _Countries.Add(dataObject);
            }

            return rowID;
        }

        public override bool Update(Country dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var country = _Countries.Find(item => item.ID == dataObject.ID);

            if (country != null)
            {
                var status = base.Update(dataObject, dataSourceName, dataSourceType);

                if (status)
                {
                    _Countries.Remove(country);

                    dataObject = dataObject.GetWithRelations(item => item.Currency);
                    _Countries.Add(dataObject);
                }

                return status;
            }
            return false;
        }

        public override bool Delete(Country dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var country = _Countries.Find(item => item.ID == dataObject.ID);

            if (country != null)
            {
                _Countries.Remove(country);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}