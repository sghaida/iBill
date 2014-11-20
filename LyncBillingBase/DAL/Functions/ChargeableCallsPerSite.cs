using LyncBillingBase.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL.Functions
{
    [DataSource(DataSourceName = "Get_ChargeableCalls_PerSite", DataSource = Enums.DataSources.DBFunction)]
    public class ChargeableCallsPerSite : PhoneCall
    {
    }
}
