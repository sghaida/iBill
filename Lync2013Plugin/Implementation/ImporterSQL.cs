using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync2013Plugin.Implementation
{
    public static class SQLs
    {
        public static string CreateImportCallsQueryLync2013(DateTime LastImportedPhoneCallDate)
        {
            string SQL = string.Empty;
            string WHERE_STATEMENT = string.Empty;
            string SELECT_STATEMENT = string.Empty;
            string ORDER_BY = string.Empty;

            //Reset the time part in the date time object
            Helpers.ResetTime(ref LastImportedPhoneCallDate);

            //Process the LastImportedPhoneCallDate as it is.
            string fromDate = LastImportedPhoneCallDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //The toDate is one day after the last date
            string toDate = LastImportedPhoneCallDate.AddDays(+1).ToString("yyyy-MM-dd HH:mm:ss.fff");


            SELECT_STATEMENT = String.Format
            (
                "SELECT  " +
                    "VoipDetails.SessionIdTime,   " +
                    "VoipDetails.SessionIdSeq, " +
                    "Users_1.UserUri AS SourceUserUri,  " +
                    "Phones_1.PhoneUri AS SourceNumberUri,   " +
                    "Users_2.UserUri AS DestinationUserUri,   " +
                    "Phones_2.PhoneUri AS DestinationNumberUri,   " +
                    "REPLACE(UserCalleeURI.URI,'sip:','') AS CalleeURI, " +
                    "MediationServers.MediationServer AS FromMediationServer,   " +
                    "MediationServers_1.MediationServer AS ToMediationServer,   " +
                    "Gateways.Gateway AS FromGateway,   " +
                    "Gateways_1.Gateway AS ToGateway,   " +
                    "EdgeServers.EdgeServer AS SourceUserEdgeServer,   " +
                    "EdgeServers_1.EdgeServer AS DestinationUserEdgeServer,   " +
                    "Servers.ServerFQDN,   " +
                    "Pools.PoolFQDN,   " +
                    "SessionDetails.ResponseTime,   " +
                    "SessionDetails.SessionEndTime,   " +
                    "OnBehalf.UserUri AS OnBehalf,  " +
                    "ReferredBy.UserUri AS ReferredBy, " +
                    "CONVERT(decimal(8, 0), DATEDIFF(second, SessionDetails.ResponseTime,  SessionDetails.SessionEndTime)) AS Duration    " +
                "FROM DialogsView " +
                "RIGHT OUTER JOIN VoipDetails as VoipDetails on  " +
                    "VoipDetails.SessionIdTime = DialogsView.LsCDRSessionIdTime and  " +
                    "VoipDetails.SessionIdSeq = DialogsView.LsCDRSessionIdSeq " +
                "LEFT OUTER JOIN SessionDetails as SessionDetails on  " +
                    "SessionDetails.SessionIdTime = DialogsView.LsCDRSessionIdTime and  " +
                    "VoipDetails.SessionIdSeq = DialogsView.LsCDRSessionIdSeq " +
                "LEFT OUTER JOIN [QoEMetrics].[dbo].[Session] as QoEMetricsSession on  " +
                    "QoEMetricsSession.ConferenceDateTime =  DialogsView.QoEMetricsSessionIdTime and  " +
                    "QoEMetricsSession.SessionSeq =  DialogsView.QoEMetricsSessionIdSeq " +
                "LEFT OUTER JOIN Servers ON  " +
                    "SessionDetails.ServerId = Servers.ServerId   " +
                "LEFT OUTER JOIN Pools ON  " +
                    "SessionDetails.PoolId = Pools.PoolId   " +
                "LEFT OUTER JOIN SIPResponseMetaData ON  " +
                    "SessionDetails.ResponseCode = SIPResponseMetaData.ResponseCode   " +
                "LEFT OUTER JOIN EdgeServers AS EdgeServers_1 ON  " +
                    "SessionDetails.User2EdgeServerId = EdgeServers_1.EdgeServerId   " +
                "LEFT OUTER JOIN EdgeServers ON  " +
                    "SessionDetails.User1EdgeServerId = EdgeServers.EdgeServerId   " +
                "LEFT OUTER JOIN Users AS Users_2 ON  " +
                    "SessionDetails.User2Id = Users_2.UserId   " +
                "LEFT OUTER JOIN Users AS Users_1 ON  " +
                    "SessionDetails.User1Id = Users_1.UserId   " +
                "LEFT OUTER JOIN Users AS OnBehalf ON  " +
                    "SessionDetails.OnBehalfOfId = OnBehalf.UserId    " +
                "LEFT OUTER JOIN Users AS ReferredBy ON   " +
                    "SessionDetails.ReferredById = ReferredBy.UserId  " +
                "LEFT OUTER JOIN Gateways AS Gateways_1 ON  " +
                    "VoipDetails.ToGatewayId = Gateways_1.GatewayId   " +
                "LEFT OUTER JOIN Gateways ON  " +
                    "VoipDetails.FromGatewayId = Gateways.GatewayId   " +
                "LEFT OUTER JOIN MediationServers AS MediationServers_1 ON  " +
                    "VoipDetails.ToMediationServerId = MediationServers_1.MediationServerId   " +
                "LEFT OUTER JOIN MediationServers ON  " +
                    "VoipDetails.FromMediationServerId = MediationServers.MediationServerId   " +
                "LEFT OUTER JOIN Phones AS Phones_1 ON  " +
                    "VoipDetails.FromNumberId = Phones_1.PhoneId    " +
                "LEFT OUTER JOIN Phones AS Phones_2 ON  " +
                    "VoipDetails.ConnectedNumberId = Phones_2.PhoneId   " +
                "LEFT OUTER JOIN [QoEMetrics].[dbo].[User] AS UserCalleeURI on  " +
                    "QoEMetricsSession.CalleeURI = UserCalleeURI.UserKey "
            );

            if (LastImportedPhoneCallDate != null)
            {
                WHERE_STATEMENT = String.Format(
                    " WHERE " +
                        "Users_1.UserUri IS NOT NULL AND " +
                        "Users_1.UserUri NOT LIKE '%;phone%' AND " +
                        "Users_1.UserUri NOT LIKE '%;user%' AND " +
                        "Users_1.UserUri NOT LIKE '+%@%' AND " +
                        "SessionDetails.ResponseCode = 200 AND " +
                        "SessionDetails.MediaTypes = 16 AND " +
                        "VoipDetails.SessionIdTime between  '{0}' AND '{1}'",
                        fromDate,
                        toDate
                );
            }
            else
            {
                WHERE_STATEMENT = string.Format(
                    " WHERE " +
                    "Users_1.UserUri IS NOT NULL AND " +
                    "Users_1.UserUri NOT LIKE '%;phone%' AND " +
                    "Users_1.UserUri NOT LIKE '%;user%' AND " +
                    "Users_1.UserUri NOT LIKE '+%@%' AND " +
                    "SessionDetails.ResponseCode = 200 AND " +
                    "SessionDetails.MediaTypes = 16 "                    
                );
            }

            ORDER_BY = " ORDER BY VoipDetails.SessionIdTime ASC ";

            return SELECT_STATEMENT + WHERE_STATEMENT + ORDER_BY;
        }

        public static string GetLastImportedPhonecallDate(string tableName, bool isRemote )
        {
            if(isRemote == false)
                return string.Format("SELECT MAX(SessionIdTime) as SessionIdTime FROM {0}", tableName);
            else
                return string.Format("SELECT MIN(LsCDRSessionIdTime) as SessionIdTime FROM {0}", tableName);
        }

        
    }
}
