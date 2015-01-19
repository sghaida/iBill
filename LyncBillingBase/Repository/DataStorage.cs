using LyncBillingBase.DataMappers;

namespace LyncBillingBase.Repository
{
    public sealed class DataStorage
    {
        /***
         * Singleton implementation with an attempted thread-safety using double-check locking
         * @source: http://csharpindepth.com/articles/general/singleton.aspx
         */
        // internal datastorage singleton container
        private static DataStorage _instance;
        // lock for thread-safety laziness
        private static readonly object Mutex = new object();
        /***
         * DataStorage Repositories
         */
        // System Wide
        public AnnouncementsDataMapper Announcements = new AnnouncementsDataMapper();
        public BundledAccountsDataMapper BundledAccounts = new BundledAccountsDataMapper();

        // Backend, Call Marking
        public CallMarkerStatusDataMapper CallMarkers = new CallMarkerStatusDataMapper();
        public CallTypesDataMapper CallTypes = new CallTypesDataMapper();

        // Countries, Sites, Departments, and Currencies
        public CountriesDataMapper Countries = new CountriesDataMapper();
        public CurrenciesDataMapper Currencies = new CurrenciesDataMapper();
        public DelegateRolesDataMapper DelegateRoles = new DelegateRolesDataMapper();
        public DepartmentHeadRolesDataMapper DepartmentHeads = new DepartmentHeadRolesDataMapper();
        public DepartmentsDataMapper Departments = new DepartmentsDataMapper();
        public DiDsDataMapper DiDs = new DiDsDataMapper();
        public GatewaysDataMapper Gateways = new GatewaysDataMapper();
        public GatewaysInfoDataMapper GatewaysInfo = new GatewaysInfoDataMapper();
        public GatewaysRatesDataMapper GatewaysRates = new GatewaysRatesDataMapper();
        public MailTemplatesDataMapper MailTemplates = new MailTemplatesDataMapper();
        public MonitoringServersDataMapper MonitoringServers = new MonitoringServersDataMapper();

        // NumberingPlan, DIDs, CallTypes and Rates
        public NumberingPlansDataMapper NumberingPlans = new NumberingPlansDataMapper();
        public NumberingPlansForNgnDataMapper NumberingPlansForNgn = new NumberingPlansForNgnDataMapper();

        // User
        public PhoneBookContactsDataMapper PhoneBooks = new PhoneBookContactsDataMapper();

        // PhoneCalls
        public PhoneCallsDataMapper PhoneCalls = new PhoneCallsDataMapper();
        public PhoneCallExclusionsDataMapper PhoneCallsExclusions = new PhoneCallExclusionsDataMapper();
        public PoolsDataMapper Pools = new PoolsDataMapper();
        public RatesDataMapper Rates = new RatesDataMapper();
        public RatesForNgnDataMapper RatesForNgn = new RatesForNgnDataMapper();

        // Roles
        public RolesDataMapper Roles = new RolesDataMapper();
        public SitesDataMapper Sites = new SitesDataMapper();
        public SitesDepartmentsDataMapper SitesDepartments = SitesDepartmentsDataMapper.Instance;
        public SystemRolesDataMapper SystemRoles = new SystemRolesDataMapper();
        public UsersDataMapper Users = new UsersDataMapper();

        // Calls Summaries
        public UsersCallsSummariesDataMapper UsersCallsSummaries = new UsersCallsSummariesDataMapper();
        public DepartmentCallsSummariesDataMapper DepartmensCallsSummaries = new DepartmentCallsSummariesDataMapper();
        public SitesCallsSummariesDataMapper SitesCallsSummaries = new SitesCallsSummariesDataMapper();
        public GatewaysCallsSummariesDataMapper GatewaysCallsSummaries = new GatewaysCallsSummariesDataMapper();
        public TopDestinationNumbersDataMapper TopDestinationNumbers = new TopDestinationNumbersDataMapper();
        public TopDestinationCountriesDataMapper TopDestinationCountries = new TopDestinationCountriesDataMapper();
        public MailReportsDataMapper MailReports = new MailReportsDataMapper();
        public ChartsReportsDataMapper ChartsReports = new ChartsReportsDataMapper();

        // empty constuctor
        private DataStorage()
        {
        }

        //The only public method, used to obtain an instance of DataStorage
        public static DataStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new DataStorage();
                        }
                    }
                }

                return _instance;
            }
        }
    }
}