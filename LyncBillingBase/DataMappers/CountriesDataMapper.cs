using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataModels;
using LyncBillingBase.DataAccess;

namespace LyncBillingBase.DataMappers
{
    public class CountriesDataMapper : DataAccess<Country>
    {
        //Get country by its' ISO 2 Code
        public Country GetCountryByISO2Code(string ISO2Code)
        {
            try
            {
                var condition = new Dictionary<string, object>();
                condition.Add("ISO2Code", ISO2Code);

                var results = Get(whereConditions: condition, limit: 1).ToList<Country>();

                if(results != null && results.Count() > 0)
                {
                    return results.First();
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        //Get country by its' ISO 3 Code
        public Country GetCountryByISO3Code(string ISO3Code)
        {
            try
            {
                var condition = new Dictionary<string, object>();
                condition.Add("ISO3Code", ISO3Code);

                var results = Get(whereConditions: condition, limit: 1).ToList<Country>();

                if (results != null && results.Count() > 0)
                {
                    return results.First();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
