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
            bool status = false;

            
            /***
             * 
             */

        }


        public static void InsertUpdateDeleteTests()
        {
            DataStorage _STORAGE = DataStorage.Instance;

            bool status = false;


            /***
             * TESTING GATEWAYS INFO DATA MAPPER
             */
            Gateway newGateway = new Gateway()
            {
                Name = "Sameer"
            };

            newGateway.ID = _STORAGE.Gateways.Insert(newGateway);

            GatewayInfo newGatewayInfo = new GatewayInfo()
            {
                Description = "New Info for Gateway",
                GatewayID = newGateway.ID,
                PoolID = 2,
                SiteID = 29
            };

            _STORAGE.GatewaysInfo.Insert(newGatewayInfo);

            var AllGatewayInfo = _STORAGE.GatewaysInfo.GetByGatewayID(newGatewayInfo.GatewayID).ToList();

            newGatewayInfo.Description = "Info For Gateway - UPDATED.";

            status = _STORAGE.GatewaysInfo.Update(newGatewayInfo);
            status = _STORAGE.GatewaysInfo.Delete(newGatewayInfo);
            status = _STORAGE.Gateways.Delete(newGateway);



            /***
             * TESTING GATEWAYS DATA MAPPER
             */
            Gateway newGateway = new Gateway()
            {
                Name = "Sameer"
            };

            newGateway.ID = _STORAGE.Gateways.Insert(newGateway);

            newGateway.Name = "Sameer-02";

            status = _STORAGE.Gateways.Update(newGateway);
            status = _STORAGE.Gateways.Delete(newGateway);



            /***
             * TESTING DIDs DATA MAPPER
             */
            DID newDID = new DID()
            {
                Description = "SAMPLE DID",
                Regex = "SAMPLE REGEX",
                SiteID = 29
            };

            newDID.ID = _STORAGE.DIDs.Insert(newDID);

            newDID = _STORAGE.DIDs.GetById(newDID.ID);

            newDID.Description = "SAMPLE DID - UPDATED DESCRIPTION";

            status = _STORAGE.DIDs.Update(newDID);
            status = _STORAGE.DIDs.Delete(newDID);



            /***
             * TESTING DEPARTMENTS
             */
            Department department = new Department()
            {
                Name = "SUMURMUR",
                Description = "Sumurmur Department"
            };

            department.ID = _STORAGE.Departments.Insert(department);

            department = _STORAGE.Departments.GetById(department.ID);

            department.Description = "Never mind!";

            status = _STORAGE.Departments.Update(department);
            status = _STORAGE.Departments.Delete(department);


            /***
             * TESTING DEPARTMENT HEAD ROLES
             */
            var MOA = _STORAGE.Sites.GetById(29);
            var MOA_ISD = _STORAGE.SitesDepartments.GetBySiteID(29).ToList<SiteDepartment>().Find(item => item.Department.Name == "ISD");
            DepartmentHeadRole depHead = new DepartmentHeadRole()
            {
                SipAccount = "aalhour@ccc.gr",
                SiteDepartmentID = MOA_ISD.ID
            };

            depHead.ID = _STORAGE.DepartmentHeads.Insert(depHead);

            depHead = _STORAGE.DepartmentHeads.GetById(depHead.ID);

            depHead.SipAccount = "sghaida@ccc.gr";

            status = _STORAGE.DepartmentHeads.Update(depHead);
            status = _STORAGE.DepartmentHeads.Delete(depHead);



            /***
             * TESTING DELEGATE ROLES DATA MAPPER
             */
            MOA = _STORAGE.Sites.GetById(29);
            MOA_ISD = _STORAGE.SitesDepartments.GetBySiteID(29).ToList<SiteDepartment>().Find(item => item.Department.Name == "ISD");

            DelegateRole userDelegate = new DelegateRole()
            {
                DelegationType = 3,
                DelegeeSipAccount = "aalhour@ccc.gr",
                ManagedUserSipAccount = "ghassan@ccc.gr"
            };

            DelegateRole departmentDelegate = new DelegateRole()
            {
                DelegationType = 2,
                DelegeeSipAccount = "aalhour@ccc.gr",
                ManagedSiteDepartmentID = MOA_ISD.ID
            };

            DelegateRole siteDelegate = new DelegateRole()
            {
                DelegationType = 1,
                DelegeeSipAccount = "aalhour@ccc.gr",
                ManagedSiteID = MOA.ID
            };

            userDelegate.ID = _STORAGE.DelegateRoles.Insert(userDelegate);
            departmentDelegate.ID = _STORAGE.DelegateRoles.Insert(departmentDelegate);
            siteDelegate.ID = _STORAGE.DelegateRoles.Insert(siteDelegate);

            var allRoles = _STORAGE.DelegateRoles.GetByDelegeeSipAccount(userDelegate.DelegeeSipAccount);

            userDelegate.DelegeeSipAccount = "sghaida@ccc.gr";
            departmentDelegate.DelegeeSipAccount = "sghaida@ccc.gr";
            siteDelegate.DelegeeSipAccount = "sghaida@ccc.gr";

            status = _STORAGE.DelegateRoles.Update(userDelegate);
            status = _STORAGE.DelegateRoles.Update(departmentDelegate);
            status = _STORAGE.DelegateRoles.Update(siteDelegate);

            allRoles = _STORAGE.DelegateRoles.GetByDelegeeSipAccount(userDelegate.DelegeeSipAccount);

            status = _STORAGE.DelegateRoles.Delete(userDelegate);
            status = _STORAGE.DelegateRoles.Delete(departmentDelegate);
            status = _STORAGE.DelegateRoles.Delete(siteDelegate);



            /***
             * TESTING Currencies DATA MAPPER
             */
            Currency newCurrency = new Currency()
            {
                ISO3Code = "ZVZ",
                Name = "ZeeVeeZee"
            };
            newCurrency.ID = _STORAGE.Currencies.Insert(newCurrency);
            newCurrency.Name = "ZeeeeVeZe";
            status = _STORAGE.Currencies.Update(newCurrency);
            status = _STORAGE.Currencies.Delete(newCurrency);



            /***
             * TESTING COUNTRIES DATA MAPPER
             */
            var EURO = _STORAGE.Currencies.GetByISO3Code("EUR");
            Country newCountry = new Country()
            {
                CurrencyID = EURO.ID,
                ISO2Code = "ZZ",
                ISO3Code = "ZVZ",
                Name = "ZeeVeeZee"
            };
            newCountry.ID = _STORAGE.Countries.Insert(newCountry);
            newCountry = _STORAGE.Countries.GetById(newCountry.ID);
            newCountry.ISO3Code = "VVZ";
            newCountry.Name = "VeeVeeZee";
            status = _STORAGE.Countries.Update(newCountry);
            status = _STORAGE.Countries.Delete(newCountry);



            /***
             * TESTING CALL TYPES DATA MAPPER
             */
            CallType newCallType = new CallType()
            {
                Description = "Testing call types data mapper.",
                Name = "TEST-CALL-TYPE",
                TypeID = 2345123
            };

            newCallType.ID = _STORAGE.CallTypes.Insert(newCallType);
            
            newCallType.Description = newCallType.Description.ToUpper();
            newCallType.Name = (newCallType.Name + "-02").ToUpper();

            status = _STORAGE.CallTypes.Update(newCallType);
            status = _STORAGE.CallTypes.Delete(newCallType);



            /***
             * TESTING CALL MARKER STATUS DATA MAPPER
             */
            CallMarkerStatus newMarkerStatus = new CallMarkerStatus()
            {
                PhoneCallsTable = "PhoneCalls2015",
                Timestamp = DateTime.Now,
                Type = GLOBALS.CallMarkerStatus.Type.CallsMarking.Value()
            };

            newMarkerStatus.ID = _STORAGE.CallMarkers.Insert(newMarkerStatus);
            
            newMarkerStatus.Type = GLOBALS.CallMarkerStatus.Type.ApplyingRates.Value();
            newMarkerStatus.Timestamp = DateTime.Now;
            
            status = _STORAGE.CallMarkers.Update(newMarkerStatus);
            status = _STORAGE.CallMarkers.Delete(newMarkerStatus);



            /***
             * TESTING BUNDLED ACCOUNTS
             */
            BundledAccount bundled1 = new BundledAccount()
            {
                PrimarySipAccount = "aalhour@ccc.gr",
                AssociatedSipAccount = "sghaida@ccc.gr"
            };

            BundledAccount bundled2 = new BundledAccount()
            {
                PrimarySipAccount = "aalhour@ccc.gr",
                AssociatedSipAccount = "ghassan@ccc.gr"
            };

            bundled1.ID = _STORAGE.BundledAccounts.Insert(bundled1);
            bundled2.ID = _STORAGE.BundledAccounts.Insert(bundled2);
            
            var allBundled = _STORAGE.BundledAccounts.GetAll();
            var aalhourBundled = _STORAGE.BundledAccounts.GetAssociatedSipAccounts("aalhour@ccc.gr");

            bundled2.AssociatedSipAccount = "nafez@ccc.gr";
            
            status = _STORAGE.BundledAccounts.Update(bundled2);
            status = _STORAGE.BundledAccounts.Delete(bundled1);
            status = _STORAGE.BundledAccounts.Delete(bundled2);



            /***
             * TESTING ANNOUNCEMENTS 
             */
            Announcement ann = new Announcement
            {
                ForRole = 10,
                ForSite = 29,
                PublishOn = DateTime.Now,
                AnnouncementBody = "Hello Developer."
            };

            ann.ID = _STORAGE.Announcements.Insert(ann);

            ann = _STORAGE.Announcements.GetById(ann.ID);
            
            ann.AnnouncementBody = "Hello Developer. Things have changed.";
            
            status = _STORAGE.Announcements.Update(ann);
            status = _STORAGE.Announcements.Delete(ann);
        }

    }

}
