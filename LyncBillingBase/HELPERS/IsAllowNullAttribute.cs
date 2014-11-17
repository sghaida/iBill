using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.HELPERS
{

    [System.AttributeUsage(System.AttributeTargets.Property)]
    class IsAllowNullAttribute : Attribute
    {

        public IsAllowNullAttribute(bool Value)
        {
            this.value = Value;
        }
        
        protected bool value;
        
        public bool Value 
        {
            get 
            {
                return this.value;
                 
            }            
        }    
    }
}
