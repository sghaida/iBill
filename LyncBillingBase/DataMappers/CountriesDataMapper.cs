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
        /// <summary>
        /// Given an ISO2 Country Code, return the Country object.
        /// </summary>
        /// <param name="ISO2Code">ISO2 Code, such as: GR, US, UK, JO.</param>
        /// <returns>Country object.</returns>
        public Country GetByISO2Code(string ISO2Code)
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


        /// <summary>
        /// Given an ISO3 Country Code, return the Country object.
        /// </summary>
        /// <param name="ISO3Code">ISO3 Code, such as: GRC, USA, GBR, JOR, ARE.</param>
        /// <returns>Country object.</returns>
        public Country GetByISO3Code(string ISO3Code)
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


        /// <summary>
        /// Given a Country ID, return the Country's Currency object.
        /// </summary>
        /// <param name="CountryID">CountryID (int).</param>
        /// <returns>Currency object.</returns>
        public Currency GetCurrency(int CountryID)
        {
            Country country = null;
            Currency currency = null;

            try
            {
                country = GetById(CountryID);

                if (country != null && country.Currency != null)
                    currency = country.Currency;

                return currency;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }

}
