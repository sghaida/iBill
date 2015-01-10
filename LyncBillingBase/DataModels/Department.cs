using LyncBillingBase.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DALDotNet;
using DALDotNet.DataAccess;
using DALDotNet.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "Departments", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class Department : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("DepartmentName")]
        public string Name { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { get; set; }
    }
}
