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
        /// <summary>
        /// Given a PhoneCalls Table Name, return all the CallMarkerStatus objects associated with it.
        /// </summary>
        /// <param name="PhoneCallsTable">CallMarkerStatus.PhoneCallsTable (string)</param>
        /// <returns>List of CallMarkerStatus objects</returns>
        public List<CallMarkerStatus> GetByPhoneCallsTable(string PhoneCallsTable)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("phoneCallsTable", PhoneCallsTable);

            try
            {
                return Get(whereConditions: condition, limit: 0).ToList<CallMarkerStatus>();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a PhoneCalls Table Name and a Call Marker Type, return the CallMarkerStatus object associated with them.
        /// </summary>
        /// <param name="PhoneCallsTable">CallMarkerStatus.PhoneCallsTable (string)</param>
        /// <param name="Type">CallMarkerStatus.Type (string)</param>
        /// <returns>List of CallMarkerStatus objects</returns>
        public CallMarkerStatus GetByPhoneCallsTableAndType(string PhoneCallsTable, string Type)
        {
            CallMarkerStatus markerStatus = null;

            var conditions = new Dictionary<string, object>();
            conditions.Add("phoneCallsTable", PhoneCallsTable);
            conditions.Add("type", Type);

            try
            {
                var results = Get(whereConditions: conditions, limit: 1).ToList<CallMarkerStatus>();

                if (results != null && results.Count > 0)
                {
                    markerStatus = results.First();
                }

                return markerStatus;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
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
