using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class Users
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string SipAccount { get; set; }
        public string SiteName { get; set; }
        public string Department { get; set; }
        public string TelephoneNumber { get; set; }
        public bool NotifyUser { get; set; }
        public bool UpdatedByAD { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
