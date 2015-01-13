using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class NumberingPlansDataMapper : DataAccess<NumberingPlan>
    {
        /**
         * This instance of the Countries DataMapper is used for data reading only.
         */
        private readonly CountriesDataMapper _countriesDataMapper = new CountriesDataMapper();

        /// <summary>
        ///     Given a list of Numbering Plan objects, fill their Countries objects with the Country's Data Relations.
        ///     We are doing this here, because there is no feature for executing nested data relations.
        ///     We have to fill the data relations inside the local Countries objects ourselves.
        /// </summary>
        /// <param name="numberingPlan">A list of Numbering Plan objects</param>
        private void FillCountriesAndCurrenciesData(ref IEnumerable<NumberingPlan> numberingPlan)
        {
            try
            {
                var allCountries = _countriesDataMapper.GetAll();

                // Enable parallelization on the enumerable collection
                //allCountries = allCountries.AsParallel<Country>();
                //numberingPlan = numberingPlan.AsParallel<NumberingPlan>();

                numberingPlan =
                    (from dialingRecord in numberingPlan
                        where (dialingRecord.Country != null && dialingRecord.Country.Id > 0)
                        join countryObject in allCountries on dialingRecord.Country.Id equals countryObject.Id
                        select new NumberingPlan
                        {
                            DialingPrefix = dialingRecord.DialingPrefix,
                            Iso2CountryCode = dialingRecord.Iso2CountryCode,
                            Iso3CountryCode = dialingRecord.Iso3CountryCode,
                            CountryName = dialingRecord.CountryName,
                            City = dialingRecord.City,
                            Provider = dialingRecord.Provider,
                            TypeOfService = dialingRecord.TypeOfService,
                            //RELATIONS
                            Country = countryObject
                        });
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Dialing Prefix, return all the Numbering Plan records associated with it.
        /// </summary>
        /// <param name="dialingPrefix">NumberingPlan.DialingPrefix (Int64)</param>
        /// <returns>A list of NumberingPlan objects</returns>
        public List<NumberingPlan> GetByPrefix(Int64 dialingPrefix)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("DialingPrefix", dialingPrefix);

            try
            {
                return Get(condition, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Country's ISO2Code, return all the Numbering Plan records associated with it.
        /// </summary>
        /// <param name="DialingPrefix">Country.ISO2Code (string)</param>
        /// <returns>A list of NumberingPlan objects</returns>
        public List<NumberingPlan> GetByIso2CountryCode(string iso2Code)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("Two_Digits_country_code", iso2Code);

            try
            {
                return Get(condition, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Country's ISO3Code, return all the Numbering Plan records associated with it.
        /// </summary>
        /// <param name="DialingPrefix">Country.ISO3Code (string)</param>
        /// <returns>A list of NumberingPlan objects</returns>
        public List<NumberingPlan> GetByIso3CountryCode(string iso3Code)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("Three_Digits_Country_Code", iso3Code);

            try
            {
                return Get(condition, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Telephone Number as a string, return it's Country's ISO3Code.
        /// </summary>
        /// <param name="DialingPrefix">Telephone Number (string)</param>
        /// <returns>A Country's ISO3Code (string)</returns>
        public string GetIso3CountryCodeByNumber(string telephoneNumber)
        {
            long numberToParse = 0;
            string iso3CountryCode = null;
            List<NumberingPlan> countriesCodes = null;

            var condition = new Dictionary<string, object>();
            condition.Add("Type_Of_Service", "countrycode");

            try
            {
                countriesCodes = Get(condition, 0).ToList();

                if (countriesCodes != null && countriesCodes.Count > 0)
                {
                    if (string.IsNullOrEmpty(telephoneNumber))
                    {
                        return null;
                    }
                    if (telephoneNumber.Contains(";"))
                    {
                        var parts = telephoneNumber.Split(';').ToList();

                        if (";" != parts.First())
                        {
                            telephoneNumber = parts.First();
                        }
                        else
                        {
                            telephoneNumber = parts[2];
                        }
                    }


                    //Begin by trimming the "+" symbol
                    telephoneNumber = telephoneNumber.Trim('+');


                    //Try to parse the number and match it with the numbering plan
                    if (telephoneNumber.Length >= 9)
                    {
                        long.TryParse(telephoneNumber, out numberToParse);

                        while (numberToParse > 0)
                        {
                            var number = countriesCodes.Find(item => item.DialingPrefix == numberToParse);

                            if (number != null)
                            {
                                // RETURN
                                iso3CountryCode = number.Iso3CountryCode;
                                break;
                            }
                            numberToParse = numberToParse/10;
                        } //end-while
                    } //end-inner-if
                } //end-outer-if

                return iso3CountryCode;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override NumberingPlan GetById(long id, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            NumberingPlan dialingRecord = null;

            try
            {
                dialingRecord = base.GetById(id, dataSourceName, dataSource);

                if (null != dialingRecord)
                {
                    var temporaryList = new List<NumberingPlan> {dialingRecord} as IEnumerable<NumberingPlan>;
                    FillCountriesAndCurrenciesData(ref temporaryList);
                    dialingRecord = temporaryList.First();
                }

                return dialingRecord;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<NumberingPlan> Get(Dictionary<string, object> whereConditions, int limit = 25,
            string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            IEnumerable<NumberingPlan> numberingPlan = null;

            try
            {
                numberingPlan = base.Get(whereConditions, limit, dataSourceName, dataSource);

                if (null != numberingPlan && numberingPlan.Count() > 0)
                {
                    FillCountriesAndCurrenciesData(ref numberingPlan);
                }

                return numberingPlan;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<NumberingPlan> Get(Expression<Func<NumberingPlan, bool>> predicate,
            string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            IEnumerable<NumberingPlan> numberingPlan = null;

            try
            {
                numberingPlan = base.Get(predicate, dataSourceName, dataSource);

                if (null != numberingPlan && numberingPlan.Count() > 0)
                {
                    FillCountriesAndCurrenciesData(ref numberingPlan);
                }

                return numberingPlan;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<NumberingPlan> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            IEnumerable<NumberingPlan> numberingPlan = new List<NumberingPlan>();

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

        public override IEnumerable<NumberingPlan> GetAll(string sqlQuery)
        {
            IEnumerable<NumberingPlan> numberingPlan = null;

            try
            {
                numberingPlan = base.GetAll(sqlQuery);

                if (null != numberingPlan && numberingPlan.Count() > 0)
                {
                    FillCountriesAndCurrenciesData(ref numberingPlan);
                }

                return numberingPlan;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    } //end-class
}