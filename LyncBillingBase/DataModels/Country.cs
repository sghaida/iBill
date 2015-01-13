using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;






using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;


namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NEW_Countries", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class Country : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("Name")]
        public string Name { get; set; }

        [DbColumn("ISO2Code")]
        public string ISO2Code { get; set; }

        [DbColumn("ISO3Code")]
        public string ISO3Code { get; set; }

        [DbColumn("CurrencyID")]
        public int CurrencyID { get; set; }


        //
        // Relations
        [DataRelation(WithDataModel = typeof(Currency), OnDataModelKey = "ID", ThisKey = "CurrencyID")]
        public Currency Currency { get; set; }
    }
}
