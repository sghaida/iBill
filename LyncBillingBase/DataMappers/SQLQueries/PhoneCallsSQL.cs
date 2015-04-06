using System;
using System.Collections.Generic;
using System.Linq;

namespace LyncBillingBase.DataMappers.SQLQueries
{
    public class PhoneCallsSql
    {
        //
        // Chargeable Calls Query for a User
        public string ChargableCallsBySipAccount(List<string> _dbTables, string sipAccount)
        {
            var sqlStatment = string.Empty;

            var index = 0;
            foreach (var tableName in _dbTables)
            {
                sqlStatment += String.Format
                    (
                        "SELECT *,'{0}' AS PhoneCallsTableName FROM {0} " +
                        "WHERE " + 
                            "( [ChargingParty]='{1}' OR [UI_AssignedToUser]='{1}' ) AND " + 
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND" +
                            "[Exclude]=0 AND " +
                            "[ToGateway] IS NOT NULL AND " +
                            "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL ) "
                        , tableName
                        , sipAccount
                    );

                if (index < (_dbTables.Count() - 1))
                {
                    sqlStatment += " UNION ALL ";
                    index++;
                }
            }

            return sqlStatment;
        }

        //
        // Chargeable Calls Query for a Site Department
        public string ChargeableCallsBySiteDepartment(List<string> _dbTables, string siteName)
        {
            var sqlStatment = string.Empty;

            //var index = 0;
            //foreach (var tableName in tables)
            //{
            //    sqlStatment += String.Format
            //        (
            //            "SELECT *,'{0}' AS PhoneCallsTableName FROM {0} " +
            //            "LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [{0}].[ChargingParty] = [ActiveDirectoryUsers].[SipAccount] " +
            //            "WHERE " +
            //                "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
            //                "[Exclude]=0 AND " +
            //                "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL ) AND " +
            //                "[ToGateway] IS NOT NULL AND " +
            //                "[ToGateway] IN " +
            //                "(" +
            //                    "SELECT [Gateway] " +
            //                    "FROM [GatewaysDetails] " +
            //                    "LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] " +
            //                    "LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] " +
            //                    "WHERE [SiteName]='{1}' " +
            //                ")"
            //            , tableName
            //            , siteName
            //        );

            //    if (index < (tables.Count() - 1))
            //    {
            //        sqlStatment += " UNION ALL ";
            //        index++;
            //    }
            //}

            return sqlStatment;
        }

        //
        // Chargeable Calls Query for a Site
        public string ChargeableCallsBySiteName(List<string> _dbTables, string siteName)
        {
            var sqlStatment = string.Empty;

            var index = 0;
            foreach (var tableName in _dbTables)
            {
                sqlStatment += String.Format
                    (
                        "SELECT *,'{0}' AS PhoneCallsTableName FROM {0} " +
                        "LEFT OUTER JOIN [ActiveDirectoryUsers] ON [{0}].[ChargingParty] = [ActiveDirectoryUsers].[SipAccount] " +
                        "WHERE " +
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                            "[Exclude]=0 AND " +
                            "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL ) AND " +
                            "[ToGateway] IS NOT NULL AND " +
                            "[ToGateway] IN " +
                            "(" +
                                "SELECT [Gateway] " +
                                "FROM [GatewaysDetails] " +
                                "LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] " +
                                "LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] " +
                                "WHERE [SiteName]='{1}' " +
                            ")"
                        , tableName
                        , siteName
                    );

                if (index < (_dbTables.Count() - 1))
                {
                    sqlStatment += " UNION ALL ";
                    index++;
                }
            }

            return sqlStatment;
        }


        public string GetAllPhoneCalls(List<string> _dbTables)
        {
            var sqlStatment = string.Empty;

            var index = 0;
            foreach (var tableName in _dbTables)
            {
                sqlStatment += String.Format("SELECT *,'{0}' AS PhoneCallsTableName FROM {0} ", tableName);

                if (index < (_dbTables.Count() - 1))
                {
                    sqlStatment += " UNION ALL ";
                    index++;
                }
            }

            return sqlStatment;
        }


        public string PhoneCallsWithConditions(List<string> _dbTables, Dictionary<string, object> whereConditions)
        {
            var sqlStatment = string.Empty;

            //int index = 0;
            //foreach (string tableName in tables)
            //{
            //    sqlStatment += String.Format("SELECT *,'{0}' AS PhoneCallsTableName FROM {0} ", tableName);

            //    if (index < (tables.Count() - 1))
            //    {
            //        sqlStatment += " UNION ALL ";
            //        index++;
            //    }
            //}

            return sqlStatment;
        }


        public string GetDisputedCallsForSite(List<string> _dbTables, string siteName)
        {
            var sqlStatment = string.Empty;

            var index = 0;
            foreach (var tableName in _dbTables)
            {
                sqlStatment += String.Format
                    (
                        "SELECT *,'{0}' AS PhoneCallsTableName FROM {0} " +
                        "LEFT OUTER JOIN [ActiveDirectoryUsers] ON [{0}].[ChargingParty] = [ActiveDirectoryUsers].[SipAccount] " +
                        "WHERE " +
                            "[Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND " +
                            "[Exclude]=0 AND " +
                            "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus]='Accepted' OR ([AC_DisputeStatus] IS NULL AND [UI_CallType]='Disputed')) AND " +
                            "[ToGateway] IS NOT NULL AND " +
                            "[ToGateway] IN " +
                            "(" +
                                "SELECT [Gateway] " +
                                "FROM [GatewaysDetails] " +
                                "LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] " +
                                "LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] " +
                                "WHERE [SiteName]='{1}' " +
                            ")"
                        , tableName
                        , siteName
                    );

                if (index < (_dbTables.Count() - 1))
                {
                    sqlStatment += " UNION ALL ";
                    index++;
                }
            }

            return sqlStatment;
        }

    }

}