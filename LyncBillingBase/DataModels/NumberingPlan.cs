using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NumberingPlan", Type = Globals.DataSource.Type.DBTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class NumberingPlan : DataModel
    {
        [IsIdField]
        [AllowIdInsert]
        [DbColumn("Dialing_prefix")]
        public Int64 DialingPrefix { get; set; }

        [DbColumn("Country_Name")]
        public string CountryName { get; set; }

        [DbColumn("Two_Digits_country_code")]
        public string Iso2CountryCode { get; set; }

        [DbColumn("Three_Digits_Country_Code")]
        public string Iso3CountryCode { get; set; }

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
        [DataRelation(WithDataModel = typeof (Country), OnDataModelKey = "Iso3Code", ThisKey = "Iso3CountryCode")]
        public Country Country { get; set; }
    }
}