using System;
using System.Collections.Generic;
using System.Linq;

namespace LyncBillingBase.DataMappers.SQLQueries
{
    public class CallsSummariesForDestinationCountriesSQL
    {
        public string GetTopDestinationCountriesForUser(string sipAccount, string startingDate, string endingDate, int limit, List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                    "SELECT TOP ({0}) [Country_Name] as [Country], " +
                        "SUM ([Marker_CallCost]) AS [CallsCost], " +
                        "CAST (COUNT ([SessionIdTime]) AS BIGINT) [CallsCount], " +
                        "CAST (SUM ([Duration]) AS BIGINT) AS [CallsDuration] "
                    , limit
                    );

                //
                // Start the FROM_PART
                fromPart = String.Format("FROM  ( ");

                var index = 0;
                foreach (var tableName in dbTables)
                {
                    fromPart = String.Format(
                        "{0} " +
                        "SELECT * FROM [{1}] " +
                        "LEFT OUTER JOIN [NumberingPlan] ON [{1}].[Marker_CallTo] = [NumberingPlan].[Dialing_prefix] " + 
                        "WHERE " +
                            "[ChargingParty]='{2}' AND " +
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                            "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL ) AND " +
                            "[SessionIdTime] BETWEEN '{3}' AND '{4}' "
                        , fromPart
                        , tableName
                        , sipAccount
                        , startingDate
                        , endingDate
                    );

                    if (index < (dbTables.Count() - 1))
                    {
                        fromPart += " UNION ALL ";
                        index++;
                    }
                }

                // 
                // Close the FROM PART
                fromPart += String.Format(" ) AS [DestinationCountriesReport] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                        "[Marker_CallToCountry] " + 
                        "[Country_Name] " + 
                    "ORDER BY " +
                        "[CallsCount] DESC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

        public string GetTopDestinationCountriesForSiteDepartment(string siteName, string departmentName, string startingDate, string endingDate, int limit, List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                     "SELECT TOP ({0}) [Country_Name] as [Country], " +
                        "SUM ([Marker_CallCost]) AS [CallsCost], " +
                        "CAST (COUNT ([SessionIdTime]) AS BIGINT) [CallsCount], " +
                        "CAST (SUM ([Duration]) AS BIGINT) AS [CallsDuration] "
                    , limit
                    );

                //
                // Start the FROM_PART
                fromPart = String.Format("FROM  ( ");

                var index = 0;
                foreach (var tableName in dbTables)
                {
                    fromPart = String.Format(
                        "{0} " +
                        "SELECT * FROM [{1}] " + 
                        "LEFT OUTER JOIN [NumberingPlan] ON [{1}].[Marker_CallTo] = [NumberingPlan].[Dialing_prefix] " + 
                        "LEFT OUTER JOIN [ActiveDirectoryUsers] ON [{1}].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] " + 
                        "WHERE " +
                            "[AD_PhysicalDeliveryOfficeName]='{2}' AND " +
                            "[AD_Department]='{3}' AND " +
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                            "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL ) AND " +
                            "[SessionIdTime] BETWEEN '{3}' AND '{4}' "
                        , fromPart
                        , tableName
                        , siteName
                        , departmentName
                        , startingDate
                        , endingDate
                    );

                    if (index < (dbTables.Count() - 1))
                    {
                        fromPart += " UNION ALL ";
                        index++;
                    }
                }

                // 
                // Close the FROM PART
                fromPart += String.Format(" ) AS [DestinationCountriesReport] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                        "[Marker_CallToCountry] " + 
                        "[Country_Name] " + 
                    "ORDER BY " +
                        "[CallsCount] DESC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

        public string GetTopDestinationCountriesForSite(string siteName, string startingDate, string endingDate, int limit, List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                    "SELECT TOP ({0}) [Country_Name] as [Country], " +
                        "SUM ([Marker_CallCost]) AS [CallsCost], " +
                        "CAST (COUNT ([SessionIdTime]) AS BIGINT) [CallsCount], " +
                        "CAST (SUM ([Duration]) AS BIGINT) AS [CallsDuration] "
                    , limit
                    );

                //
                // Start the FROM_PART
                fromPart = String.Format("FROM  ( ");

                var index = 0;
                foreach (var tableName in dbTables)
                {
                    fromPart = String.Format(
                        "{0} " +
                        "SELECT * FROM [{1}] " + 
                        "LEFT OUTER JOIN [NumberingPlan] ON [{1}].[Marker_CallTo] = [NumberingPlan].[Dialing_prefix] " + 
                        "LEFT OUTER JOIN [ActiveDirectoryUsers] ON [{1}].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] " + 
                        "WHERE " +
                            "[AD_PhysicalDeliveryOfficeName]='{2}' AND " + 
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                            "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL ) AND " +
                            "[SessionIdTime] BETWEEN '{3}' AND '{4}' "
                        , fromPart
                        , tableName
                        , siteName
                        , startingDate
                        , endingDate
                    );

                    if (index < (dbTables.Count() - 1))
                    {
                        fromPart += " UNION ALL ";
                        index++;
                    }
                }

                // 
                // Close the FROM PART
                fromPart += String.Format(" ) AS [DestinationCountriesReport] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " + 
                        "[Marker_CallToCountry] " + 
                        "[Country_Name] " + 
                    "ORDER BY " + 
                        "[CallsCount] DESC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

    }

}