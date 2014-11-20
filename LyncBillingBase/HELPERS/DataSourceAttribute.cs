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
        private Enums.DataSources _DataSource;
        private string _DataSourceName;

        public Enums.DataSources DataSource
        {
            get { return this._DataSource; }
            set { this._DataSource = value;  }
        }

        public string DataSourceName
        {
            get { return this._DataSourceName; }
            set { this._DataSourceName = value; }
        }

        public DataSourceAttribute() { }

    }

   
}
