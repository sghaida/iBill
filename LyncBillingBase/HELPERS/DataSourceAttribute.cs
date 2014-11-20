using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase;


namespace LyncBillingBase.Helpers
{
    /// <summary>
    /// This attribute is designed to tell the repository that the class or struct which is decorated with it is resembles a Database Table and sets it's name.
    /// </summary>
    
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class DataSourceAttribute : System.Attribute
    {
        private Enums.DataSources _SourceType;
        private string _Name;

        public Enums.DataSources SourceType
        {
            get { return this._SourceType; }
            set { this._SourceType = value;  }
        }

        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }

        public DataSourceAttribute() { }

    }

   
}
