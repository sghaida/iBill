using LyncBillingBase;
using LyncBillingBase.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL.Functions
{
    [DataSource(DataSourceName="Get_ChargeableCalls_ForUser", DataSource=Enums.DataSources.DBFunction)]
    public class ChargeableCallsPerUser : PhoneCall
    {
        [FunctionsParameters(ParamerterName = "ChargingParty", Position=1)]
        public string ChargingParty { set; get; } 
    }
}
