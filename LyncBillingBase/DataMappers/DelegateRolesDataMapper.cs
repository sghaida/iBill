using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataModels;
using LyncBillingBase.Helpers;

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
                return Get(conditions, 0)
                    .GetWithRelations(
                        item => item.DelegeeAccount, 
                        item => item.ManagedUser, 
                        item => item.ManagedSite, 
                        item => item.ManagedSiteDepartment)
                    .IncludeSiteDepartments()
                    .ToList();
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
                var delegees = Get(conditions, 0);

                if(delegees != null)
                {
                    if(delegationType == 1)
                    {
                        delegees = delegees.GetWithRelations(
                                item => item.DelegeeAccount,
                                item => item.ManagedSite);
                    }
                    else if(delegationType == 2)
                    {
                        delegees = delegees.GetWithRelations(
                                item => item.DelegeeAccount,
                                item => item.ManagedSiteDepartment)
                            .IncludeSiteDepartments();
                    }
                    else if(delegationType == 3)
                    {
                        delegees = delegees.GetWithRelations(
                            item => item.DelegeeAccount,
                            item => item.ManagedUser);
                    }
                }

                return delegees.ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}