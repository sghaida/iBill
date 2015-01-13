using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ORM;
using ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class CallTypesDataMapper : DataAccess<CallType>
    {
        private static List<CallType> _CallTypes = new List<CallType>();

        private void LoadCallTypes()
        {
            if(_CallTypes == null || _CallTypes.Count == 0)
            {
                _CallTypes = base.GetAll().ToList();
            }
        }


        public CallTypesDataMapper()
        {
            LoadCallTypes();
        }


        /// <summary>
        /// Return the list of NGN (Non-Geographical Numbers) Call Types.
        /// </summary>
        /// <returns>List of CallType objects.</returns>
        public List<CallType> GetNGNs()
        {
            try
            {
                return _CallTypes.Where(type => type.Name == "NGN" || type.Name == "TOLL-FREE" || type.Name == "PUSH-TO-TALK").ToList<CallType>();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public override IEnumerable<CallType> GetAll(string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _CallTypes;
        }


        public override int Insert(CallType dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            bool isContained = _CallTypes.Contains(dataObject);
            bool itExists = _CallTypes.Exists(item => item.TypeID == dataObject.TypeID && item.Name == dataObject.Name);

            if (isContained || itExists)
            {
                return -1;
            }
            else
            {
                dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
                _CallTypes.Add(dataObject);

                return dataObject.ID;
            }
        }


        public override bool Update(CallType dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var callType = _CallTypes.Find(item => item.ID == dataObject.ID);

            if (callType != null)
            {
                _CallTypes.Remove(callType);
                _CallTypes.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }


        public override bool Delete(CallType dataObject, string dataSourceName = null, GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var callType = _CallTypes.Find(item => item.ID == dataObject.ID);

            if (callType != null)
            {
                _CallTypes.Remove(callType);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            else
            {
                return false;
            }
        }
    }

}
