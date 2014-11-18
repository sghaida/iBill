using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Helpers
{
    /// <summary>
    /// This attribute tells the Repository that it's associated property resembles a Database Table ID Column.
    /// </summary>
    
    class IsIDFieldAttribute : Attribute
    {
        public bool Status { public get; private set; }

        public IsIDFieldAttribute(bool status = true) 
        {
            this.Status = status;
        }
    }
}
