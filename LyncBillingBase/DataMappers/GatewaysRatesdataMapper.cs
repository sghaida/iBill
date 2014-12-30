using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;


using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DataMappers
{
    public class GatewaysRatesDataMapper : DataAccess<GatewayRate>
    {
        //This will always be filled and return to whomever he wants to consume
        private static IEnumerable<GatewayRate> _GatewaysRates = new List<GatewayRate>();

        /// <summary>
        /// Given a Gateway.ID, return all of it's GatewayRates records.
        /// </summary>
        /// <param name="GatewayID">Gateway.ID</param>
        /// <returns>List of GatewayRate objects.</returns>
        public List<GatewayRate> GetByGatewayID(int GatewayID)
        {
            LoadGatewayRates();

            return _GatewaysRates.Where(item=>item.GatewayID == GatewayID).ToList();

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

        public override IEnumerable<GatewayRate> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            LoadGatewayRates();
            
            return _GatewaysRates;
            
        }

        private void LoadGatewayRates()
        {
             if (_GatewaysRates == null || _GatewaysRates.Count() == 0) 
            {
                _GatewaysRates = base.GetAll().IncludeM(item => item.Gateway);
            }
        }

    }

}
