using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.DataAttributes
{
    /// <summary>
    /// This attribute tells the Repository that it's associated property (DbColumn) can be excluded on Select, Insert, or Update or on all of them.
    /// </summary>

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ExcludeAttribute : Attribute
    {
        public bool OnSelect { get; set; }
        public bool OnInsert { get; set; }
        public bool OnUpdate { get; set; }

        public ExcludeAttribute() { }
    }
}
