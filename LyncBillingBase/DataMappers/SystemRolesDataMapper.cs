using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class SystemRolesDataMapper : DataAccess<SystemRole>
    {
        /// <summary>
        ///     Given a User SipAccount, return the list of System Roles.
        /// </summary>
        /// <param name="SipAccount"></param>
        /// <returns>List of SystemRole objects</returns>
        public List<SystemRole> GetBySipAccount(string SipAccount)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("SipAccount", SipAccount);

            try
            {
                return Get(condition, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a User SipAccount and Role's ID, return whether this user has this role (true or false).
        /// </summary>
        /// <param name="RoleID">Role.ID (int)</param>
        /// <param name="SipAccount">User.SipAccount (string)</param>
        /// <returns>boolean</returns>
        public bool ValidateRoleForUser(int RoleID, string SipAccount)
        {
            List<SystemRole> systemRoles = null;

            var condition = new Dictionary<string, object>();
            condition.Add("SipAccount", SipAccount);
            condition.Add("RoleID", RoleID);

            try
            {
                systemRoles = Get(condition, 1).ToList();

                if (systemRoles != null && systemRoles.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a User SipAccount and Role object, return whether this user has this role (true or false).
        /// </summary>
        /// <param name="UserRole">Role (object)</param>
        /// <param name="SipAccount">User.SipAccount (string)</param>
        /// <returns>boolean</returns>
        public bool ValidateRoleForUser(Role UserRole, string SipAccount)
        {
            List<SystemRole> systemRoles = null;

            var condition = new Dictionary<string, object>();
            condition.Add("SipAccount", SipAccount);
            condition.Add("RoleID", UserRole.RoleID);

            try
            {
                systemRoles = Get(condition, 1).ToList();

                if (systemRoles != null && systemRoles.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}