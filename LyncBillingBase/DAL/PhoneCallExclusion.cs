 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DAL
{
    [DataSource(DataSourceName = "ExceptionsList", DataSource = Enums.DataSources.DBTable)]
    public class PhoneCallExclusion
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
