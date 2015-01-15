using System;
using System.Collections.Generic;
using System.Linq;

namespace LyncBillingBase.DataMappers.SQLQueries
{
    public class CallsSummariesForUsersSql
    {
        /// <summary>
        /// </summary>
        /// <param name="userSipAccount"></param>
        /// <param name="startingDate"></param>
        /// <param name="endingDate"></param>
        /// <param name="dbTables"></param>
        /// <returns></returns>
        public string GetCallsSummariesForUser(string userSipAccount, string startingDate, string endingDate,
            List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                    "SELECT TOP 100 PERCENT " +
                    "YEAR(ResponseTime) AS [Year], " +
                    "MONTH(ResponseTime) AS [Month], " +
                    "(CAST(CAST(YEAR(ResponseTime) AS varchar) + '/' + CAST(MONTH(ResponseTime) AS varchar) + '/' +CAST(1 AS VARCHAR) AS DATETIME)) AS Date, " +
                    "[ChargingParty] AS [ChargingParty], " +
                    "CAST(SUM(CASE WHEN [UI_CallType] = 'Business' THEN [Duration] END) AS BIGINT) AS [BusinessCallsDuration], " +
                    "CAST(COUNT(CASE WHEN [UI_CallType] = 'Business' THEN 1 END) AS BIGINT) AS [BusinessCallsCount], " +
                    "SUM(CASE WHEN [UI_CallType] = 'Business' THEN [Marker_CallCost] END) AS [BusinessCallsCost], " +
                    "CAST(SUM(CASE WHEN [UI_CallType] = 'Personal' THEN [Duration] END) AS BIGINT) AS [PersonalCallsDuration], " +
                    "CAST(COUNT(CASE WHEN [UI_CallType] = 'Personal' THEN 1 END) AS BIGINT) AS [PersonalCallsCount], " +
                    "SUM(CASE WHEN [UI_CallType] = 'Personal' THEN [Marker_CallCost] END) AS [PersonalCallsCost], " +
                    "CAST(SUM(CASE WHEN [UI_CallType] IS NULL THEN [Duration] END) AS BIGINT) AS [UnmarkedCallsDuration], " +
                    "CAST(COUNT (CASE WHEN [UI_CallType] IS NULL THEN 1 END) AS BIGINT) AS [UnmarkedCallsCount], " +
                    "SUM(CASE WHEN [UI_CallType] IS NULL THEN [Marker_CallCost] END) AS [UnmarkedCallsCost], " +
                    "NULL as [AC_IsInvoiced]");

                //
                // Start the FROM_PART
                fromPart = String.Format("FROM  (");

                var index = 0;
                foreach (var tableName in dbTables)
                {
                    fromPart += String.Format(
                        "SELECT * FROM [{0}] " +
                        "WHERE " +
                        "[ChargingParty]='{1}' AND " +
                        "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                        "([SessionIdTime] BETWEEN '{2}' AND '{3}') AND " +
                        "[Exclude]=0 AND " +
                        "[ToGateway] IS NOT NULL AND " +
                        "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL ) "
                        , tableName
                        , userSipAccount
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
                fromPart += String.Format(") AS [UserCallsSummary] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                    "YEAR(ResponseTime), " +
                    "MONTH(ResponseTime), " +
                    "[ChargingParty] " +
                    "ORDER BY " + 
                        "[ChargingParty] ASC, " + 
                        "YEAR(ResponseTime) ASC, " +
                        "MONTH(ResponseTime) ASC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

        /// <summary>
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="startingDate"></param>
        /// <param name="endingDate"></param>
        /// <param name="dbTables"></param>
        /// <returns></returns>
        public string GetCallsSummariesForUsersInSite(string siteName, string startingDate, string endingDate,
            List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                    "SELECT TOP 100 PERCENT " +
                    "YEAR(ResponseTime) AS [Year], " +
                    "MONTH(ResponseTime) AS [Month], " +
                    "(CAST(CAST(YEAR(ResponseTime) AS varchar) + '/' + CAST(MONTH(ResponseTime) AS varchar) + '/' +CAST(1 AS VARCHAR) AS DATETIME)) AS Date, " +
                    "[ChargingParty] AS [ChargingParty], " +
                    "NULL as [AC_IsInvoiced], " +
                    "CAST(SUM(CASE WHEN [UI_CallType] = 'Business' THEN [Duration] END) AS BIGINT) AS [BusinessCallsDuration], " +
                    "CAST(COUNT(CASE WHEN [UI_CallType] = 'Business' THEN 1 END) AS BIGINT) AS [BusinessCallsCount], " +
                    "SUM(CASE WHEN [UI_CallType] = 'Business' THEN [Marker_CallCost] END) AS [BusinessCallsCost], " +
                    "CAST(SUM(CASE WHEN [UI_CallType] = 'Personal' THEN [Duration] END) AS BIGINT) AS [PersonalCallsDuration], " +
                    "CAST(COUNT(CASE WHEN [UI_CallType] = 'Personal' THEN 1 END) AS BIGINT) AS [PersonalCallsCount], " +
                    "SUM(CASE WHEN [UI_CallType] = 'Personal' THEN [Marker_CallCost] END) AS [PersonalCallsCost], " +
                    "CAST(SUM(CASE WHEN [UI_CallType] IS NULL THEN [Duration] END) AS BIGINT) AS [UnmarkedCallsDuration], " +
                    "CAST(COUNT (CASE WHEN [UI_CallType] IS NULL THEN 1 END) AS BIGINT) AS [UnmarkedCallsCount], " +
                    "SUM(CASE WHEN [UI_CallType] IS NULL THEN [Marker_CallCost] END) AS [UnmarkedCallsCost] ");

                //
                // Start the FROM_PART
                fromPart = String.Format("FROM  (");

                var index = 0;
                foreach (var tableName in dbTables)
                {
                    // Concatenate the FROM_PART with the below
                    fromPart = String.Format(
                        "{0} " +
                        "SELECT * FROM [{1}] " +
                        "WHERE " +
                        "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                        "[Exclude]=0 AND " +
                        "([SessionIdTime] BETWEEN '{2}' AND '{3}') AND " +
                        "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL ) AND " +
                        "[ToGateway] IS NOT NULL AND " +
                        "[ToGateway] IN " +
                        "( " +
                        "SELECT [Gateway] " +
                        "FROM [GatewaysDetails] " +
                        "LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] " +
                        "LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] " +
                        "WHERE " +
                        "[SiteName]='{4}' " +
                        ")  "
                        , fromPart
                        , tableName
                        , startingDate
                        , endingDate
                        , siteName
                        );

                    if (index < (dbTables.Count() - 1))
                    {
                        // Add the UNION ALL phrase between each inner-select
                        fromPart = String.Format("{0} UNION ALL ", fromPart);
                        index++;
                    }
                }

                // 
                // Close the FROM PART
                fromPart = String.Format("{0} ) AS [UserCallsSummary] ", fromPart);

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                    "YEAR(ResponseTime), " +
                    "MONTH(ResponseTime), " +
                    "[ChargingParty], " +
                    "[AC_IsInvoiced] " +
                    "ORDER BY [ChargingParty] ASC");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

    }

}