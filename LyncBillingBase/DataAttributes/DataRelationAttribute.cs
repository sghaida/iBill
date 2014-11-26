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
        /// <summary>
        /// Relation Descriptive Name
        /// </summary>
        private string _name = string.Empty;
        public string Name
        {
            set { this._name = value; }
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    //Sample: CountryID_Country.ID
                    this._name = String.Format("{0}.{1}_{2}", WithDataModel.Name, OnDataModelKey, ThisKey);
                }

                return _name;
            }
        }

        /// <summary>
        /// The data model type this relation is associated with
        /// </summary>
        public Type WithDataModel { get; set; }

        /// <summary>
        /// The data modle key this relation is defined on
        /// </summary>
        public string OnDataModelKey { get; set; }

        /// <summary>
        /// The class instance field name that shares the relation with the destination data model
        /// </summary>
        public string ThisKey { get; set; }
    }
}
