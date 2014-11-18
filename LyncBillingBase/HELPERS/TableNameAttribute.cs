using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Helpers
{
    /// <summary>
    /// This attribute is designed to tell the repository that the class or struct which is decorated with it is resembles a Database Table and sets it's name.
    /// </summary>
    
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class TableNameAttribute : System.Attribute
    {
        public string Name { get; private set; }

        public TableNameAttribute(string name) 
        {
            this.Name = name;
        }

    }
}
