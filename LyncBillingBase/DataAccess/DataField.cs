using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataAccess
{
    public class DataField
    {
        public DbTableField TableField { get; set; }
        public DbRelation Relation { get; set; }
    }

    public class DbTableField
    {
        public string ColumnName { get; set; }
        public bool IsIDField { get; set; }
        public bool AllowNull { get; set; }
        public bool AllowIDInsert { get; set; }
        public Type FieldType { get; set; }
    }

    public class DbRelation
    {
        /// <summary>
        /// The object that will hold the data returned from the relation query
        /// </summary>
        public string DataField { get; set; }

        /// <summary>
        /// The descriptive relation name
        /// </summary>
        public string RelationName { get; set; }

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
