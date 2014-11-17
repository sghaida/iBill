using LyncBillingBase.CONF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class StoreLoader
    {
        public static BillableCallTypesSection section;

        public static List<int> ListofChargeableCallTypes;
        public static List<int> ListOfFixedLinesIDs;
        public static List<int> ListOfMobileLinesIDs;
        public static List<int> ListOfNGNIDs;

        public static List<Sites> LISTOFSITES;
        public static List<Users> LISTOFUSERS;

        public static List<NumberingPlan> numberingPlan;
        public static List<NumberingPlanNGN> numberingPlanNGN;

        public static List<GatewaysRates> gatewayRates;

        public static List<Gateways> gateways;
        public static List<string> ListofGatewaysNames;

        public static Dictionary<int, List<RatesNGN>> ngnRatesPerGateway;
        public static Dictionary<string, List<RatesNGN>> ngnGatewaysRates;

        public static Dictionary<int, List<Rates_International>> ratesPerGatway;

        public static List<DIDs> dids;

        public static List<CallTypes> callTypes;

        public static List<ExceptionsList> ListOfExceptions;

        public static Dictionary<string, Dictionary<string, List<Rates_National>>> nationalRates;
    }
}
