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
    public class GatewaysDataMapper : DataAccess<GatewayInfo>
    {
        /// <summary>
        /// Returns all the Gateways
        /// </summary>
        /// <returns>List of Gateway objects.</returns>
        public List<Gateway> GetAllGateways()
        {
            List<Gateway> gateways = null;
            List<GatewayInfo> gatewaysInfo = null;

            try
            {
                gatewaysInfo = GetAll().ToList<GatewayInfo>();

                if (gatewaysInfo != null && gatewaysInfo.Count > 0)
                {
                    gateways = gatewaysInfo.Select<GatewayInfo, Gateway>(item => item.Gateway).ToList<Gateway>();
                }

                return gateways;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Site's ID, return the list of Gateways mapped to it.
        /// </summary>
        /// <param name="SiteID">Site.ID (int)</param>
        /// <returns>List of Gateway objects.</returns>
        public List<Gateway> GetGatewaysForSite(int SiteID)
        {
            List<Gateway> gateways = null;
            List<GatewayInfo> gatewaysInfo = null;

            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("SiteID", SiteID);

            try
            {
                gatewaysInfo = Get(whereConditions: conditions, limit: 0).ToList<GatewayInfo>();

                if(gatewaysInfo != null && gatewaysInfo.Count > 0)
                { 
                    gateways = gatewaysInfo.Select<GatewayInfo, Gateway>(item => item.Gateway).ToList<Gateway>();
                }

                return gateways;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a Gateway's ID, return the list of Sites it is associated with.
        /// </summary>
        /// <param name="GatewayID">Gateway.ID (int), GatewayInfo.GatewayID (int)</param>
        /// <returns></returns>
        public List<Site> GetSitesOfGateway(int GatewayID)
        {
            List<Site> sites = null;
            List<GatewayInfo> gatewaysInfo = null;

            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("GatewayID", GatewayID);

            try
            {
                gatewaysInfo = Get(whereConditions: conditions, limit: 0).ToList<GatewayInfo>();

                if (gatewaysInfo != null && gatewaysInfo.Count > 0)
                {
                    sites = gatewaysInfo.Select<GatewayInfo, Site>(item => item.Site).ToList<Site>();
                }

                return sites;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
