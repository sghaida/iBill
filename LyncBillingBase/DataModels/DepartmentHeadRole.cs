using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Roles_DepartmentsHeads", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class DepartmentHeadRole : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("SiteDepartmentID")]
        public int SiteDepartmentID { get; set; }

        [DbColumn("SipAccount")]
        public string SipAccount { get; set; }

        //
        // Relations
        [DataRelation(Name = "SipAccount_User.SipAccount", WithDataModel = typeof(User), OnDataModelKey = "SipAccount", ThisKey = "SipAccount")]
        public User User { get; set; }

        [DataRelation(Name = "SiteDepartmentID_SiteDepartment.ID", WithDataModel = typeof(SiteDepartment), OnDataModelKey = "ID", ThisKey = "SiteDepartmentID")]
        public SiteDepartment SiteDepartment { get; set; }
    }
}
