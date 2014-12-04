using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataModels;
using LyncBillingBase.DataAccess;

namespace LyncBillingBase.DataMappers
{
    public class RatesForNGNDataMapper : DataAccess<RateForNGN>
    {
        /***
         * These data mappers are used to complete the relations data.
         */
        private GatewaysRatesDataMapper _gatewaysRatesDataMapper = new GatewaysRatesDataMapper();
        private NumberingPlansForNGNDataMapper _ngnNumberingPlanDataMapper = new NumberingPlansForNGNDataMapper();

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
        private void FillNumberingPlanData(ref IEnumerable<RateForNGN> rates)
        {
            try
            {
                IEnumerable<NumberingPlanForNGN> allNumberingPlan = _ngnNumberingPlanDataMapper.GetAll();

                // Enable parallelization on the enumerable collection
                allNumberingPlan = allNumberingPlan.AsParallel<NumberingPlanForNGN>();
                rates = rates.AsParallel<RateForNGN>();

                rates =
                    (from rate in rates
                     where (rate.NumberingPlanForNGN != null && rate.NumberingPlanForNGN.ID > 0)
                     join numPlan in allNumberingPlan on rate.NumberingPlanForNGN.ID equals numPlan.ID
                     select new RateForNGN
                     {
                         RateID = rate.RateID,
                         DialingCodeID = rate.DialingCodeID,
                         Rate = rate.Rate,
                         //relations
                         NumberingPlanForNGN = numPlan
                     }).AsEnumerable<RateForNGN>();
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
        /// 
        /// </summary>
        /// <param name="GatewayID"></param>
        /// <returns></returns>
        public List<RateForNGN> GetByGatewayID(int GatewayID)
        {
            try
            {
                var tableName = GetTableNameByGatewayID(GatewayID);
                var SQL = RATES_SQL_QUERIES.GetNGNRates(tableName);

                //return base.GetAll(SQL_QUERY: SQL).ToList<RateForNGN>();

                return base.GetAll(dataSourceName: tableName, dataSource: GLOBALS.DataSource.Type.DBTable).ToList<RateForNGN>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, List<RateForNGN>> GetAllGatewaysRatesList()
        {
            throw new NotImplementedException();

            //Dictionary<int, List<RateForNGN>> allRates = new Dictionary<int, List<RateForNGN>>();
            //List<RateForNGN> ratesPerGateway;

            ////Get Entire GatewaysRates to be able to get all the rates  

            //if (StoreLoader.gatewayRates.Count > 0)
            //{
            //    foreach (GatewaysRates GatewayRateTable in StoreLoader.gatewayRates)
            //    {
            //        // Check RateTable Exists and Rates_International ending time is not null or set : to get uptodate rates table
            //        if (GatewayRateTable.NgnRatesTableName != null &&
            //            (GatewayRateTable.EndingDate != DateTime.MinValue ||
            //            GatewayRateTable.EndingDate != null))
            //        {
            //            ratesPerGateway = new List<RatesNGN>();
            //            ratesPerGateway = GetRates(GatewayRateTable.NgnRatesTableName);
            //        }
            //        else
            //        {
            //            continue;
            //        }

            //        allRates.Add(GatewayRateTable.GatewayID, ratesPerGateway);
            //    }
            //}

            //return allRates;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, List<RateForNGN>> GetAllGatewaysRatesDictionary()
        {
            throw new NotImplementedException();

            //List<RatesNGN> ratesPerGateway;
            //string gatewayName = string.Empty;
            //Dictionary<string, List<RatesNGN>> allRates = new Dictionary<string, List<RatesNGN>>();

            ////Get Entire GatewaysRates to be able to get all the rates  

            //if (StoreLoader.gatewayRates.Count > 0)
            //{
            //    foreach (GatewaysRates GatewayRateTable in StoreLoader.gatewayRates)
            //    {
            //        gatewayName = string.Empty;

            //        // Check RateTable Exists and Rates_International ending time is not null or set : to get uptodate rates table
            //        if (GatewayRateTable.NgnRatesTableName != null &&
            //            (GatewayRateTable.EndingDate != DateTime.MinValue ||
            //            GatewayRateTable.EndingDate != null))
            //        {
            //            ratesPerGateway = new List<RatesNGN>();
            //            ratesPerGateway = GetRates(GatewayRateTable.NgnRatesTableName);
            //        }
            //        else
            //        {
            //            continue;
            //        }

            //        //Example:
            //        // GatewayRateTable.RatesTableName := "Rates_10.1.1.3_2013_04_02"
            //        // after splitting ===> gatewayRatesTableName := ["Rates_International", "10.1.1.3", "2013", "04", "02"] or ["Rates_International", "NGN","10.1.1.3", "2013", "04", "02"]
            //        var gatewayRatesTableName = GatewayRateTable.NgnRatesTableName.Split('_');

            //        if (gatewayRatesTableName.Contains("NGN"))
            //        {

            //            gatewayName = gatewayRatesTableName[2];
            //        }
            //        else
            //        {

            //            gatewayName = gatewayRatesTableName[1];
            //        }



            //        if (!allRates.Keys.Contains(gatewayName))
            //            allRates.Add(gatewayName, ratesPerGateway);
            //    }
            //}

            //return allRates;
        }


        /// <summary>
        /// Insert RateForNGN object into the Gateway's NGN Rates table.
        /// </summary>
        /// <param name="ngnRateObject">The Rate object to insert.</param>
        /// <param name="GatewayID">The Gateway's ID to insert the Rate object into it's NGN Rates table.</param>
        /// <returns>Database Row ID</returns>
        public int Insert(RateForNGN ngnRateObject, int GatewayID)
        {
            try
            {
                var tableName = GetTableNameByGatewayID(GatewayID);

                return base.Insert(ngnRateObject, tableName, GLOBALS.DataSource.Type.DBTable);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Update a RateForNGN object in a Gateway's NGN Rates table.
        /// </summary>
        /// <param name="ngnRateObject">The NGN Rate object to insert.</param>
        /// <param name="GatewayID">The Gateway's ID to update the Rate object from it's NGN Rates table.</param>
        /// <returns>Update boolean</returns>
        public bool Update(RateForNGN ngnRateObject, int GatewayID)
        {
            try
            {
                var tableName = GetTableNameByGatewayID(GatewayID);

                return base.Update(ngnRateObject, tableName, GLOBALS.DataSource.Type.DBTable);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Delete a RateForNGN object from a Gateway's NGN Rates table.
        /// </summary>
        /// <param name="ngnRateObject">The NGN Rate object to insert.</param>
        /// <param name="GatewayID">The Gateway's ID to delete the Rate object from it's NGN Rates table.</param>
        /// <returns>Delete boolean</returns>
        public bool Delete(RateForNGN ngnRateObject, int GatewayID)
        {
            try
            {
                var tableName = GetTableNameByGatewayID(GatewayID);

                return base.Delete(ngnRateObject, tableName, GLOBALS.DataSource.Type.DBTable);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }



        /***
         * Disable the default parent functions
         */
        public override int Insert(RateForNGN dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }


        public override bool Update(RateForNGN dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }


        public override bool Delete(RateForNGN dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default)
        {
            throw new NotImplementedException();
        }


        public override RateForNGN GetById(long id, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            throw new NotImplementedException();
        }


        public override IEnumerable<RateForNGN> Get(Dictionary<string, object> whereConditions, int limit = 25, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            throw new NotImplementedException();
        }


        public override IEnumerable<RateForNGN> Get(System.Linq.Expressions.Expression<Func<RateForNGN, bool>> predicate, string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            throw new NotImplementedException();
        }


        public override IEnumerable<RateForNGN> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSource = GLOBALS.DataSource.Type.Default, bool IncludeDataRelations = true)
        {
            throw new NotImplementedException();
        }


        public override IEnumerable<RateForNGN> GetAll(string sql)
        {
            throw new NotImplementedException();
        }
    }

}
