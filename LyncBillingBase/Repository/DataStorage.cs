using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;
using LyncBillingBase.DAL;
using System.Threading;
using LyncBillingBase.DAL.Functions;

namespace LyncBillingBase.Repository
{
    public sealed class DataStorage
    {
        /***
         * DataStorage Repositories
         */
        public Repository<PhoneCall> PhoneCalls = new Repository<PhoneCall>();
        public Repository<Announcement> Announcements = new Repository<Announcement>();
        public Repository<BundledAccount> BundledAccounts = new Repository<BundledAccount>();
        public Repository<CallMarkerStatus> CallMarkersStatus = new Repository<CallMarkerStatus>();
        public Repository<CallType> CallTypes = new Repository<CallType>();
        //public Repository<Country> Countries = new Repository<Country>();
        public Repository<Department> Departments = new Repository<Department>();
        //public Repository<DialingPrefixRate> DialingPrefixesRates = new Repository<DialingPrefixRate>();
        public Repository<DID> DIDs = new Repository<DID>();
        public Repository<Gateway> Gateways = new Repository<Gateway>();
        public Repository<GatewayDetail> GatewaysDetails = new Repository<GatewayDetail>();
        public Repository<GatewayRate> GatewaysRates = new Repository<GatewayRate>();
        public Repository<MailTemplate> MailTemplates = new Repository<MailTemplate>();
        //public Repository<MarkingStatus> MarkingsStatus = new Repository<MarkingStatus>();
        public Repository<MonitoringServerInfo> MonitoringServersInfo = new Repository<MonitoringServerInfo>();
        public Repository<NumberingPlan> NumberingPlan = new Repository<NumberingPlan>();
        public Repository<NumberingPlanNGN> NumberingPlanNGN = new Repository<NumberingPlanNGN>();
        public Repository<PhoneBookContact> PhoneBook = new Repository<PhoneBookContact>();
        public Repository<PhoneCallExclusion> PhoneCallsExclusions = new Repository<PhoneCallExclusion>();
        public Repository<Pool> Pools = new Repository<Pool>();
        //public Repository<Rate> Rates = new Repository<Rate>();
        //public Repository<RatesNGN> Rates_NGN = new Repository<RatesNGN>();
        //public Repository<Rates_National> Rates_National = new Repository<Rates_National>();
        //public Repository<Rates_International> Rates_International = new Repository<Rates_International>();
        public Repository<Site> Sites = new Repository<Site>();
        public Repository<SiteDepartment> SitesDepartments = new Repository<SiteDepartment>();
        public Repository<User> Users = new Repository<User>();

        //DB Functions
        public Repository<ChargeableCallsPerUser> ChargeableCallsPerUSer = new Repository<ChargeableCallsPerUser>();

        // NOT LOADED ON PURPOSE
        // public Repository<PhoneCall> PhoneCalls = new Repository<PhoneCall>();



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
