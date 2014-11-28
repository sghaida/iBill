using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    public class Role : DataModel
    {
        // Data Attributes
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        //
        // Lookup Role Types IDs
        // System Roles
        public static int DeveloperRoleID { get { return Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.DeveloperRole)); } }
        public static int SystemAdminRoleID { get { return Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SystemAdminRole)); } }
        public static int SiteAdminRoleID { get { return Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SiteAdminRole)); } }
        public static int SiteAccountantRoleID { get { return Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SiteAccountantRole)); } }
        // Delegee Roles
        public static int UserDelegeeTypeID { get { return Convert.ToInt32(Enums.GetValue(Enums.DelegateTypes.UserDelegeeType)); } }
        public static int DepartmentDelegeeTypeID { get { return Convert.ToInt32(Enums.GetValue(Enums.DelegateTypes.DepartemntDelegeeType)); ; } }
        public static int SiteDelegeeTypeID { get { return Convert.ToInt32(Enums.GetValue(Enums.DelegateTypes.SiteDelegeeType)); ; } }

        //
        // Boolean Values Checkers
        // System Roles
        public static bool IsDeveloper(int RoleID) { return RoleID == DeveloperRoleID ? true : false; }
        public static bool IsSystemAdmin(int RoleID) { return RoleID == SystemAdminRoleID ? true : false; }
        public static bool IsSiteAdmin(int RoleID) { return RoleID == SiteAdminRoleID ? true : false; }
        public static bool IsSiteAccountant(int RoleID) { return RoleID == SiteAccountantRoleID ? true : false; }
        // Delegee Roles
        public static int IsUserDelegee(int DelegeeType) { return DelegeeType = UserDelegeeTypeID; }
        public static int IsDepartmentDelegee(int DelegeeType) { return DelegeeType = DepartmentDelegeeTypeID; }
        public static int IsSiteDelegee(int DelegeeType) { return DelegeeType = SiteDelegeeTypeID; }


        //
        // Returns a list of the above information with the names of each role.
        public static List<Role> GetRolesInformation()
        {
            List<Role> AllRolesInfo = new List<Role>();

            //Developer
            AllRolesInfo.Add(new Role
            {
                RoleID = DeveloperRoleID,
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.DeveloperRole))
            });

            //System Admin
            AllRolesInfo.Add(new Role 
            {
                RoleID = SystemAdminRoleID,
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.SystemAdminRole))
            });

            //Sites Admin
            AllRolesInfo.Add(new Role
            {
                RoleID = SiteAdminRoleID,
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.SiteAdminRole))
            });

            //Sites Accountant
            AllRolesInfo.Add(new Role
            {
                RoleID = SiteAccountantRoleID,
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

            //Site Delegate
            AllRolesInfo.Add(new Role
            {
                RoleID = SiteDelegeeTypeID,
                RoleName = Convert.ToString(Enums.GetDescription(Enums.DelegateTypes.SiteDelegeeType))
            });

            //Department Delegate
            AllRolesInfo.Add(new Role
            {
                RoleID = DepartmentDelegeeTypeID,
                RoleName = Convert.ToString(Enums.GetDescription(Enums.DelegateTypes.DepartemntDelegeeType))
            });

            //User Delegate
            AllRolesInfo.Add(new Role
            {
                RoleID = UserDelegeeTypeID,
                RoleName = Convert.ToString(Enums.GetDescription(Enums.DelegateTypes.UserDelegeeType))
            });

            return AllRolesInfo;
        }

    }

}
