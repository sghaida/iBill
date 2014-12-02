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
        public List<User> GetBySiteID(int SiteID)
        {
            throw new NotImplementedException();
        }


        public List<User> GetBySite(Site site)
        {
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("SiteName", site.Name);

            try
            {
                return Get(whereConditions: condition, limit: 0).ToList<User>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public List<User> GetBySearchTerm(string searchTerm)
        {
            List<User> users = new List<User>();

            var sipAccountCondition = new Dictionary<string, object>();
            sipAccountCondition.Add("SipAccount", String.Format("like '%{0}%'", searchTerm));

            var displayNameCondition = new Dictionary<string, object>();
            displayNameCondition.Add("AD_DisplayName", String.Format("like '%{0}%'", searchTerm));

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


        public Site GetSiteBySipAccount(string UserSipAccount)
        {
            Site site = null;
            User user = GetBySipAccount(UserSipAccount);

            if (user.Site != null && user.Site.ID > 0)
            {
                site = user.Site;
            }

            return site;
        }


        public Site GetSiteByUserID(int UserID)
        {
            Site site = null;
            User user = GetByUserID(UserID);

            if (user.Site != null && user.Site.ID > 0)
            {
                site = user.Site;
            }

            return site;
        }


        public Department GetDepartmentBySipAccount(string UserSipAccount)
        {
            Department department = null;
            User user = GetBySipAccount(UserSipAccount);

            if (user.Department != null && user.Department.ID > 0)
            {
                department = user.Department;
            }

            return department;
        }


        public Department GetDepartmentByUserID(int UserID)
        {
            Department department = null;
            User user = GetByUserID(UserID);

            if (user.Department != null && user.Department.ID > 0)
            {
                department = user.Department;
            }

            return department;
        }
    }
}
