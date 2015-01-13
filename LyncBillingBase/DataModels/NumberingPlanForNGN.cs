using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NGN_NumberingPlan", Type = Globals.DataSource.Type.DBTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class NumberingPlanForNgn : DataModel
    {
        [IsIdField]
        [DbColumn("ID")]
        public long Id { get; set; }

        [DbColumn("DialingCode")]
        public string DialingCode { get; set; }

        [DbColumn("CountryCodeISO3")]
        public string Iso3CountryCode { get; set; }

        [AllowNull]
        [DbColumn("Provider")]
        public string Provider { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { get; set; }

        [AllowNull]
        [DbColumn("TypeOfServiceID")]
        public int TypeOfServiceId { get; set; }

        //
        // Relations
        [DataRelation(WithDataModel = typeof (CallType), OnDataModelKey = "TypeID", ThisKey = "TypeOfServiceID")]
        public CallType TypeOfService { get; set; }

        [DataRelation(WithDataModel = typeof (Country), OnDataModelKey = "ISO3Code", ThisKey = "ISO3CountryCode")]
        public Country Country { get; set; }
    }
}