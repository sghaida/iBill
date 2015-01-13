using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataMappers.SQLQueries;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class RatesForNgnDataMapper : DataAccess<RateForNgn>
    {
        /***
         * These data mappers are used to complete the relations data.
         */
        private readonly GatewaysRatesDataMapper _gatewaysRatesDataMapper = new GatewaysRatesDataMapper();

        private readonly NumberingPlansForNgnDataMapper _ngnNumberingPlanDataMapper =
            new NumberingPlansForNgnDataMapper();

        /***
         * The SQL Queries Repository
         */
        private RatesSql _ratesSqlQueries = new RatesSql();

        /// <summary>
        ///     Given a list of Rates Objects, fill their Numbering Plan objects with the Numbering Plan's Data Relations.
        ///     We are doing this here, because there is no feature for executing nested data relations.
        ///     We have to fill the data relations inside the local Numbering Plan objects ourselves.
        /// </summary>
        /// <param name="numberingPlan">A list of Numbering Plan objects</param>
        private void FillNumberingPlanData(ref IEnumerable<RateForNgn> rates)
        {
            try
            {
                var allNumberingPlan = _ngnNumberingPlanDataMapper.GetAll();

                // Enable parallelization on the enumerable collection
                allNumberingPlan = allNumberingPlan.AsParallel();
                rates = rates.AsParallel();

                rates =
                    (from rate in rates
                        where (rate.NumberingPlanForNgn != null && rate.NumberingPlanForNgn.Id > 0)
                        join numPlan in allNumberingPlan on rate.NumberingPlanForNgn.Id equals numPlan.Id
                        select new RateForNgn
                        {
                            Id = rate.Id,
                            DialingCodeId = rate.DialingCodeId,
                            Rate = rate.Rate,
                            //relations
                            NumberingPlanForNgn = numPlan
                        }).AsEnumerable<RateForNgn>();
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
                    var currentRates = gatewayRatesInfo.Find(info => info.EndingDate == DateTime.MinValue);

                    if (currentRates != null && !string.IsNullOrEmpty(currentRates.NgnRatesTableName))
                    {
                        tableName = currentRates.NgnRatesTableName;
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
        public List<RateForNgn> GetByGatewayId(int gatewayId)
        {
            var gatewayNgnRates = new List<RateForNgn>();

            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);

                if (string.IsNullOrEmpty(tableName))
                {
                    return null;
                }

                //var SQL = RATES_SQL_QUERIES.GetNGNRates(tableName);
                return gatewayNgnRates.GetWithRelations(tableName, item => item.NumberingPlanForNgn).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Returns All Gateways Rates Info Key value Per and the key is the Gateway ID
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<RateForNgn>> GetGatewaysNgnRatesById()
        {
            var ngnlRates = new Dictionary<int, List<RateForNgn>>();

            var gatewayRatesInfo = _gatewaysRatesDataMapper.GetAll().ToList();

            Parallel.ForEach(gatewayRatesInfo, item =>
            {
                lock (ngnlRates)
                {
                    ngnlRates.Add(item.Gateway.Id, GetByGatewayId(item.Gateway.Id));
                }
            });

            return ngnlRates;
        }

        /// <summary>
        ///     Returns All Gateways NGN Rates Info Key value Per and the key is the Gateway name
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<RateForNgn>> GetGatewaysNgnRatesByName()
        {
            var ngnlRates = new Dictionary<string, List<RateForNgn>>();

            var gatewayRatesInfo = _gatewaysRatesDataMapper.GetAll().ToList();

            Parallel.ForEach(gatewayRatesInfo, item =>
            {
                lock (ngnlRates)
                {
                    ngnlRates.Add(item.Gateway.Name, GetByGatewayId(item.Gateway.Id));
                }
            });

            return ngnlRates;
        }

        /// <summary>
        ///     Insert RateForNGN object into the Gateway's NGN Rates table.
        /// </summary>
        /// <param name="ngnRateObject">The Rate object to insert.</param>
        /// <param name="gatewayId">The Gateway's ID to insert the Rate object into it's NGN Rates table.</param>
        /// <returns>Database Row ID</returns>
        public int Insert(RateForNgn ngnRateObject, int gatewayId)
        {
            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);

                return base.Insert(ngnRateObject, tableName, Globals.DataSource.Type.DBTable);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Update a RateForNGN object in a Gateway's NGN Rates table.
        /// </summary>
        /// <param name="ngnRateObject">The NGN Rate object to insert.</param>
        /// <param name="gatewayId">The Gateway's ID to update the Rate object from it's NGN Rates table.</param>
        /// <returns>Update boolean</returns>
        public bool Update(RateForNgn ngnRateObject, int gatewayId)
        {
            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);

                return base.Update(ngnRateObject, tableName, Globals.DataSource.Type.DBTable);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Delete a RateForNGN object from a Gateway's NGN Rates table.
        /// </summary>
        /// <param name="ngnRateObject">The NGN Rate object to insert.</param>
        /// <param name="gatewayId">The Gateway's ID to delete the Rate object from it's NGN Rates table.</param>
        /// <returns>Delete boolean</returns>
        public bool Delete(RateForNgn ngnRateObject, int gatewayId)
        {
            try
            {
                var tableName = GetTableNameByGatewayId(gatewayId);

                return base.Delete(ngnRateObject, tableName, Globals.DataSource.Type.DBTable);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /***
         * Disable the default parent functions
         */

        public override int Insert(RateForNgn dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override bool Update(RateForNgn dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(RateForNgn dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override RateForNgn GetById(long id, string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<RateForNgn> Get(Dictionary<string, object> whereConditions, int limit = 25,
            string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<RateForNgn> Get(Expression<Func<RateForNgn, bool>> predicate,
            string dataSourceName = null, Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<RateForNgn> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSource = Globals.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<RateForNgn> GetAll(string sqlQuery)
        {
            throw new NotImplementedException();
        }
    }
}