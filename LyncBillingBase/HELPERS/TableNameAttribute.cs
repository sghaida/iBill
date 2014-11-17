using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.HELPERS
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    class TableNameAttribute : System.Attribute
    {
        public TableNameAttribute(String Descrition)
        {
            this.description = Description;
        }
        
        protected String description;
        
        public String Description 
        {
            get 
            {
                return this.description;
                 
            }            
        }    
    }
}
