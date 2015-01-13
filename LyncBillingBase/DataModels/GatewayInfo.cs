using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "GatewaysDetails", Type = Globals.DataSource.Type.DBTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class GatewayInfo : DataModel
    {
        [IsIdField]
        [AllowIdInsert]
        [DbColumn("GatewayID")]
        public int GatewayId { set; get; }

        [DbColumn("SiteID")]
        public int SiteId { set; get; }

        [DbColumn("PoolID")]
        public int PoolId { set; get; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { set; get; }

        //
        // Relations
        [DataRelation(WithDataModel = typeof (Gateway), OnDataModelKey = "ID", ThisKey = "GatewayID")]
        public Gateway Gateway { get; set; }

        [DataRelation(WithDataModel = typeof (GatewayRate), OnDataModelKey = "GatewayID", ThisKey = "GatewayID",
            RelationType = Globals.DataRelation.Type.UNION)]
        public GatewayRate GatewayRatesInfo { get; set; }

        [DataRelation(WithDataModel = typeof (Site), OnDataModelKey = "ID", ThisKey = "SiteID")]
        public Site Site { get; set; }

        [DataRelation(WithDataModel = typeof (Pool), OnDataModelKey = "ID", ThisKey = "PoolID")]
        public Pool Pool { get; set; }
    }
}