using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataAttributes
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    class FunctionsParametersAttribute : Attribute
    {
        private string _ParamerterName;
        private int _Position;

        public string ParamerterName 
        {
            get { return this._ParamerterName; }
            set { this._ParamerterName = value; }
        }
        public int Position 
        {
            get { return this._Position; }
            set { this._Position = value; }
        }


    }
}
