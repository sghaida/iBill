using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;
using CCC.ORM.Helpers;

namespace LyncBillingBase.DataMappers
{
    public class UsersDataMapper : DataAccess<User>
    {
        private static SitesDataMapper _sitesDataMapper = new SitesDataMapper();

        /// <summary>
        ///     Given a Site's ID, return all the Users that belong to that Site.
        /// </summary>
        /// <param name="siteId">Site.ID (int)</param>
        /// <returns>List of User objects. Users that belong to that Site.</returns>
        public List<User> GetBySiteId(int siteId)
        {
            var site = _sitesDataMapper.GetById(siteId);

            try
            {
                return this.GetBySite(site);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Given a Site object, return all the Users that belong to that Site.
        /// </summary>
        /// <param name="Site">Site (object)</param>
        /// <returns>List of User objects. Users that belong to that Site.</returns>
        public List<User> GetBySite(Site site)
        {
            //var condition = new Dictionary<string, object>();
            //condition.Add("AD_PhysicalDeliveryOfficeName", site.Name);

            try
            {
                //return Get(condition, 0).ToList();
                var results = (new List<User>()).GetWithRelations(item => item.Site, item => item.Department).ToList() ?? (new List<User>());

                if(site != null && results.Any())
                {
                    results = results.Where(item => item.Site.Id == site.Id || item.Site.Name == site.Name).ToList();
                }

                return results;
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

            //var condition = new Dictionary<string, object>();
            //condition.Add("SipAccount", userSipAccount);

            try
            {
                //var result = Get(condition, 1).ToList();
                var result = (new List<User>()).GetWithRelations(item => item.Site, item => item.Department).ToList() ?? (new List<User>());

                if (result.Any())
                {
                    user = result.Find(item => item.SipAccount == userSipAccount);
                }

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

            //var condition = new Dictionary<string, object>();
            //condition.Add("AD_UserID", userId);

            try
            {
                //var result = Get(condition, 1).ToList();
                var result = (new List<User>()).GetWithRelations(item => item.Site, item => item.Department).ToList() ?? (new List<User>());

                if (result.Any())
                {
                    user = result.Find(item => item.EmployeeId == userId);
                }

                return user;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}