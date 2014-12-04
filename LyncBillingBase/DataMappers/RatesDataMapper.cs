using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataModels;
using LyncBillingBase.DataAccess;

namespace LyncBillingBase.DataMappers
{
    public class RatesDataMapper : DataAccess<Rate>
    {
        /***
         * These data mappers are responsible for converting the in the Rates tables to different meaningful data models.
         */
        private DataAccess<Rates_National> _nationalRatesDataMapper = new DataAccess<Rates_National>();
        private DataAccess<Rates_International> _interRatesDataMapper = new DataAccess<Rates_International>();

        /***
         * These data mappers are used to complete the relations data.
         */
        private GatewaysRatesDataMapper _gatewaysRatesDataMapper = new GatewaysRatesDataMapper();
        private NumberingPlansDataMapper _numberingPlanDataMapper = new NumberingPlansDataMapper();

        /***
         * The SQL Queries Repository
         */
        private SQLQueries.RatesSQL RATES_SQL_QUERIES = new SQLQueries.RatesSQL();
        

        /// <summary>
        /// Given a list of Rates Objects, fill their Numbering Plan objects with the Numbering Plan's Data Relations.
        /// We are doing this here, because there is no feature for executing nested data relations.
        /// We have to fill the data relations inside the local Numbering Plan objects ourselves.
        /// </summary>
        /// <param name="numberingPlan">A list of Numbering Plan objects</param>
        private void FillNumberingPlanData(ref IEnumerable<Rate> rates)
        {
            try
            {
                IEnumerable<NumberingPlan> allNumberingPlan = _numberingPlanDataMapper.GetAll();

                // Enable parallelization on the enumerable collection
                allNumberingPlan = allNumberingPlan.AsParallel<NumberingPlan>();
                rates = rates.AsParallel<Rate>();

                rates =
                    (from rate in rates
                     where (rate.NumberingPlan != null && rate.NumberingPlan.DialingPrefix > 0)
                     join numPlan in allNumberingPlan on rate.NumberingPlan.DialingPrefix equals numPlan.DialingPrefix
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
        /// Given a Gateway's ID, return it's currently active Rates table name
        /// </summary>
        /// <param name="GatewayID">Gateway.ID</param>
        /// <returns>Rates table name</returns>
        private string GetTableNameByGatewayID(int GatewayID)
        {
            string tableName = string.Empty;
            
            try
            { 
                var gatewayRatesInfo = _gatewaysRatesDataMapper.GetByGatewayID(GatewayID);

                if(gatewayRatesInfo != null && gatewayRatesInfo.Count > 0)
                {
                    var currentRates = gatewayRatesInfo.Find(info => info.EndingDate == DateTime.MinValue);

                    if(currentRates != null && !string.IsNullOrEmpty(currentRates.RatesTableName))
                    {
                        tableName = currentRates.RatesTableName;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }

            return tableName;
        }


        public RatesDataMapper() : base() { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="GatewayID"></param>
        /// <returns></returns>
        public List<Rate> GetByGatewayID(int GatewayID)
        {
            try
            {
                var tableName = GetTableNameByGatewayID(GatewayID);

                return base.GetAll(dataSourceName: tableName, dataSource: GLOBALS.DataSource.Type.DBTable).ToList<Rate>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// 
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

                return _nationalRatesDataMapper.GetAll(SQL_QUERY: SQL).ToList<Rates_National>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// 
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

                return _interRatesDataMapper.GetAll(SQL_QUERY: SQL).ToList<Rates_International>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        
        
        /// <summary>
        /// Insert Rate object into the Gateway's rates table.
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
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Update a Rate object in a Gateway's rates table.
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
        /// Delete a Rate object from a Gateway's rates table.
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
        public override int Insert(Rate dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }


        public override bool Update(Rate dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }


        public override bool Delete(Rate dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }


        public override Rate GetById(long id, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            throw new NotImplementedException();
        }


        public override IEnumerable<Rate> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            throw new NotImplementedException();
        }


        public override IEnumerable<Rate> Get(System.Linq.Expressions.Expression<Func<Rate, bool>> predicate, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            throw new NotImplementedException();
        }


        public override IEnumerable<Rate> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            throw new NotImplementedException();
        }


        public override IEnumerable<Rate> GetAll(string sql)
        {
            throw new NotImplementedException();
        }

    }

}
