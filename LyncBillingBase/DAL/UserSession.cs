using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    class UserSession
    {
        private static List<UserSession> usersSessions = new List<UserSession>();
        private List<DelegateRole> userDelegees = new List<DelegateRole>();
    }
}
