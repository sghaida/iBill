using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NEW_Countries", SourceType = Enums.DataSourceType.DBTable)]
    public class Country
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
        public string CurrencyID { get; set; }

        [DataRelation(Name = "Country.CunrrncyID_Currency.ID", WithDataModel = typeof(Currency), OnDataModelKey = "ID", ThisKey = "CurrencyID")]
        public Currency LocalCurrency { get; set; }
    }
}
