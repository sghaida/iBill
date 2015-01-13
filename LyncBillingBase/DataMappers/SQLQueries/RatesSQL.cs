using System;

namespace LyncBillingBase.DataMappers.SQLQueries
{
    public class RatesSql
    {
        public string GetInternationalRates(string ratesTableName)
        {
            var sql = String.Format(
                "SELECT " +
                "Country_Name as 'CountryName', " +
                "Two_Digits_country_code as 'ISO2CountryCode', " +
                "Three_Digits_Country_Code as 'ISO3CountryCode', " +
                "MAX(CASE WHEN Type_Of_Service <> 'gsm' THEN rate END) FixedLineRate, " +
                "MAX(CASE WHEN Type_Of_Service = 'gsm' THEN rate END) MobileLineRate " +
                "FROM " +
                "(" +
                "SELECT	DISTINCT " +
                "numberingplan.Country_Name, " +
                "numberingplan.Two_Digits_country_code, " +
                "numberingplan.Three_Digits_Country_Code, " +
                "numberingplan.Type_Of_Service, " +
                "fixedrate.rate as rate " +
                "FROM  " +
                "dbo.NumberingPlan as numberingplan " +
                "LEFT OUTER JOIN " +
                "dbo.[{0}] as fixedrate ON numberingplan.Dialing_prefix = fixedrate.country_code_dialing_prefix " +
                ") SRC " +
                "GROUP BY Country_Name, Two_Digits_country_code, Three_Digits_Country_Code "
                , ratesTableName);

            return sql;
        }

        public string GetNationalRatesForCountry(string ratesTableName, string iso3CountryCode)
        {
            var sql = String.Format(
                "SELECT " +
                "r.Rate_ID as 'Rate_ID', " +
                "np.Dialing_prefix as 'DialingCode', " +
                "np.Country_Name as 'CountryName', " +
                "np.Three_Digits_Country_Code as 'ISO3CountryCode', " +
                "np.Type_Of_Service as 'TypeOfService', " +
                "r.rate as 'Rate' " +
                "FROM dbo.[NumberingPlan] np " +
                "LEFT OUTER JOIN dbo.[{0}] r on np.Dialing_prefix = r.country_code_dialing_prefix " +
                "WHERE np.Three_Digits_Country_Code = '{1}' "
                , ratesTableName
                , iso3CountryCode);

            return sql;
        }

        public string GetNgnRates(string ratesTableName)
        {
            var sql = String.Format(
                "SELECT  " +
                "RateID,  " +
                "[{0}].[DialingCodeID] as DialingCodeID,  " +
                "[NGN_NumberingPlan].[DialingCode] as [DialingCode], " +
                "Countries.CountryName as CountryName,  " +
                "[NGN_NumberingPlan].[CountryCodeISO3] as CountryCodeISO3,  " +
                "[NGN_NumberingPlan].[TypeOfServiceID] as [TypeOfServiceID], " +
                "[CallTypes].[CallType] as CallType, " +
                "Rate as Rate " +
                "FROM " +
                "[{0}]  " +
                "LEFT OUTER JOIN NGN_NumberingPlan on NGN_NumberingPlan.ID = [{0}].[DialingCodeID]  " +
                "LEFT OUTER JOIN Countries on Countries.CountryCodeISO3 = [NGN_NumberingPlan].[CountryCodeISO3] " +
                "LEFT OUTER JOIN [CallTypes] on [CallTypes].id = [NGN_NumberingPlan].[TypeOfServiceID] "
                , ratesTableName);

            return sql;
        }
    }
}