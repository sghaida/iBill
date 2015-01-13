using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class GatewaysInfoDataMapper : DataAccess<GatewayInfo>
    {
        private static List<GatewayInfo> _gatewaysInfo = new List<GatewayInfo>();

        public GatewaysInfoDataMapper()
        {
            LoadGatewaysInfo();
        }

        private void LoadGatewaysInfo()
        {
            if (_gatewaysInfo == null || _gatewaysInfo.Count == 0)
            {
                _gatewaysInfo = _gatewaysInfo.GetWithRelations(
                    item => item.Gateway,
                    item => item.GatewayRatesInfo,
                    item => item.Site,
                    item => item.Pool)
                    .ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <returns></returns>
        public List<GatewayInfo> GetByGatewayId(int gatewayId)
        {
            try
            {
                return _gatewaysInfo.Where(item => item.GatewayId == gatewayId).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Site's ID, return the list of Gateways mapped to it.
        /// </summary>
        /// <param name="siteId">Site.ID (int)</param>
        /// <returns>List of Gateway objects.</returns>
        public List<Gateway> GetGatewaysBySiteId(int siteId)
        {
            List<Gateway> gateways = null;
            List<GatewayInfo> gatewaysInfo = null;

            try
            {
                gatewaysInfo = _gatewaysInfo.Where(item => item.SiteId == siteId).ToList();

                if (gatewaysInfo != null && gatewaysInfo.Count > 0)
                {
                    gateways = gatewaysInfo.Select(item => item.Gateway).ToList();
                }

                return gateways;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Gateway's ID, return the list of Sites it is associated with.
        /// </summary>
        /// <param name="gatewayId">Gateway.ID (int), GatewayInfo.GatewayID (int)</param>
        /// <returns></returns>
        public List<Site> GetSitesByGatewayId(int gatewayId)
        {
            List<Site> sites = null;
            List<GatewayInfo> gatewaysInfo = null;

            try
            {
                gatewaysInfo = _gatewaysInfo.Where(item => item.GatewayId == gatewayId).ToList();

                if (gatewaysInfo != null && gatewaysInfo.Count > 0)
                {
                    sites = gatewaysInfo.Select(item => item.Site).ToList();
                }

                return sites;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<GatewayInfo> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _gatewaysInfo;
        }

        public override int Insert(GatewayInfo dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _gatewaysInfo.Contains(dataObject);
            var itExists =
                _gatewaysInfo.Exists(item => item.GatewayId == dataObject.GatewayId && item.SiteId == dataObject.SiteId);

            if (isContained || itExists)
            {
                return -1;
            }
            var rowId = base.Insert(dataObject, dataSourceName, dataSourceType);

            if (rowId > 0)
            {
                dataObject = dataObject.GetWithRelations(
                    item => item.Gateway,
                    item => item.GatewayRatesInfo,
                    item => item.Site,
                    item => item.Pool);
            }

            _gatewaysInfo.Add(dataObject);

            return rowId;
        }

        public override bool Update(GatewayInfo dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var gatewayInfo =
                _gatewaysInfo.Find(item => item.GatewayId == dataObject.GatewayId && item.SiteId == dataObject.SiteId);

            if (gatewayInfo != null)
            {
                var status = base.Update(dataObject, dataSourceName, dataSourceType);

                if (status)
                {
                    _gatewaysInfo.Remove(gatewayInfo);

                    dataObject = dataObject.GetWithRelations(
                        item => item.Gateway,
                        item => item.GatewayRatesInfo,
                        item => item.Site,
                        item => item.Pool);

                    _gatewaysInfo.Add(dataObject);
                }

                return status;
            }
            return false;
        }

        public override bool Delete(GatewayInfo dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var gatewayInfo =
                _gatewaysInfo.Find(
                    item =>
                        item.GatewayId == dataObject.GatewayId && item.SiteId == dataObject.SiteId &&
                        item.PoolId == dataObject.PoolId);

            if (gatewayInfo != null)
            {
                _gatewaysInfo.Remove(gatewayInfo);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}