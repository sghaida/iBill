using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace LyncBillingBase.DAL
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

        public enum MonitoringServersInfo
        {
            [Description("MonitoringServersInfo")]
            TableName,
            [Description("id")]
            Id,
            [Description("instanceHostName")]
            InstanceHostName,
            [Description("instanceName")]
            InstanceName,
            [Description("databaseName")]
            DatabaseName,
            [Description("userName")]
            Userame,
            [Description("password")]
            Password,
            [Description("TelephonySolutionName")]
            TelephonySolutionName,
            [Description("phoneCallsTable")]
            PhoneCallsTable,
            [Description("description")]
            Description,
            [Description("created_at")]
            CreatedAt

        }
        
        public enum PhoneCalls
        {
            [Description("PhoneCallsTableName")]
            PhoneCallsTableName,
            [Description("SessionIdTime")]
            SessionIdTime,
            [Description("SessionIdSeq")]
            SessionIdSeq,
            [Description("ResponseTime")]
            ResponseTime,
            [Description("SessionEndTime")]
            SessionEndTime,
            [Description("SourceUserUri")]
            SourceUserUri,
            [Description("SourceNumberUri")]
            SourceNumberUri,
            [Description("DestinationUserUri")]
            DestinationUserUri,
            [Description("DestinationNumberUri")]
            DestinationNumberUri,
            [Description("FromMediationServer")]
            FromMediationServer,
            [Description("ToMediationServer")]
            ToMediationServer,
            [Description("FromGateway")]
            FromGateway,
            [Description("ToGateway")]
            ToGateway,
            [Description("SourceUserEdgeServer")]
            SourceUserEdgeServer,
            [Description("DestinationUserEdgeServer")]
            DestinationUserEdgeServer,
            [Description("ServerFQDN")]
            ServerFQDN,
            [Description("PoolFQDN")]
            PoolFQDN,
            [Description("OnBehalf")]
            OnBehalf,
            [Description("ReferredBy")]
            ReferredBy,
            [Description("ChargingParty")]
            ChargingParty,
            [Description("Duration")]
            Duration,
            [Description("marker_CallFrom")]
            Marker_CallFrom,
            [Description("marker_CallTo")]
            Marker_CallTo,
            [Description("marker_CallToCountry")]
            Marker_CallToCountry,
            [Description("marker_CallType")]
            Marker_CallType,
            [Description("marker_CallTypeID")]
            Marker_CallTypeID,
            [Description("marker_CallCost")]
            Marker_CallCost,
            [Description("ui_MarkedOn")]
            UI_MarkedOn,
            [Description("ui_UpdatedByUser")]
            UI_UpdatedByUser,
            [Description("ui_AssignedByUser")]
            UI_AssignedByUser,
            [Description("ui_AssignedToUser")]
            UI_AssignedToUser,
            [Description("ui_AssignedOn,")]
            UI_AssignedOn,
            [Description("ui_CallType")]
            UI_CallType,
            [Description("ac_DisputeStatus")]
            AC_DisputeStatus,
            [Description("ac_DisputeResolvedOn")]
            AC_DisputeResolvedOn,
            [Description("ac_IsInvoiced")]
            AC_IsInvoiced,
            [Description("ac_InvoiceDate")]
            AC_InvoiceDate,
            [Description("Exclude")]
            Exclude,
            [Description("CalleeURI")]
            CalleeURI
        }

        public enum CallMarkerStatus
        {
            [Description("CallMarkerStatus")]
            TableName,
            [Description("markerId")]
            MarkerId,
            [Description("phoneCallsTable")]
            PhoneCallsTable,
            [Description("type")]
            Type,
            [Description("timestamp")]
            Timestamp
        }

        public enum Countries
        {
            [Description("Countries")]
            TableName,
            [Description("CountryName")]
            CountryName,
            [Description("CurrencyName")]
            CurrencyName,
            [Description("CurrencyISOName")]
            CurrencyISOName,
            [Description("CountryCodeISO2")]
            CountryCodeISO2,
            [Description("CountryCodeISO3")]
            CountryCodeISO3
        }

        public enum Gateways
        {
            [Description("Gateways")]
            TableName,
            [Description("GatewayId")]
            GatewayId,
            [Description("Gateway")]
            GatewayName
        }

        public enum Sites
        {
            [Description("Sites")]
            TableName,
            [Description("SiteID")]
            SiteID,
            [Description("SiteName")]
            SiteName,
            [Description("CountryCode")]
            CountryCode,
            [Description("Description")]
            Description
        }

        /// <summary>
        /// Departments lookup table fields Names
        /// </summary>
        public enum Departments
        {
            [Description("Departments")]
            TableName,
            [Description("ID")]
            ID,
            [Description("DepartmentName")]
            DepartmentName,
            [Description("Description")]
            Description
        }

        public enum SitesDepartmnets
        {
            [Description("Sites_Departments")]
            TableName,
            [Description("ID")]
            ID,
            [Description("SiteID")]
            SiteID,
            [Description("DepartmentID")]
            DepartmentID
        }

        public enum Pools
        {
            [Description("Pools")]
            TableName,
            [Description("PoolId")]
            PoolId,
            [Description("PoolFQDN")]
            PoolFQDN
        }

        public enum GatewaysDetails
        {
            [Description("GatewaysDetails")]
            TableName,
            [Description("GatewayID")]
            GatewayID,
            [Description("SiteID")]
            SiteID,
            [Description("PoolID")]
            PoolID,
            [Description("Description")]
            Description
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

        public enum GatewaysRates
        {
            [Description("GatewaysRates")]
            TableName,
            [Description("GatewaysRatesID")]
            GatewaysRatesID,
            [Description("GatewayID")]
            GatewayID,
            [Description("RatesTableName")]
            RatesTableName,
            [Description("NgnRatesTableName")]
            NgnRatesTableName,
            [Description("StartingDate")]
            StartingDate,
            [Description("EndingDate")]
            EndingDate,
            [Description("ProviderName")]
            ProviderName,
            [Description("CurrencyCode")]
            CurrencyCode
        }

        public enum NumberingPlan
        {
            [Description("NumberingPlan")]
            TableName,
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
            TypeOfService
        }

        public enum NumberingPlanNGN
        {
            [Description("NGN_NumberingPlan")]
            TableName,
            [Description("ID")]
            ID,
            [Description("DialingCode")]
            DialingCode,
            [Description("CountryCodeISO3")]
            CountryCodeISO3,
            [Description("Provider")]
            Provider,
            [Description("TypeOfService")]
            TypeOfService,
            [Description("TypeOfServiceID")]
            TypeOfServiceID,
            [Description("Description")]
            Description
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

        public enum CallTypes
        {
            [Description("CallTypes")]
            TableName,
            [Description("id")]
            id,
            [Description("CallType")]
            CallType
        }

        public enum DIDs
        {
            [Description("DIDs")]
            TableName,
            [Description("id")]
            id,
            [Description("Regex")]
            Regex,
            [Description("description")]
            description
        }

        public enum PhoneCallsExceptions
        {
            [Description("PhoneCallsExceptions")]
            TableName,
            [Description("ID")]
            ID,
            [Description("UserUri")]
            UserUri,
            [Description("Number")]
            Number,
            [Description("Description")]
            Description
        }

        public enum ExceptionsList 
        {
            [Description("ExceptionsList")]
            TableName,
            [Description("ID")]
            ID,
            [Description("Entity")]
            Entity,
            [Description("EntityType")]
            EntityType,
            [Description("ZeroCost")]
            ZeroCost,
            [Description("AutoMark")]
            AutoMark,
            [Description("SiteID")]
            SiteID,
            [Description("Description")]
            Description
        }

        public enum MailStatistics
        {
            [Description("MailStatistics")]
            TableName,
            [Description("id")]
            ID,
            [Description("EmailAddress")]
            EmailAddress,
            [Description("RecievedCount")]
            ReceivedCount,
            [Description("RecievedSize")]
            ReceivedSize,
            [Description("SentCount")]
            SentCount,
            [Description("SentSize")]
            SentSize,
            [Description("TimeStamp")]
            TimeStamp
        }

        /// <summary>
        /// Users Database table Fields Names
        /// </summary>
        public enum Users
        {
            [Description("ActiveDirectoryUsers")]
            TableName,
            [Description("AD_UserID")]
            AD_UserID,
            [Description("SipAccount")]
            SipAccount,
            [Description("AD_DisplayName")]
            AD_DisplayName,
            [Description("AD_PhysicalDeliveryOfficeName")]
            AD_PhysicalDeliveryOfficeName,
            [Description("AD_Department")]
            AD_Department,
            [Description("AD_TelephoneNumber")]
            AD_TelephoneNumber,
            [Description("NotifyUser")]
            NotifyUser,
            [Description("UpdatedByAD")]
            UpdatedByAD,
            [Description("CreatedAt")]
            CreatedAt,
            [Description("UpdatedAt")]
            UpdatedAt
        }

        public enum Announcements
        {
            [Description("Announcements")]
            TableName,
            [Description("ID")]
            ID,
            [Description("Announcement")]
            Announcement,
            [Description("ForRole")]
            ForRole,
            [Description("ForSite")]
            ForSite,
            [Description("PublishOn")]
            PublishOn
        }

        public enum PhoneBook
        {
            [Description("PhoneBook")]
            TableName,
            [Description("ID")]
            ID,
            [Description("SipAccount")]
            SipAccount,
            [Description("DestinationNumber")]
            DestinationNumber,
            [Description("DestinationCountry")]
            DestinationCountry,
            [Description("Type")]
            Type,
            [Description("Name")]
            Name
        }

        public enum MailTemplates
        {
            [Description("MailTemplates")]
            TableName,
            [Description("TemplateID")]
            TemplateID,
            [Description("TemplateSubject")]
            TemplateSubject,
            [Description("TemplateBody")]
            TemplateBody
        }

        public enum Persistence
        {
            [Description("Persistence")]
            TableName,
            [Description("ID")]
            ID,
            [Description("Module")]
            Module,
            [Description("Module_Key")]
            ModuleKey,
            [Description("Module_Value")]
            ModuleValue
        }

        public enum BundledAccounts
        {
            [Description("BundledAccounts")]
            TableName,
            [Description("ID")]
            ID,
            [Description("PrimarySipAccount")]
            PrimarySipAccount,
            [Description("AssociatedSipAccount")]
            AssociatedSipAccount
        }

        /// <summary>
        /// SystemRoles Database table fields Names
        /// </summary>
        public enum Roles
        {
            [Description("SystemRoles")]
            TableName,
            [Description("RoleID")]
            RoleID,
            [Description("RoleName")]
            RoleName,
            [Description("RoleDescription")]
            RoleDescription
        }

        public enum DelegateTypes
        {
            [Description("DelegeeType")]
            TableName,
            [Description("ID")]
            ID,
            [Description("DelegeeType")]
            DelegeeType,
            [Description("Description")]
            Description,
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

        public enum DelegateRoles
        {
            [Description("Roles_Delegates")]
            TableName,
            [Description("ID")]
            ID,
            [Description("SiteID")]
            SiteID,
            [Description("DepartmentID")]
            DepartmentID,
            [Description("SipAccount")]
            SipAccount,
            [Description("DelegeeType")]
            DelegeeType,
            [Description("Delegee")]
            Delegee,
            [Description("Description")]
            Description
        }

        public enum SystemRoles
        {
            [Description("Roles_System")]
            TableName,
            [Description("ID")]
            ID,
            [Description("RoleID")]
            RoleID,
            [Description("SipAccount")]
            SipAccount,
            [Description("SiteID")]
            SiteID,
            [Description("Description")]
            Description,

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

        public enum DepartmentHeadRoles
        {
            [Description("Roles_DepartmentsHeads")]
            TableName,
            [Description("ID")]
            ID,
            [Description("SiteDepartmentID")]
            SiteDepartmentID,
            [Description("SipAccount")]
            SipAccount
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
