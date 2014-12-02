using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class CallMarkerStatusDataMapper : DataAccess<CallMarkerStatus>
    {
        public CallMarkerStatus GetCallMarkerStatus(string phoneCallTable, string type)
        {
            throw new NotImplementedException();
        }

        public void UpdateCallMarkerStatus(string phoneCallTable, string type, string timestamp, bool Update = true)
        {
            throw new NotImplementedException();

            //if (Update == true)
            //{
            //    Dictionary<string, object> callMarkerStatusData = new Dictionary<string, object>
            //    {
            //        {Enums.GetDescription(Enums.CallMarkerStatus.Type), type},
            //        {Enums.GetDescription(Enums.CallMarkerStatus.Timestamp), timestamp},
            //        {Enums.GetDescription(Enums.CallMarkerStatus.PhoneCallsTable), phoneCallTable}

            //    };

            //    var existingMarkerStatus = CallMarkerStatus.GetCallMarkerStatus(phoneCallTable, type);

            //    if (existingMarkerStatus == null)
            //    {
            //        DBRoutines.INSERT(Enums.GetDescription(Enums.CallMarkerStatus.TableName), callMarkerStatusData);
            //    }
            //    else
            //    {
            //        //DBRoutines.UPDATE(Enums.GetDescription(Enums.CallMarkerStatus.TableName), callMarkerStatusData, whereClause);
            //        DBRoutines.UPDATE(Enums.GetDescription(Enums.CallMarkerStatus.TableName), callMarkerStatusData, Enums.GetDescription(Enums.CallMarkerStatus.MarkerId), existingMarkerStatus.MarkerId, ref conn);
            //    }
            //}
        }

    }

}
