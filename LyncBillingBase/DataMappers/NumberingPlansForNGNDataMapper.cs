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
    public class NumberingPlansForNgnDataMapper : DataAccess<NumberingPlanForNgn>
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
        /// <param name="ngnNumberingPlan">A list of Numbering Plan objects</param>
        private void FillCountriesAndCurrenciesData(ref IEnumerable<NumberingPlanForNgn> ngnNumberingPlan)
        {
            try
            {
                var allCountries = _countriesDataMapper.GetAll();

                // Enable parallelization on the enumerable collection
                allCountries = allCountries.AsParallel();
                ngnNumberingPlan = ngnNumberingPlan.AsParallel();

                ngnNumberingPlan =
                    (from dialingRecord in ngnNumberingPlan
                        where (dialingRecord.Country != null && dialingRecord.Country.Id > 0)
                        join countryObject in allCountries on dialingRecord.Country.Id equals countryObject.Id
                        select new NumberingPlanForNgn
                        {
                            Id = dialingRecord.Id,
                            DialingCode = dialingRecord.DialingCode,
                            Iso3CountryCode = dialingRecord.Iso3CountryCode,
                            Provider = dialingRecord.Provider,
                            TypeOfServiceId = dialingRecord.TypeOfServiceId,
                            Description = dialingRecord.Description,
                            //RELATIONS
                            TypeOfService = dialingRecord.TypeOfService,
                            Country = countryObject
                        }).AsEnumerable<NumberingPlanForNgn>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Dialing Prefix, return all the NGN Numbering Plan records associated with it.
        /// </summary>
        /// <param name="DialingPrefix">NumberingPlanForNGN.DialingPrefix (string)</param>
        /// <returns>A list of NumberingPlanForNGN objects</returns>
        public List<NumberingPlanForNgn> GetByPrefix(string dialingCode)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("DialingCode", dialingCode);

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
        ///     Given a Country's ISO3Code, return all the NGN Numbering Plan records associated with it.
        /// </summary>
        /// <param name="DialingPrefix">Country.ISO3Code (string)</param>
        /// <returns>A list of NumberingPlanForNGN objects</returns>
        public List<NumberingPlanForNgn> GetByIso3CountryCode(string iso3Code)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("CountryCodeISO3", iso3Code);

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
            List<NumberingPlanForNgn> ngnCountriesCodes = null;

            var condition = new Dictionary<string, object>();
            condition.Add("Type_Of_Service", "countrycode");

            try
            {
                ngnCountriesCodes = Get(condition, 0).ToList();

                if (ngnCountriesCodes != null && ngnCountriesCodes.Count > 0)
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
                            var number =
                                ngnCountriesCodes.Find(item => item.DialingCode == Convert.ToString(numberToParse));

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

        public override NumberingPlanForNgn GetById(long id, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            NumberingPlanForNgn dialingRecord = null;

            try
            {
                dialingRecord = base.GetById(id, dataSourceName, dataSource);

                if (null != dialingRecord)
                {
                    var temporaryList =
                        new List<NumberingPlanForNgn> {dialingRecord} as IEnumerable<NumberingPlanForNgn>;
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

        public override IEnumerable<NumberingPlanForNgn> Get(Dictionary<string, object> whereConditions, int limit = 25,
            string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            IEnumerable<NumberingPlanForNgn> ngnNumberingPlan = null;

            try
            {
                ngnNumberingPlan = base.Get(whereConditions, limit, dataSourceName, dataSource);

                if (null != ngnNumberingPlan && ngnNumberingPlan.Count() > 0)
                {
                    FillCountriesAndCurrenciesData(ref ngnNumberingPlan);
                }

                return ngnNumberingPlan;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<NumberingPlanForNgn> Get(Expression<Func<NumberingPlanForNgn, bool>> predicate,
            string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            IEnumerable<NumberingPlanForNgn> ngnNumberingPlan = null;

            try
            {
                ngnNumberingPlan = base.Get(predicate, dataSourceName, dataSource);

                if (null != ngnNumberingPlan && ngnNumberingPlan.Count() > 0)
                {
                    FillCountriesAndCurrenciesData(ref ngnNumberingPlan);
                }

                return ngnNumberingPlan;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<NumberingPlanForNgn> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            IEnumerable<NumberingPlanForNgn> ngnNumberingPlan = new List<NumberingPlanForNgn>();

            try
            {
                //ngnNumberingPlan = base.GetAll(dataSourceName, dataSource);
                ngnNumberingPlan = ngnNumberingPlan.GetWithRelations(item => item.Country, item => item.TypeOfService);

                //if (null != ngnNumberingPlan && ngnNumberingPlan.Count() > 0)
                //{
                //    this.FillCountriesAndCurrenciesData(ref ngnNumberingPlan);
                //}

                return ngnNumberingPlan;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<NumberingPlanForNgn> GetAll(string sqlQuery)
        {
            IEnumerable<NumberingPlanForNgn> ngnNumberingPlan = null;

            try
            {
                ngnNumberingPlan = base.GetAll(sqlQuery);

                if (null != ngnNumberingPlan && ngnNumberingPlan.Count() > 0)
                {
                    FillCountriesAndCurrenciesData(ref ngnNumberingPlan);
                }

                return ngnNumberingPlan;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}