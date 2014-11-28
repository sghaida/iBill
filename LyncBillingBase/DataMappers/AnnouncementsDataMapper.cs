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
        private SitesDataMapper SitesAccessor = new SitesDataMapper();
        private List<Role> RolesInformation = Role.GetRolesInformation();


        //Get announcements for a specific role
        public List<Announcement> GetAnnouncementsForRole(int RoleID)
        {
            //Expression<Func<Announcement, bool>> predicate = (item => item.ForRole == RoleID);
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("ForRole", RoleID);

            try
            {
                var sites = SitesAccessor.GetAll().ToList<Site>();
                var announcements = base.Get(whereCondition: conditions, limit: 0).ToList<Announcement>();

                announcements = announcements
                    .Select(item => {
                        item.RoleName = (RolesInformation.Find(role => role.RoleID == item.ForRole) != null ? RolesInformation.Find(role => role.RoleID == item.ForRole).RoleName : string.Empty);
                        item.SiteName = (sites.Find(site => site.ID == item.ForSite) != null ? sites.Find(site => site.ID == item.ForSite).Name : string.Empty);
                        return item;
                    })
                    .ToList();

                return announcements;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<Announcement> GetAnnouncementsForSite(int SiteID)
        {
            //Expression<Func<Announcement, bool>> predicate = (item => item.ForSite == SiteID);

            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("ForSite", SiteID);

            try
            {
                var sites = SitesAccessor.GetAll().ToList<Site>();
                var announcements = base.Get(whereCondition: conditions, limit: 0).ToList<Announcement>();

                announcements = announcements
                    .Select(item =>
                    {
                        item.RoleName = (RolesInformation.Find(role => role.RoleID == item.ForRole) != null ? RolesInformation.Find(role => role.RoleID == item.ForRole).RoleName : string.Empty);
                        item.SiteName = (sites.Find(site => site.ID == item.ForSite) != null ? sites.Find(site => site.ID == item.ForSite).Name : string.Empty);
                        return item;
                    })
                    .ToList();

                return announcements;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
