using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using ORM;
using ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class CallMarkerStatusDataMapper : DataAccess<CallMarkerStatus>
    {
        private static List<CallMarkerStatus> _CallMarkerStatus;

        public CallMarkerStatusDataMapper() 
        {
            SetData();
        }

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

            try
            {
                var results = _CallMarkerStatus.Where(item => item.PhoneCallsTable == PhoneCallsTable && item.Type == Type);
                
                if (results != null && results.Count() > 0)
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

        public void UpdateCallMarkerStatus(string phoneCallTable, string type, string timestamp)
        {

            CallMarkerStatus markerStatus = new CallMarkerStatus();

            markerStatus.PhoneCallsTable = phoneCallTable;
            markerStatus.Type = type;
            markerStatus.Timestamp = Convert.ToDateTime(timestamp);
               

            var existingMarkerStatus = GetByPhoneCallsTableAndType(phoneCallTable, type);

            if (existingMarkerStatus == null)
            {
                Insert(markerStatus);
            }
            else
            {
                markerStatus.ID = existingMarkerStatus.ID;
                Update(markerStatus);
            }
            
        }

        public override IEnumerable<CallMarkerStatus> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _CallMarkerStatus;
        }

        public override bool Update(CallMarkerStatus dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {

            var markerstatus = _CallMarkerStatus.FirstOrDefault(item => item.ID == dataObject.ID);

            if (markerstatus != null)
            {
                _CallMarkerStatus.Remove(markerstatus);
                _CallMarkerStatus.Add(dataObject);
                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            else { return false; }
            
        }

        public override int Insert(CallMarkerStatus dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var markerstatus = _CallMarkerStatus.FirstOrDefault(
                item => 
                    item.PhoneCallsTable == dataObject.PhoneCallsTable || 
                    item.Type == dataObject.Type || 
                    item.Timestamp == dataObject.Timestamp
            );

            if (markerstatus == null)
            {
                int rowID = base.Insert(dataObject, dataSourceName, dataSourceType);
                dataObject.ID = rowID;

                _CallMarkerStatus.Add(dataObject);

                return rowID;

            }
            else { return -1; }
        }

        public void SetData() 
        {
            if (_CallMarkerStatus == null || _CallMarkerStatus.Count() == 0) 
            {
                _CallMarkerStatus = base.GetAll().ToList();
            }
        }

    }

}
