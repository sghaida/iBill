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
    [DataSource(Name = "Roles_System", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class DepartmentHeadRole : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("SiteDepartmentID")]
        public int SiteDepartmentID { get; set; }


        //
        // Relations
        [DataRelation(Name = "SiteDepartmentID_SiteDepartment.ID", WithDataModel = typeof(SiteDepartment), OnDataModelKey = "ID", ThisKey = "SiteDepartmentID")]
        public SiteDepartment EffectiveSiteDepartment { get; set; }
    }
}
