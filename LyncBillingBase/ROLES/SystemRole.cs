using LyncBillingBase.DAL;
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

        //To be used from outside the class as lookup values
        public static int DeveloperRoleID { get { return Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.DeveloperRole)); } }
        public static int SystemAdminRoleID { get { return Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SystemAdminRole)); } }
        public static int SiteAdminRoleID { get { return Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SiteAdminRole)); } }
        public static int SiteAccountantRoleID { get { return Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SiteAccountantRole)); } }

        //"This" System Role Flags
        public bool IsDeveloper() { return this.RoleID == DeveloperRoleID ? true : false; }
        public bool IsSystemAdmin() { return this.RoleID == SystemAdminRoleID ? true : false; }
        public bool IsSiteAdmin() { return this.RoleID == SiteAdminRoleID ? true : false; }
        public bool IsSiteAccountant() { return this.RoleID == SiteAccountantRoleID ? true : false; }
    }
}
