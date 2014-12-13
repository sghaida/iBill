using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataMappers.SQLQueries
{
    public class PhoneCallsSQL
    {
        public string ChargableCallsPerUser(List<string> tables, string sipAccount)
        {
            String sqlStatment = string.Empty;

            int index = 0;
            foreach (string tableName in tables)
            {
                sqlStatment += String.Format
                    (
                        "SELECT *,'{0}' AS PhoneCallsTableName FROM {0} " +
                        "WHERE ([ChargingParty]='{1}'  OR [UI_AssignedToUser]='{1}' ) AND [Marker_CallTypeID] in (1,2,3,4,5,6,21,19,22,24) AND" +
                        "[Exclude]=0 AND " +
                        "[ToGateway] IS NOT NULL AND " +
                        "([AC_DisputeStatus]='Rejected' OR [AC_DisputeStatus] IS NULL ) ", tableName, sipAccount
                    );

                if (index < (tables.Count() - 1))
                {
                    sqlStatment += " UNION ALL ";
                    index++;
                }
            }

            return sqlStatment;
        }


        public string ChargeableCallsForSite(List<string> tables, string siteName)
        {
            String sqlStatment = string.Empty;

            int index = 0;
            foreach (string tableName in tables)
            {
                sqlStatment += String.Format
                    (
                        "SELECT *,'{0}' AS PhoneCallsTableName FROM {0} " +
                        "LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [{0}].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] " +
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

                if (index < (tables.Count() - 1))
                {
                    sqlStatment += " UNION ALL ";
                    index++;
                }
            }

            return sqlStatment;
        }


        public string GetAllPhoneCalls(List<string> tables)
        {
            String sqlStatment = string.Empty;

            int index = 0;
            foreach (string tableName in tables)
            {
                sqlStatment += String.Format("SELECT *,'{0}' AS PhoneCallsTableName FROM {0} ", tableName);

                if (index < (tables.Count() - 1))
                {
                    sqlStatment += " UNION ALL ";
                    index++;
                }
            }

            return sqlStatment;
        }


        public string PhoneCallsWithConditions(List<string> tables, Dictionary<string, object> whereConditions)
        {
            String sqlStatment = string.Empty;

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

    }

}
