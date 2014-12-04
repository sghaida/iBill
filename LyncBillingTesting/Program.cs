using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase;
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
            DataStorage _STORAGE = DataStorage.Instance;

            /***
             * DelegateRoles Tests;
             */
            var allDelegates = _STORAGE.DelegateRoles.GetAll().ToList<DelegateRole>();
            //bool isUserDelegate = _STORAGE.DelegateRoles.IsUserDelegate("aalhour@ccc.gr");
            //bool isDepartmentDelegate = _STORAGE.DelegateRoles.IsSiteDepartmentDelegate("aalhour@ccc.gr");
            //bool isSiteDelegate = _STORAGE.DelegateRoles.IsSiteDelegate("aalhour@ccc.gr");


            //var markerStatus = _STORAGE.CallMarkers.GetAll();
            //var all_2010_status = _STORAGE.CallMarkers.GetByPhoneCallsTable("PhoneCalls2010");
            //var rates_applier_2010 = _STORAGE.CallMarkers.GetByPhoneCallsTableAndType("PhoneCalls2010", GLOBALS.CallMarkerStatus.Type.ApplyingRates.Value());
            //var calls_marker_2013 = _STORAGE.CallMarkers.GetByPhoneCallsTableAndType("PhoneCalls2013", GLOBALS.CallMarkerStatus.Type.CallsMarking.Value());

            string x = string.Empty;


            /**
             * Monitoring Servers Info Tests
             */
            //var monServersInfo = _STORAGE.MonitoringServers.GetAll().ToList<MonitoringServerInfo>();
            //var monServersInfoMap = _STORAGE.MonitoringServers.GetMonitoringServersInfoMap();


            /***
             * Numbering Plan Tests
             */
            //var countries = _STORAGE.Countries.GetAll();
            //var GREECE = _STORAGE.Countries.GetByISO3Code("GRC");
            //var allPlan = _STORAGE.NumberingPlans.GetAll().ToList<NumberingPlan>();
            //var greeceNumberingPlan = _STORAGE.NumberingPlans.GetByISO3CountryCode(GREECE.ISO3Code);
            //var allNGNPlan = _STORAGE.NumberingPlansForNGN.GetAll().ToList<NumberingPlanForNGN>();
            //var greeceNGNPlan = _STORAGE.NumberingPlansForNGN.GetByISO3CountryCode(GREECE.ISO3Code);


            /***
             * Users Tests
             */
            //var users = _STORAGE.Users.GetAll();
            //var aalhour = _STORAGE.Users.GetBySipAccount("aalhour@ccc.gr");
            //var aalhour_Site = aalhour.Site;
            //var aalhour_Department = aalhour.Department;
            //var aalhour_Colleagues = _STORAGE.Users.GetBySite(aalhour_Site);
            //UNKNOWN USER TEST
            //var UNKNOWN_SIPACCOUNT = "unknown@unknown.domain";
            //var unknown = _STORAGE.Users.GetBySipAccount(UNKNOWN_SIPACCOUNT);
            //bool isSiteNull = (unknown == null || (unknown != null && unknown.Site == null));
            //var isDepartmentNull = (unknown == null || (unknown != null && unknown.Department == null));


            /***
             * Department Head Roles Tests
             */
            //var MOA_ISD = _STORAGE.SitesDepartments.GetBySiteID(aalhour.Site.ID).Find(item => item.Department != null && item.Department.Name == "ISD");
            //var departmentHeads = _STORAGE.DepartmentHeads.GetAll();
            //var MOA_ISD_DepartmentHeads = _STORAGE.DepartmentHeads.GetBySiteDepartmentID(MOA_ISD.ID);
            //var isAAlhourDepartmenHead = _STORAGE.DepartmentHeads.IsDepartmentHead(aalhour.SipAccount);


            /***
             * Phone Call Exclusions Tests
             */
            //var exclusion = _STORAGE.PhoneCallsExclusions.GetAll();
            //var aalhour = _STORAGE.Users.GetBySipAccount("aalhour@ccc.gr");
            //var MOA_Exclusions = _STORAGE.PhoneCallsExclusions.GetBySiteID(aalhour.Site.ID);
            //var MOA_Sources = _STORAGE.PhoneCallsExclusions.GetSourcesBySiteID(aalhour.Site.ID);
            //var MOA_Destinations = _STORAGE.PhoneCallsExclusions.GetDestinationsBySiteID(aalhour.Site.ID);


            /***
             * Announcements Tests;
             */
            //var announcementsForRole = _STORAGE.Announcements.GetAnnouncementsForRole(2);
            //var announcementsForSite = _STORAGE.Announcements.GetAnnouncementsForSite(1);


            /***
             * SystemRoles Tests;
             */
            //var systemRoles = _STORAGE.SystemRoles.GetAll().ToList<SystemRole>();


            /***
             * Sites Tests;
             */
            //var MOA = _STORAGE.Sites.GetById(29);


            /***
             * SitesDepartments Tests;
             */
            //var sitesDepartments = _STORAGE.SitesDepartments.GetAll();
            //var MOA_Departments = _STORAGE.SitesDepartments.GetDepartmentsForSite(Convert.ToInt64(MOA.ID));


            /***
             * Gateways Tests;
             */
            //var gatewaysInfo = _STORAGE.Gateways.GetAll(IncludeDataRelations: false);
            //var allGatewaysInfo = _STORAGE.Gateways.GetAll();
            //var MOA_Gateways = _STORAGE.Gateways.GetGatewaysForSite(MOA.ID); ;


            /***
             * PhoneCalls Tests;
             */
            //var MOA_Calls = _STORAGE.PhoneCalls.GetChargeableCallsForSite(MOA.Name);

        }

    }

}
