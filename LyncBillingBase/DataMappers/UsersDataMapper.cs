using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class UsersDataMapper : DataAccess<User>
    {
        /// <summary>
        /// Given a Site's ID, return all the Users that belong to that Site.
        /// </summary>
        /// <param name="SiteID">Site.ID (int)</param>
        /// <returns>List of User objects. Users that belong to that Site.</returns>
        public List<User> GetBySiteID(int SiteID)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Given a Site object, return all the Users that belong to that Site.
        /// </summary>
        /// <param name="Site">Site (object)</param>
        /// <returns>List of User objects. Users that belong to that Site.</returns>
        public List<User> GetBySite(Site site)
        {
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("AD_PhysicalDeliveryOfficeName", site.Name);

            try
            {
                return Get(whereConditions: condition, limit: 0).ToList<User>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Given a search term (string), return all the Users whose SipAccount and/or DisplayName match that term or part of it.
        /// </summary>
        /// <param name="SearchTerm">SearchTerm (string)</param>
        /// <returns>List of User objects. Users that matched the search term.</returns>
        public List<User> GetBySearchTerm(string SearchTerm)
        {
            List<User> users = new List<User>();

            var sipAccountCondition = new Dictionary<string, object>();
            sipAccountCondition.Add("SipAccount", String.Format("like '%{0}%'", SearchTerm));

            var displayNameCondition = new Dictionary<string, object>();
            displayNameCondition.Add("AD_DisplayName", String.Format("like '%{0}%'", SearchTerm));

            try
            {
                var sipAccountSearch = Get(whereConditions: sipAccountCondition, limit: 0).ToList<User>();
                var displayNameSearch = Get(whereConditions: displayNameCondition, limit: 0).ToList<User>();

                if (sipAccountSearch != null && sipAccountSearch.Count > 0)
                    users = users.Concat(sipAccountSearch).ToList<User>();

                if (displayNameSearch != null && displayNameSearch.Count > 0)
                    users = users.Concat(displayNameSearch).ToList<User>();

                return users;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// Get the User with the given SipAccount.
        /// </summary>
        /// <param name="UserSipAccount">User.SipAccount (string)</param>
        /// <returns>A User object.</returns>
        public User GetBySipAccount(string UserSipAccount)
        {
            User user = null;

            var condition = new Dictionary<string, object>();
            condition.Add("SipAccount", UserSipAccount);

            try
            {
                var result = Get(whereConditions: condition, limit: 1).ToList<User>();

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
        /// Get the User with the given UserID.
        /// </summary>
        /// <param name="UserID">User.ID (int)</param>
        /// <returns>A User object.</returns>
        public User GetByUserID(int UserID)
        {
            User user = null;

            var condition = new Dictionary<string, object>();
            condition.Add("AD_UserID", UserID);

            try
            {
                var result = Get(whereConditions: condition, limit: 1).ToList<User>();

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
