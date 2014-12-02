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
        public AnnouncementsDataMapper Announcements = new AnnouncementsDataMapper();
        public BundledAccountsDataMapper BundledAccounts = new BundledAccountsDataMapper();
        public CallMarkerStatusDataMapper CallMarkers = new CallMarkerStatusDataMapper();
        public CallTypesDataMapper CallTypes = new CallTypesDataMapper();
        
        public CountriesDataMapper Countries = new CountriesDataMapper();
        public SitesDepartmentsDataMapper Sites = new SitesDepartmentsDataMapper();
        public GatewaysDataMapper Gateways = new GatewaysDataMapper();
        public DepartmentsDataMapper Departments = new DepartmentsDataMapper();
        public SitesDepartmentsDataMapper SitesDepartments = new SitesDepartmentsDataMapper();
        public CurrenciesDataMapper Currencies = new CurrenciesDataMapper();

        public RolesDataMapper Roles = new RolesDataMapper();
        public SystemRolesDataMapper SystemRoles = new SystemRolesDataMapper();
        public DelegateRolesDataMapper DelegateRoles = new DelegateRolesDataMapper();
        public DepartmentHeadRolesDataMapper DepartmentHeads = new DepartmentHeadRolesDataMapper();

        public DIDDataMapper DIDs = new DIDDataMapper();
        public PhoneCallsDataMapper PhoneCalls = new PhoneCallsDataMapper();



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
