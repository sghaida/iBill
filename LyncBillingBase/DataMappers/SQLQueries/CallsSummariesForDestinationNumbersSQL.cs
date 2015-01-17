using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataMappers.SQLQueries
{
    public class CallsSummariesForDestinationNumbersSQL
    {
        public string GetTopDestinationNumbersForUser(string sipAccount, string startingDate, string endingDate, int limit, List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                    "SELECT TOP ({0}) ISNULL([DestinationNumberUri], " + 
                        "[DestinationUserUri]) AS [PhoneNumber], " + 
                        "[Marker_CallToCountry] AS [Country], " +
                        "SUM ([Marker_CallCost]) AS [CallsCost], " +
                        "CAST (COUNT ([SessionIdTime]) AS BIGINT) AS [CallsCount], " +
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
                fromPart += String.Format(" ) AS [DestinationNumbersReport] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                        "ISNULL([DestinationNumberUri], [DestinationUserUri]), " + 
		                "[Marker_CallToCountry] " + 
                    "ORDER BY " +
                        "[CallsCount] DESC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

        public string GetTopDestinationNumbersForSiteDepartment(string siteName, string departmentName, string startingDate, string endingDate, int limit, List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                    "SELECT TOP ({0}) ISNULL([DestinationNumberUri], " +
                        "[DestinationUserUri]) AS [PhoneNumber], " +
                        "[Marker_CallToCountry] AS [Country], " +
                        "SUM ([Marker_CallCost]) AS [CallsCost], " +
                        "CAST (COUNT ([SessionIdTime]) AS BIGINT) AS [CallsCount], " +
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
                        "LEFT OUTER JOIN [ActiveDirectoryUsers] ON [{1}].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] " +
                        "WHERE " +
                            "[AD_PhysicalDeliveryOfficeName]='{2}' AND " + 
			                "[AD_Department]='{3}' AND " + 
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                            "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL ) AND " +
                            "[SessionIdTime] BETWEEN '{4}' AND '{5}' "
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
                fromPart += String.Format(" ) AS [DestinationNumbersReport] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                        "ISNULL([DestinationNumberUri], [DestinationUserUri]), " +
                        "[Marker_CallToCountry] " +
                    "ORDER BY " +
                        "[CallsCount] DESC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

        public string GetTopDestinationNumbersForSite(string siteName, string startingDate, string endingDate, int limit, List<string> dbTables)
        {
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            if (dbTables != null && dbTables.Count > 0)
            {
                selectPart = String.Format(
                    "SELECT TOP ({0}) ISNULL([DestinationNumberUri], " +
                        "[DestinationUserUri]) AS [PhoneNumber], " +
                        "[Marker_CallToCountry] AS [Country], " +
                        "SUM ([Marker_CallCost]) AS [CallsCost], " +
                        "CAST (COUNT ([SessionIdTime]) AS BIGINT) AS [CallsCount], " +
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
                fromPart += String.Format(" ) AS [DestinationNumbersReport] ");

                groupByOrderByPart = String.Format(
                    "GROUP BY " +
                        "ISNULL([DestinationNumberUri], [DestinationUserUri]), " +
                        "[Marker_CallToCountry] " +
                    "ORDER BY " +
                        "[CallsCount] DESC ");

                sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);
            }

            return sqlQuery;
        }

    }

}
