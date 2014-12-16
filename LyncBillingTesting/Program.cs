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
            bool status = false;
            DataStorage _STORAGE = DataStorage.Instance;
            

            //var allDelegates = _STORAGE.DelegateRoles.GetAll();
            //allDelegates = allDelegates.Include(item => 
            //    item.ManagedSite,
            //    item => item.ManagedUser,
            //    item => item.DelegeeAccount);
            //allDelegates = allDelegates.IncludeSiteDepartments();


            //allDelegates = allDelegates.Include(item => item.ManagedSite);
            //allDelegates = allDelegates.Include(item => item.ManagedUser);
            //allDelegates = allDelegates.Include(item => item.DelegeeAccount);


            var allSitesDepartments = _STORAGE.SitesDepartments.GetAll();


            Site newSite = new Site()
            {
                CountryCode = "GRC",
                Description = "Sample Description",
                Name = "TEST-SITE"
            };

            Department newDepartment = new Department()
            {
                Name = "TEST-DEPARTMENT",
                Description = "Sample Description"
            };

            newSite.ID = _STORAGE.Sites.Insert(newSite);
            newDepartment.ID = _STORAGE.Departments.Insert(newDepartment);

            SiteDepartment newSiteDepartment = new SiteDepartment()
            {
                SiteID = newSite.ID,
                DepartmentID = newDepartment.ID
            };

            newSiteDepartment.ID = _STORAGE.SitesDepartments.Insert(newSiteDepartment);

            allSitesDepartments = _STORAGE.SitesDepartments.GetAll();

            newSiteDepartment.SiteID = 29;

            status = _STORAGE.SitesDepartments.Update(newSiteDepartment);

            newSiteDepartment = _STORAGE.SitesDepartments.GetById(newSiteDepartment.ID);

            allSitesDepartments = _STORAGE.SitesDepartments.GetAll();

            status = _STORAGE.SitesDepartments.Delete(newSiteDepartment);

            status = _STORAGE.Departments.Delete(newDepartment);

            status = _STORAGE.Sites.Delete(newSite);

            string x = string.Empty;


            /***
             * TESTING PHONE CALLS DATA MAPPER
             */
            //string PhoneCallsTable = "PhoneCalls2013";
            //var phoneCalls = _STORAGE.PhoneCalls.GetChargableCallsPerUser("aalhour@ccc.gr");

            //PhoneCall phoneCall = phoneCalls.First();
               
            //phoneCall.ChargingParty = "sameeer@ccc.gr";
            //phoneCall.SessionIdTime = HelperFunctions.ConvertDate(DateTime.Now, excludeHoursAndMinutes: true);

            //_STORAGE.PhoneCalls.Insert(phoneCall, dataSourceName: PhoneCallsTable);

            //phoneCalls = _STORAGE.PhoneCalls.GetChargableCallsPerUser(phoneCall.ChargingParty);

            //phoneCall.UI_MarkedOn = HelperFunctions.ConvertDate(DateTime.Now, excludeHoursAndMinutes: true);
            //phoneCall.UI_UpdatedByUser = "sameeer@ccc.gr";
            //phoneCall.UI_CallType = GLOBALS.PhoneCalls.CallTypes.Personal.Value();

            //status = _STORAGE.PhoneCalls.Update(phoneCall, dataSourceName: PhoneCallsTable);
            //status = _STORAGE.PhoneCalls.Delete(phoneCall, dataSourceName: PhoneCallsTable);

            //var depheads = _STORAGE.DepartmentHeads.GetAll();
        }


        public static void InsertUpdateDeleteTests()
        {
            DataStorage _STORAGE = DataStorage.Instance;

            bool status = false;



            /***
             * TESTING USERS DATA MAPPER
             */
            var allUsers = _STORAGE.Users.GetAll();

            User newUser = new User()
            {
                EmployeeID = 99887766,
                SipAccount = "unknown@ccc.gr",
                FullName = "UNKNOWN SAMPLE USER",
                DisplayName = "UNKNOWN USER",
                SiteName = "MOA",
                DepartmentName = "ISD",
                NotifyUser = "N",
                TelephoneNumber = "12334545667",
                UpdatedAt = DateTime.MinValue,
                //UpdatedByAD = 1,
                CreatedAt = DateTime.Now
            };

            _STORAGE.Users.Insert(newUser);

            newUser = _STORAGE.Users.GetBySipAccount(newUser.SipAccount);

            newUser.DisplayName = "UNKNOWN";

            status = _STORAGE.Users.Update(newUser);
            status = _STORAGE.Users.Delete(newUser);



            /***
             * TESTING SYSTEM ROLES
             */
            var DEVELOPER_ROLE = _STORAGE.Roles.GetByRoleID(10);

            SystemRole systemRole = new SystemRole()
            {
                Description = "TESTING SYSTEM ROLE",
                RoleID = DEVELOPER_ROLE.RoleID,
                SipAccount = "nafez@ccc.gr",
                SiteID = 29
            };

            systemRole.ID = _STORAGE.SystemRoles.Insert(systemRole);

            systemRole = _STORAGE.SystemRoles.GetById(systemRole.ID);

            systemRole.SiteID = 31;

            status = _STORAGE.SystemRoles.Update(systemRole);

            systemRole = _STORAGE.SystemRoles.GetById(systemRole.ID);

            status = _STORAGE.SystemRoles.Delete(systemRole);



            /***
             * TESTING SITES DEPARTMENTS DATA MAPPER
             */
            var moa_site = _STORAGE.Sites.GetById(29);
            var raso_site = _STORAGE.Sites.GetById(31);
            var isd_department = _STORAGE.Departments.GetByName("ISD");

            SiteDepartment siteDepartment = new SiteDepartment()
            {
                SiteID = moa_site.ID,
                DepartmentID = isd_department.ID
            };

            siteDepartment.ID = _STORAGE.SitesDepartments.Insert(siteDepartment);

            siteDepartment = _STORAGE.SitesDepartments.GetById(siteDepartment.ID);

            siteDepartment.SiteID = raso_site.ID;

            status = _STORAGE.SitesDepartments.Update(siteDepartment);

            siteDepartment = _STORAGE.SitesDepartments.GetById(siteDepartment.ID);

            status = _STORAGE.SitesDepartments.Delete(siteDepartment);



            /***
             * TESTING SITES DATA MAPPER
             */
            Site site = new Site()
            {
                CountryCode = "GRC",
                Description = "SAMPLE GREECE SITE",
                Name = "SAMPLE SITE"
            };

            site.ID = _STORAGE.Sites.Insert(site);

            site = _STORAGE.Sites.GetById(site.ID);

            site.Name = "sample sample site";

            status = _STORAGE.Sites.Update(site);
            status = _STORAGE.Sites.Delete(site);



            /***
             * TESTING ROLES DATA MAPPER
             */
            Role role123 = new Role()
            {
                RoleDescription = "SAMPLE ROLE",
                RoleID = 123123,
                RoleName = "SAMPLE-01"
            };

            role123.ID = _STORAGE.Roles.Insert(role123);

            role123.RoleName = "sample-02.01";

            status = _STORAGE.Roles.Update(role123);
            status = _STORAGE.Roles.Delete(role123);



            /***
             * TESTING NGN RATES DATA MAPPER
             */
            RateForNGN NGNRate = new RateForNGN()
            {
                DialingCodeID = 1,
                Rate = Convert.ToDecimal(15.45)
            };

            NGNRate.ID = 19; //_STORAGE.RatesForNGN.Insert(NGNRate, 10);

            NGNRate = _STORAGE.RatesForNGN.GetByGatewayID(10).Find(rate => rate.ID == 19);

            NGNRate.Rate = Convert.ToDecimal(20.45);

            status = _STORAGE.RatesForNGN.Update(NGNRate, 10);
            status = _STORAGE.RatesForNGN.Delete(NGNRate, 10);



            /***
             * TESTING POOLS DATA MAPPER
             */
            Pool newPool = new Pool()
            {
                FQDN = "TESTING POOL FQDN"
            };

            newPool.ID = _STORAGE.Pools.Insert(newPool);

            newPool.FQDN = "CHANGED FQDN";

            status = _STORAGE.Pools.Update(newPool);
            status = _STORAGE.Pools.Delete(newPool);



            /***
             * TESTING PHONE CALLS EXCLUSIONS
             */
            PhoneCallExclusion exclusion = new PhoneCallExclusion()
            {
                ExclusionSubject = "aalhour@ccc.gr",
                Description = "SAMPLE EXCLUSION TEST",
                ExclusionType = GLOBALS.PhoneCallExclusion.Type.Source.Value(),
                SiteID = 29,
                ZeroCost = GLOBALS.PhoneCallExclusion.ZeroCost.No.Value(),
                AutoMark = GLOBALS.PhoneCallExclusion.AutoMark.Business.Value()
            };

            exclusion.ID = _STORAGE.PhoneCallsExclusions.Insert(exclusion);

            exclusion = _STORAGE.PhoneCallsExclusions.GetById(789);

            exclusion.AutoMark = "";
            exclusion.ZeroCost = GLOBALS.PhoneCallExclusion.ZeroCost.Yes.Value();

            status = _STORAGE.PhoneCallsExclusions.Update(exclusion);
            status = _STORAGE.PhoneCallsExclusions.Delete(exclusion);



            /***
             * TESTING PHONE BOOK CONTACTS DATA MAPPER
             */
            PhoneBookContact contact = new PhoneBookContact()
            {
                DestinationCountry = "JOR",
                DestinationNumber = "123123123123123123",
                Name = "SAMPLE CONTACT",
                SipAccount = "aalhour@ccc.gr",
                Type = "Personal"
            };

            contact.ID = _STORAGE.PhoneBooks.Insert(contact);

            var allContacts = _STORAGE.PhoneBooks.GetBySipAccount(contact.SipAccount);

            contact.Name = "SAMPLE PHONE BOOK CONTANCT NAME";
            contact.Type = "Business";

            status = _STORAGE.PhoneBooks.Update(contact);
            status = _STORAGE.PhoneBooks.Delete(contact);



            /***
             * TESTING NUMBERING PLAN FOR NGN
             */
            NumberingPlanForNGN NGN = new NumberingPlanForNGN()
            {
                Description = "TEST NGN",
                DialingCode = "8000000000000",
                ISO3CountryCode = "GRC",
                TypeOfServiceID = 6,
                Provider = "SAMPLE PROVIDER"
            };

            NGN.ID = _STORAGE.NumberingPlansForNGN.Insert(NGN);

            NGN = _STORAGE.NumberingPlansForNGN.GetById(NGN.ID);

            NGN.Description = "TEST NGN - UPDATED.";

            status = _STORAGE.NumberingPlansForNGN.Update(NGN);
            status = _STORAGE.NumberingPlansForNGN.Delete(NGN);



            /***
             * TESTING NUMBER PLAN DATA MAPPER
             */
            NumberingPlan numPlan = new NumberingPlan()
            {
                City = "Xin Che Bang",
                CountryName = "ChinaYaNa",
                DialingPrefix = 90909,
                ISO2CountryCode = "CY",
                ISO3CountryCode = "CYN",
                Provider = string.Empty,
                TypeOfService = "countrycode"
            };

            _STORAGE.NumberingPlans.Insert(numPlan);

            numPlan.CountryName = "ChynaYaNa";

            status = _STORAGE.NumberingPlans.Update(numPlan);
            status = _STORAGE.NumberingPlans.Delete(numPlan);



            /***
             * TESTING MONITORING SERVERS INFO DATA MAPPER
             */
            MonitoringServerInfo monServer = new MonitoringServerInfo()
            {
                CreatedAt = DateTime.Now,
                DatabaseName = "asdasdasd",
                Description = "TESTING MONITORING SERVER",
                InstanceHostName = "SAMPL INSTANCE HOST NAME",
                InstanceName = "SAMPLE HOST NAME",
                Password = "SAMPLE PW",
                PhoneCallsTable = "PhoneCalls2012310123",
                TelephonySolutionName = "Lync123124123",
                Username = "SEMSEM"
            };

            monServer.ID = _STORAGE.MonitoringServers.Insert(monServer);

            monServer.Username = monServer.Username.ToLower();
            monServer.Password = "123123";

            status = _STORAGE.MonitoringServers.Update(monServer);
            status = _STORAGE.MonitoringServers.Delete(monServer);



            /***
             * TESTING MAIL TAMPLEATES DATA MAPPER
             */
            MailTemplate newTemplate = new MailTemplate()
            {
                TemplateBody = "SAMPLE",
                Subject = "SAMPLE"
            };

            newTemplate.ID = _STORAGE.MailTemplates.Insert(newTemplate);

            newTemplate.Subject = "TESTING TEMPLATE";
            newTemplate.TemplateBody = "TESTING TEMPLATE BODY TEXT";

            status = _STORAGE.MailTemplates.Update(newTemplate);
            status = _STORAGE.MailTemplates.Delete(newTemplate);



            /***
             * TESTING GATEWAYS RATES DATA MAPPER
             */
            GatewayRate gatewayRateInfo = new GatewayRate()
            {
                CurrencyCode = "EUR",
                StartingDate = DateTime.Now,
                EndingDate = DateTime.MinValue,
                RatesTableName = null,
                NgnRatesTableName = null,
                ProviderName = "SAMPLE PROVIDER",
                GatewayID = 10
            };

            gatewayRateInfo.ID = _STORAGE.GatewaysRates.Insert(gatewayRateInfo);

            gatewayRateInfo = _STORAGE.GatewaysRates.GetById(gatewayRateInfo.ID);

            gatewayRateInfo.CurrencyCode = "USD";

            status = _STORAGE.GatewaysRates.Update(gatewayRateInfo);
            status = _STORAGE.GatewaysRates.Delete(gatewayRateInfo);



            /***
             * TESTING GATEWAYS INFO DATA MAPPER
             */
            Gateway sampleGateway = new Gateway()
            {
                Name = "SAMPLE GATEWAY"
            };

            sampleGateway.ID = _STORAGE.Gateways.Insert(sampleGateway);

            GatewayInfo newGatewayInfo = new GatewayInfo()
            {
                Description = "New Info for Gateway",
                GatewayID = sampleGateway.ID,
                PoolID = 2,
                SiteID = 29
            };

            _STORAGE.GatewaysInfo.Insert(newGatewayInfo);

            var AllGatewayInfo = _STORAGE.GatewaysInfo.GetByGatewayID(newGatewayInfo.GatewayID).ToList();

            newGatewayInfo.Description = "Info For Gateway - UPDATED.";

            status = _STORAGE.GatewaysInfo.Update(newGatewayInfo);
            status = _STORAGE.GatewaysInfo.Delete(newGatewayInfo);
            status = _STORAGE.Gateways.Delete(sampleGateway);



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
