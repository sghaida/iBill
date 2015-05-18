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
            try
            {
                return this.GetAll()
                    .Where(delegee => delegee.DelegeeSipAccount.ToLower() == delegeeSipAccount.ToLower())
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
            try
            {
                return GetByDelegeeSipAccount(delegeeSipAccount)
                    .Where(delegee => delegee.DelegationType == delegationType)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public override IEnumerable<DelegateRole> GetAll(string dataSourceName = null, CCC.ORM.Globals.DataSource.Type dataSourceType = CCC.ORM.Globals.DataSource.Type.Default)
        {
            try
            {
                return (new List<DelegateRole>())
                    .GetWithRelations(
                        item => item.DelegeeAccount,
                        item => item.ManagedUser,
                        item => item.ManagedSiteDepartment,
                        item => item.ManagedSite)
                    .IncludeSiteDepartments();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}