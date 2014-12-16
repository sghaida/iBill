using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataMappers.SQLQueries
{
    public class CallsSummariesSQL
    {
        public string GetCallsSummariesForUser(string UserSipAccount, string StartingDate, string EndingDate, List<string> DBTables)
        {
            string SQL_QUERY = string.Empty;
            string SELECT_PART = string.Empty;
            string FROM_PART = string.Empty;
            string GROUP_BY_ORDER_BY_PART = string.Empty;

            if (DBTables != null && DBTables.Count > 0)
            {
                SELECT_PART = String.Format(
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
                FROM_PART = String.Format("FROM  (");

                int index = 0;
                foreach (string tableName in DBTables)
                {
                    FROM_PART += String.Format(
                            "SELECT * FROM [{0}] " + 
                            "WHERE " + 
                                "[ChargingParty]='{1}' AND " + 
                                "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " + 
                                "([SessionIdTime] BETWEEN '{2}' AND '{3}') AND " + 
                                "[Exclude]=0 AND " + 
                                "[ToGateway] IS NOT NULL AND " + 
                                "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL ) "
                            , tableName
                            , UserSipAccount
                            , StartingDate
                            , EndingDate
                        );

                    if (index < (DBTables.Count() - 1))
                    {
                        FROM_PART += " UNION ALL ";
                        index++;
                    }
                }

                // 
                // Close the FROM PART
                FROM_PART += String.Format(") AS [UserCallsSummary] ");

                GROUP_BY_ORDER_BY_PART = String.Format(
                    "GROUP BY " +
                        "YEAR(ResponseTime), " +
                        "MONTH(ResponseTime), " +
                        "[ChargingParty] " +
                    "ORDER BY [ChargingParty] ASC ");

                SQL_QUERY = String.Format("{0} {1} {2}", SELECT_PART, FROM_PART, GROUP_BY_ORDER_BY_PART);
            }

            return SQL_QUERY;
        }
    }
}
