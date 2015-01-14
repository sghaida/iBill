using System.ComponentModel;

namespace CCC.ORM
{
    public static class Globals
    {
        public enum ActualRates
        {
            [Description("GatewaysDetails")] TableName,
            [Description("Rate_ID")] RateId,
            [Description("Dialing_prefix")] DialingPrefix,
            [Description("Country_Name")] CountryName,
            [Description("Two_Digits_country_code")] TwoDigitsCountryCode,
            [Description("Three_Digits_Country_Code")] ThreeDigitsCountryCode,
            [Description("City")] City,
            [Description("Provider")] Provider,
            [Description("Type_Of_Service")] TypeOfService,
            [Description("rate")] Rate
        }

        // List of Database Functions and their DB Names
        public enum DatabaseFunctionsNames
        {
            [Description("Get_ChargeableCalls_ForUser")] GetChargeableCallsForUser,

            [Description("Get_ChargeableCalls_ForSite")] GetChargeableCallsForSite,

            [Description("Get_DisputedCalls_ForUser")] GetDisputedCallsForUser,

            [Description("Get_DisputedCalls_ForSite")] GetDisputedCallsForSite,

            [Description("Get_CallsSummary_ForUser")] GetCallsSummaryForUser,

            [Description("Get_CallsSummary_ForSiteDepartment")] GetCallsSummaryForSiteDepartment,

            [Description("Get_CallsSummary_ForUsers_PerSite")] GetCallsSummaryForUsersPerSite,

            [Description("Get_CallsSummary_ForUsers_PerSite_PDF")] GetCallsSummaryForUsersPerSitePdf,

            [Description("Get_CallsSummary_ForSite")] GetCallsSummaryForSite,

            [Description("Get_DestinationsNumbers_ForUser")] GetDestinationsNumbersForUser,

            [Description("Get_DestinationCountries_ForUser")] GetDestinationCountriesForUser,

            [Description("Get_DestinationCountries_ForSiteDepartment")] GetDestinationCountriesForSiteDepartment,

            [Description("Get_DestinationCountries_ForSite")] GetDestinationCountriesForSite,

            [Description("Get_GatewaySummary_PerSite")] GetGatewaySummaryPerSite,

            [Description("Get_GatewaySummary_ForAll_Sites")] GetGatewaySummaryForAllSites,

            [Description("Get_MailStatistics_PerSiteDepartment")] GetMailStatisticsPerSiteDepartment
        }

        public enum GatewaysSummary
        {
            [Description("Gateway")] Gateway,
            [Description("Year")] Year,
            [Description("Month")] Month,
            [Description("CallsDuration")] CallsDuration,
            [Description("CallsCount")] CallsCount,
            [Description("CallsCost")] CallsCost
        }

        public enum PhoneCallSummary
        {
            [Description("")] TableName,
            [Description("Year")] Year,
            [Description("Month")] Month,
            [Description("Date")] Date,
            [Description("startingDate")] StartingDate,
            [Description("endingDate")] EndingDate,
            [Description("AD_UserID")] EmployeeId,
            [Description("ChargingParty")] ChargingParty,
            [Description("AD_DisplayName")] DisplayName,
            [Description("AD_Department")] Department,
            [Description("AD_PhysicalDeliveryOfficeName")] SiteName,
            [Description("ac_IsInvoiced")] AcIsInvoiced,
            [Description("Duration")] Duration,

            [Description("BusinessCallsDuration")] BusinessCallsDuration,
            [Description("BusinessCallsCount")] BusinessCallsCount,
            [Description("BusinessCallsCost")] BusinessCallsCost,

            [Description("PersonalCallsDuration")] PersonalCallsDuration,
            [Description("PersonalCallsCount")] PersonalCallsCount,
            [Description("PersonalCallsCost")] PersonalCallsCost,

            [Description("UnmarkedCallsDuration")] UnmarkedCallsDuration,
            [Description("UnmarkedCallsCount")] UnmarkedCallsCount,
            [Description("UnmarkedCallsCost")] UnmarkedCallsCost,

            [Description("TotalCallsCost")] TotalCallsCost,
            [Description("TotalCallsDuration")] TotalCallsDuration,
            [Description("TotalCallsCount")] TotalCallsCount,

            [Description("NumberOfDisputedCalls")] NumberOfDisputedCalls
        }

        /// <summary>
        ///     Rates Database table fields Names
        /// </summary>
        public enum Rates
        {
            [Description("Rate_ID")] RateId,
            [Description("Three_Digits_Country_Code")] CountryCode,
            [Description("Country_Name")] CountryName,
            [Description("Fixedline")] FixedlineRate,
            [Description("GSM")] MobileLineRate
        }

        public enum RatesInternational
        {
            [Description("RateID")] RateId,
            [Description("Three_Digits_Country_Code")] CountryCode,
            [Description("Country_Name")] CountryName,
            [Description("Fixedline")] FixedlineRate,
            [Description("GSM")] MobileLineRate
        }

        public enum RatesNational
        {
            [Description("Rate_ID")] RateId,
            [Description("Three_Digits_Country_Code")] CountryCode,
            [Description("Country_Name")] CountryName,
            [Description("Dialing_prefix")] DialingPrefix,
            [Description("Type_Of_Service")] TypeOfService,
            [Description("Rate")] Rate
        }

        public enum RatesNgn
        {
            [Description("RateID")] RateId,
            [Description("DialingCodeID")] DialingCodeId,
            [Description("DialingCode")] DialingCode,
            [Description("CountryCodeISO3")] CountryCodeIso3,
            [Description("CountryName")] CountryName,
            [Description("TypeOfServiceID")] TypeOfServiceId,
            [Description("CallType")] CallType,
            [Description("Rate")] Rate
        }

        // Store Procedure Parameters
        public enum SpInvoiceAllocatedChargeableCallsForSite
        {
            [Description("OfficeName")] OfficeName,
            [Description("FromDate")] FromDate,
            [Description("ToDate")] ToDate,
            [Description("InvoiceDate")] InvoiceDate
        }

        public enum SpInvoiceUnallocatedChargeableCallsForSite
        {
            [Description("OfficeName")] OfficeName,
            [Description("FromDate")] FromDate,
            [Description("ToDate")] ToDate,
            [Description("InvoiceDate")] InvoiceDate
        }

        public enum SpMarkUnallocatedCallsAsPendingForSite
        {
            [Description("OfficeName")] OfficeName,
            [Description("FromDate")] FromDate,
            [Description("ToDate")] ToDate,
            [Description("InvoiceDate")] InvoiceDate
        }

        public enum SpecialDateTime
        {
            [Description("1st Quarter")] [DefaultValue(1)] FirstQuarter,

            [Description("2nd Quarter")] [DefaultValue(2)] SecondQuarter,

            [Description("3rd Quarter")] [DefaultValue(3)] ThirdQuarter,

            [Description("4th Quarter")] [DefaultValue(4)] FourthQuarter,

            [Description("All Quarters")] [DefaultValue(5)] AllQuarters,

            [Description("One Year Ago from Today")] [DefaultValue(-1)] OneYearAgoFromToday,

            [Description("Two Years Ago from Today")] [DefaultValue(-2)] TwoYearsAgoFromToday
        }

        // List of Store Procedures and their DB names.
        public enum StoreProcedureNames
        {
            [Description("sp_Invoice_Allocated_ChargeableCalls_ForSite")] SpInvoiceAllocatedChargeableCallsForSite,

            [Description("sp_Invoice_Unallocated_ChargeableCalls_ForSite")] SpInvoiceUnallocatedChargeableCallsForSite,

            [Description("sp_Mark_UnallocatedCalls_AsPending_ForSite")] SpMarkUnallocatedCallsAsPendingForSite
        }

        public enum TopDestinationCountries
        {
            [Description("Country_Name")] Country,
            [Description("CallsDuration")] CallsDuration,
            [Description("CallsCost")] CallsCost,
            [Description("CallsCount")] CallsCount
        }

        public enum TopDestinationNumbers
        {
            [Description("PhoneNumber")] PhoneNumber,
            [Description("Country")] Country,
            [Description("CallsDuration")] CallsDuration,
            [Description("CallsCost")] CallsCost,
            [Description("CallsCount")] CallsCount
        }

        // The Data Source GLOBALS
        public static class DataSource
        {
            // Data Source Access Methods
            public enum AccessMethod
            {
                [Description("Default Value")] Default = 0,

                [Description("Data is read from a single source, such as: a table, a webservice endpoint...etc")] SingleSource = 1,

                [Description(
                    "Data is read from multiple sources, this data source acts as a lookup of the other sources, such as: a lookup table, a lookup webservice endpoint...etc"
                    )] DistributedSource = 2
            }

            public enum Type
            {
                // Data Source Types
                [Description("Default Value")] [DefaultValue("N/A")] Default = 0,

                [Description("Data is read from a database table.")] [DefaultValue("DBTable")] DbTable = 1,

                [Description("Data is read from a webservice endpoint.")] [DefaultValue("WebService")] WebService = 2
            }
        }

        // The Data Relation GLOBALS
        public static class DataRelation
        {
            public enum Type
            {
                [Description("The intersection of two data models. Equivalent to an SQL INNER JOIN.")] [DefaultValue("INTERSECTION")] Intersection = 0,

                [Description("The union of two data models. Equivalent to an SQL OUTER JOIN.")] [DefaultValue("UNION")] Union = 1
            }
        }

        // The Phone Call Exclusions GLOBALS
        public static class PhoneCallExclusion
        {
            public enum AutoMark
            {
                [Description("DISABLED")] [DefaultValue("DISABLED")] Default,

                [Description("Business")] [DefaultValue("B")] Business,

                [Description("Personal")] [DefaultValue("P")] Personal
            }

            public enum Type
            {
                [Description("N/A")] [DefaultValue("N/A")] Default,

                [Description("Source")] [DefaultValue("S")] Source,

                [Description("Destination")] [DefaultValue("D")] Destination
            }

            public enum ZeroCost
            {
                [Description("N/A")] [DefaultValue("N/A")] Default,

                [Description("Yes")] [DefaultValue("Y")] Yes,

                [Description("No")] [DefaultValue("N")] No
            }
        }

        // The Call Marker Status GLOBALS
        public static class CallMarkerStatus
        {
            public enum Type
            {
                [Description("The rates applying status string.")] [DefaultValue("ApplyingRates")] ApplyingRates = 0,

                [Description("The phone calls marking status string.")] [DefaultValue("Marking")] CallsMarking = 1
            }
        }

        // The Phone Calls GLOBALS
        public static class PhoneCalls
        {
            public enum CallTypes
            {
                [Description("Business Phone Call")] [DefaultValue("Business")] Business,

                [Description("Personal Phone Call")] [DefaultValue("Personal")] Personal,

                [Description("Disputed Phone Call")] [DefaultValue("Disputed")] Disputed
            }
        }

        // The CallsSummary GLOBALS
        public static class CallsSummary
        {
            public enum GroupBy
            {
                [Description("Group By User Only")] [DefaultValue("GroupByUserOnly")] DontGroup,

                [Description("Group By User Only")] [DefaultValue("GroupByUserOnly")] UserOnly,

                [Description("Group By User And Invoice Flag")] [DefaultValue("GroupByUserAndInvoiceFlag")] UserAndInvoiceFlag
            }
        }
    }
}