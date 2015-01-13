using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<< HEAD
using ORM;
using ORM.DataAccess;
using ORM.DataAttributes;
=======
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;
>>>>>>> 4d2825ed2d6c07fa47ef8a534e938e39e0b8f09c

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "ActiveDirectoryUsers", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
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
        public Byte UpdatedByAD { get; set; }

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
        [DataRelation(WithDataModel = typeof(Site), OnDataModelKey = "Name", ThisKey = "SiteName", RelationType = GLOBALS.DataRelation.Type.UNION)]
        public Site Site { get; set; }

        [DataRelation(WithDataModel = typeof(Department), OnDataModelKey = "Name", ThisKey = "DepartmentName", RelationType = GLOBALS.DataRelation.Type.UNION)]
        public Department Department { get; set; }
    }
}
