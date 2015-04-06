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

        private static List<NumberingPlan> _numberingPlan = new List<NumberingPlan>();

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

        public NumberingPlansDataMapper()
        {
            if (_numberingPlan == null || _numberingPlan.Count == 0)
            {
                GetAll();
            }
        }

        /// <summary>
        ///     Given a Dialing Prefix, return all the Numbering Plan records associated with it.
        /// </summary>
        /// <param name="dialingPrefix">NumberingPlan.DialingPrefix (Int64)</param>
        /// <returns>A list of NumberingPlan objects</returns>
        public List<NumberingPlan> GetByPrefix(Int64 dialingPrefix)
        {
            if(_numberingPlan == null || _numberingPlan.Count == 0)
            {
                GetAll();
            }

            try
            {
                return _numberingPlan.Where(item => item.DialingPrefix == dialingPrefix).ToList();
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
            if (_numberingPlan == null || _numberingPlan.Count == 0)
            {
                GetAll();
            }

            try
            {
                iso2Code = iso2Code.ToLower();
                return _numberingPlan.Where(item => item.Iso2CountryCode.ToLower() == iso2Code).ToList();
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
            if (_numberingPlan == null || _numberingPlan.Count == 0)
            {
                GetAll();
            }

            try
            {
                iso3Code = iso3Code.ToLower();
                return _numberingPlan.Where(item => item.Iso3CountryCode.ToLower() == iso3Code).ToList();
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
            List<NumberingPlan> countriesCodes;
            string countryCodeTypeOfService = "countrycode";

            //
            // Validation of telephone number;
            if (string.IsNullOrEmpty(telephoneNumber))
            {
                return null;
            }

            //
            // Make sure the local cache of data is available.
            if (_numberingPlan == null || _numberingPlan.Count == 0)
            {
                GetAll();
            }

            try
            {
                countriesCodes = _numberingPlan.Where(item => item.TypeOfService.ToLower() == countryCodeTypeOfService).ToList() ?? (new List<NumberingPlan>());

                if (countriesCodes.Any())
                {
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

                    //
                    // Begin by trimming the "+" symbol
                    telephoneNumber = telephoneNumber.Trim('+');

                    //
                    // Try to parse the number and match it with the numbering plan
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

        public string GetTypeOfServiceByNumber(string telephoneNumber)
        {
            long numberToParse = 0;
            string typeOfService = string.Empty;
            string iso3CountryCode = string.Empty;
            List<NumberingPlan> countryCodes;

            //
            // Validation of telephone number;
            if(string.IsNullOrEmpty(telephoneNumber))
            {
                return null;
            }

            //
            // Make sure the local cache of data is available.
            if (_numberingPlan == null || _numberingPlan.Count == 0)
            {
                GetAll();
            }

            try
            {
                iso3CountryCode = this.GetIso3CountryCodeByNumber(telephoneNumber);
                countryCodes = _numberingPlan.Where(item => item.Iso3CountryCode == iso3CountryCode).ToList();

                if(countryCodes.Any())
                {
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

                    // 
                    // Begin by trimming the "+" symbol
                    telephoneNumber = telephoneNumber.Trim('+');

                    //
                    // Try to parse the number and match it with the numbering plan
                    if (telephoneNumber.Length >= 9)
                    {
                        long.TryParse(telephoneNumber, out numberToParse);

                        while (numberToParse > 0)
                        {
                            var number = countryCodes.Find(item => item.DialingPrefix == numberToParse);

                            if (number != null)
                            {
                                // RETURN
                                typeOfService = number.TypeOfService;
                                break;
                            }

                            numberToParse = numberToParse / 10;
                        } //end-while

                    } //end-inner-if

                }//end-outer-if
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }

            return typeOfService;
        }

        public override NumberingPlan GetById(long id, string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
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

        public override IEnumerable<NumberingPlan> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<NumberingPlan> Get(Expression<Func<NumberingPlan, bool>> predicate, string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<NumberingPlan> GetAll(string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            try
            {
                if (_numberingPlan == null || _numberingPlan.Count == 0)
                {
                    lock (_numberingPlan)
                    {
                        _numberingPlan = _numberingPlan.GetWithRelations(item => item.Country).ToList() ?? (new List<NumberingPlan>());
                    }
                }

                return _numberingPlan as IEnumerable<NumberingPlan>;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override int Insert(NumberingPlan dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            int rowNumber = -1;
            bool exists = false;

            //
            // Make sure the local cache is initialized and has data.
            if (_numberingPlan == null || _numberingPlan.Count == 0)
            {
                GetAll();
            }

            //
            // Does the data object exist?
            exists = _numberingPlan.Exists(item => item.DialingPrefix == dataObject.DialingPrefix);

            //
            // In case it doesn't exist, insert it to the database and then to the local cache.
            if (exists != true)
            {
                try
                {
                    rowNumber = base.Insert(dataObject, dataSourceName, dataSourceType);

                    lock (dataObject)
                    {
                        _numberingPlan.Add(dataObject);
                    }
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            //
            // Return the rowNumber variable.
            return rowNumber;
        }

        public override bool Update(NumberingPlan dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            bool updateStatus = false;

            //
            // Make sure the local cache is initialized and has data.
            if (_numberingPlan == null || _numberingPlan.Count == 0)
            {
                GetAll();
            }

            //
            // Does the data object exist?
            var existingItem = _numberingPlan.Find(item => item.DialingPrefix == dataObject.DialingPrefix);

            //
            // In case it does exist, update it in the database and in the local cache.
            if (existingItem != null)
            {
                try
                {
                    updateStatus = base.Update(dataObject, dataSourceName, dataSourceType);

                    if(updateStatus == true)
                    {
                        lock(_numberingPlan)
                        {
                            _numberingPlan.Remove(existingItem);
                            _numberingPlan.Add(dataObject);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            return updateStatus;
        }

        public override bool Delete(NumberingPlan dataObject, string dataSourceName = null, Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            bool deleteStatus = false;

            //
            // Make sure the local cache is initialized and has data.
            if (_numberingPlan == null || _numberingPlan.Count == 0)
            {
                GetAll();
            }

            //
            // Does the data object exist?
            var existingItem = _numberingPlan.Find(item => item.DialingPrefix == dataObject.DialingPrefix);

            //
            // In case it does exist, update it in the database and in the local cache.
            if (existingItem != null)
            {
                try
                {
                    deleteStatus = base.Delete(dataObject, dataSourceName, dataSourceType);

                    if(deleteStatus == true)
                    {
                        lock(_numberingPlan)
                        {
                            _numberingPlan.Remove(existingItem);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            return deleteStatus;
        }


        /***
         * DISABLED FUNCTIONS
         */
        [Obsolete]
        public override IEnumerable<NumberingPlan> GetAll(string sqlQuery)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override int Insert(string sql)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override bool Update(string sql)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override bool Delete(string sql)
        {
            throw new NotImplementedException();
        }

    } //end-class

}