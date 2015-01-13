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
        private static List<Country> _countries = new List<Country>();

        public CountriesDataMapper()
        {
            LoadCountries();
        }

        private void LoadCountries()
        {
            if (_countries == null || _countries.Count == 0)
            {
                _countries = _countries.GetWithRelations(item => item.Currency).ToList();
            }
        }

        /// <summary>
        ///     Given an ISO2 Country Code, return the Country object.
        /// </summary>
        /// <param name="iso2Code">ISO2 Code, such as: GR, US, UK, JO.</param>
        /// <returns>Country object.</returns>
        public Country GetByIso2Code(string iso2Code)
        {
            Country country = null;

            var condition = new Dictionary<string, object>();
            condition.Add("ISO2Code", iso2Code);

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
        /// <param name="iso3Code">ISO3 Code, such as: GRC, USA, GBR, JOR, ARE.</param>
        /// <returns>Country object.</returns>
        public Country GetByIso3Code(string iso3Code)
        {
            return _countries.FirstOrDefault(item => item.Iso3Code == iso3Code);
        }

        public override IEnumerable<Country> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _countries;
        }

        public override int Insert(Country dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _countries.Contains(dataObject);
            var itExists =
                _countries.Exists(
                    item =>
                        item.Iso3Code == dataObject.Iso3Code || item.Iso2Code == dataObject.Iso2Code ||
                        item.Name == dataObject.Name);


            if (isContained || itExists)
            {
                return -1;
            }
            var rowId = base.Insert(dataObject, dataSourceName, dataSourceType);

            if (rowId > 0)
            {
                dataObject.Id = rowId;
                dataObject = dataObject.GetWithRelations(item => item.Currency);
                _countries.Add(dataObject);
            }

            return rowId;
        }

        public override bool Update(Country dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var country = _countries.Find(item => item.Id == dataObject.Id);

            if (country != null)
            {
                var status = base.Update(dataObject, dataSourceName, dataSourceType);

                if (status)
                {
                    _countries.Remove(country);

                    dataObject = dataObject.GetWithRelations(item => item.Currency);
                    _countries.Add(dataObject);
                }

                return status;
            }
            return false;
        }

        public override bool Delete(Country dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var country = _countries.Find(item => item.Id == dataObject.Id);

            if (country != null)
            {
                _countries.Remove(country);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}