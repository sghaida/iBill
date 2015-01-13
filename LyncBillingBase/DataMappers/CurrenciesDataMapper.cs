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
    public class CurrenciesDataMapper : DataAccess<Currency>
    {
        private static List<Currency> _Currencies = new List<Currency>();


        public CurrenciesDataMapper()
        {
            LoadCurrencies();
        }


        private void LoadCurrencies()
        {
            if(_Currencies == null || _Currencies.Count == 0)
            {
                _Currencies = base.GetAll().ToList();
            }
        }


        public Currency GetByISO3Code(string ISO3Code)
        {
            Currency currency = null;

            var condition = new Dictionary<string, object>();
            condition.Add("ISO3Code", ISO3Code);

            try
            {
                var results = base.Get(whereConditions: condition, limit: 1).ToList<Currency>();

                if(results != null && results.Count > 0)
                {
                    currency = results.First();
                }

                return currency;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<Currency> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _Currencies;
        }


        public override int Insert(Currency dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            bool isContained = _Currencies.Contains(dataObject);
            bool itExists = _Currencies.Exists(item => item.Name == dataObject.Name || item.ISO3Code == dataObject.ISO3Code);

            if(isContained || itExists)
            {
                return -1;
            }
            else
            {
                dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
                _Currencies.Add(dataObject);

                return dataObject.ID;
            }
        }


        public override bool Update(Currency dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var currency = _Currencies.Find(item => item.ID == dataObject.ID);

            if(currency != null)
            {
                _Currencies.Remove(currency);
                _Currencies.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }


        public override bool Delete(Currency dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var currency = _Currencies.Find(item => item.ID == dataObject.ID);

            if (currency != null)
            {
                _Currencies.Remove(currency);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }

    }

}
