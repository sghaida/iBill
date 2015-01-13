using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using ORM.Helpers;
using ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class DelegateRolesDataMapper : DataAccess<DelegateRole>
    {
        /// <summary>
        /// Given a User's SipAccount, return all the authorized Users, Sites-Departments and Sites that this user is a delegate on.
        /// </summary>
        /// <param name="DelegeeSipAccount">The Delegee SipAccount</param>
        /// <returns>List of DelegateRole</returns>
        public List<DelegateRole> GetByDelegeeSipAccount(string DelegeeSipAccount)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("DelegeeSipAccount", DelegeeSipAccount);

            try
            {
                return this.Get(whereConditions: conditions, limit: 0).ToList<DelegateRole>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a sip account and a delegation type ID, this function will return all the data that this user is managing for this kind of delegation...
        /// Managed Users, Managed Sites, Managed Sites-Departments
        /// </summary>
        /// <param name="DelegeeSipAccount">The Delegee SipAccount</param>
        /// <param name="DelegationType">The Delegation TypeID</param>
        public List<DelegateRole> GetByDelegeeSipAccount(string DelegeeSipAccount, int DelegationType)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("DelegeeSipAccount", DelegeeSipAccount);
            conditions.Add("DelegationType", DelegationType);

            try
            {
                return this.Get(whereConditions: conditions, limit: 0).ToList<DelegateRole>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }

}
