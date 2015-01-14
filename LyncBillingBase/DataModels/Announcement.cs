using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Announcements", Type = Globals.DataSource.Type.DBTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class Announcement : DataModel
    {
        [IsIdField]
        [DbColumn("ID")]
        public int Id { get; set; }

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
        [DataRelation( WithDataModel = typeof( Role ) , OnDataModelKey = "RoleId" , ThisKey = "ForRole" )]
        public Role Role { get; set; }

        [DataRelation( WithDataModel = typeof( Site ) , OnDataModelKey = "Id" , ThisKey = "ForSite" )]
        public Site Site { get; set; }
    }
}