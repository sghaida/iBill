using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DALDotNet;
using DALDotNet.DataAccess;
using DALDotNet.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NumberingPlan", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class NumberingPlan : DataModel
    {
        [IsIDField]
        [AllowIDInsert]
        [DbColumn("Dialing_prefix")]
        public Int64 DialingPrefix { get; set; }

        [DbColumn("Country_Name")]
        public string CountryName { get; set; }

        [DbColumn("Two_Digits_country_code")]
        public string ISO2CountryCode { get; set; }

        [DbColumn("Three_Digits_Country_Code")]
        public string ISO3CountryCode { get; set; }

        [AllowNull]
        [DbColumn("City")]
        public string City { get; set; }

        [AllowNull]
        [DbColumn("Provider")]
        public string Provider { get; set; }

        [AllowNull]
        [DbColumn("Type_Of_Service")]
        public string TypeOfService { get; set; }


        //
        // Relations
        [DataRelation(WithDataModel = typeof(Country), OnDataModelKey = "ISO3Code", ThisKey = "ISO3CountryCode")]
        public Country Country { get; set; }

        //[DataRelation(Name="CountryID_Country.ID", WithDataModel = typeof(Country), OnDataModelKey = "ID", ThisKey = "CountryID")]
        //public Country Country { get; set; }

        //[DataRelation(Name = "TypeOfServiceID_CallType.ID", WithDataModel = typeof(CallType), OnDataModelKey = "ID", ThisKey = "TypeOfServiceID")]
        //public CallType TypeOfService { get; set; }

    }
}
