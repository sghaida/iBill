using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataAttributes
{
    /// <summary>
    /// This attribute tells the Repository that it's associated property resembles a Data Relation of source_key -> destination_key.
    /// It is a data relation because the "DataSources" of the different data models might be different than each other, but he repository will manage to join the data together.
    /// The SourceDataModel is basically a class name that belongs in the DataModels namespace.
    /// </summary>
    
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class DataRelationAttribute : System.Attribute
    {
        private string _name = string.Empty;
        public string Name
        {
            set { this._name = value; }
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    //Sample: CountryID_Country.ID
                    this._name = String.Format("{0}.{1}_{3}", SourceDataModel.Name, SourceKeyName, LocalKeyName);
                }

                return _name;
            }
        }

        public Type SourceDataModel { get; set; }
        public string SourceKeyName { get; set; }
        public string LocalKeyName { get; set; }
        public string IncludeProperties { get; set; }
    }
}
