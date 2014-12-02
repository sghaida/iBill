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
            //AnnouncementsDataMapper AnnouncemenetsDM = new AnnouncementsDataMapper();
            //var announcementsForRole = AnnouncemenetsDM.GetAnnouncementsForRole(2);
            //var announcementsForSite = AnnouncemenetsDM.GetAnnouncementsForSite(1);


            /***
             * SystemRoles Tests;
             */
            //SystemRolesDataMapper SystemRolesDM = new SystemRolesDataMapper();
            //var systemRoles = SystemRolesDM.GetAll().ToList<SystemRole>();


            /***
             * Sites Tests;
             */
            //SitesDataMapper SitesMapper = new SitesDataMapper();
            //var MOA = SitesMapper.GetById(29);


            /***
             * SitesDepartments Tests;
             */
            //SitesDepartmentsDataMapper SitesDepartementsMapper = new SitesDepartmentsDataMapper();
            //var sitesDepartments = SitesDepartementsMapper.GetAll();
            //var MOA_Departments = SitesDepartementsMapper.GetDepartmentsForSite(Convert.ToInt64(MOA.ID));


            /***
             * Gateways Tests;
             */
            //GatewaysDataMapper GatewaysMapper = new GatewaysDataMapper();
            //var gatewaysInfo = GatewaysMapper.GetAll(IncludeDataRelations: false);
            //var allGatewaysInfo = GatewaysMapper.GetAll();
            //var MOA_Gateways = GatewaysMapper.GetGatewaysForSite(MOA.ID); ;


            /***
             * PhoneCalls Tests;
             */
            //PhoneCallsDataMapper PhoneCallsMapper = new PhoneCallsDataMapper();
            //var MOA_Calls = PhoneCallsMapper.GetChargeableCallsForSite(MOA.Name);


            /***
             * DelegateRoles Tests;
             */
            //DelegateRolesDataMapper DelegatesMapper = new DelegateRolesDataMapper();
            //var allDelegates =  DelegatesMapper.GetAll().ToList<DelegateRole>();
            //bool isUserDelegate = DelegatesMapper.IsUserDelegate("aalhour@ccc.gr");
            //bool isDepartmentDelegate = DelegatesMapper.IsSiteDepartmentDelegate("aalhour@ccc.gr");
            //bool isSiteDelegate = DelegatesMapper.IsSiteDelegate("aalhour@ccc.gr");


            /***
             * DepartmentHeadRoles Tests;
             */
            //DepartmentHeadRolesDataMapper DepartmentHeadsMapper = new DepartmentHeadRolesDataMapper();
            //List<DepartmentHeadRole> departmentHeads = DepartmentHeadsMapper.GetAll().ToList<DepartmentHeadRole>();
            //List<SiteDepartment> siteDepartments = DepartmentHeadsMapper.GetSiteDepartmentsForUser("aalhour@ccc.gr");
            //bool isDepartmentHead = DepartmentHeadsMapper.IsDepartmentHead("aalhour@ccc.gr");
        }

    }

}
