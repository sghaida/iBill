using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataAttributes
{
    /// <summary>
    /// This attribute tells the Repository that it's associated property resembles a Database Table Key.
    /// </summary>

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class IsKeyAttribute : Attribute
    {
        public bool Status { get; private set; }

        public IsKeyAttribute(bool status = true) 
        {
            this.Status = status;
        }
    }
}
