using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.DataModels;

namespace Lync2013Plugin.Interfaces
{
    public interface IPhoneCall
    {
        PhoneCall SetCallType(PhoneCall thisCall);

        PhoneCall ApplyRate(PhoneCall thisCall);

        PhoneCall ApplyExceptions(PhoneCall thisCall);
    }
}
