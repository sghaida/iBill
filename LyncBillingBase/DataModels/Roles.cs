using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.Helpers;
using LyncBillingBase.Libs;

namespace LyncBillingBase.Roles
{
    public class Role : DataModel
    {
        //Data Attributes
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        //To be used from outside the class as lookup values
        public static int DeveloperRoleID { get { return Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.DeveloperRole)); } }
        public static int SystemAdminRoleID { get { return Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SystemAdminRole)); } }
        public static int SiteAdminRoleID { get { return Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SiteAdminRole)); } }
        public static int SiteAccountantRoleID { get { return Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SiteAccountantRole)); } }


        public static List<Role> GetRolesInformation()
        {
            List<Role> AllRolesInfo = new List<Role>();

            //System Admin
            AllRolesInfo.Add(new Role 
            { 
                RoleID = Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SystemAdminRole)),
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.SystemAdminRole))
            });

            //Sites Admin
            AllRolesInfo.Add(new Role
            {
                RoleID = Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SiteAdminRole)),
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.SiteAdminRole))
            });

            //Sites Accountant
            AllRolesInfo.Add(new Role
            {
                RoleID = Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SiteAccountantRole)),
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.SiteAccountantRole))
            });

            //Departments Head
            AllRolesInfo.Add(new Role
            {
                RoleID = Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.DepartmentHeadRole)),
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.DepartmentHeadRole))
            });

            //Normal User
            AllRolesInfo.Add(new Role
            {
                RoleID = Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.NormalUserRole)),
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.NormalUserRole))
            });

            return AllRolesInfo;
        }

    }

}
