using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class AnnouncementsDataMapper : DataAccess<Announcement>
    {
        /// <summary>
        ///     Given a Role ID, return all the announcements that are associated with it
        /// </summary>
        /// <param name="roleId">System Role ID or Delegation Type ID.</param>
        /// <returns>List of announcements objects/</returns>
        public List<Announcement> GetByRoleId(int roleId)
        {
            var conditions = new Dictionary<string, object>();
            conditions.Add("ForRole", roleId);

            try
            {
                return
                    Get(conditions, 0).GetWithRelations(item => item.Role).GetWithRelations(item => item.Site).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Given a Site ID, return all the announcements that are associated with it
        /// </summary>
        /// <param name="RoleID">Site ID</param>
        /// <returns>List of announcements objects.</returns>
        public List<Announcement> GetBySiteId(int siteId)
        {
            var conditions = new Dictionary<string, object>();
            conditions.Add("ForSite", siteId);

            try
            {
                return Get(conditions, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}