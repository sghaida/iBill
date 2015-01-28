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
        public static string Tolower(string text)
        {
            return text.ToLower();
        }

        public static void Main(string[] args)
        {
            DataStorage DB = DataStorage.Instance;

            var startDate = new DateTime(2014, 1, 1);
            var endDate = DateTime.Now;

            //var users_summaries = DB.UsersCallsSummaries.GetBySipAccount("aalhour@ccc.gr", startDate, endDate);
            //users_summaries = DB.UsersCallsSummaries.GetBySite("MOA", startDate, endDate, Globals.CallsSummary.GroupBy.UserAndInvoiceFlag);
            //users_summaries = DB.UsersCallsSummaries.GetByGateway("10.1.1.3");
            //var first_user_summary = summaries.First();

            //var sites_summaries = DB.SitesCallsSummaries.GetBySite("MOA", startDate, endDate);

            //var yearsForAAlhour = DB.UsersCallsSummaries.GetYearsBySipAccount("aalhour@ccc.gr");

            //var siteSummaries = DB.SitesCallsSummaries.GetBySite("MOA");
            //siteSummaries = DB.SitesCallsSummaries.GetBySite("MOA", startDate, endDate);

            //var departmensSummaries = DB.DepartmensCallsSummaries.GetByDepartment("MOA", "ISD");
            //var departmentTotal = DB.DepartmensCallsSummaries.GetTotalByDepartment("MOA", "ISD");
            //var siteDepartmensTotals = DB.DepartmensCallsSummaries.GetTotalsForEachDepartmentInSite("MOA");

            //var MOA_Gateways_Summaries = DB.GatewaysCallsSummaries.GetBySite("MOA", groupBy: Globals.CallsSummaryForGateway.GroupBy.GatewayNameOnly);
            //var MOA_101012_Summaries = DB.GatewaysCallsSummaries.GetBySiteAndGateway("MOA", "10.1.0.12");

            //var Gateways_Usage = DB.GatewaysCallsSummaries.GetUsageForAllGateways(startDate, endDate);
            //var Usage_Stats = DB.GatewaysCallsSummaries.GetGatewaysStatisticsResults(Gateways_Usage);

            //var topNumbers = DB.TopDestinationNumbers.GetBySipAccount("aalhour@ccc.gr", 10);

            //var topCountriesForUser = DB.TopDestinationCountries.GetByUser("aalhour@ccc.gr", 10);
            //var topCountriesForDepartment = DB.TopDestinationCountries.GetByDepartment("MOA", "ISD", 10);
            //var topCountriesForSite = DB.TopDestinationCountries.GetBySite("MOA", 10);

            //var mailReportsForUser = DB.MailReports.GetByUser("aalhour@ccc.gr", (new DateTime(2013, 10, 1)));
            //var mailReportForUser = DB.MailReports.GetTotalByUser("aalhour@ccc.gr", (new DateTime(2013, 10, 1)));
            //var mailReportForDepartment = DB.MailReports.GetByDepartment("MOA", "ISD", (new DateTime(2013, 10, 1)));

            //var userChartReports = DB.ChartsReports.GetByUser("aalhour@ccc.gr");
            //var departmentChartReports = DB.ChartsReports.GetByDepartment("MOA", "ISD");
            //var siteChartsReports = DB.ChartsReports.GetBySite("MOA");
            var gatewayChartsReports = DB.ChartsReports.GetByGateway("MOA", "10.1.0.12");

            string x = string.Empty;
        }

        public static void InsertUpdateDeleteTests()
        {
            var storage = DataStorage.Instance;

            var status = false;


            /***
             * TESTING NUMBERING PLAN
             */
            var numberingPlan = new NumberingPlansDataMapper();

            var athens = "athens";
            var kaz = "kaz";

            var plan = new NumberingPlan();
            plan.City = athens;
            plan.Iso3CountryCode = kaz;

            var ev = new CustomExpressionVisitor();

            Expression<Func<NumberingPlan, bool>> exp1 = item => item.City.ToLower() == athens;
            Expression<Func<NumberingPlan, bool>> exp2 = (item => item.Iso3CountryCode.ToLower() == kaz);

            var data1 = numberingPlan.Get(exp1).ToList();
            var data2 = numberingPlan.Get(exp2).ToList();


            /***
             * TESTING THE SINGLETON SITES DEPARTMENTS DATA MAPPER
             */
            var allSitesDepartments = storage.SitesDepartments.GetAll();


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

            newSite.Id = storage.Sites.Insert(newSite);
            newDepartment.Id = storage.Departments.Insert(newDepartment);

            var newSiteDepartment = new SiteDepartment
            {
                SiteId = newSite.Id,
                DepartmentId = newDepartment.Id
            };

            newSiteDepartment.Id = storage.SitesDepartments.Insert(newSiteDepartment);

            allSitesDepartments = storage.SitesDepartments.GetAll();

            newSiteDepartment.SiteId = 29;

            status = storage.SitesDepartments.Update(newSiteDepartment);

            newSiteDepartment = storage.SitesDepartments.GetById(newSiteDepartment.Id);

            allSitesDepartments = storage.SitesDepartments.GetAll();

            status = storage.SitesDepartments.Delete(newSiteDepartment);

            status = storage.Departments.Delete(newDepartment);

            status = storage.Sites.Delete(newSite);


            /***
             * TESTING USERS DATA MAPPER
             */
            var allUsers = storage.Users.GetAll();

            var newUser = new User
            {
                EmployeeId = 99887766,
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

            storage.Users.Insert(newUser);

            newUser = storage.Users.GetBySipAccount(newUser.SipAccount);

            newUser.DisplayName = "UNKNOWN";

            status = storage.Users.Update(newUser);
            status = storage.Users.Delete(newUser);


            /***
             * TESTING SYSTEM ROLES
             */
            var developerRole = storage.Roles.GetByRoleId(10);

            var systemRole = new SystemRole
            {
                Description = "TESTING SYSTEM ROLE",
                RoleId = developerRole.RoleId,
                SipAccount = "nafez@ccc.gr",
                SiteId = 29
            };

            systemRole.Id = storage.SystemRoles.Insert(systemRole);

            systemRole = storage.SystemRoles.GetById(systemRole.Id);

            systemRole.SiteId = 31;

            status = storage.SystemRoles.Update(systemRole);

            systemRole = storage.SystemRoles.GetById(systemRole.Id);

            status = storage.SystemRoles.Delete(systemRole);


            /***
             * TESTING SITES DEPARTMENTS DATA MAPPER
             */
            var moaSite = storage.Sites.GetById(29);
            var rasoSite = storage.Sites.GetById(31);
            var isdDepartment = storage.Departments.GetByName("ISD");

            var siteDepartment = new SiteDepartment
            {
                SiteId = moaSite.Id,
                DepartmentId = isdDepartment.Id
            };

            siteDepartment.Id = storage.SitesDepartments.Insert(siteDepartment);

            siteDepartment = storage.SitesDepartments.GetById(siteDepartment.Id);

            siteDepartment.SiteId = rasoSite.Id;

            status = storage.SitesDepartments.Update(siteDepartment);

            siteDepartment = storage.SitesDepartments.GetById(siteDepartment.Id);

            status = storage.SitesDepartments.Delete(siteDepartment);


            /***
             * TESTING SITES DATA MAPPER
             */
            var site = new Site
            {
                CountryCode = "GRC",
                Description = "SAMPLE GREECE SITE",
                Name = "SAMPLE SITE"
            };

            site.Id = storage.Sites.Insert(site);

            site = storage.Sites.GetById(site.Id);

            site.Name = "sample sample site";

            status = storage.Sites.Update(site);
            status = storage.Sites.Delete(site);


            /***
             * TESTING ROLES DATA MAPPER
             */
            var role123 = new Role
            {
                RoleDescription = "SAMPLE ROLE",
                RoleId = 123123,
                RoleName = "SAMPLE-01"
            };

            role123.Id = storage.Roles.Insert(role123);

            role123.RoleName = "sample-02.01";

            status = storage.Roles.Update(role123);
            status = storage.Roles.Delete(role123);


            /***
             * TESTING NGN RATES DATA MAPPER
             */
            var ngnRate = new RateForNgn
            {
                DialingCodeId = 1,
                Rate = Convert.ToDecimal(15.45)
            };

            ngnRate.Id = 19; //_STORAGE.RatesForNGN.Insert(NGNRate, 10);

            ngnRate = storage.RatesForNgn.GetByGatewayId(10).Find(rate => rate.Id == 19);

            ngnRate.Rate = Convert.ToDecimal(20.45);

            status = storage.RatesForNgn.Update(ngnRate, 10);
            status = storage.RatesForNgn.Delete(ngnRate, 10);


            /***
             * TESTING POOLS DATA MAPPER
             */
            var newPool = new Pool
            {
                Fqdn = "TESTING POOL FQDN"
            };

            newPool.Id = storage.Pools.Insert(newPool);

            newPool.Fqdn = "CHANGED FQDN";

            status = storage.Pools.Update(newPool);
            status = storage.Pools.Delete(newPool);


            /***
             * TESTING PHONE CALLS EXCLUSIONS
             */
            var exclusion = new PhoneCallExclusion
            {
                ExclusionSubject = "aalhour@ccc.gr",
                Description = "SAMPLE EXCLUSION TEST",
                ExclusionType = Globals.PhoneCallExclusion.Type.Source.Value(),
                SiteId = 29,
                ZeroCost = Globals.PhoneCallExclusion.ZeroCost.No.Value(),
                AutoMark = Globals.PhoneCallExclusion.AutoMark.Business.Value()
            };

            exclusion.Id = storage.PhoneCallsExclusions.Insert(exclusion);

            exclusion = storage.PhoneCallsExclusions.GetById(789);

            exclusion.AutoMark = "";
            exclusion.ZeroCost = Globals.PhoneCallExclusion.ZeroCost.Yes.Value();

            status = storage.PhoneCallsExclusions.Update(exclusion);
            status = storage.PhoneCallsExclusions.Delete(exclusion);


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

            contact.Id = storage.PhoneBooks.Insert(contact);

            var allContacts = storage.PhoneBooks.GetBySipAccount(contact.SipAccount);

            contact.Name = "SAMPLE PHONE BOOK CONTANCT NAME";
            contact.Type = "Business";

            status = storage.PhoneBooks.Update(contact);
            status = storage.PhoneBooks.Delete(contact);


            /***
             * TESTING NUMBERING PLAN FOR NGN
             */
            var ngn = new NumberingPlanForNgn
            {
                Description = "TEST NGN",
                DialingCode = "8000000000000",
                Iso3CountryCode = "GRC",
                TypeOfServiceId = 6,
                Provider = "SAMPLE PROVIDER"
            };

            ngn.Id = storage.NumberingPlansForNgn.Insert(ngn);

            ngn = storage.NumberingPlansForNgn.GetById(ngn.Id);

            ngn.Description = "TEST NGN - UPDATED.";

            status = storage.NumberingPlansForNgn.Update(ngn);
            status = storage.NumberingPlansForNgn.Delete(ngn);


            /***
             * TESTING NUMBER PLAN DATA MAPPER
             */
            var numPlan = new NumberingPlan
            {
                City = "Xin Che Bang",
                CountryName = "ChinaYaNa",
                DialingPrefix = 90909,
                Iso2CountryCode = "CY",
                Iso3CountryCode = "CYN",
                Provider = string.Empty,
                TypeOfService = "countrycode"
            };

            storage.NumberingPlans.Insert(numPlan);

            numPlan.CountryName = "ChynaYaNa";

            status = storage.NumberingPlans.Update(numPlan);
            status = storage.NumberingPlans.Delete(numPlan);


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

            monServer.Id = storage.MonitoringServers.Insert(monServer);

            monServer.Username = monServer.Username.ToLower();
            monServer.Password = "123123";

            status = storage.MonitoringServers.Update(monServer);
            status = storage.MonitoringServers.Delete(monServer);


            /***
             * TESTING MAIL TAMPLEATES DATA MAPPER
             */
            var newTemplate = new MailTemplate
            {
                TemplateBody = "SAMPLE",
                Subject = "SAMPLE"
            };

            newTemplate.Id = storage.MailTemplates.Insert(newTemplate);

            newTemplate.Subject = "TESTING TEMPLATE";
            newTemplate.TemplateBody = "TESTING TEMPLATE BODY TEXT";

            status = storage.MailTemplates.Update(newTemplate);
            status = storage.MailTemplates.Delete(newTemplate);


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
                GatewayId = 10
            };

            gatewayRateInfo.Id = storage.GatewaysRates.Insert(gatewayRateInfo);

            gatewayRateInfo = storage.GatewaysRates.GetById(gatewayRateInfo.Id);

            gatewayRateInfo.CurrencyCode = "USD";

            status = storage.GatewaysRates.Update(gatewayRateInfo);
            status = storage.GatewaysRates.Delete(gatewayRateInfo);


            /***
             * TESTING GATEWAYS INFO DATA MAPPER
             */
            var sampleGateway = new Gateway
            {
                Name = "SAMPLE GATEWAY"
            };

            sampleGateway.Id = storage.Gateways.Insert(sampleGateway);

            var newGatewayInfo = new GatewayInfo
            {
                Description = "New Info for Gateway",
                GatewayId = sampleGateway.Id,
                PoolId = 2,
                SiteId = 29
            };

            storage.GatewaysInfo.Insert(newGatewayInfo);

            var allGatewayInfo = storage.GatewaysInfo.GetByGatewayId(newGatewayInfo.GatewayId).ToList();

            newGatewayInfo.Description = "Info For Gateway - UPDATED.";

            status = storage.GatewaysInfo.Update(newGatewayInfo);
            status = storage.GatewaysInfo.Delete(newGatewayInfo);
            status = storage.Gateways.Delete(sampleGateway);


            /***
             * TESTING GATEWAYS DATA MAPPER
             */
            var newGateway = new Gateway
            {
                Name = "Sameer"
            };

            newGateway.Id = storage.Gateways.Insert(newGateway);

            newGateway.Name = "Sameer-02";

            status = storage.Gateways.Update(newGateway);
            status = storage.Gateways.Delete(newGateway);


            /***
             * TESTING DIDs DATA MAPPER
             */
            var newDid = new Did
            {
                Description = "SAMPLE DID",
                Regex = "SAMPLE REGEX",
                SiteId = 29
            };

            newDid.Id = storage.DiDs.Insert(newDid);

            newDid = storage.DiDs.GetById(newDid.Id);

            newDid.Description = "SAMPLE DID - UPDATED DESCRIPTION";

            status = storage.DiDs.Update(newDid);
            status = storage.DiDs.Delete(newDid);


            /***
             * TESTING DEPARTMENTS
             */
            var department = new Department
            {
                Name = "SUMURMUR",
                Description = "Sumurmur Department"
            };

            department.Id = storage.Departments.Insert(department);

            department = storage.Departments.GetById(department.Id);

            department.Description = "Never mind!";

            status = storage.Departments.Update(department);
            status = storage.Departments.Delete(department);


            /***
             * TESTING DEPARTMENT HEAD ROLES
             */
            var moa = storage.Sites.GetById(29);
            var moaIsd = storage.SitesDepartments.GetBySiteId(29).ToList().Find(item => item.Department.Name == "ISD");
            var depHead = new DepartmentHeadRole
            {
                SipAccount = "aalhour@ccc.gr",
                SiteDepartmentId = moaIsd.Id
            };

            depHead.Id = storage.DepartmentHeads.Insert(depHead);

            depHead = storage.DepartmentHeads.GetById(depHead.Id);

            depHead.SipAccount = "sghaida@ccc.gr";

            status = storage.DepartmentHeads.Update(depHead);
            status = storage.DepartmentHeads.Delete(depHead);


            /***
             * TESTING DELEGATE ROLES DATA MAPPER
             */
            moa = storage.Sites.GetById(29);
            moaIsd = storage.SitesDepartments.GetBySiteId(29).ToList().Find(item => item.Department.Name == "ISD");

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
                ManagedSiteDepartmentId = moaIsd.Id
            };

            var siteDelegate = new DelegateRole
            {
                DelegationType = 1,
                DelegeeSipAccount = "aalhour@ccc.gr",
                ManagedSiteId = moa.Id
            };

            userDelegate.Id = storage.DelegateRoles.Insert(userDelegate);
            departmentDelegate.Id = storage.DelegateRoles.Insert(departmentDelegate);
            siteDelegate.Id = storage.DelegateRoles.Insert(siteDelegate);

            var allRoles = storage.DelegateRoles.GetByDelegeeSipAccount(userDelegate.DelegeeSipAccount);

            userDelegate.DelegeeSipAccount = "sghaida@ccc.gr";
            departmentDelegate.DelegeeSipAccount = "sghaida@ccc.gr";
            siteDelegate.DelegeeSipAccount = "sghaida@ccc.gr";

            status = storage.DelegateRoles.Update(userDelegate);
            status = storage.DelegateRoles.Update(departmentDelegate);
            status = storage.DelegateRoles.Update(siteDelegate);

            allRoles = storage.DelegateRoles.GetByDelegeeSipAccount(userDelegate.DelegeeSipAccount);

            status = storage.DelegateRoles.Delete(userDelegate);
            status = storage.DelegateRoles.Delete(departmentDelegate);
            status = storage.DelegateRoles.Delete(siteDelegate);


            /***
             * TESTING Currencies DATA MAPPER
             */
            var newCurrency = new Currency
            {
                Iso3Code = "ZVZ",
                Name = "ZeeVeeZee"
            };
            newCurrency.Id = storage.Currencies.Insert(newCurrency);
            newCurrency.Name = "ZeeeeVeZe";
            status = storage.Currencies.Update(newCurrency);
            status = storage.Currencies.Delete(newCurrency);


            /***
             * TESTING COUNTRIES DATA MAPPER
             */
            var euro = storage.Currencies.GetByIso3Code("EUR");
            var newCountry = new Country
            {
                CurrencyId = euro.Id,
                Iso2Code = "ZZ",
                Iso3Code = "ZVZ",
                Name = "ZeeVeeZee"
            };
            newCountry.Id = storage.Countries.Insert(newCountry);
            newCountry = storage.Countries.GetById(newCountry.Id);
            newCountry.Iso3Code = "VVZ";
            newCountry.Name = "VeeVeeZee";
            status = storage.Countries.Update(newCountry);
            status = storage.Countries.Delete(newCountry);


            /***
             * TESTING CALL TYPES DATA MAPPER
             */
            var newCallType = new CallType
            {
                Description = "Testing call types data mapper.",
                Name = "TEST-CALL-TYPE",
                TypeId = 2345123
            };

            newCallType.Id = storage.CallTypes.Insert(newCallType);

            newCallType.Description = newCallType.Description.ToUpper();
            newCallType.Name = (newCallType.Name + "-02").ToUpper();

            status = storage.CallTypes.Update(newCallType);
            status = storage.CallTypes.Delete(newCallType);


            /***
             * TESTING CALL MARKER STATUS DATA MAPPER
             */
            var newMarkerStatus = new CallMarkerStatus
            {
                PhoneCallsTable = "PhoneCalls2015",
                Timestamp = DateTime.Now,
                Type = Globals.CallMarkerStatus.Type.CallsMarking.Value()
            };

            newMarkerStatus.Id = storage.CallMarkers.Insert(newMarkerStatus);

            newMarkerStatus.Type = Globals.CallMarkerStatus.Type.ApplyingRates.Value();
            newMarkerStatus.Timestamp = DateTime.Now;

            status = storage.CallMarkers.Update(newMarkerStatus);
            status = storage.CallMarkers.Delete(newMarkerStatus);


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

            bundled1.Id = storage.BundledAccounts.Insert(bundled1);
            bundled2.Id = storage.BundledAccounts.Insert(bundled2);

            var allBundled = storage.BundledAccounts.GetAll();
            var aalhourBundled = storage.BundledAccounts.GetAssociatedSipAccounts("aalhour@ccc.gr");

            bundled2.AssociatedSipAccount = "nafez@ccc.gr";

            status = storage.BundledAccounts.Update(bundled2);
            status = storage.BundledAccounts.Delete(bundled1);
            status = storage.BundledAccounts.Delete(bundled2);


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

            ann.Id = storage.Announcements.Insert(ann);

            ann = storage.Announcements.GetById(ann.Id);

            ann.AnnouncementBody = "Hello Developer. Things have changed.";

            status = storage.Announcements.Update(ann);
            status = storage.Announcements.Delete(ann);
        }
    }
}