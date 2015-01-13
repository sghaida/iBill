using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class UsersDataMapper : DataAccess<User>
    {
        /// <summary>
        ///     Given a Site's ID, return all the Users that belong to that Site.
        /// </summary>
        /// <param name="siteId">Site.ID (int)</param>
        /// <returns>List of User objects. Users that belong to that Site.</returns>
        public List<User> GetBySiteId(int siteId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Given a Site object, return all the Users that belong to that Site.
        /// </summary>
        /// <param name="Site">Site (object)</param>
        /// <returns>List of User objects. Users that belong to that Site.</returns>
        public List<User> GetBySite(Site site)
        {
            var condition = new Dictionary<string, object>();
            condition.Add("AD_PhysicalDeliveryOfficeName", site.Name);

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
        ///     Given a search term (string), return all the Users whose SipAccount and/or DisplayName match that term or part of
        ///     it.
        /// </summary>
        /// <param name="searchTerm">SearchTerm (string)</param>
        /// <returns>List of User objects. Users that matched the search term.</returns>
        public List<User> GetBySearchTerm(string searchTerm)
        {
            var users = new List<User>();

            var sipAccountCondition = new Dictionary<string, object>();
            sipAccountCondition.Add("SipAccount", String.Format("like '%{0}%'", searchTerm));

            var displayNameCondition = new Dictionary<string, object>();
            displayNameCondition.Add("AD_DisplayName", String.Format("like '%{0}%'", searchTerm));

            try
            {
                var sipAccountSearch = Get(sipAccountCondition, 0).ToList();
                var displayNameSearch = Get(displayNameCondition, 0).ToList();

                if (sipAccountSearch != null && sipAccountSearch.Count > 0)
                    users = users.Concat(sipAccountSearch).ToList();

                if (displayNameSearch != null && displayNameSearch.Count > 0)
                    users = users.Concat(displayNameSearch).ToList();

                return users;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Get the User with the given SipAccount.
        /// </summary>
        /// <param name="userSipAccount">User.SipAccount (string)</param>
        /// <returns>A User object.</returns>
        public User GetBySipAccount(string userSipAccount)
        {
            User user = null;

            var condition = new Dictionary<string, object>();
            condition.Add("SipAccount", userSipAccount);

            try
            {
                var result = Get(condition, 1).ToList();

                if (result != null && result.Count > 0)
                    user = result.First();

                return user;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Get the User with the given UserID.
        /// </summary>
        /// <param name="userId">User.ID (int)</param>
        /// <returns>A User object.</returns>
        public User GetByUserId(int userId)
        {
            User user = null;

            var condition = new Dictionary<string, object>();
            condition.Add("AD_UserID", userId);

            try
            {
                var result = Get(condition, 1).ToList();

                if (result != null && result.Count > 0)
                    user = result.First();

                return user;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}