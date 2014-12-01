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
        public List<Gateway> GetGatewaysForSite(int siteID)
        {
            List<GatewayInfo> gatewaysInfo = new List<GatewayInfo>();

            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("SiteID", siteID);

            try
            {
                gatewaysInfo = Get(whereConditions: conditions, limit: 0).ToList<GatewayInfo>();

                var gateways = gatewaysInfo.Select<GatewayInfo, Gateway>(item => item.GatewayData).ToList<Gateway>();

                return gateways;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
