using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class CallMarkerStatusDataMapper : DataAccess<CallMarkerStatus>
    {
        private static List<CallMarkerStatus> _callMarkerStatus;

        public CallMarkerStatusDataMapper()
        {
            SetData();
        }

        /// <summary>
        ///     Given a PhoneCalls Table Name, return all the CallMarkerStatus objects associated with it.
        /// </summary>
        /// <param name="phoneCallsTable">CallMarkerStatus.PhoneCallsTable (string)</param>
        /// <returns>List of CallMarkerStatus objects</returns>
        public List<CallMarkerStatus> GetByPhoneCallsTable(string phoneCallsTable)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("phoneCallsTable", phoneCallsTable);

            try
            {
                return Get(condition, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a PhoneCalls Table Name and a Call Marker Type, return the CallMarkerStatus object associated with them.
        /// </summary>
        /// <param name="phoneCallsTable">CallMarkerStatus.PhoneCallsTable (string)</param>
        /// <param name="type">CallMarkerStatus.Type (string)</param>
        /// <returns>List of CallMarkerStatus objects</returns>
        public CallMarkerStatus GetByPhoneCallsTableAndType(string phoneCallsTable, string type)
        {
            CallMarkerStatus markerStatus = null;

            try
            {
                var results =
                    _callMarkerStatus.Where(item => item.PhoneCallsTable == phoneCallsTable && item.Type == type);

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
            var markerStatus = new CallMarkerStatus();

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
                markerStatus.Id = existingMarkerStatus.Id;
                Update(markerStatus);
            }
        }

        public override IEnumerable<CallMarkerStatus> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _callMarkerStatus;
        }

        public override bool Update(CallMarkerStatus dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var markerstatus = _callMarkerStatus.FirstOrDefault(item => item.Id == dataObject.Id);

            if (markerstatus != null)
            {
                _callMarkerStatus.Remove(markerstatus);
                _callMarkerStatus.Add(dataObject);
                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override int Insert(CallMarkerStatus dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var markerstatus = _callMarkerStatus.FirstOrDefault(
                item =>
                    item.PhoneCallsTable == dataObject.PhoneCallsTable ||
                    item.Type == dataObject.Type ||
                    item.Timestamp == dataObject.Timestamp
                );

            if (markerstatus == null)
            {
                var rowId = base.Insert(dataObject, dataSourceName, dataSourceType);
                dataObject.Id = rowId;

                _callMarkerStatus.Add(dataObject);

                return rowId;
            }
            return -1;
        }

        public void SetData()
        {
            if (_callMarkerStatus == null || _callMarkerStatus.Count() == 0)
            {
                _callMarkerStatus = base.GetAll().ToList();
            }
        }
    }
}