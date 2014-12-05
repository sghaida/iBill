 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "ExceptionsList", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class PhoneCallExclusion : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public long ID { set; get; }

        [DbColumn("Entity")]
        public string ExclusionSubject { set; get; }

        [DbColumn("EntityType")]
        public string ExclusionType { set; get; }

        [DbColumn("SiteID")]
        public int SiteID { set; get; }

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
        [DataRelation(WithDataModel = typeof(Site), OnDataModelKey = "ID", ThisKey = "SiteID")]
        public Site Site { get; set; }

        [DataRelation(WithDataModel = typeof(User), OnDataModelKey = "SipAccount", ThisKey = "ExclusionSubject", RelationType = GLOBALS.DataRelation.Type.UNION)]
        public User User { get; set; }
    }
}
