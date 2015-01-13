using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class GatewaysRatesDataMapper : DataAccess<GatewayRate>
    {
        //This will always be filled and return to whomever he wants to consume
        private static List<GatewayRate> _gatewaysRates = new List<GatewayRate>();

        public GatewaysRatesDataMapper()
        {
            LoadGatewayRates();
        }

        private void LoadGatewayRates()
        {
            if (_gatewaysRates == null || _gatewaysRates.Count == 0)
            {
                _gatewaysRates = base.GetAll().GetWithRelations(item => item.Gateway).ToList();
            }
        }

        /// <summary>
        ///     Given a Gateway's name, and a DateTime object, construct a Rates TableName for it.
        /// </summary>
        /// <param name="gatewayName">Gateway.Name</param>
        /// <param name="startingDate">DateTime object to represent the starting date of this rates table.</param>
        /// <returns>New Rates Table name</returns>
        public string ConstructRatesTableName(string gatewayName, DateTime startingDate)
        {
            var ratesTableName = new StringBuilder();

            try
            {
                ratesTableName.Append("Rates_" + gatewayName + "_" + startingDate.Date.ToString("yyyymmdd"));

                return ratesTableName.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Given a Gateway.ID, return all of it's GatewayRates records.
        /// </summary>
        /// <param name="gatewayId">Gateway.ID</param>
        /// <returns>List of GatewayRate objects.</returns>
        public List<GatewayRate> GetByGatewayId(int gatewayId)
        {
            return _gatewaysRates.Where(item => item.GatewayId == gatewayId).ToList();
        }

        public override IEnumerable<GatewayRate> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _gatewaysRates;
        }

        public override int Insert(GatewayRate dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _gatewaysRates.Contains(dataObject);
            var itExists = _gatewaysRates.Exists(
                item =>
                    (
                        (item.GatewayId == dataObject.GatewayId && item.RatesTableName == dataObject.RatesTableName &&
                         item.StartingDate == dataObject.StartingDate) &&
                        (item.EndingDate == DateTime.MinValue || item.EndingDate == dataObject.EndingDate)
                        ) ||
                    (item.GatewayId == dataObject.GatewayId && item.NgnRatesTableName == dataObject.NgnRatesTableName)
                );


            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);
            _gatewaysRates.Add(dataObject);

            return dataObject.Id;
        }

        public override bool Update(GatewayRate dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var gatewayRate = _gatewaysRates.Find(item => item.Id == dataObject.Id);

            if (gatewayRate != null)
            {
                _gatewaysRates.Add(gatewayRate);
                _gatewaysRates.Remove(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(GatewayRate dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var gatewayRate = _gatewaysRates.Find(item => item.Id == dataObject.Id);

            if (gatewayRate != null)
            {
                _gatewaysRates.Add(gatewayRate);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}