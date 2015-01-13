using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class DelegateRolesDataMapper : DataAccess<DelegateRole>
    {
        /// <summary>
        ///     Given a User's SipAccount, return all the authorized Users, Sites-Departments and Sites that this user is a
        ///     delegate on.
        /// </summary>
        /// <param name="delegeeSipAccount">The Delegee SipAccount</param>
        /// <returns>List of DelegateRole</returns>
        public List<DelegateRole> GetByDelegeeSipAccount(string delegeeSipAccount)
        {
            var conditions = new Dictionary<string, object>();
            conditions.Add("DelegeeSipAccount", delegeeSipAccount);

            try
            {
                return Get(conditions, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a sip account and a delegation type ID, this function will return all the data that this user is managing for
        ///     this kind of delegation...
        ///     Managed Users, Managed Sites, Managed Sites-Departments
        /// </summary>
        /// <param name="delegeeSipAccount">The Delegee SipAccount</param>
        /// <param name="delegationType">The Delegation TypeID</param>
        public List<DelegateRole> GetByDelegeeSipAccount(string delegeeSipAccount, int delegationType)
        {
            var conditions = new Dictionary<string, object>();
            conditions.Add("DelegeeSipAccount", delegeeSipAccount);
            conditions.Add("DelegationType", delegationType);

            try
            {
                return Get(conditions, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}