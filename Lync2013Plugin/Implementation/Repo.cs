using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataModels;
using LyncBillingBase.Repository;

namespace Lync2013Plugin.Implementation
{
    /// <summary>
    /// This Class will Load all the data which is needed to do the marking
    /// </summary>
    public static class Repo
    {
        private static DataStorage repo = DataStorage.Instance;

        public static List<User> users = repo.Users.GetAll().ToList();
        public static List<DID> dids = repo.DIDs.GetAll().ToList();
        public static List<Site> sites = repo.Sites.GetAll().ToList();
        public static List<NumberingPlan> numberingPlan = repo.NumberingPlans.GetAll().ToList();
        public static List<NumberingPlanForNGN> numberingPlanNGN = repo.NumberingPlansForNGN.GetAll().ToList();
        public static List<CallType> callTypes = repo.CallTypes.GetAll().ToList();
        public static List<Gateway> gateways = repo.Gateways.GetAll().ToList();
        public static List<GatewayRate> gatewayRates = repo.GatewaysRates.GetAll().ToList();
        public static List<PhoneCallExclusion> phoneCallsExclusions = repo.PhoneCallsExclusions.GetAll().ToList();
        public static Dictionary<string,MonitoringServerInfo> monitoringServerInfo = repo.MonitoringServers.GetMonitoringServersInfoMap();

        //TYPEID was Considered instead of ID
        public static List<int> ListofChargeableCallTypes = new List<int>() { 1, 2, 3, 4, 5, 6, 19, 21, 22, 24 };
        public static List<int> ListOfFixedLinesIDs = new List<int>() { 1, 2, 4, 21 };
        public static List<int> ListOfMobileLinesIDs = new List<int>() { 3, 5, 22 };
        public static List<int> ListOfNGNIDs = new List<int>() { 6, 24 };
        
        //To be Implemented 
        public static Dictionary<int, List<RateForNGN>> ngnRatesPerGateway = repo.RatesForNGN.GetGatewaysNGNRatesByID();
        public static Dictionary<string, List<RateForNGN>> ngnGatewaysRates = repo.RatesForNGN.GetGatewaysNGNRatesByName();

        public static Dictionary<int, List<Rates_International>> ratesPerGatway = repo.Rates.GetGatewaysRatesByID();
        public static Dictionary<string, Dictionary<string, List<Rates_National>>> nationalRates = new Dictionary<string,Dictionary<string,List<Rates_National>>>();

        public static List<Rates_National> GetNationalRates(Gateway gateway, string CountryCodeISO3)
        {
            List<Rates_National> tempNationalRates = repo.Rates.GetNationalRatesForCountryByGatewayID(gateway.ID, CountryCodeISO3).ToList();

            //Check of the nationalRates container is instantiated
            if (Repo.nationalRates == null)
                Repo.nationalRates = new Dictionary<string, Dictionary<string, List<Rates_National>>>();

            //Temporary Containers

            Dictionary<string, List<Rates_National>> tempCountryRates = new Dictionary<string, List<Rates_National>>();

            var gatewayRatesObj = Repo.gatewayRates.Find(item => item.GatewayID == gateway.ID);


            if (Repo.nationalRates.Keys.Contains(gateway.Name) == true)
            {
                tempCountryRates = Repo.nationalRates[gateway.Name];

                if (tempCountryRates != null && tempCountryRates.Keys.Contains(CountryCodeISO3))
                {
                    tempNationalRates = tempCountryRates[CountryCodeISO3];
                }
                else
                {
                    if (gatewayRatesObj != null && !string.IsNullOrEmpty(gatewayRatesObj.RatesTableName))
                    {
                        tempCountryRates.Add(CountryCodeISO3, tempNationalRates);
                        Repo.nationalRates[gateway.Name] = tempCountryRates;
                    }
                }
            }
            else
            {
                if (gatewayRatesObj != null && !string.IsNullOrEmpty(gatewayRatesObj.RatesTableName))
                {
                    tempCountryRates.Add(CountryCodeISO3, tempNationalRates);
                    Repo.nationalRates.Add(gateway.Name, tempCountryRates);
                }
            }

            return tempNationalRates;
        }
        
    }

}
