using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LyncBillingUI.Helpers
{
    public static class Functions
    {
        //
        // System Roles Names - Lookup variables
        public static string SystemAdminRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SystemAdminRoleID); } }
        public static string SiteAdminRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SiteAdminRoleID); } }
        public static string SiteAccountantRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SiteAccountantRoleID); } }
        public static string DepartmentHeadRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.DepartmentHeadRoleID); } }

        //
        // Delegee Roles Names - Lookup variables
        public static string UserDelegeeRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.UserDelegeeRoleID); } }
        public static string DepartmentDelegeeRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.DepartmentDelegeeRoleID); } }
        public static string SiteDelegeeRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SiteDelegeeRoleID); } }

        //
        // Normal User Role - Lookup variable
        public static string NormalUserRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.UserRoleID); } }
    }
}