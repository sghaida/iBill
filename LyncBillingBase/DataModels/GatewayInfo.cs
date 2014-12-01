using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "GatewaysDetails", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class GatewayInfo : DataModel
    {
        [IsIDField]
        [AllowIDInsert]
        [DbColumn("GatewayID")]
        public int GatewayID { set; get; }

        [DbColumn("SiteID")]
        public int SiteID { set; get; }

        [DbColumn("PoolID")]
        public int PoolID { set; get; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { set; get; }


        //
        // Relations
        [DataRelation(Name = "GatewayID_Gateway.ID", WithDataModel = typeof(Gateway), OnDataModelKey = "ID", ThisKey = "GatewayID")]
        public Gateway GatewayData { get; set; }

        [DataRelation(Name = "SiteID_Site.ID", WithDataModel = typeof(Site), OnDataModelKey = "ID", ThisKey = "SiteID")]
        public Site GatewaySite { get; set; }

        [DataRelation(Name = "PoolID_Pool.ID", WithDataModel = typeof(Pool), OnDataModelKey = "ID", ThisKey = "PoolID")]
        public Pool GatewayPool { get; set; }
    }
}
