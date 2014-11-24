using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace LyncBillingBase
{
    public static class Enums
    {
       
        public enum SpecialDateTime
        {
            [Description("1st Quarter")]
            [DefaultValue(1)]
            FirstQuarter,

            [Description("2nd Quarter")]
            [DefaultValue(2)]
            SecondQuarter,

            [Description("3rd Quarter")]
            [DefaultValue(3)]
            ThirdQuarter,

            [Description("4th Quarter")]
            [DefaultValue(4)]
            FourthQuarter,

            [Description("All Quarters")]
            [DefaultValue(5)]
            AllQuarters,

            [Description("One Year Ago from Today")]
            [DefaultValue(-1)]
            OneYearAgoFromToday,

            [Description("Two Years Ago from Today")]
            [DefaultValue(-2)]
            TwoYearsAgoFromToday
        }

        /// <summary>
        /// Rates Database table fields Names
        /// </summary>
        public enum Rates
        {
            [Description("Rate_ID")]
            RateID,
            [Description("Three_Digits_Country_Code")]
            CountryCode,
            [Description("Country_Name")]
            CountryName,
            [Description("Fixedline")]
            FixedlineRate,
            [Description("GSM")]
            MobileLineRate
        }

        public enum Rates_International
        {
            [Description("RateID")]
            RateID,
            [Description("Three_Digits_Country_Code")]
            CountryCode,
            [Description("Country_Name")]
            CountryName,
            [Description("Fixedline")]
            FixedlineRate,
            [Description("GSM")]
            MobileLineRate
        }

        public enum Rates_National
        {
            [Description("Rate_ID")]
            RateID,
            [Description("Three_Digits_Country_Code")]
            CountryCode,
            [Description("Country_Name")]
            CountryName,
            [Description("Dialing_prefix")]
            DialingPrefix,
            [Description("Type_Of_Service")]
            TypeOfService,
            [Description("Rate")]
            Rate
        }

        public enum RatesNGN
        {
            [Description("RateID")]
            RateID,
            [Description("DialingCodeID")]
            DialingCodeID,
            [Description("DialingCode")]
            DialingCode,
            [Description("CountryCodeISO3")]
            CountryCodeISO3,
            [Description("CountryName")]
            CountryName,
            [Description("TypeOfServiceID")]
            TypeOfServiceID,
            [Description("CallType")]
            CallType,
            [Description("Rate")]
            Rate
        }

        public enum ActualRates
        {
            [Description("GatewaysDetails")]
            TableName,
            [Description("Rate_ID")]
            RateID,
            [Description("Dialing_prefix")]
            DialingPrefix,
            [Description("Country_Name")]
            CountryName,
            [Description("Two_Digits_country_code")]
            TwoDigitsCountryCode,
            [Description("Three_Digits_Country_Code")]
            ThreeDigitsCountryCode,
            [Description("City")]
            City,
            [Description("Provider")]
            Provider,
            [Description("Type_Of_Service")]
            TypeOfService,
            [Description("rate")]
            Rate,
        }

        public enum SystemRoles
        {
            [DefaultValue(10)]
            [Description("System Developer")]
            DeveloperRole,
            [DefaultValue(20)]
            [Description("System Admin")]
            SystemAdminRole,
            [DefaultValue(30)]
            [Description("Sites Administrator")]
            SiteAdminRole,
            [DefaultValue(40)]
            [Description("Sites Accountant")]
            SiteAccountantRole,
            [DefaultValue(100)]
            [Description("Departments Head")]
            DepartmentHeadRole,
            [DefaultValue(1000)]
            [Description("Normal User")]
            NormalUserRole
        }

        public enum ValidRoles
        {
            [Description("IsDeveloper")]
            IsDeveloper,
            [Description("IsSystemAdmin")]
            IsSystemAdmin,
            [Description("IsSiteAdmin")]
            IsSiteAdmin,
            [Description("IsSiteAccountant")]
            IsSiteAccountant,
            [Description("IsDepartmentHead")]
            IsDepartmentHead
        }

        public enum ActiveRoleNames
        {
            [Description("developer")]
            Developer,
            [Description("sysadmin")]
            SystemAdmin,
            [Description("admin")]
            SiteAdmin,
            [Description("accounting")]
            SiteAccountant,
            [Description("dephead")]
            DepartmentHead,
            [Description("userdelegee")]
            UserDelegee,
            [Description("depdelegee")]
            DepartmentDelegee,
            [Description("sitedelegee")]
            SiteDelegee,
            [Description("user")]
            NormalUser
        }

        public enum DelegateTypes 
        {
            [DefaultValue(1)]
            [Description("Users-Delegee")]
            UserDelegeeType,
            [DefaultValue(2)]
            [Description("Departments-Delegee")]
            DepartemntDelegeeType,
            [DefaultValue(3)]
            [Description("Sites-Delegee")]
            SiteDelegeeType
        }

        public enum PhoneCallSummary
        {
            [Description("")]
            TableName,
            [Description("Year")]
            Year,
            [Description("Month")]
            Month,
            [Description("Date")]
            Date,
            [Description("startingDate")]
            StartingDate,
            [Description("endingDate")]
            EndingDate,
            [Description("AD_UserID")]
            EmployeeID,
            [Description("ChargingParty")]
            ChargingParty,
            [Description("AD_DisplayName")]
            DisplayName,
            [Description("AD_Department")]
            Department,
            [Description("AD_PhysicalDeliveryOfficeName")]
            SiteName,
            [Description("ac_IsInvoiced")]
            AC_IsInvoiced,
            [Description("Duration")]
            Duration,

            [Description("BusinessCallsDuration")]
            BusinessCallsDuration,
            [Description("BusinessCallsCount")]
            BusinessCallsCount,
            [Description("BusinessCallsCost")]
            BusinessCallsCost,

            [Description("PersonalCallsDuration")]
            PersonalCallsDuration,
            [Description("PersonalCallsCount")]
            PersonalCallsCount,
            [Description("PersonalCallsCost")]
            PersonalCallsCost,

            [Description("UnmarkedCallsDuration")]
            UnmarkedCallsDuration,
            [Description("UnmarkedCallsCount")]
            UnmarkedCallsCount,
            [Description("UnmarkedCallsCost")]
            UnmarkedCallsCost,

            [Description("TotalCallsCost")]
            TotalCallsCost,
            [Description("TotalCallsDuration")]
            TotalCallsDuration,
            [Description("TotalCallsCount")]
            TotalCallsCount,

            [Description("NumberOfDisputedCalls")]
            NumberOfDisputedCalls,
        }

        public enum GatewaysSummary
        {
            [Description("Gateway")]
            Gateway,
            [Description("Year")]
            Year,
            [Description("Month")]
            Month,
            [Description("CallsDuration")]
            CallsDuration,
            [Description("CallsCount")]
            CallsCount,
            [Description("CallsCost")]
            CallsCost
        }

        public enum TopDestinationCountries
        {
            [Description("Country_Name")]
            Country,
            [Description("CallsDuration")]
            CallsDuration,
            [Description("CallsCost")]
            CallsCost,
            [Description("CallsCount")]
            CallsCount,
        }

        public enum TopDestinationNumbers
        {
            [Description("PhoneNumber")]
            PhoneNumber,
            [Description("Country")]
            Country,
            [Description("CallsDuration")]
            CallsDuration,
            [Description("CallsCost")]
            CallsCost,
            [Description("CallsCount")]
            CallsCount
        }

        // List of Database Functions and their DB Names
        public enum DatabaseFunctionsNames
        {
            [Description("Get_ChargeableCalls_ForUser")]
            Get_ChargeableCalls_ForUser,

            [Description("Get_ChargeableCalls_ForSite")]
            Get_ChargeableCalls_ForSite,

            [Description("Get_DisputedCalls_ForUser")]
            Get_DisputedCalls_ForUser,

            [Description("Get_DisputedCalls_ForSite")]
            Get_DisputedCalls_ForSite,

            [Description("Get_CallsSummary_ForUser")]
            Get_CallsSummary_ForUser,

            [Description("Get_CallsSummary_ForSiteDepartment")]
            Get_CallsSummary_ForSiteDepartment,

            [Description("Get_CallsSummary_ForUsers_PerSite")]
            Get_CallsSummary_ForUsers_PerSite,

            [Description("Get_CallsSummary_ForUsers_PerSite_PDF")]
            Get_CallsSummary_ForUsers_PerSite_PDF,

            [Description("Get_CallsSummary_ForSite")]
            Get_CallsSummary_ForSite,

            [Description("Get_DestinationsNumbers_ForUser")]
            Get_DestinationsNumbers_ForUser,

            [Description("Get_DestinationCountries_ForUser")]
            Get_DestinationCountries_ForUser,

            [Description("Get_DestinationCountries_ForSiteDepartment")]
            Get_DestinationCountries_ForSiteDepartment,

            [Description("Get_DestinationCountries_ForSite")]
            Get_DestinationCountries_ForSite,

            [Description("Get_GatewaySummary_PerSite")]
            Get_GatewaySummary_PerSite,

            [Description("Get_GatewaySummary_ForAll_Sites")]
            Get_GatewaySummary_ForAll_Sites,

            [Description("Get_MailStatistics_PerSiteDepartment")]
            Get_MailStatistics_PerSiteDepartment
        }

        // List of Store Procedures and their DB names.
        public enum StoreProcedureNames
        {
            [Description("sp_Invoice_Allocated_ChargeableCalls_ForSite")]
            SP_Invoice_Allocated_ChargeableCalls_ForSite,

            [Description("sp_Invoice_Unallocated_ChargeableCalls_ForSite")]
            SP_Invoice_Unallocated_ChargeableCalls_ForSite,

            [Description("sp_Mark_UnallocatedCalls_AsPending_ForSite")]
            SP_Mark_UnallocatedCalls_AsPending_ForSite
        }

        // Store Procedure Parameters
        public enum SP_Invoice_Allocated_ChargeableCalls_ForSite
        {
            [Description("OfficeName")]
            OfficeName,
            [Description("FromDate")]
            FromDate,
            [Description("ToDate")]
            ToDate,
            [Description("InvoiceDate")]
            InvoiceDate
        }

        public enum SP_Invoice_Unallocated_ChargeableCalls_ForSite
        {
            [Description("OfficeName")]
            OfficeName,
            [Description("FromDate")]
            FromDate,
            [Description("ToDate")]
            ToDate,
            [Description("InvoiceDate")]
            InvoiceDate
        }

        public enum SP_Mark_UnallocatedCalls_AsPending_ForSite
        {
            [Description("OfficeName")]
            OfficeName,
            [Description("FromDate")]
            FromDate,
            [Description("ToDate")]
            ToDate,
            [Description("InvoiceDate")]
            InvoiceDate
        }

        public enum DataSourceType
        {
            Default,
            DBTable,
            WS
        }

        public enum DataSourceAccessType
        {
            Default,
            SingleSource,
            Distributed
        }

        /// <summary>
        /// Gets the Name of DB table Field
        /// </summary>
        /// <param name="value">Enum Name</param>
        /// <returns>Field Description</returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] descAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descAttributes != null && descAttributes.Length > 0)
                return descAttributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        /// Gets the DefaultValue attribute of the enum
        /// </summary>
        /// <param name="value">Enum Name</param>
        /// <returns>Field Description</returns>
        public static string GetValue(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            DefaultValueAttribute[] valueAttributes = (DefaultValueAttribute[])fieldInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false);

            if (valueAttributes != null && valueAttributes.Length > 0)
                return valueAttributes[0].Value.ToString();
            else
                return value.ToString();
        }

        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T is not of System.Enum Type");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }
    }
}
