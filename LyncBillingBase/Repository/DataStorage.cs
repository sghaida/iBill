using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using LyncBillingBase.Helpers;
using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;
using LyncBillingBase.DataMappers;


namespace LyncBillingBase.Repository
{
    public sealed class DataStorage
    {
        /***
         * DataStorage Repositories
         */
        // System Wide
        public AnnouncementsDataMapper Announcements = new AnnouncementsDataMapper();
        public BundledAccountsDataMapper BundledAccounts = new BundledAccountsDataMapper();
        public MailTemplatesDataMapper MailTemplates = new MailTemplatesDataMapper();
        
        // Countries, Sites, Departments, and Currencies
        public CountriesDataMapper Countries = new CountriesDataMapper();
        public SitesDepartmentsDataMapper Sites = new SitesDepartmentsDataMapper();
        public GatewaysDataMapper Gateways = new GatewaysDataMapper();
        public DepartmentsDataMapper Departments = new DepartmentsDataMapper();
        public SitesDepartmentsDataMapper SitesDepartments = new SitesDepartmentsDataMapper();
        public CurrenciesDataMapper Currencies = new CurrenciesDataMapper();

        // Roles
        public RolesDataMapper Roles = new RolesDataMapper();
        public SystemRolesDataMapper SystemRoles = new SystemRolesDataMapper();
        public DelegateRolesDataMapper DelegateRoles = new DelegateRolesDataMapper();
        public DepartmentHeadRolesDataMapper DepartmentHeads = new DepartmentHeadRolesDataMapper();

        // Phone Calls, DIDs, CallTypes;
        public NumberingPlansDataMapper NumberingPlans = new NumberingPlansDataMapper();
        public NumberingPlansForNGNDataMapper NumberingPlansForNGN = new NumberingPlansForNGNDataMapper();
        public DIDDataMapper DIDs = new DIDDataMapper();
        public CallTypesDataMapper CallTypes = new CallTypesDataMapper();
        public PhoneCallsDataMapper PhoneCalls = new PhoneCallsDataMapper();

        // Backend, Call Marking
        public CallMarkerStatusDataMapper CallMarkers = new CallMarkerStatusDataMapper();
        public MonitoringServersDataMapper MonitoringServers = new MonitoringServersDataMapper();


        /***
         * Singleton implementation with an attempted thread-safety using double-check locking
         * @source: http://csharpindepth.com/articles/general/singleton.aspx
         */
        // internal datastorage singleton container
        private static DataStorage _instance = null;

        // lock for thread-safety laziness
        private static readonly object _mutex = new object();

        // empty constuctor
        private DataStorage() { }

        //The only public method, used to obtain an instance of DataStorage
        public static DataStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock(_mutex)
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
