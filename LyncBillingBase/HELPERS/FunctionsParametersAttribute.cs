using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Helpers
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    class FunctionsParametersAttribute : Attribute
    {
        private string _ParamerterName;
        private int _position;

        public string ParamerterName 
        {
            get { return this._ParamerterName; }
            set { this._ParamerterName = value; }
        }
        public int position 
        {
            get { return this._position; }
            set { this._position = value; }
        }


    }
}
