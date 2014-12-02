using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NEW_CallTypes", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class CallType : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("Name")]
        public string Name { get; set; }
    }
}
