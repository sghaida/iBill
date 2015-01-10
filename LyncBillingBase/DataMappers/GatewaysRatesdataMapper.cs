using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using DALDotNet;
using DALDotNet.DataAccess;
using DALDotNet.Helpers;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class GatewaysRatesDataMapper : DataAccess<GatewayRate>
    {
        //This will always be filled and return to whomever he wants to consume
        private static List<GatewayRate> _GatewaysRates = new List<GatewayRate>();


        public GatewaysRatesDataMapper()
        {
            LoadGatewayRates();
        }


        private void LoadGatewayRates()
        {
            if (_GatewaysRates == null || _GatewaysRates.Count == 0)
            {
                _GatewaysRates = base.GetAll().GetWithRelations(item => item.Gateway).ToList();
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
        
        
        /// <summary>
        /// Given a Gateway.ID, return all of it's GatewayRates records.
        /// </summary>
        /// <param name="GatewayID">Gateway.ID</param>
        /// <returns>List of GatewayRate objects.</returns>
        public List<GatewayRate> GetByGatewayID(int GatewayID)
        {
            return _GatewaysRates.Where(item=>item.GatewayID == GatewayID).ToList();
        }


        public override IEnumerable<GatewayRate> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _GatewaysRates;
        }


        public override int Insert(GatewayRate dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            bool isContained = _GatewaysRates.Contains(dataObject);
            bool itExists = _GatewaysRates.Exists(
                item => 
                    (
                      (item.GatewayID == dataObject.GatewayID && item.RatesTableName == dataObject.RatesTableName && item.StartingDate == dataObject.StartingDate) &&
                      (item.EndingDate == DateTime.MinValue || item.EndingDate == dataObject.EndingDate)
                    ) ||
                    (item.GatewayID == dataObject.GatewayID && item.NgnRatesTableName == dataObject.NgnRatesTableName)
                );


            if(isContained || itExists)
            {
                return -1;
            }
            else
            {
                dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
                _GatewaysRates.Add(dataObject);

                return dataObject.ID;
            }
        }


        public override bool Update(GatewayRate dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var gatewayRate = _GatewaysRates.Find(item => item.ID == dataObject.ID);

            if(gatewayRate != null)
            {
                _GatewaysRates.Add(gatewayRate);
                _GatewaysRates.Remove(dataObject);
                
                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }


        public override bool Delete(GatewayRate dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var gatewayRate = _GatewaysRates.Find(item => item.ID == dataObject.ID);

            if(gatewayRate != null)
            {
                _GatewaysRates.Add(gatewayRate);
                
                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }

    }

}
