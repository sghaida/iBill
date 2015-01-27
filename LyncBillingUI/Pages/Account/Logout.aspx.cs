using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using LyncBillingUI.Account;

namespace LyncBillingUI.Pages.Account
{
    public partial class Logout : System.Web.UI.Page
    {
        public UserSession current_session { get; set; }
        public static string normalUserRoleName { get; set; }
        public static string userDelegeeRoleName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetRolesNames();

            // get the current session
            current_session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            if (current_session != null)
            {
                if (current_session.ActiveRoleName == normalUserRoleName)
                {
                    //Session.Abandon();
                    HttpContext.Current.Session.Contents["UserData"] = null;
                    HttpContext.Current.Session.Abandon();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.ClearHeaders();

                    Response.Redirect(Global.APPLICATION_URL + "/Login");
                }
                else
                { 
                    Response.Redirect(Global.APPLICATION_URL + "/User/Dashboard");
                }
            }
            else
            { 
                Response.Redirect(Global.APPLICATION_URL + "/Login");
            }
        }


        //
        // Set the role names of User and Delegee
        private void SetRolesNames()
        {
            if (string.IsNullOrEmpty(normalUserRoleName))
            {
                var normalUserRole = Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.UserRoleID);
                normalUserRoleName = (normalUserRole != null ? normalUserRole.RoleName : string.Empty);
            }

            if (string.IsNullOrEmpty(userDelegeeRoleName))
            {
                var delegeeUserRole = Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.UserDelegeeRoleID);
                userDelegeeRoleName = (delegeeUserRole != null ? delegeeUserRole.RoleName : string.Empty);
            }
        }

    }
}