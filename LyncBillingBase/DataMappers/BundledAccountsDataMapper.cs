using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataModels;
using LyncBillingBase.DataAccess;

namespace LyncBillingBase.DataMappers
{
    public class BundledAccountsDataMapper : DataAccess<BundledAccount>
    {
        public List<string> GetAssociatedSipAccounts(string primarySipAccount)
        {
            throw new NotImplementedException();
        }
    }
}
