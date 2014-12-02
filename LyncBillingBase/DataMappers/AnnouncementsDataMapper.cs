using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class AnnouncementsDataMapper : DataAccess<Announcement>
    {
        /// <summary>
        /// Given a Role ID, return all the announcements that are associated with it
        /// </summary>
        /// <param name="RoleID">System Role ID or Delegation Type ID.</param>
        /// <returns>List of announcements objects/</returns>
        public List<Announcement> GetAnnouncementsForRole(int RoleID)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("ForRole", RoleID);

            try
            {
                var announcements = Get(whereConditions: conditions, limit: 0).ToList<Announcement>();

                return announcements;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Given a Site ID, return all the announcements that are associated with it
        /// </summary>
        /// <param name="RoleID">Site ID</param>
        /// <returns>List of announcements objects.</returns>
        public List<Announcement> GetAnnouncementsForSite(int SiteID)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("ForSite", SiteID);

            try
            {
                var announcements = Get(whereConditions: conditions, limit: 0).ToList<Announcement>();

                return announcements;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
