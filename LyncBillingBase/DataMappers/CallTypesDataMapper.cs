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
        private static List<CallType> _callTypes = new List<CallType>();

        public CallTypesDataMapper()
        {
            LoadCallTypes();
        }

        private void LoadCallTypes()
        {
            if (_callTypes == null || _callTypes.Count == 0)
            {
                _callTypes = base.GetAll().ToList();
            }
        }

        /// <summary>
        ///     Return the list of NGN (Non-Geographical Numbers) Call Types.
        /// </summary>
        /// <returns>List of CallType objects.</returns>
        public List<CallType> GetNgNs()
        {
            try
            {
                return
                    _callTypes.Where(
                        type => type.Name == "NGN" || type.Name == "TOLL-FREE" || type.Name == "PUSH-TO-TALK").ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<CallType> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _callTypes;
        }

        public override int Insert(CallType dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _callTypes.Contains(dataObject);
            var itExists = _callTypes.Exists(item => item.TypeId == dataObject.TypeId && item.Name == dataObject.Name);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);
            _callTypes.Add(dataObject);

            return dataObject.Id;
        }

        public override bool Update(CallType dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var callType = _callTypes.Find(item => item.Id == dataObject.Id);

            if (callType != null)
            {
                _callTypes.Remove(callType);
                _callTypes.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(CallType dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var callType = _callTypes.Find(item => item.Id == dataObject.Id);

            if (callType != null)
            {
                _callTypes.Remove(callType);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}