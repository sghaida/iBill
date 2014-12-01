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
        public List<CallType> GetNGNCallTypes()
        {
            try
            {
                var callTypes = GetAll().ToList<CallType>();

                callTypes = callTypes
                    .Where(type =>
                        type.Name == "NGN" ||
                        type.Name == "TOLL-FREE" ||
                        type.Name == "PUSH-TO-TALK")
                    .ToList<CallType>();

                return callTypes;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
