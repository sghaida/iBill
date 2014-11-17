using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class BundledAccounts
    {
        public string PrimarySipAccount { get; set; }
        public Users PrimaryUserAccount { get; set; }
        public List<string> AssociatedSipAccounts { get; set; }
    }
}
