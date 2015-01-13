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
        public static List<int> ListOfNGNIDs = new List<int> {6, 24};

        public static Dictionary<string, Dictionary<string, List<Rates_National>>> nationalRates =
            new Dictionary<string, Dictionary<string, List<Rates_National>>>();

        public static List<Rates_National> GetNationalRates(Gateway gateway, string CountryCodeISO3)
        {
            var tempNationalRates =
                repo.Rates.GetNationalRatesForCountryByGatewayID(gateway.ID, CountryCodeISO3).ToList();

            //Check of the nationalRates container is instantiated
            if (nationalRates == null)
                nationalRates = new Dictionary<string, Dictionary<string, List<Rates_National>>>();

            //Temporary Containers

            var tempCountryRates = new Dictionary<string, List<Rates_National>>();

            var gatewayRatesObj = gatewayRates.Find(item => item.GatewayID == gateway.ID);


            if (nationalRates.Keys.Contains(gateway.Name))
            {
                tempCountryRates = nationalRates[gateway.Name];

                if (tempCountryRates != null && tempCountryRates.Keys.Contains(CountryCodeISO3))
                {
                    tempNationalRates = tempCountryRates[CountryCodeISO3];
                }
                else
                {
                    if (gatewayRatesObj != null && !string.IsNullOrEmpty(gatewayRatesObj.RatesTableName))
                    {
                        tempCountryRates.Add(CountryCodeISO3, tempNationalRates);
                        nationalRates[gateway.Name] = tempCountryRates;
                    }
                }
            }
            else
            {
                if (gatewayRatesObj != null && !string.IsNullOrEmpty(gatewayRatesObj.RatesTableName))
                {
                    tempCountryRates.Add(CountryCodeISO3, tempNationalRates);
                    nationalRates.Add(gateway.Name, tempCountryRates);
                }
            }

            return tempNationalRates;
        }

        private static readonly DataStorage repo = DataStorage.Instance;
        public static List<User> users = repo.Users.GetAll().ToList();
        public static List<DID> dids = repo.DIDs.GetAll().ToList();
        public static List<Site> sites = repo.Sites.GetAll().ToList();
        public static List<NumberingPlan> numberingPlan = repo.NumberingPlans.GetAll().ToList();
        public static List<NumberingPlanForNGN> numberingPlanNGN = repo.NumberingPlansForNGN.GetAll().ToList();
        public static List<CallType> callTypes = repo.CallTypes.GetAll().ToList();
        public static List<Gateway> gateways = repo.Gateways.GetAll().ToList();
        public static List<GatewayRate> gatewayRates = repo.GatewaysRates.GetAll().ToList();
        public static List<PhoneCallExclusion> phoneCallsExclusions = repo.PhoneCallsExclusions.GetAll().ToList();

        public static Dictionary<string, MonitoringServerInfo> monitoringServerInfo =
            repo.MonitoringServers.GetMonitoringServersInfoMap();

        //To be Implemented 
        public static Dictionary<int, List<RateForNGN>> ngnRatesPerGateway = repo.RatesForNGN.GetGatewaysNGNRatesByID();

        public static Dictionary<string, List<RateForNGN>> ngnGatewaysRates =
            repo.RatesForNGN.GetGatewaysNGNRatesByName();

        public static Dictionary<int, List<Rates_International>> ratesPerGatway = repo.Rates.GetGatewaysRatesByID();
    }
}