using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NGN_NumberingPlan", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
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
        [DataRelation(WithDataModel = typeof(CallType), OnDataModelKey = "ID", ThisKey = "TypeOfServiceID")]
        public CallType TypeOfService { get; set; }

        [DataRelation(WithDataModel = typeof(Country), OnDataModelKey = "ISO3Code", ThisKey = "ISO3CountryCode")]
        public Country Country { get; set; }

        //[DataRelation(Name="CountryID_Country.ID", WithDataModel = typeof(Country), OnDataModelKey = "ID", ThisKey = "CountryID")]
        //public Country Country { get; set; }
    }
}
