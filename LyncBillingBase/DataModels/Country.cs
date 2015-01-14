using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NEW_Countries", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class Country : DataModel
    {
        [IsIdField]
        [DbColumn("ID")]
        public int Id { get; set; }

        [DbColumn("Name")]
        public string Name { get; set; }

        [DbColumn("ISO2Code")]
        public string Iso2Code { get; set; }

        [DbColumn("ISO3Code")]
        public string Iso3Code { get; set; }

        [DbColumn("CurrencyID")]
        public int CurrencyId { get; set; }

        //
        // Relations
        [DataRelation( WithDataModel = typeof( Currency ) , OnDataModelKey = "Id" , ThisKey = "CurrencyId" )]
        public Currency Currency { get; set; }
    }
}