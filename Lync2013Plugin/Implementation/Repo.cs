using System.Collections.Generic;
using System.Linq;
using LyncBillingBase.DataModels;
using LyncBillingBase.Repository;

namespace Lync2013Plugin.Implementation
{
    /// <summary>
    ///     This Class will Load all the data which is needed to do the marking
    /// </summary>
    public static class Repo
    {
        //TYPEID was Considered instead of ID
        public static List<int> ListofChargeableCallTypes = new List<int> {1, 2, 3, 4, 5, 6, 19, 21, 22, 24};
        public static List<int> ListOfFixedLinesIDs = new List<int> {1, 2, 4, 21};
        public static List<int> ListOfMobileLinesIDs = new List<int> {3, 5, 22};
        public static List<int> ListOfNgniDs = new List<int> {6, 24};

        public static Dictionary<string, Dictionary<string, List<RatesNational>>> NationalRates =
            new Dictionary<string, Dictionary<string, List<RatesNational>>>();

        public static List<RatesNational> GetNationalRates(Gateway gateway, string countryCodeIso3)
        {
            var tempNationalRates =
                repo.Rates.GetNationalRatesForCountryByGatewayId(gateway.Id, countryCodeIso3).ToList();

            //Check of the nationalRates container is instantiated
            if (NationalRates == null)
                NationalRates = new Dictionary<string, Dictionary<string, List<RatesNational>>>();

            //Temporary Containers

            var tempCountryRates = new Dictionary<string, List<RatesNational>>();

            var gatewayRatesObj = GatewayRates.Find(item => item.GatewayId == gateway.Id);


            if (NationalRates.Keys.Contains(gateway.Name))
            {
                tempCountryRates = NationalRates[gateway.Name];

                if (tempCountryRates != null && tempCountryRates.Keys.Contains(countryCodeIso3))
                {
                    tempNationalRates = tempCountryRates[countryCodeIso3];
                }
                else
                {
                    if (gatewayRatesObj != null && !string.IsNullOrEmpty(gatewayRatesObj.RatesTableName))
                    {
                        tempCountryRates.Add(countryCodeIso3, tempNationalRates);
                        NationalRates[gateway.Name] = tempCountryRates;
                    }
                }
            }
            else
            {
                if (gatewayRatesObj != null && !string.IsNullOrEmpty(gatewayRatesObj.RatesTableName))
                {
                    tempCountryRates.Add(countryCodeIso3, tempNationalRates);
                    NationalRates.Add(gateway.Name, tempCountryRates);
                }
            }

            return tempNationalRates;
        }

        private static readonly DataStorage repo = DataStorage.Instance;
        public static List<User> Users = repo.Users.GetAll().ToList();
        public static List<Did> Dids = repo.DiDs.GetAll().ToList();
        public static List<Site> Sites = repo.Sites.GetAll().ToList();
        public static List<NumberingPlan> NumberingPlan = repo.NumberingPlans.GetAll().ToList();
        public static List<NumberingPlanForNgn> NumberingPlanNgn = repo.NumberingPlansForNgn.GetAll().ToList();
        public static List<CallType> CallTypes = repo.CallTypes.GetAll().ToList();
        public static List<Gateway> Gateways = repo.Gateways.GetAll().ToList();
        public static List<GatewayRate> GatewayRates = repo.GatewaysRates.GetAll().ToList();
        public static List<PhoneCallExclusion> PhoneCallsExclusions = repo.PhoneCallsExclusions.GetAll().ToList();

        public static Dictionary<string, MonitoringServerInfo> MonitoringServerInfo =
            repo.MonitoringServers.GetMonitoringServersInfoMap();

        //To be Implemented 
        public static Dictionary<int, List<RateForNgn>> NgnRatesPerGateway = repo.RatesForNgn.GetGatewaysNgnRatesById();

        public static Dictionary<string, List<RateForNgn>> NgnGatewaysRates =
            repo.RatesForNgn.GetGatewaysNgnRatesByName();

        public static Dictionary<int, List<RatesInternational>> RatesPerGatway = repo.Rates.GetGatewaysRatesById();
    }
}