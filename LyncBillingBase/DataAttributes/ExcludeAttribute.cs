using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataAttributes
{
    /// <summary>
    /// This attribute tells the Repository that it's associated property resembles a Database Table Foreign Key.
    /// </summary>

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ExcludeAttribute : Attribute
    {
        public bool OnSelect { get; private set; }
        public bool OnInsert { get; private set; }
        public bool OnUpdate { get; private set; }
    }
}
