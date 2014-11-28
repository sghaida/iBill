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
    public class DelegatesRolesMapper : DataAccess<DelegateRole>
    {
        public bool IsUserDelegate(string userSipAccount)
        {
            throw new NotImplementedException();
        }

        public bool IsSiteDelegate(string userSipAccount)
        {
            throw new NotImplementedException();
        }

        public bool IsDepartmentDelegate(string userSipAccount)
        {
            throw new NotImplementedException();
        }

        public List<DelegateRole> GetDelegees(string userSipAccount, int DelegateTypeID)
        {
            throw new NotImplementedException();
        }
    }
}
