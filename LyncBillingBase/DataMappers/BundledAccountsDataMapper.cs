using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;





namespace LyncBillingBase.DataMappers
{
    public class BundledAccountsDataMapper : DataAccess<BundledAccount>
    {
        /// <summary>
        /// Given a User SipAccount, return all the other SipAccounts (strings) that are marked as bundled to it.
        /// </summary>
        /// <param name="primarySipAccount">Primary User SipAccount.</param>
        /// <returns>List of SipAccounts (strings).</returns>
        public List<string> GetAssociatedSipAccounts(string primarySipAccount)
        {
            List<string> associatedSipAccounts = null;

            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("PrimarySipAccount", primarySipAccount);

            try
            {
                var bundledAccounts = Get(whereConditions: condition, limit: 0).ToList<BundledAccount>();

                if(bundledAccounts != null && bundledAccounts.Count > 0)
                {
                    associatedSipAccounts = bundledAccounts.Select<BundledAccount, string>(account => account.AssociatedSipAccount).ToList<string>();
                }

                return associatedSipAccounts;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
