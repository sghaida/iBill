using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;
using LyncBillingBase.DataMappers;
using LyncBillingBase.Helpers;
using LyncBillingBase.Repository;

namespace LyncBillingTesting
{
    class Program
    {
        public  static string tolower(string text)
        {
            return text.ToLower();
        }

        public static void Main(string[] args)
        {
            var _dbStorage = DataStorage.Instance;
            
            /***
             * Announcements Tests;
             */
            //var announcementsForRole = _dbStorage.Announcements.GetAnnouncementsForRole(2);
            //var announcementsForSite = _dbStorage.Announcements.GetAnnouncementsForSite(1);


            /***
             * SystemRoles Tests;
             */
            //var systemRoles = _dbStorage.SystemRoles.GetAll().ToList<SystemRole>();


            /***
             * Sites Tests;
             */
            //var MOA = _dbStorage.Sites.GetById(29);


            /***
             * SitesDepartments Tests;
             */
            //var sitesDepartments = _dbStorage.SitesDepartments.GetAll();
            //var MOA_Departments = _dbStorage.SitesDepartments.GetDepartmentsForSite(Convert.ToInt64(MOA.ID));


            /***
             * Gateways Tests;
             */
            //var gatewaysInfo = _dbStorage.Gateways.GetAll(IncludeDataRelations: false);
            //var allGatewaysInfo = _dbStorage.Gateways.GetAll();
            //var MOA_Gateways = _dbStorage.Gateways.GetGatewaysForSite(MOA.ID); ;


            /***
             * PhoneCalls Tests;
             */
            //var MOA_Calls = _dbStorage.PhoneCalls.GetChargeableCallsForSite(MOA.Name);


            /***
             * DelegateRoles Tests;
             */
            //var allDelegates =  _dbStorage.DelegateRoles.GetAll().ToList<DelegateRole>();
            //bool isUserDelegate = _dbStorage.DelegateRoles.IsUserDelegate("aalhour@ccc.gr");
            //bool isDepartmentDelegate = _dbStorage.DelegateRoles.IsSiteDepartmentDelegate("aalhour@ccc.gr");
            //bool isSiteDelegate = _dbStorage.DelegateRoles.IsSiteDelegate("aalhour@ccc.gr");


            /***
             * DepartmentHeadRoles Tests;
             */
            //List<DepartmentHeadRole> departmentHeads = _dbStorage.DepartmentHeads.GetAll().ToList<DepartmentHeadRole>();
            //List<SiteDepartment> siteDepartments = _dbStorage.DepartmentHeads.GetSiteDepartmentsForUser("aalhour@ccc.gr");
            //bool isDepartmentHead = _dbStorage.DepartmentHeads.IsDepartmentHead("aalhour@ccc.gr");


            /***
             * Users Tests
             */
            //var users = _dbStorage.Users.GetAll();
            var aalhour = _dbStorage.Users.GetBySipAccount("aalhour@ccc.gr");
            var aalhour_Site = aalhour.Site;
            var aalhour_Department = aalhour.Department;
            var aalhour_Colleagues = _dbStorage.Users.GetBySite(aalhour_Site);

            var UNKNOWN_SIPACCOUNT = "unknown@unknown.domain";
            var unknown = _dbStorage.Users.GetBySipAccount(UNKNOWN_SIPACCOUNT);
            bool isSiteNull = (unknown == null || (unknown != null && unknown.Site == null));
            var isDepartmentNull = (unknown == null || (unknown != null && unknown.Department == null));

            var unknown_systemroles = _dbStorage.SystemRoles.GetBySipAccount(UNKNOWN_SIPACCOUNT);
        }

    }

}
