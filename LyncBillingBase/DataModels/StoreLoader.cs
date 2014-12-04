using LyncBillingBase.Conf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataModels
{
    public class StoreLoader
    {
        public static BillableCallTypesSection section;

        public static List<int> ListofChargeableCallTypes;
        public static List<int> ListOfFixedLinesIDs;
        public static List<int> ListOfMobileLinesIDs;
        public static List<int> ListOfNGNIDs;

        public static List<Site> LISTOFSITES;
        public static List<User> LISTOFUSERS;

        public static List<NumberingPlan> numberingPlan;
        public static List<NumberingPlanForNGN> numberingPlanNGN;

        //public static List<GatewayRate> gatewayRates;

        public static List<Gateway> gateways;
        public static List<string> ListofGatewaysNames;

        public static Dictionary<int, List<Rates_NGN>> ngnRatesPerGateway;
        public static Dictionary<string, List<Rates_NGN>> ngnGatewaysRates;

        public static Dictionary<int, List<Rates_International>> ratesPerGatway;

        public static List<DID> dids;

        public static List<CallType> callTypes;

        public static List<PhoneCallExclusion> ListOfExceptions;

        public static Dictionary<string, Dictionary<string, List<Rates_National>>> nationalRates;
    }
}
