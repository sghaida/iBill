using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

using DALDotNet.Helpers;

namespace DALDotNet
{
    public static class GLOBALS
    {
        // The Data Source GLOBALS
        public static class DataSource
        {
            public enum Type
            {
                // Data Source Types
                [Description("Default Value")]
                [DefaultValue("N/A")]
                Default = 0,

                [Description("Data is read from a database table.")]
                [DefaultValue("DBTable")]
                DBTable = 1,

                [Description("Data is read from a webservice endpoint.")]
                [DefaultValue("WebService")]
                WebService = 2
            }

            // Data Source Access Methods
            public enum AccessMethod
            {
                [Description("Default Value")]
                Default = 0,

                [Description("Data is read from a single source, such as: a table, a webservice endpoint...etc")]
                SingleSource = 1,

                [Description("Data is read from multiple sources, this data source acts as a lookup of the other sources, such as: a lookup table, a lookup webservice endpoint...etc")]
                DistributedSource = 2
            }
        }


        // The Data Relation GLOBALS
        public static class DataRelation
        {
            public enum Type
            {
                [Description("The intersection of two data models. Equivalent to an SQL INNER JOIN.")]
                [DefaultValue("INTERSECTION")]
                INTERSECTION = 0,

                [Description("The union of two data models. Equivalent to an SQL OUTER JOIN.")]
                [DefaultValue("UNION")]
                UNION = 1
            }
        }


        // The Phone Call Exclusions GLOBALS
        public static class PhoneCallExclusion
        {
            public enum Type
            {
                [Description("N/A")]
                [DefaultValue("N/A")]
                Default,

                [Description("Source")]
                [DefaultValue("S")]
                Source,

                [Description("Destination")]
                [DefaultValue("D")]
                Destination
            }

            public enum ZeroCost
            {
                [Description("N/A")]
                [DefaultValue("N/A")]
                Default,

                [Description("Yes")]
                [DefaultValue("Y")]
                Yes,

                [Description("No")]
                [DefaultValue("N")]
                No
            }

            public enum AutoMark
            {
                [Description("DISABLED")]
                [DefaultValue("DISABLED")]
                Default,

                [Description("Business")]
                [DefaultValue("B")]
                Business,

                [Description("Personal")]
                [DefaultValue("P")]
                Personal
            }
        }


        // The Call Marker Status GLOBALS
        public static class CallMarkerStatus
        {
            public enum Type
            {
                [Description("The rates applying status string.")]
                [DefaultValue("ApplyingRates")]
                ApplyingRates = 0,

                [Description("The phone calls marking status string.")]
                [DefaultValue("Marking")]
                CallsMarking = 1,
            }
        }


        // The Phone Calls GLOBALS
        public static class PhoneCalls
        {
            public enum CallTypes
            {
                [Description("Business Phone Call")]
                [DefaultValue("Business")]
                Business,

                [Description("Personal Phone Call")]
                [DefaultValue("Personal")]
                Personal,

                [Description("Disputed Phone Call")]
                [DefaultValue("Disputed")]
                Disputed
            }
        }


        // The CallsSummary GLOBALS
        public static class CallsSummary
        {
            public enum GroupBy
            {
                [Description("Group By User Only")]
                [DefaultValue("GroupByUserOnly")]
                DontGroup,

                [Description("Group By User Only")]
                [DefaultValue("GroupByUserOnly")]
                UserOnly,

                [Description("Group By User And Invoice Flag")]
                [DefaultValue("GroupByUserAndInvoiceFlag")]
                UserAndInvoiceFlag
            }
        }


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


    }

}
