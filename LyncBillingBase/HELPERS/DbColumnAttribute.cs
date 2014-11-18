using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Helpers
{
    /// <summary>
    /// This attribute tells the Repository that it's associated property resembles a Database Column and with a specific name.
    /// </summary>
    
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class DbColumnAttribute : Attribute
    {
        public string Name { private set; public get; }

        public DbColumnAttribute(string name)
        {
            this.Name = name;
        }
    }
}
