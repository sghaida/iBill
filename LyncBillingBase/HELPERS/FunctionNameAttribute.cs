using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Helpers
{

    /// <summary>
    /// This attribute is designed to tell the repository that the class or struct which is decorated with it is resembles a Database function and sets it's name.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    class FunctionNameAttribute :Attribute
    {
        public string Name { get; private set; }

        public FunctionNameAttribute(string name) 
        {
            this.Name = name;
        }
    }
}
