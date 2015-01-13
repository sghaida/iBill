using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class CurrenciesDataMapper : DataAccess<Currency>
    {
        private static List<Currency> _currencies = new List<Currency>();

        public CurrenciesDataMapper()
        {
            LoadCurrencies();
        }

        private void LoadCurrencies()
        {
            if (_currencies == null || _currencies.Count == 0)
            {
                _currencies = base.GetAll().ToList();
            }
        }

        public Currency GetByIso3Code(string iso3Code)
        {
            Currency currency = null;

            var condition = new Dictionary<string, object>();
            condition.Add("ISO3Code", iso3Code);

            try
            {
                var results = Get(condition, 1).ToList();

                if (results != null && results.Count > 0)
                {
                    currency = results.First();
                }

                return currency;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<Currency> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _currencies;
        }

        public override int Insert(Currency dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _currencies.Contains(dataObject);
            var itExists =
                _currencies.Exists(item => item.Name == dataObject.Name || item.Iso3Code == dataObject.Iso3Code);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);
            _currencies.Add(dataObject);

            return dataObject.Id;
        }

        public override bool Update(Currency dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var currency = _currencies.Find(item => item.Id == dataObject.Id);

            if (currency != null)
            {
                _currencies.Remove(currency);
                _currencies.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(Currency dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var currency = _currencies.Find(item => item.Id == dataObject.Id);

            if (currency != null)
            {
                _currencies.Remove(currency);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}