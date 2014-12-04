using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class GatewaysRatesDataMapper : DataAccess<GatewayRate>
    {
        /// <summary>
        /// Given a Gateway.ID, return all of it's GatewayRates records.
        /// </summary>
        /// <param name="GatewayID">Gateway.ID</param>
        /// <returns>List of GatewayRate objects.</returns>
        public List<GatewayRate> GetByGatewayID(int GatewayID)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("GatewayID", GatewayID);

            try
            {
                return Get(whereConditions: condition).ToList<GatewayRate>();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Gateway's name, and a DateTime object, construct a Rates TableName for it.
        /// </summary>
        /// <param name="GatewayName">Gateway.Name</param>
        /// <param name="StartingDate">DateTime object to represent the starting date of this rates table.</param>
        /// <returns>New Rates Table name</returns>
        public string ConstructRatesTableName(string GatewayName, DateTime StartingDate)
        {
            StringBuilder RatesTableName = new StringBuilder();

            try
            {
                RatesTableName.Append("Rates_" + GatewayName + "_" + StartingDate.Date.ToString("yyyymmdd"));

                return RatesTableName.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
