using System.Collections.Generic;

using LyncBillingBase.DataModels;

namespace LyncBillingUI.Account
{
    public class DelegeeUserAccount
    {
        public int DelegeeTypeId { get; set; }

        //Sites Delegate Role related
        public Site DelegeeSiteAccount { get; set; }
        
        //Departent Delegate Role related
        public SiteDepartment DelegeeDepartmentAccount { get; set; }
        
        //Users Delegate Role related
        public User User { get; set; }
        public List<PhoneCall> DelegeeUserPhonecalls { get; set; }
        public Dictionary<string, PhoneBookContact> DelegeeUserAddressbook { set; get; }
    }
}