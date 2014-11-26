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
    public class DataMapperAttribute : System.Attribute
    {
        public string RelationName { get; set; }
        public Type SourceDataModel { get; set; }
        public string SourceDataAttribute { get; set; }
    }
}
