using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DA
{
    public class DbTableField
    {
        public string ColumnName { get; set; }
        public bool IsIDField { get; set; }
        public bool AllowNull { get; set; }
        public bool AllowIDInsert { get; set; }
        public Type FieldType { get; set; }
    }
}
