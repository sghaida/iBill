using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "ExceptionsList", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class PhoneCallExclusion : DataModel
    {
        [IsIdField]
        [DbColumn("ID")]
        public long Id { set; get; }

        [DbColumn("Entity")]
        public string ExclusionSubject { set; get; }

        [DbColumn("EntityType")]
        public string ExclusionType { set; get; }

        [DbColumn("SiteID")]
        public int SiteId { set; get; }

        [DbColumn("ZeroCost")]
        public string ZeroCost { set; get; }

        [AllowNull]
        [DbColumn("AutoMark")]
        public string AutoMark { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { set; get; }

        //
        // Relations
        [DataRelation(WithDataModel = typeof (Site), OnDataModelKey = "Id", ThisKey = "SiteId")]
        public Site Site { get; set; }

        [DataRelation(WithDataModel = typeof (User), OnDataModelKey = "SipAccount", ThisKey = "ExclusionSubject",
            RelationType = Globals.DataRelation.Type.Union)]
        public User User { get; set; }
    }
}