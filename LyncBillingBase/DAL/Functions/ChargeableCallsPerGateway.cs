using LyncBillingBase.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL.Functions
{
    [DataSource(DataSourceName = "Get_ChargeableCalls_PerGateway", DataSource = Enums.DataSources.DBTable)]
    public class ChargeableCallsPerGateway : PhoneCall
    {
    }
}
