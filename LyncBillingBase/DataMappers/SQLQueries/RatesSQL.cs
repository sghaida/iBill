using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataMappers.SQLQueries
{
    public class RatesSQL
    {
        public string GetInternationalRates(string RatesTableName)
        {
            string SQL = String.Format(
                "SELECT " +
                    "Country_Name, " +
                    "Two_Digits_country_code, " +
                    "Three_Digits_Country_Code, " +
                    "MAX(CASE WHEN Type_Of_Service <> 'gsm' THEN rate END) Fixedline, " +
                    "MAX(CASE WHEN Type_Of_Service = 'gsm' THEN rate END) GSM " +
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
                    
                , RatesTableName);

            return SQL;
        }


        public string GetNationalRatesForCountry(string RatesTableName, string CountryCodeISO3)
        {
            string SQL = String.Format(
                "SELECT " +
                    "r.Rate_ID as 'Rate_ID', " +
                    "np.Dialing_prefix as 'Dialing_prefix', " +
                    "np.Country_Name as 'Country_Name', " +
                    "np.Three_Digits_Country_Code as 'Three_Digits_Country_Code', " +
                    "np.Type_Of_Service as 'Type_Of_Service', " +
                    "r.rate as 'Rate' " +
                "FROM dbo.[NumberingPlan] np " +
                "LEFT OUTER JOIN dbo.[{0}] r on np.Dialing_prefix = r.country_code_dialing_prefix " +
                "WHERE np.Three_Digits_Country_Code = '{1}' "

                , RatesTableName
                , CountryCodeISO3);

            return SQL;
        }


        public string GetNGNRates(string RatesTableName)
        {
            string SQL = String.Format(
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
                
                , RatesTableName);

            return SQL;
        }

    }

}
