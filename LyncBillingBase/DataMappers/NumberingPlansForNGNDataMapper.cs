using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DataMappers
{
    public class NumberingPlansForNGNDataMapper : DataAccess<NumberingPlanForNGN>
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
        /// <param name="ngnNumberingPlan">A list of Numbering Plan objects</param>
        private void FillCountriesAndCurrenciesData(ref IEnumerable<NumberingPlanForNGN> ngnNumberingPlan)
        {
            try
            {
                IEnumerable<Country> allCountries = _countriesDataMapper.GetAll();

                // Enable parallelization on the enumerable collection
                allCountries = allCountries.AsParallel<Country>();
                ngnNumberingPlan = ngnNumberingPlan.AsParallel<NumberingPlanForNGN>();

                ngnNumberingPlan = 
                    (from dialingRecord in ngnNumberingPlan
                     where (dialingRecord.Country != null && dialingRecord.Country.ID > 0)
                     join countryObject in allCountries on dialingRecord.Country.ID equals countryObject.ID
                     select new NumberingPlanForNGN
                     {
                         ID  = dialingRecord.ID,
                         DialingCode = dialingRecord.DialingCode,
                         ISO3CountryCode = dialingRecord.ISO3CountryCode,
                         Provider = dialingRecord.Provider,
                         TypeOfServiceID = dialingRecord.TypeOfServiceID,
                         Description = dialingRecord.Description,
                         //RELATIONS
                         TypeOfService = dialingRecord.TypeOfService,
                         Country = countryObject
                     }).AsEnumerable<NumberingPlanForNGN>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Dialing Prefix, return all the NGN Numbering Plan records associated with it.
        /// </summary>
        /// <param name="DialingPrefix">NumberingPlanForNGN.DialingPrefix (string)</param>
        /// <returns>A list of NumberingPlanForNGN objects</returns>
        public List<NumberingPlanForNGN> GetByPrefix(string DialingCode)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("DialingCode", DialingCode);

            try
            {
                return this.Get(whereConditions: condition, limit: 0).ToList<NumberingPlanForNGN>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Country's ISO3Code, return all the NGN Numbering Plan records associated with it.
        /// </summary>
        /// <param name="DialingPrefix">Country.ISO3Code (string)</param>
        /// <returns>A list of NumberingPlanForNGN objects</returns>
        public List<NumberingPlanForNGN> GetByISO3CountryCode(string ISO3Code)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("CountryCodeISO3", ISO3Code);

            try
            {
                return this.Get(whereConditions: condition, limit: 0).ToList<NumberingPlanForNGN>();
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
            List<NumberingPlanForNGN> ngnCountriesCodes = null;

            var condition = new Dictionary<string, object>();
            condition.Add("Type_Of_Service", "countrycode");

            try
            {
                ngnCountriesCodes = this.Get(whereConditions: condition, limit: 0).ToList<NumberingPlanForNGN>();

                if (ngnCountriesCodes != null && ngnCountriesCodes.Count > 0)
                {
                    if (string.IsNullOrEmpty(TelephoneNumber))
                    {
                        return null;
                    }
                    else if (TelephoneNumber.Contains(";"))
                    {
                        var parts = TelephoneNumber.Split(';').ToList();

                        if (";" != parts.First())
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
                            var number = ngnCountriesCodes.Find(item => item.DialingCode == Convert.ToString(numberToParse));

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


        public override NumberingPlanForNGN GetById(long id, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            NumberingPlanForNGN dialingRecord = null;

            try
            {
                dialingRecord = base.GetById(id, dataSourceName, dataSource);

                if (null != dialingRecord)
                {
                    var temporaryList = new List<NumberingPlanForNGN>() { dialingRecord } as IEnumerable<NumberingPlanForNGN>;
                    this.FillCountriesAndCurrenciesData(ref temporaryList);
                    dialingRecord = temporaryList.First();
                }

                return dialingRecord;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<NumberingPlanForNGN> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            IEnumerable<NumberingPlanForNGN> ngnNumberingPlan = null;

            try
            {
                ngnNumberingPlan = base.Get(whereConditions, limit, dataSourceName, dataSource);

                if (null != ngnNumberingPlan && ngnNumberingPlan.Count() > 0)
                {
                    this.FillCountriesAndCurrenciesData(ref ngnNumberingPlan);
                }

                return ngnNumberingPlan;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<NumberingPlanForNGN> Get(System.Linq.Expressions.Expression<Func<NumberingPlanForNGN, bool>> predicate, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            IEnumerable<NumberingPlanForNGN> ngnNumberingPlan = null;

            try
            {
                ngnNumberingPlan = base.Get(predicate, dataSourceName, dataSource);

                if (null != ngnNumberingPlan && ngnNumberingPlan.Count() > 0)
                {
                    this.FillCountriesAndCurrenciesData(ref ngnNumberingPlan);
                }

                return ngnNumberingPlan;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<NumberingPlanForNGN> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            IEnumerable<NumberingPlanForNGN> ngnNumberingPlan = new List<NumberingPlanForNGN>();

            try
            {
                //ngnNumberingPlan = base.GetAll(dataSourceName, dataSource);
                ngnNumberingPlan = ngnNumberingPlan.IncludeM(item => item.Country,item=>item.TypeOfService);

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


        public override IEnumerable<NumberingPlanForNGN> GetAll(string sql)
        {
            IEnumerable<NumberingPlanForNGN> ngnNumberingPlan = null;

            try
            {
                ngnNumberingPlan = base.GetAll(sql);

                if (null != ngnNumberingPlan && ngnNumberingPlan.Count() > 0)
                {
                    this.FillCountriesAndCurrenciesData(ref ngnNumberingPlan);
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
