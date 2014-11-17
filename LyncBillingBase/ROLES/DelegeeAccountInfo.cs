using LyncBillingBase.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Roles
{
    public class DelegeeAccountInfo
    {
        public int DelegeeTypeID { get; set; }

        //Sites Delegate Role related
        public Sites DelegeeSiteAccount { get; set; }

        //Departent Delegate Role related
        public SitesDepartments DelegeeDepartmentAccount { get; set; }

        //Users Delegate Role related
        public Users DelegeeUserAccount { get; set; }
        public List<PhoneCalls> DelegeeUserPhonecalls { get; set; }
        public Dictionary<string, PhoneBook> DelegeeUserAddressbook { set; get; }
    }
}
