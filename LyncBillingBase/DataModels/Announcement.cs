using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [Serializable]
    [DataSource(Name = "Announcements", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class Announcement : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("Announcement")]
        public string AnnouncementBody { get; set; }

        [DbColumn("PublishOn")]
        public DateTime PublishOn { get; set; }

        [AllowNull]
        [DbColumn("ForRole")]
        public int ForRole { get; set; }

        [AllowNull]
        [DbColumn("ForSite")]
        public int ForSite { get; set; }


        //
        // Relations
        [DataRelation(Name = "ForRole_Role.RoleID", WithDataModel = typeof(Role), OnDataModelKey = "RoleID", ThisKey = "ForRole")]
        public Role Role { get; set; }

        [DataRelation(Name = "ForSite_Site.ID", WithDataModel = typeof(Site), OnDataModelKey = "ID", ThisKey = "ForSite")]
        public Site Site { get; set; }
    }
}
