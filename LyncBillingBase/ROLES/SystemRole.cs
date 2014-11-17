using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.ROLES
{
    public class SystemRole
    {
        public int ID { set; get; }
        public string SipAccount { get; set; }
        public int RoleID { get; set; }
        public int SiteID { get; set; }
        public string Description { get; set; }

        //The following are logical representation of existing data, they don't belong to the table
        public string SiteName { get; set; }
        public string RoleOwnerName { get; set; }
        public string RoleDescription { get; set; }
    }
}
