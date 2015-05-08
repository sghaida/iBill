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
        private readonly DataAccess<RatesInternational> _interRatesDataMapper = new DataAccess<RatesInternational>();
        /***
         * These data mappers are responsible for converting the in the Rates tables to different meaningful data models.
         */
        private readonly DataAccess<RatesNational> _nationalRatesDataMapper = new DataAccess<RatesNational>();
        private readonly NumberingPlansDataMapper _numberingPlanDataMapper = new NumberingPlansDataMapper();
        /***
         * The SQL Queries Repository
         */
        private readonly RatesSql _ratesSqlQueries = new RatesSql();

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
                            RateId = rate.RateId,
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
        /// <param name="gatewayId">Gateway.ID</param>
        /// <returns>Rates table name</returns>
        private string GetTableNameByGatewayId(int gatewayId)
        {
            var tableName = string.Empty;

            try
            {
                var gatewayRatesInfo = _gatewaysRatesDataMapper.GetByGatewayId(gatewayId);

                if (gatewayRatesInfo != null && gatewayRatesInfo.Count > 0)
                {
                    var currentRates = gatewayRatesInfo.First(info => info.EndingDate == DateTime.MinValue);

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
        /// <param name="gatewayId"></param>
        /// <returns></returns>
        public List<Rate> GetByGatewayId(int gatewayId)
        {
            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);

                if (string.IsNullOrEmpty(tableName))
                {
                    return null;
                }

                return base.GetAll(tableName, Globals.DataSource.Type.DbTable).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <returns></returns>
        public List<Rate> GetByDialingCode(int gatewayId, long dialingCode)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("country_code_dialing_prefix", dialingCode);

            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);

                if (string.IsNullOrEmpty(tableName))
                {
                    return null;
                }

                return base.Get(condition, dataSourceName: tableName, dataSourceType: Globals.DataSource.Type.DbTable).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <param name="iso3CountryCode"></param>
        /// <returns></returns>
        public List<RatesNational> GetNationalRatesForCountryByGatewayId(int gatewayId, string iso3CountryCode)
        {
            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);

                //Check if there is a rates table to get the rates from
                if (string.IsNullOrEmpty(tableName))
                {
                    return null;
                }

                var sql = _ratesSqlQueries.GetNationalRatesForCountry(tableName, iso3CountryCode);
                return _nationalRatesDataMapper.GetAll(sql).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <param name="ISO3CountryCode"></param>
        /// <returns></returns>
        public List<RatesInternational> GetInternationalRatesByGatewayId(int gatewayId)
        {
            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);
                
                //Check if there is a rates table to get the rates from
                if (string.IsNullOrEmpty(tableName))
                {
                    return null;
                }

                var sql = _ratesSqlQueries.GetInternationalRates(tableName);
                return _interRatesDataMapper.GetAll(sql).ToList();
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
        public Dictionary<int, List<RatesInternational>> GetGatewaysRatesById()
        {
            var gatewayRateInfo = _gatewaysRatesDataMapper.GetAll().ToList();
            var internationalRates = new Dictionary<int, List<RatesInternational>>();

            Parallel.ForEach(gatewayRateInfo, item =>
            {
                lock (internationalRates)
                {
                    internationalRates.Add(item.Gateway.Id, GetInternationalRatesByGatewayId(item.Gateway.Id));
                }
            });

            return internationalRates;
        }

        /// <summary>
        ///     Returns All Gateways International Rates Info Key value Per and the key is the Gateway name
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<RatesInternational>> GetGatewaysRatesByName()
        {
            var gatewayRateInfo = _gatewaysRatesDataMapper.GetAll().ToList();
            var internationalRates = new Dictionary<string, List<RatesInternational>>();

            Parallel.ForEach(gatewayRateInfo, item =>
            {
                lock (internationalRates)
                {
                    internationalRates.Add(item.Gateway.Name, GetInternationalRatesByGatewayId(item.Gateway.Id));
                }
            });

            return internationalRates;
        }

        /// <summary>
        ///     Insert Rate object into the Gateway's rates table.
        /// </summary>
        /// <param name="rateObject">The Rate object to insert.</param>
        /// <param name="gatewayId">The Gateway's ID to insert the Rate object into it's Rates table.</param>
        /// <returns>Database Row ID</returns>
        public int Insert(Rate rateObject, int gatewayId)
        {
            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);

                return base.Insert(rateObject, tableName, Globals.DataSource.Type.DbTable);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        ///     Insert Rate object into the Gateway's rates table.
        /// </summary>
        /// <param name="rateObject">The Rate object to insert.</param>
        /// <param name="gatewayId">The Gateway's ID to insert the Rate object into it's Rates table.</param>
        /// <returns>Database Row ID</returns>
        public int Insert(RatesNational rateObject, int gatewayId)
        {
            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);
                var tempRate = new Rate()
                {
                    DialingCode = rateObject.DialingCode,
                    Price = rateObject.Rate
                };

                return base.Insert(tempRate, tableName, Globals.DataSource.Type.DbTable);
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
        /// <param name="gatewayId">The Gateway's ID to update the Rate object from it's Rates table.</param>
        /// <returns>Update boolean</returns>
        public bool Update(Rate rateObject, int gatewayId)
        {
            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);

                return base.Update(rateObject, tableName, Globals.DataSource.Type.DbTable);
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
        /// <param name="gatewayId">The Gateway's ID to update the Rate object from it's Rates table.</param>
        /// <returns>Update boolean</returns>
        public bool Update(RatesNational rateObject, int gatewayId)
        {
            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);
                var tempRate = new Rate()
                {
                    Price = rateObject.Rate,
                    DialingCode = rateObject.DialingCode
                };

                return base.Update(tempRate, tableName, Globals.DataSource.Type.DbTable);
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
        /// <param name="gatewayId">The Gateway's ID to delete the Rate object from it's Rates table.</param>
        /// <returns>Delete boolean</returns>
        public bool Delete(Rate rateObject, int gatewayId)
        {
            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);

                return base.Delete(rateObject, tableName, Globals.DataSource.Type.DbTable);
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
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override bool Update(Rate dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Rate dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override Rate GetById(long id, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Rate> Get(Dictionary<string, object> whereConditions, int limit = 25,
            string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Rate> Get(Expression<Func<Rate, bool>> predicate, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Rate> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Rate> GetAll(string sqlQuery)
        {
            throw new NotImplementedException();
        }
    }
}