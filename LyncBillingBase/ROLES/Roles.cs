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
    public class Roles
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        public static List<Roles> GetRoles(List<string> columns, Dictionary<string, object> wherePart, int limits)
        {
            DataTable dt = new DataTable();
            List<Roles> Roles = new List<Roles>();
           
            return Roles;
        }

        
        public static List<Roles> GetRolesInformation()
        {
            List<Roles> AllRolesInfo = new List<Roles>();

            //System Admin
            AllRolesInfo.Add(new Roles 
            { 
                RoleID = Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SystemAdminRole)),
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.SystemAdminRole))
            });

            //Sites Admin
            AllRolesInfo.Add(new Roles
            {
                RoleID = Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SiteAdminRole)),
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.SiteAdminRole))
            });

            //Sites Accountant
            AllRolesInfo.Add(new Roles
            {
                RoleID = Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.SiteAccountantRole)),
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.SiteAccountantRole))
            });

            //Departments Head
            AllRolesInfo.Add(new Roles
            {
                RoleID = Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.DepartmentHeadRole)),
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.DepartmentHeadRole))
            });

            //Normal User
            AllRolesInfo.Add(new Roles
            {
                RoleID = Convert.ToInt32(Enums.GetValue(Enums.SystemRoles.NormalUserRole)),
                RoleName = Convert.ToString(Enums.GetDescription(Enums.SystemRoles.NormalUserRole))
            });


            return AllRolesInfo;
        }


        public static int InsertRole(Roles role)
        {
            int rowID = 0;
           

            return rowID;
        }


        public static bool UpdateRole(Roles role)
        {
            bool status = false;

            Dictionary<string, object> setPart = new Dictionary<string, object>();

            return status;
        }


        public static bool DeleteRole(Roles role)
        {
            bool status = false;

            try
            {
                //status = DBRoutines.DELETE(Enums.GetDescription(Enums.Roles.TableName), Enums.GetDescription(Enums.Roles.RoleID), role.RoleID);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return status;
        }

    }
}
