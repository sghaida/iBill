using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class CurrenciesDataMapper : DataAccess<Currency>
    {
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

    }

}
