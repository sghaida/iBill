using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Helpers
{
    public class DbColumnAttribute : Attribute
    {
        string Name { get; private set; }

        public DbColumnAttribute(string name)
        {
            this.Name = name;
        }
    }
}
