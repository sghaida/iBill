using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class CallTypesDataMapper : DataAccess<CallType>
    {
        private static List<CallType> _CallTypes = new List<CallType>();

        public CallTypesDataMapper()
        {
            LoadCallTypes();
        }

        private void LoadCallTypes()
        {
            if (_CallTypes == null || _CallTypes.Count == 0)
            {
                _CallTypes = base.GetAll().ToList();
            }
        }

        /// <summary>
        ///     Return the list of NGN (Non-Geographical Numbers) Call Types.
        /// </summary>
        /// <returns>List of CallType objects.</returns>
        public List<CallType> GetNGNs()
        {
            try
            {
                return
                    _CallTypes.Where(
                        type => type.Name == "NGN" || type.Name == "TOLL-FREE" || type.Name == "PUSH-TO-TALK").ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<CallType> GetAll(string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _CallTypes;
        }

        public override int Insert(CallType dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var isContained = _CallTypes.Contains(dataObject);
            var itExists = _CallTypes.Exists(item => item.TypeID == dataObject.TypeID && item.Name == dataObject.Name);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
            _CallTypes.Add(dataObject);

            return dataObject.ID;
        }

        public override bool Update(CallType dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var callType = _CallTypes.Find(item => item.ID == dataObject.ID);

            if (callType != null)
            {
                _CallTypes.Remove(callType);
                _CallTypes.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(CallType dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var callType = _CallTypes.Find(item => item.ID == dataObject.ID);

            if (callType != null)
            {
                _CallTypes.Remove(callType);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}