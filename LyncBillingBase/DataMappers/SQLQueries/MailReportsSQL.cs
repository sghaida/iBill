using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataMappers.SQLQueries
{
    public class MailReportsSQL
    {
        public string GetMailReportsForDepartment(string siteName, string departmentName, string startingDate, string endingDate)
        {
            var tableName = "MailStatistics";
            var sqlQuery = string.Empty;
            var selectPart = string.Empty;
            var fromPart = string.Empty;
            var groupByOrderByPart = string.Empty;

            selectPart = String.Format(
                "SELECT TOP 100 PERCENT " + 
		        "SUM ([RecievedCount]) AS RecievedCount, " + 
		        "SUM ([RecievedSize]) AS RecievedSize, " + 
		        "SUM ([SentCount]) AS SentCount, " + 
		        "SUM ([SentSize]) AS SentSize "
            );

            
            fromPart = String.Format(
                "FROM [{0}] " +
                "LEFT OUTER JOIN [ActiveDirectoryUsers] ON [{0}].[EmailAddress] = [ActiveDirectoryUsers].[SipAccount] " +
                "WHERE " +
                    "[AD_PhysicalDeliveryOfficeName]='{1}' AND " +
                    "[AD_Department]='{2}' AND " +
                    "[TimeStamp] BETWEEN '{3}' AND '{4}' "
                , tableName
                , siteName
                , departmentName
                , startingDate
                , endingDate
            );

            groupByOrderByPart = String.Format("");

            sqlQuery = String.Format("{0} {1} {2}", selectPart, fromPart, groupByOrderByPart);

            return sqlQuery;
        }

    }

}
