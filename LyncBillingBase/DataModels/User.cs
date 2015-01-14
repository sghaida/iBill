using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "ActiveDirectoryUsers", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class User : DataModel
    {
        [IsIdField]
        [AllowIdInsert]
        [DbColumn("AD_UserID")]
        public int EmployeeId { get; set; }

        [DbColumn("AD_DisplayName")]
        public string DisplayName { get; set; }

        [DbColumn("SipAccount")]
        public string SipAccount { get; set; }

        [AllowNull]
        [DbColumn("AD_PhysicalDeliveryOfficeName")]
        public string SiteName { get; set; }

        [AllowNull]
        [DbColumn("AD_Department")]
        public string DepartmentName { get; set; }

        [AllowNull]
        [DbColumn("AD_TelephoneNumber")]
        public string TelephoneNumber { get; set; }

        [AllowNull]
        [DbColumn("UpdatedByAD")]
        public Byte UpdatedByAd { get; set; }

        [DbColumn("NotifyUser")]
        public string NotifyUser { get; set; }

        [AllowNull]
        [DbColumn("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

        [DbColumn("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        public string FullName { get; set; }
        //
        // Relations
        [DataRelation(WithDataModel = typeof (Site), OnDataModelKey = "Name", ThisKey = "SiteName",
            RelationType = Globals.DataRelation.Type.Union )]
        public Site Site { get; set; }

        [DataRelation(WithDataModel = typeof (Department), OnDataModelKey = "Name", ThisKey = "DepartmentName",
            RelationType = Globals.DataRelation.Type.Union )]
        public Department Department { get; set; }
    }
}