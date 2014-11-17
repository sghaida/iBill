using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Helpers
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    class TableNameAttribute : System.Attribute
    {
        public string Description { private set; get; }

        public TableNameAttribute(string Description) 
        {
            this.Description = Description;
        }

    }
}
