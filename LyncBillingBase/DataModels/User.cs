using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "ActiveDirectoryUsers", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class User : DataModel
    {
        [IsIDField]
        [AllowIDInsert]
        [DbColumn("AD_UserID")]
        public int EmployeeID { get; set; }

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
        public bool UpdatedByAD { get; set; }

        [DbColumn("NotifyUser")]
        public bool NotifyUser { get; set; }

        [AllowNull]
        [DbColumn("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

        [DbColumn("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        
        public string FullName { get; set; }


        //
        // Relations
        [DataRelation(Name = "SiteName_Site.Name", WithDataModel = typeof(Site), OnDataModelKey = "Name", ThisKey = "SiteName", RelationType = Enums.DataRelationType.UNION)]
        public Site Site { get; set; }

        [DataRelation(Name = "DepartmentName_Departemnt.Name", WithDataModel = typeof(Department), OnDataModelKey = "Name", ThisKey = "DepartmentName", RelationType = Enums.DataRelationType.UNION)]
        public Department Department { get; set; }
    }
}
