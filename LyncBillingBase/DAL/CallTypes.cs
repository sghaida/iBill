using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DAL
{
    [TableName("CallTypes")]
    public class CallTypes
    {
        [IsIDField]
        [AllowIDInsert]
        [DbColumn("id")]
        public int ID { get; set; }

        [DbColumn("CallType")]
        public string CallType { get; set; }
    }
}
