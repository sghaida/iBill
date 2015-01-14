using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "DIDs", Type = Globals.DataSource.Type.DBTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class Did : DataModel
    {
        [IsIdField]
        [DbColumn("ID")]
        public int Id { get; set; }

        [DbColumn("Regex")]
        public string Regex { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { get; set; }

        [AllowNull]
        [DbColumn("SiteID")]
        public int SiteId { get; set; }

        //
        // Relations
        [DataRelation(WithDataModel = typeof (Site), OnDataModelKey = "Id", ThisKey = "SiteId")]
        public Site Site { get; set; }
    }
}