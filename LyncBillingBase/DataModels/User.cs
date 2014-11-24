using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "ActiveDirectoryUsers", SourceType = Enums.DataSourceType.DBTable)]
    public class User
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
        public string Department { get; set; }

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

    }
}
