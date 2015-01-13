using System.Collections.Generic;
using CCC.ORM.DataAccess;

namespace LyncBillingBase.DataModels
{
    public class DelegeeAccountInfo : DataModel
    {
        public int DelegeeTypeID { get; set; }
        //Sites Delegate Role related
        public Site DelegeeSiteAccount { get; set; }
        //Departent Delegate Role related
        public SiteDepartment DelegeeDepartmentAccount { get; set; }
        //Users Delegate Role related
        public User DelegeeUserAccount { get; set; }
        public List<PhoneCall> DelegeeUserPhonecalls { get; set; }
        public Dictionary<string, PhoneBookContact> DelegeeUserAddressbook { set; get; }
    }
}