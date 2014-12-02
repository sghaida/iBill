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
            var aalhour_Site = _dbStorage.Users.GetSiteBySipAccount(aalhour.SipAccount);
            var aalhour_Department = _dbStorage.Users.GetDepartmentBySipAccount(aalhour.SipAccount);
            var aalhour_Colleagues = _dbStorage.Users.GetBySite(aalhour_Site);

            var unknown_sipaccount = "unknown@unknown.domain";
            var unknown = _dbStorage.Users.GetBySipAccount("unknown@unknown.domain");
            var unknown_Site = _dbStorage.Users.GetSiteBySipAccount("unknown@unknown.domain");
            var unknown_Department = _dbStorage.Users.GetDepartmentBySipAccount("unknown@unknown.domain");

            var unknown_systemroles = _dbStorage.SystemRoles.GetBySipAccount(unknown_sipaccount);
        }

    }

}
