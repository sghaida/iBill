using LyncBillingBase.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL.Functions
{
    [FunctionName("Get_ChargeableCalls_ForUser")]
    class ChargableCallsPerUser : PhoneCall
    {
        [FunctionsParameters(ParamerterName = "ChargingParty", Position=1)]
        string ChargingParty { set; get; } 
    }
}
