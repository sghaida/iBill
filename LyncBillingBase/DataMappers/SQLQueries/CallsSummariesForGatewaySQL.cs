using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataMappers.SQLQueries
{
    public class CallsSummariesForGatewaySQL
    {
        public string GetCallsSummariesForUser(string sipAccount, string startingDate, string endingDate, List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                    "SELECT TOP 100 PERCENT " +
                        "[ChargingParty] AS [ChargingParty], " + 
                        "[ToGateway] AS [GatewayName], " +
                        "YEAR(ResponseTime) AS [Year], " +
                        "MONTH(ResponseTime) AS [Month], " +
                        "CAST(SUM([Duration]) AS BIGINT) AS [CallsDuration], " +
                        "CAST(COUNT([SessionIdTime]) AS BIGINT) AS [CallsCount], " +
                        "SUM([Marker_CallCost]) AS [CallsCost], "
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
                        "LEFT OUTER JOIN [ActiveDirectoryUsers] ON [{1}].[ChargingParty] = [ActiveDirectoryUsers].[SipAccount] " +
                        "WHERE " +
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                            "([SessionIdTime] BETWEEN '{2}' AND '{3}') AND " +
                            "[ChargingParty]='{4}' AND " + 
                            "[ToGateway] IS NOT NULL  "
                        , fromPart //0
                        , tableName //1
                        , startingDate //2
                        , endingDate //3
                        , sipAccount
                    );

                    if (index < (dbTables.Count() - 1))
                    {
                        fromPart += " UNION ALL ";
                        index++;
                    }
                }

                // 
                // Close the FROM PART
                fromPart += String.Format(") AS [AllSitesCallsSummary] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                        "[ChargingParty], " + 
                        "[ToGateway], " +
                        "YEAR(ResponseTime), " +
                        "MONTH(ResponseTime) " +
                    "ORDER BY " + 
                        "[ToGateway] ASC, " +
                        "YEAR(ResponseTime) ASC, " +
                        "MONTH(ResponseTime) ASC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

        public string GetCallsSummariesForSiteDepartment(string siteName, string departmentName, string startingDate, string endingDate, List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                    "SELECT TOP 100 PERCENT " +
                        "[ToGateway] AS [GatewayName], " +
                        "YEAR(ResponseTime) AS [Year], " +
                        "MONTH(ResponseTime) AS [Month], " +
                        "CAST(SUM([Duration]) AS BIGINT) AS [CallsDuration], " +
                        "CAST(COUNT([SessionIdTime]) AS BIGINT) AS [CallsCount], " +
                        "SUM([Marker_CallCost]) AS [CallsCost], "
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
                        "LEFT OUTER JOIN [ActiveDirectoryUsers] ON [{1}].[ChargingParty] = [ActiveDirectoryUsers].[SipAccount] " +
                        "WHERE " +
                            "([SessionIdTime] BETWEEN '{2}' AND '{3}') AND " +
                            "[ActiveDirectoryUsers].[AD_PhysicalDeliveryOfficeName] = '{4}' AND " +
                            "[ActiveDirectoryUsers].[AD_Department]='{5}' AND " +
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                            "[Exclude]=0 AND " +
                            "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL) AND " +
                            "[ToGateway] IN ( " +
                                "SELECT [Gateway] " +
                                "FROM [GatewaysDetails] " +
                                    "LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] " +
                                    "LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] " +
                                 "WHERE " +
                                    "[SiteName]='{4}' " +
                             ") "
                        , fromPart //0
                        , tableName //1
                        , startingDate //2
                        , endingDate //3
                        , siteName
                        , departmentName
                    );

                    if (index < (dbTables.Count() - 1))
                    {
                        fromPart += " UNION ALL ";
                        index++;
                    }
                }

                // 
                // Close the FROM PART
                fromPart += String.Format(") AS [SiteDepartmentCallsSummary] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                        "[ToGateway], " +
                        "YEAR(ResponseTime), " +
                        "MONTH(ResponseTime) " +
                    "ORDER BY " +
                        "[ToGateway] ASC, " +
                        "YEAR(ResponseTime) ASC, " +
                        "MONTH(ResponseTime) ASC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

        public string GetCallsSummariesForSite(string siteName, string startingDate, string endingDate, List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                    "SELECT TOP 100 PERCENT " +
                        "[ToGateway] AS [GatewayName], " +

                        "YEAR(ResponseTime) AS [Year], " +
                        "MONTH(ResponseTime) AS [Month], " +
                        "(CAST(CAST(YEAR(ResponseTime) AS varchar) + '/' + CAST(MONTH(ResponseTime) AS varchar) + '/' + CAST(1 AS VARCHAR) AS DATETIME)) AS Date, " +

                        "CAST(SUM([Duration]) AS BIGINT) AS [CallsDuration], " +
                        "CAST(COUNT([SessionIdTime]) AS BIGINT) AS [CallsCount], " +
                        "SUM([Marker_CallCost]) AS [CallsCost], " +

                        "CAST(SUM(CASE WHEN [UI_CallType] = 'Business' THEN [Duration] END) AS BIGINT) AS [BusinessCallsDuration], " +
                        "CAST(COUNT(CASE WHEN [UI_CallType] = 'Business' THEN 1 END) AS BIGINT) AS [BusinessCallsCount], " +
                        "SUM(CASE WHEN [UI_CallType] = 'Business' THEN [Marker_CallCost] END) AS [BusinessCallsCost], " +
                        "CAST(SUM(CASE WHEN [UI_CallType] = 'Personal' THEN [Duration] END) AS BIGINT) AS [PersonalCallsDuration], " +
                        "CAST(COUNT(CASE WHEN [UI_CallType] = 'Personal' THEN 1 END) AS BIGINT) AS [PersonalCallsCount], " +
                        "SUM(CASE WHEN [UI_CallType] = 'Personal' THEN [Marker_CallCost] END) AS [PersonalCallsCost], " +
                        "CAST(SUM(CASE WHEN [UI_CallType] IS NULL THEN [Duration] END) AS BIGINT) AS [UnmarkedCallsDuration], " +
                        "CAST(COUNT(CASE WHEN [UI_CallType] IS NULL THEN 1 END) AS BIGINT) AS [UnmarkedCallsCount], " +
                        "SUM(CASE WHEN [UI_CallType] IS NULL THEN [Marker_CallCost] END) AS [UnmarkedCallsCost] "
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
                        "LEFT OUTER JOIN [ActiveDirectoryUsers] ON [{1}].[ChargingParty] = [ActiveDirectoryUsers].[SipAccount] " +
                        "WHERE " +
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                            "([SessionIdTime] BETWEEN '{2}' AND '{3}') AND " +
                            "[ActiveDirectoryUsers].[AD_PhysicalDeliveryOfficeName]='{4}' AND " +
                            "[Exclude]=0 AND " +
                            "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL) AND " +
                            "[ToGateway] IN ( " +
                                "SELECT [Gateway] " +
                                "FROM [GatewaysDetails] " +
                                "LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] " +
                                "LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] " +
                                 "WHERE " +
                                    "[SiteName]='{4}' " +
                             ") "
                        , fromPart //0
                        , tableName //1
                        , startingDate //2
                        , endingDate //3
                        , siteName //4
                    );

                    if (index < (dbTables.Count() - 1))
                    {
                        fromPart += " UNION ALL ";
                        index++;
                    }
                }

                // 
                // Close the FROM PART
                fromPart += String.Format(") AS [SiteCallsSummary] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                        "[ToGateway], " +
                        "YEAR(ResponseTime), " +
                        "MONTH(ResponseTime) " +
                    "ORDER BY " +
                        "[ToGateway] ASC, " +
                        "YEAR(ResponseTime) ASC, " +
                        "MONTH(ResponseTime) ASC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

        public string GetCallsSummariesForAllSites(string startingDate, string endingDate, List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                    "SELECT TOP 100 PERCENT " +
                        "[ToGateway] AS [GatewayName], " +
                        "YEAR(ResponseTime) AS [Year], " +
                        "MONTH(ResponseTime) AS [Month], " +
                        "CAST(SUM([Duration]) AS BIGINT) AS [CallsDuration], " +
                        "CAST(COUNT([SessionIdTime]) AS BIGINT) AS [CallsCount], " +
                        "SUM([Marker_CallCost]) AS [CallsCost], "
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
                        "WHERE " +
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                            "([SessionIdTime] BETWEEN '{2}' AND '{3}') AND " +
                            "[ToGateway] IS NOT NULL  "
                        , fromPart //0
                        , tableName //1
                        , startingDate //2
                        , endingDate //3
                    );

                    if (index < (dbTables.Count() - 1))
                    {
                        fromPart += " UNION ALL ";
                        index++;
                    }
                }

                // 
                // Close the FROM PART
                fromPart += String.Format(") AS [AllSitesCallsSummary] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                        "[ToGateway], " +
                        "YEAR(ResponseTime), " +
                        "MONTH(ResponseTime) " +
                    "ORDER BY " +
                        "[ToGateway] ASC, " +
                        "YEAR(ResponseTime) ASC, " +
                        "MONTH(ResponseTime) ASC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

        public string GetCallsSummariesYears(string startingDate, string endingDate, List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                    "SELECT TOP 100 PERCENT " +
                        "[ToGateway] AS [GatewayName], " +
                        "YEAR(ResponseTime) AS [Year] "
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
                        "WHERE " +
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                            "([SessionIdTime] BETWEEN '{2}' AND '{3}') AND " +
                            "[ToGateway] IS NOT NULL  "
                        , fromPart //0
                        , tableName //1
                        , startingDate //2
                        , endingDate //3
                    );

                    if (index < (dbTables.Count() - 1))
                    {
                        fromPart += " UNION ALL ";
                        index++;
                    }
                }

                // 
                // Close the FROM PART
                fromPart += String.Format(") AS [AllSitesCallsSummary] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                        "[ToGateway], " +
                        "YEAR(ResponseTime), " +
                        "MONTH(ResponseTime) " +
                    "ORDER BY " +
                        "[ToGateway] ASC, " +
                        "YEAR(ResponseTime) ASC, " +
                        "MONTH(ResponseTime) ASC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

    }

}
