using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataModels;
using LyncBillingBase.DataAccess;

namespace LyncBillingBase.DataMappers
{
    public class CallTypesDataMapper : DataAccess<CallType>
    {
        /// <summary>
        /// Return the list of NGN (Non-Geographical Numbers) Call Types.
        /// </summary>
        /// <returns>List of CallType objects.</returns>
        public List<CallType> GetNGNs()
        {
            List<CallType> NGNTypes = null;

            try
            {
                var callTypes = GetAll().ToList<CallType>();

                if(callTypes != null && callTypes.Count > 0)
                {
                    NGNTypes = callTypes.Where(type => type.Name == "NGN" || type.Name == "TOLL-FREE" || type.Name == "PUSH-TO-TALK").ToList<CallType>();
                }

                return NGNTypes;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
