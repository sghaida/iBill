 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "ExceptionsList", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class PhoneCallExclusion : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public long ID { set; get; }

        [DbColumn("Entity")]
        public string Entity { set; get; }

        [DbColumn("EntityType")]
        public char EntityType { set; get; }

        [DbColumn("SiteID")]
        public int SiteID { set; get; }

        [DbColumn("ZeroCost")]
        public char ZeroCost { set; get; }

        [AllowNull]
        [DbColumn("AutoMark")]
        public char AutoMark { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { set; get; }
    }
}
