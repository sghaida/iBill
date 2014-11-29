using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "CallTypes", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class CallType : DataModel
    {
        [IsIDField]
        [DbColumn("id")]
        public int ID { get; set; }

        [DbColumn("CallType")]
        public string Name { get; set; }
    }
}
