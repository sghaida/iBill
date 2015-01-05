using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class NumberingPlansDataMapper : DataAccess<NumberingPlan>
    {
        /**
         * This instance of the Countries DataMapper is used for data reading only.
         */
        private CountriesDataMapper _countriesDataMapper = new CountriesDataMapper();

        /// <summary>
        /// Given a list of Numbering Plan objects, fill their Countries objects with the Country's Data Relations.
        /// We are doing this here, because there is no feature for executing nested data relations.
        /// We have to fill the data relations inside the local Countries objects ourselves.
        /// </summary>
        /// <param name="numberingPlan">A list of Numbering Plan objects</param>
        private void FillCountriesAndCurrenciesData(ref IEnumerable<NumberingPlan> numberingPlan)
        {
            try
            {
                IEnumerable<Country> allCountries = _countriesDataMapper.GetAll();

                // Enable parallelization on the enumerable collection
                //allCountries = allCountries.AsParallel<Country>();
                //numberingPlan = numberingPlan.AsParallel<NumberingPlan>();

                numberingPlan = 
                    (from dialingRecord in numberingPlan
                     where (dialingRecord.Country != null && dialingRecord.Country.ID > 0)
                     join countryObject in allCountries on dialingRecord.Country.ID equals countryObject.ID
                     select new NumberingPlan
                     {
                         DialingPrefix = dialingRecord.DialingPrefix,
                         ISO2CountryCode = dialingRecord.ISO2CountryCode,
                         ISO3CountryCode = dialingRecord.ISO3CountryCode,
                         CountryName = dialingRecord.CountryName,
                         City = dialingRecord.City,
                         Provider = dialingRecord.Provider,
                         TypeOfService = dialingRecord.TypeOfService,
                         //RELATIONS
                         Country = countryObject
                     });
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Dialing Prefix, return all the Numbering Plan records associated with it.
        /// </summary>
        /// <param name="DialingPrefix">NumberingPlan.DialingPrefix (Int64)</param>
        /// <returns>A list of NumberingPlan objects</returns>
        public List<NumberingPlan> GetByPrefix(Int64 DialingPrefix)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("DialingPrefix", DialingPrefix);

            try
            {
                return this.Get(whereConditions: condition, limit: 0).ToList<NumberingPlan>();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Country's ISO2Code, return all the Numbering Plan records associated with it.
        /// </summary>
        /// <param name="DialingPrefix">Country.ISO2Code (string)</param>
        /// <returns>A list of NumberingPlan objects</returns>
        public List<NumberingPlan> GetByISO2CountryCode(string ISO2Code)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("Two_Digits_country_code", ISO2Code);

            try
            {
                return this.Get(whereConditions: condition, limit: 0).ToList<NumberingPlan>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Country's ISO3Code, return all the Numbering Plan records associated with it.
        /// </summary>
        /// <param name="DialingPrefix">Country.ISO3Code (string)</param>
        /// <returns>A list of NumberingPlan objects</returns>
        public List<NumberingPlan> GetByISO3CountryCode(string ISO3Code)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("Three_Digits_Country_Code", ISO3Code);

            try
            {
                return this.Get(whereConditions: condition, limit: 0).ToList<NumberingPlan>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Telephone Number as a string, return it's Country's ISO3Code.
        /// </summary>
        /// <param name="DialingPrefix">Telephone Number (string)</param>
        /// <returns>A Country's ISO3Code (string)</returns>
        public string GetISO3CountryCodeByNumber(string TelephoneNumber)
        {
            long numberToParse = 0;
            string ISO3CountryCode = null;
            List<NumberingPlan> countriesCodes = null;

            var condition = new Dictionary<string, object>();
            condition.Add("Type_Of_Service", "countrycode");

            try
            {
                countriesCodes = this.Get(whereConditions: condition, limit: 0).ToList<NumberingPlan>();

                if(countriesCodes != null && countriesCodes.Count > 0)
                {
                    if (string.IsNullOrEmpty(TelephoneNumber))
                    {
                        return null;
                    }
                    else if (TelephoneNumber.Contains(";"))
                    {
                        var parts = TelephoneNumber.Split(';').ToList();

                        if(";" != parts.First())
                        {
                            TelephoneNumber = parts.First();
                        }
                        else
                        {
                            TelephoneNumber = parts[2];
                        }
                    }


                    //Begin by trimming the "+" symbol
                    TelephoneNumber = TelephoneNumber.Trim('+');


                    //Try to parse the number and match it with the numbering plan
                    if (TelephoneNumber.Length >= 9)
                    {
                        long.TryParse(TelephoneNumber, out numberToParse);

                        while (numberToParse > 0)
                        {
                            var number = countriesCodes.Find(item => item.DialingPrefix == numberToParse);

                            if (number != null)
                            {
                                // RETURN
                                ISO3CountryCode = number.ISO3CountryCode;
                                break;
                            }
                            else
                            {
                                numberToParse = numberToParse / 10;
                                continue;
                            }
                        }//end-while

                    }//end-inner-if

                }//end-outer-if

                return ISO3CountryCode;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override NumberingPlan GetById(long id, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            NumberingPlan dialingRecord = null;

            try
            {
                dialingRecord = base.GetById(id, dataSourceName, dataSource);

                if(null != dialingRecord)
                {
                    var temporaryList = new List<NumberingPlan>() { dialingRecord } as IEnumerable<NumberingPlan>;
                    this.FillCountriesAndCurrenciesData(ref temporaryList);
                    dialingRecord = temporaryList.First();
                }

                return dialingRecord;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<NumberingPlan> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            IEnumerable<NumberingPlan> numberingPlan = null;

            try
            { 
                numberingPlan = base.Get(whereConditions, limit, dataSourceName, dataSource);

                if(null != numberingPlan && numberingPlan.Count() > 0)
                {
                    this.FillCountriesAndCurrenciesData(ref numberingPlan);
                }

                return numberingPlan;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<NumberingPlan> Get(System.Linq.Expressions.Expression<Func<NumberingPlan, bool>> predicate, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            IEnumerable<NumberingPlan> numberingPlan = null;

            try
            {
                numberingPlan = base.Get(predicate, dataSourceName, dataSource);

                if (null != numberingPlan && numberingPlan.Count() > 0)
                {
                    this.FillCountriesAndCurrenciesData(ref numberingPlan);
                }

                return numberingPlan;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<NumberingPlan> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            IEnumerable<NumberingPlan> numberingPlan = new List<NumberingPlan>() ;

            try
            {
                numberingPlan = numberingPlan.GetWithRelations(item => item.Country);

                //if (null != numberingPlan && numberingPlan.Count() > 0)
                //{
                //    this.FillCountriesAndCurrenciesData(ref numberingPlan);
                //}

                return numberingPlan;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<NumberingPlan> GetAll(string sql)
        {
            IEnumerable<NumberingPlan> numberingPlan = null;

            try
            {
                numberingPlan = base.GetAll(sql);

                if (null != numberingPlan && numberingPlan.Count() > 0)
                {
                    this.FillCountriesAndCurrenciesData(ref numberingPlan);
                }

                return numberingPlan;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }//end-class

}
