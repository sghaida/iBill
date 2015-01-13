using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

<<<<<<< HEAD
using ORM;
using ORM.DataAccess;
using ORM.Helpers;
=======
using CCC.ORM;
using CCC.ORM.Helpers;
using CCC.ORM.DataAccess;
>>>>>>> 4d2825ed2d6c07fa47ef8a534e938e39e0b8f09c
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class GatewaysInfoDataMapper : DataAccess<GatewayInfo>
    {
        private static List<GatewayInfo> _GatewaysInfo = new List<GatewayInfo>();

        private void LoadGatewaysInfo()
        {
            if(_GatewaysInfo == null || _GatewaysInfo.Count == 0)
            {
                _GatewaysInfo = _GatewaysInfo.GetWithRelations(
                    item => item.Gateway, 
                    item => item.GatewayRatesInfo,
                    item => item.Site, 
                    item => item.Pool)
                .ToList();
            }
        }


        public GatewaysInfoDataMapper()
        {
            LoadGatewaysInfo();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="GatewayID"></param>
        /// <returns></returns>
        public List<GatewayInfo> GetByGatewayID(int GatewayID)
        {
            try
            {
                return _GatewaysInfo.Where(item => item.GatewayID == GatewayID).ToList();
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
        public List<Gateway> GetGatewaysBySiteID(int SiteID)
        {
            List<Gateway> gateways = null;
            List<GatewayInfo> gatewaysInfo = null;

            try
            {
                gatewaysInfo = _GatewaysInfo.Where(item => item.SiteID == SiteID).ToList();

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
        public List<Site> GetSitesByGatewayID(int GatewayID)
        {
            List<Site> sites = null;
            List<GatewayInfo> gatewaysInfo = null;

            try
            {
                gatewaysInfo = _GatewaysInfo.Where(item => item.GatewayID == GatewayID).ToList();

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


        public override IEnumerable<GatewayInfo> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _GatewaysInfo;
        }


        public override int Insert(GatewayInfo dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            bool isContained = _GatewaysInfo.Contains(dataObject);
            bool itExists = _GatewaysInfo.Exists(item => item.GatewayID == dataObject.GatewayID && item.SiteID == dataObject.SiteID);

            if (isContained || itExists)
            {
                return -1;
            }
            else
            {
                int rowID = base.Insert(dataObject, dataSourceName, dataSourceType);
                
                if(rowID > 0)
                {
                    dataObject = dataObject.GetWithRelations(
                        item => item.Gateway,
                        item => item.GatewayRatesInfo,
                        item => item.Site,
                        item => item.Pool);
                }

                _GatewaysInfo.Add(dataObject);

                return rowID;
            }
        }


        public override bool Update(GatewayInfo dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var gatewayInfo = _GatewaysInfo.Find(item => item.GatewayID == dataObject.GatewayID && item.SiteID == dataObject.SiteID);

            if (gatewayInfo != null)
            {
                bool status = base.Update(dataObject, dataSourceName, dataSourceType);

                if(status == true)
                {
                    _GatewaysInfo.Remove(gatewayInfo);

                    dataObject = dataObject.GetWithRelations(
                            item => item.Gateway,
                            item => item.GatewayRatesInfo,
                            item => item.Site,
                            item => item.Pool);

                    _GatewaysInfo.Add(dataObject);
                }

                return status;
            }
            else
            {
                return false;
            }
        }


        public override bool Delete(GatewayInfo dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var gatewayInfo = _GatewaysInfo.Find(item => item.GatewayID == dataObject.GatewayID && item.SiteID == dataObject.SiteID && item.PoolID == dataObject.PoolID);

            if (gatewayInfo != null)
            {
                _GatewaysInfo.Remove(gatewayInfo);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }

    }

}
