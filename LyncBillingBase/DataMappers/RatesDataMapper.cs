using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataMappers.SQLQueries;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class RatesDataMapper : DataAccess<Rate>
    {
        /***
         * These data mappers are used to complete the relations data.
         */
        private readonly GatewaysRatesDataMapper _gatewaysRatesDataMapper = new GatewaysRatesDataMapper();
        private readonly DataAccess<Rates_International> _interRatesDataMapper = new DataAccess<Rates_International>();
        /***
         * These data mappers are responsible for converting the in the Rates tables to different meaningful data models.
         */
        private readonly DataAccess<Rates_National> _nationalRatesDataMapper = new DataAccess<Rates_National>();
        private readonly NumberingPlansDataMapper _numberingPlanDataMapper = new NumberingPlansDataMapper();
        /***
         * The SQL Queries Repository
         */
        private readonly RatesSQL RATES_SQL_QUERIES = new RatesSQL();

        /// <summary>
        ///     Given a list of Rates Objects, fill their Numbering Plan objects with the Numbering Plan's Data Relations.
        ///     We are doing this here, because there is no feature for executing nested data relations.
        ///     We have to fill the data relations inside the local Numbering Plan objects ourselves.
        /// </summary>
        /// <param name="numberingPlan">A list of Numbering Plan objects</param>
        private void FillNumberingPlanData(ref IEnumerable<Rate> rates)
        {
            try
            {
                var allNumberingPlan = _numberingPlanDataMapper.GetAll();

                // Enable parallelization on the enumerable collection
                allNumberingPlan = allNumberingPlan.AsParallel();
                rates = rates.AsParallel();

                rates =
                    (from rate in rates
                        where (rate.NumberingPlan != null && rate.NumberingPlan.DialingPrefix > 0)
                        join numPlan in allNumberingPlan on rate.NumberingPlan.DialingPrefix equals
                            numPlan.DialingPrefix
                        select new Rate
                        {
                            RateID = rate.RateID,
                            DialingCode = rate.DialingCode,
                            Price = rate.Price,
                            //relations
                            NumberingPlan = numPlan
                        }).AsEnumerable<Rate>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Gateway's ID, return it's currently active Rates table name
        /// </summary>
        /// <param name="GatewayID">Gateway.ID</param>
        /// <returns>Rates table name</returns>
        private string GetTableNameByGatewayID(int GatewayID)
        {
            var tableName = string.Empty;

            try
            {
                var gatewayRatesInfo = _gatewaysRatesDataMapper.GetByGatewayID(GatewayID);

                if (gatewayRatesInfo != null && gatewayRatesInfo.Count > 0)
                {
                    var currentRates = gatewayRatesInfo.Find(info => info.EndingDate == DateTime.MinValue);

                    if (currentRates != null && !string.IsNullOrEmpty(currentRates.RatesTableName))
                    {
                        tableName = currentRates.RatesTableName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            return tableName;
        }

        /// <summary>
        /// </summary>
        /// <param name="GatewayID"></param>
        /// <returns></returns>
        public List<Rate> GetByGatewayID(int GatewayID)
        {
            try
            {
                var tableName = GetTableNameByGatewayID(GatewayID);

                return base.GetAll(tableName, GLOBALS.DataSource.Type.DBTable).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="GatewayID"></param>
        /// <returns></returns>
        public List<Rate> GetByDialingCode(int GatewayID, long DialingCode)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("country_code_dialing_prefix", DialingCode);

            try
            {
                var tableName = GetTableNameByGatewayID(GatewayID);

                return
                    base.Get(condition, dataSourceName: tableName, dataSourceType: GLOBALS.DataSource.Type.DBTable)
                        .ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="GatewayID"></param>
        /// <param name="ISO3CountryCode"></param>
        /// <returns></returns>
        public List<Rates_National> GetNationalRatesForCountryByGatewayID(int GatewayID, string ISO3CountryCode)
        {
            try
            {
                var tableName = GetTableNameByGatewayID(GatewayID);
                var SQL = RATES_SQL_QUERIES.GetNationalRatesForCountry(tableName, ISO3CountryCode);

                return _nationalRatesDataMapper.GetAll(SQL).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="GatewayID"></param>
        /// <param name="ISO3CountryCode"></param>
        /// <returns></returns>
        public List<Rates_International> GetInternationalRatesByGatewayID(int GatewayID)
        {
            try
            {
                var tableName = GetTableNameByGatewayID(GatewayID);
                var SQL = RATES_SQL_QUERIES.GetInternationalRates(tableName);

                //Check if there is a rates table to get the rates from
                if (string.IsNullOrEmpty(tableName))
                {
                    return null;
                }

                return _interRatesDataMapper.GetAll(SQL).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Returns all Gateways International Rates Info Key value Per and the key is the Gateway ID
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<Rates_International>> GetGatewaysRatesByID()
        {
            var gatewayRateInfo = _gatewaysRatesDataMapper.GetAll().ToList();
            var internationalRates = new Dictionary<int, List<Rates_International>>();

            Parallel.ForEach(gatewayRateInfo, item =>
            {
                lock (internationalRates)
                {
                    internationalRates.Add(item.Gateway.ID, GetInternationalRatesByGatewayID(item.Gateway.ID));
                }
            });

            return internationalRates;
        }

        /// <summary>
        ///     Returns All Gateways International Rates Info Key value Per and the key is the Gateway name
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<Rates_International>> GetGatewaysRatesByName()
        {
            var gatewayRateInfo = _gatewaysRatesDataMapper.GetAll().ToList();
            var internationalRates = new Dictionary<string, List<Rates_International>>();

            Parallel.ForEach(gatewayRateInfo, item =>
            {
                lock (internationalRates)
                {
                    internationalRates.Add(item.Gateway.Name, GetInternationalRatesByGatewayID(item.Gateway.ID));
                }
            });

            return internationalRates;
        }

        /// <summary>
        ///     Insert Rate object into the Gateway's rates table.
        /// </summary>
        /// <param name="rateObject">The Rate object to insert.</param>
        /// <param name="GatewayID">The Gateway's ID to insert the Rate object into it's Rates table.</param>
        /// <returns>Database Row ID</returns>
        public int Insert(Rate rateObject, int GatewayID)
        {
            try
            {
                var tableName = GetTableNameByGatewayID(GatewayID);

                return base.Insert(rateObject, tableName, GLOBALS.DataSource.Type.DBTable);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Update a Rate object in a Gateway's rates table.
        /// </summary>
        /// <param name="rateObject">The Rate object to insert.</param>
        /// <param name="GatewayID">The Gateway's ID to update the Rate object from it's Rates table.</param>
        /// <returns>Update boolean</returns>
        public bool Update(Rate rateObject, int GatewayID)
        {
            try
            {
                var tableName = GetTableNameByGatewayID(GatewayID);

                return base.Update(rateObject, tableName, GLOBALS.DataSource.Type.DBTable);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Delete a Rate object from a Gateway's rates table.
        /// </summary>
        /// <param name="rateObject">The Rate object to insert.</param>
        /// <param name="GatewayID">The Gateway's ID to delete the Rate object from it's Rates table.</param>
        /// <returns>Delete boolean</returns>
        public bool Delete(Rate rateObject, int GatewayID)
        {
            try
            {
                var tableName = GetTableNameByGatewayID(GatewayID);

                return base.Delete(rateObject, tableName, GLOBALS.DataSource.Type.DBTable);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /***
         * Disable the default parent functions
         */

        public override int Insert(Rate dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override bool Update(Rate dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Rate dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override Rate GetById(long id, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Rate> Get(Dictionary<string, object> whereConditions, int limit = 25,
            string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Rate> Get(Expression<Func<Rate, bool>> predicate, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Rate> GetAll(string dataSourceName = null,
            GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Rate> GetAll(string sql)
        {
            throw new NotImplementedException();
        }
    }
}