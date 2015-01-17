using System;
using System.Collections.Generic;
using System.Linq;

namespace LyncBillingBase.DataMappers.SQLQueries
{
    public class CallsSummariesForDepartmentsSQL
    {
        public string GetCallsSummariesForDepartment(string siteName, string departmentName, List<string> dbTables)
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
		            "(CAST(CAST(YEAR(ResponseTime) AS varchar) + '/' + CAST(MONTH(ResponseTime) AS varchar) + '/' + CAST(1 AS VARCHAR) AS DATETIME)) AS Date, " + 
		            "CAST(SUM(CASE WHEN [UI_CallType] = 'Business' THEN [Duration] END) AS BIGINT) AS [BusinessCallsDuration], " + 
		            "CAST(COUNT(CASE WHEN [UI_CallType] = 'Business' THEN 1 END) AS BIGINT) AS [BusinessCallsCount], " +
                    "SUM(CASE WHEN [UI_CallType] = 'Business' THEN [Marker_CallCost] END) AS [BusinessCallsCost], " +
                    "CAST(SUM(CASE WHEN [UI_CallType] = 'Personal' THEN [Duration] END) AS BIGINT) AS [PersonalCallsDuration], " +
                    "CAST(COUNT(CASE WHEN [UI_CallType] = 'Personal' THEN 1 END) AS BIGINT) AS [PersonalCallsCount], " +
                    "SUM(CASE WHEN [UI_CallType] = 'Personal' THEN [Marker_CallCost] END) AS [PersonalCallsCost], " +
                    "CAST(SUM(CASE WHEN [UI_CallType] IS NULL THEN [Duration] END) AS BIGINT) AS [UnmarkedCallsDuration], " +
                    "CAST(COUNT(CASE WHEN [UI_CallType] IS NULL THEN 1 END) AS BIGINT) AS [UnmarkedCallsCount], " + 
		            "SUM(CASE WHEN [UI_CallType] IS NULL THEN [Marker_CallCost] END) AS [UnmarkedCallsCost] ");

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
                            "[AD_PhysicalDeliveryOfficeName]='{2}' AND " + 
			                "[AD_Department]='{3}' AND " + 
			                "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " + 
                            "[Exclude]=0 AND " + 
			                "[ToGateway] IS NOT NULL AND " + 
			                "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL "
                        , fromPart
                        , tableName
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
                fromPart += String.Format(") AS [DepartmentCallsSummary] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                        "YEAR(ResponseTime), " +
                        "MONTH(ResponseTime) " +
                    "ORDER BY " +
                        "YEAR(ResponseTime) ASC, " +
                        "MONTH(ResponseTime) ASC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

        public string GetCallsSummariesForDepartment(string siteName, string departmentName, string startingDate, string endingDate, List<string> dbTables)
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
                    "(CAST(CAST(YEAR(ResponseTime) AS varchar) + '/' + CAST(MONTH(ResponseTime) AS varchar) + '/' + CAST(1 AS VARCHAR) AS DATETIME)) AS Date, " +
                    "CAST(SUM(CASE WHEN [UI_CallType] = 'Business' THEN [Duration] END) AS BIGINT) AS [BusinessCallsDuration], " +
                    "CAST(COUNT(CASE WHEN [UI_CallType] = 'Business' THEN 1 END) AS BIGINT) AS [BusinessCallsCount], " +
                    "SUM(CASE WHEN [UI_CallType] = 'Business' THEN [Marker_CallCost] END) AS [BusinessCallsCost], " +
                    "CAST(SUM(CASE WHEN [UI_CallType] = 'Personal' THEN [Duration] END) AS BIGINT) AS [PersonalCallsDuration], " +
                    "CAST(COUNT(CASE WHEN [UI_CallType] = 'Personal' THEN 1 END) AS BIGINT) AS [PersonalCallsCount], " +
                    "SUM(CASE WHEN [UI_CallType] = 'Personal' THEN [Marker_CallCost] END) AS [PersonalCallsCost], " +
                    "CAST(SUM(CASE WHEN [UI_CallType] IS NULL THEN [Duration] END) AS BIGINT) AS [UnmarkedCallsDuration], " +
                    "CAST(COUNT(CASE WHEN [UI_CallType] IS NULL THEN 1 END) AS BIGINT) AS [UnmarkedCallsCount], " +
                    "SUM(CASE WHEN [UI_CallType] IS NULL THEN [Marker_CallCost] END) AS [UnmarkedCallsCost] ");

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
                            "[AD_PhysicalDeliveryOfficeName]='{2}' AND " +
                            "[AD_Department]='{3}' AND " +
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                            "[Exclude]=0 AND " +
                            "[ToGateway] IS NOT NULL AND " +
                            "([SessionIdTime] BETWEEN '{4}' AND '{5}') AND " + 
                            "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL) "
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
                fromPart += String.Format(") AS [DepartmentCallsSummary] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                        "YEAR(ResponseTime), " +
                        "MONTH(ResponseTime) " +
                    "ORDER BY " +
                        "YEAR(ResponseTime) ASC, " +
                        "MONTH(ResponseTime) ASC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }
    }
}