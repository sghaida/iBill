using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DAL
{
    [Serializable]
    [DataSource(Name = "Announcements", SourceType = Enums.DataSourceType.DBTable)]
    public class Announcement
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
    }
}
