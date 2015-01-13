using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NGN_NumberingPlan", Type = GLOBALS.DataSource.Type.DBTable,
        AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class NumberingPlanForNGN : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public long ID { get; set; }

        [DbColumn("DialingCode")]
        public string DialingCode { get; set; }

        [DbColumn("CountryCodeISO3")]
        public string ISO3CountryCode { get; set; }

        [AllowNull]
        [DbColumn("Provider")]
        public string Provider { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { get; set; }

        [AllowNull]
        [DbColumn("TypeOfServiceID")]
        public int TypeOfServiceID { get; set; }

        //
        // Relations
        [DataRelation(WithDataModel = typeof (CallType), OnDataModelKey = "TypeID", ThisKey = "TypeOfServiceID")]
        public CallType TypeOfService { get; set; }

        [DataRelation(WithDataModel = typeof (Country), OnDataModelKey = "ISO3Code", ThisKey = "ISO3CountryCode")]
        public Country Country { get; set; }
    }
}