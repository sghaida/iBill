using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;


namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Roles_DepartmentsHeads", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
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
        [DataRelation(WithDataModel = typeof(User), OnDataModelKey = "SipAccount", ThisKey = "SipAccount")]
        public User User { get; set; }

        [DataRelation(WithDataModel = typeof(SiteDepartment), OnDataModelKey = "ID", ThisKey = "SiteDepartmentID")]
        public SiteDepartment SiteDepartment { get; set; }
    }
}
