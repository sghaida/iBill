using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;
using LyncBillingBase.Roles;

namespace LyncBillingBase.DataMappers
{
    public class AnnouncementsDataMapper : DataAccess<Announcement>
    {
        private SitesDataMapper SitesAccessor = new SitesDataMapper();
        

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
                        item.RoleName = SystemRole.LookupRoleName(item.ForRole);
                        item.SiteName = ((Site)SitesAccessor.GetById(item.ForSite)).Name ?? string.Empty;
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

            Dictionary<string, object> conditions = new Dictionary<string, object>() {
                //ForRole DbColumn value
                { "ForSite", SiteID }
            };

            try
            {
                var sites = SitesAccessor.GetAll().ToList<Site>();
                var announcements = base.Get(whereCondition: conditions, limit: 0).ToList<Announcement>();

                announcements = announcements
                    .Select(item =>
                    {
                        item.RoleName = SystemRole.LookupRoleName(item.ForRole);
                        item.SiteName = ((Site)SitesAccessor.GetById(item.ForSite)).Name ?? string.Empty;
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
