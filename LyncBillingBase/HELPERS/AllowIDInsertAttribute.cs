using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Helpers
{
    /// <summary>
    /// This attribute tells the Repository that it's associated property is most probably a Table ID Field that is allowed to be changed and inserted into the corresponding database table.
    /// </summary>
    
    [System.AttributeUsage(System.AttributeTargets.Property)]
    class AllowIDInsertAttribute : Attribute
    {
        public bool Status { private set; public get; }

        public AllowIDInsertAttribute(bool status = true)
        {
            this.Status = status;
        }
    }
}
