﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
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
        public List<DelegateRole> GetDelegatedAccountsForUser(string DelegeeSipAccount)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("DelegeeSipAccount", DelegeeSipAccount);

            try
            {
                return Get(whereConditions: conditions, limit: 1).ToList<DelegateRole>();
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
        public List<DelegateRole> GetDelegatedAccountsForUser(string DelegeeSipAccount, int DelegationType)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("DelegeeSipAccount", DelegeeSipAccount);
            conditions.Add("DelegationType", DelegationType);

            try
            {
                return Get(whereConditions: conditions, limit: 1).ToList<DelegateRole>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Returns true or false, whether this Delegee User (SipAccount) is managing any Users
        /// </summary>
        /// <param name="DelegeeSipAccount">The Delegee User SipAccount</param>
        /// <returns>Boolean</returns>
        public bool IsUserDelegate(string DelegeeSipAccount)
        {
            DelegateRole role = null;
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("DelegeeSipAccount", DelegeeSipAccount);

            try
            {
                role = Get(whereConditions: condition, limit: 1).ToList<DelegateRole>().FirstOrDefault<DelegateRole>() ?? null;

                if (role != null)
                    return (!string.IsNullOrEmpty(role.ManagedUserSipAccount) && role.ManagedUser != null);
                else
                    return false;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Returns true or false, whether this Delegee User (SipAccount) is managing any Sites
        /// </summary>
        /// <param name="DelegeeSipAccount">The Delegee User SipAccount</param>
        /// <returns>Boolean</returns>
        public bool IsSiteDelegate(string DelegeeSipAccount)
        {
            DelegateRole role = null;
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("DelegeeSipAccount", DelegeeSipAccount);

            try
            {
                role = Get(whereConditions: condition, limit: 1).ToList<DelegateRole>().FirstOrDefault<DelegateRole>() ?? null;

                if (role != null)
                    return (role.ManagedSiteID > 0 && role.ManagedSite != null);
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        

        /// <summary>
        /// Returns true or false, whether this Delegee User (SipAccount) is managing any Sites-Departments
        /// </summary>
        /// <param name="DelegeeSipAccount">The Delegee User SipAccount</param>
        /// <returns>Boolean</returns>
        public bool IsSiteDepartmentDelegate(string DelegeeSipAccount)
        {
            DelegateRole role = null;
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("DelegeeSipAccount", DelegeeSipAccount);

            try
            {
                role = Get(whereConditions: condition, limit: 1).ToList<DelegateRole>().FirstOrDefault<DelegateRole>() ?? null;

                if (role != null)
                    return (role.ManagedSiteDepartmentID > 0 && role.ManagedSiteDepartment != null);
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }

}