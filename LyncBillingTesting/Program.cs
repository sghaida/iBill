using System;
using System.Linq;
using System.Linq.Expressions;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using LyncBillingBase.DataMappers;
using LyncBillingBase.DataModels;
using LyncBillingBase.Repository;

namespace LyncBillingTesting
{
    internal class Program
    {
        public static string tolower(string text)
        {
            return text.ToLower();
        }

        public static void Main(string[] args)
        {
            //Lync2013 processor = new Lync2013();
            //processor.ProcessPhoneCalls();

            var DelegatesRolesDM = new DelegateRolesDataMapper();
            var DepartmentHeadsDM = new DepartmentHeadRolesDataMapper();

            var allDelegates = DelegatesRolesDM.GetAll().ToList();
            var allDepartmentHeads = DepartmentHeadsDM.GetAll().ToList();
        }

        public static void InsertUpdateDeleteTests()
        {
            var _STORAGE = DataStorage.Instance;

            var status = false;


            /***
             * TESTING NUMBERING PLAN
             */
            var numberingPlan = new NumberingPlansDataMapper();

            var athens = "athens";
            var kaz = "kaz";

            var plan = new NumberingPlan();
            plan.City = athens;
            plan.ISO3CountryCode = kaz;

            var ev = new CustomExpressionVisitor();

            Expression<Func<NumberingPlan, bool>> exp1 = item => item.City.ToLower() == athens;
            Expression<Func<NumberingPlan, bool>> exp2 = (item => item.ISO3CountryCode.ToLower() == kaz);

            var data1 = numberingPlan.Get(exp1).ToList();
            var data2 = numberingPlan.Get(exp2).ToList();


            /***
             * TESTING THE SINGLETON SITES DEPARTMENTS DATA MAPPER
             */
            var allSitesDepartments = _STORAGE.SitesDepartments.GetAll();


            var newSite = new Site
            {
                CountryCode = "GRC",
                Description = "Sample Description",
                Name = "TEST-SITE"
            };

            var newDepartment = new Department
            {
                Name = "TEST-DEPARTMENT",
                Description = "Sample Description"
            };

            newSite.ID = _STORAGE.Sites.Insert(newSite);
            newDepartment.ID = _STORAGE.Departments.Insert(newDepartment);

            var newSiteDepartment = new SiteDepartment
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


            /***
             * TESTING USERS DATA MAPPER
             */
            var allUsers = _STORAGE.Users.GetAll();

            var newUser = new User
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

            var systemRole = new SystemRole
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

            var siteDepartment = new SiteDepartment
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
            var site = new Site
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
            var role123 = new Role
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
            var NGNRate = new RateForNGN
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
            var newPool = new Pool
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
            var exclusion = new PhoneCallExclusion
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
            var contact = new PhoneBookContact
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
            var NGN = new NumberingPlanForNGN
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
            var numPlan = new NumberingPlan
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
            var monServer = new MonitoringServerInfo
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
            var newTemplate = new MailTemplate
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
            var gatewayRateInfo = new GatewayRate
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
            var sampleGateway = new Gateway
            {
                Name = "SAMPLE GATEWAY"
            };

            sampleGateway.ID = _STORAGE.Gateways.Insert(sampleGateway);

            var newGatewayInfo = new GatewayInfo
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
            var newGateway = new Gateway
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
            var newDID = new DID
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
            var department = new Department
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
            var MOA_ISD = _STORAGE.SitesDepartments.GetBySiteID(29).ToList().Find(item => item.Department.Name == "ISD");
            var depHead = new DepartmentHeadRole
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
            MOA_ISD = _STORAGE.SitesDepartments.GetBySiteID(29).ToList().Find(item => item.Department.Name == "ISD");

            var userDelegate = new DelegateRole
            {
                DelegationType = 3,
                DelegeeSipAccount = "aalhour@ccc.gr",
                ManagedUserSipAccount = "ghassan@ccc.gr"
            };

            var departmentDelegate = new DelegateRole
            {
                DelegationType = 2,
                DelegeeSipAccount = "aalhour@ccc.gr",
                ManagedSiteDepartmentID = MOA_ISD.ID
            };

            var siteDelegate = new DelegateRole
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
            var newCurrency = new Currency
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
            var newCountry = new Country
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
            var newCallType = new CallType
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
            var newMarkerStatus = new CallMarkerStatus
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
            var bundled1 = new BundledAccount
            {
                PrimarySipAccount = "aalhour@ccc.gr",
                AssociatedSipAccount = "sghaida@ccc.gr"
            };

            var bundled2 = new BundledAccount
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
            var ann = new Announcement
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