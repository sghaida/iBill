using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LyncBillingUI.Account;

namespace LyncBillingUI
{
    public partial class SiteMaster : MasterPage
    {
        public UserSession current_session { get; set; }
        public string HTML_SELECTED = string.Empty;
        public string PAGE_NAME = string.Empty;

        public static string normalUserRoleName {get; set; }
        public static string userDelegeeRoleName { get; set; }

        //public variable made available for the view
        public string DisplayName = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            //
            // Set the roles names
            SetRolesNames();

            //
            // Initialize the global Application URL
            InitializeTheGlobalApplicationUrl();

            // get the current session
            current_session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            // the HTML selected class
            HTML_SELECTED = "class='active'";

            // The current page name
            PAGE_NAME = this.Page.Request.FilePath.ToString().Replace("/", "_").Replace(".aspx", "").ToLower(); //this converts the string from "/ui/example.aspx" to "_ui_example"

            if (PAGE_NAME[0] == '_')
            {
                PAGE_NAME = PAGE_NAME.Remove(0, 1); //this removes the first underscore (_), the final string will look like: "ui_example"
            }

            // Set the referrer hidden input to the current page name
            this.ThisPageReferrer.Value = PAGE_NAME;

            // format the User's effective display name
            DisplayName = GetEffectiveDisplayName();
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

            if(string.IsNullOrEmpty(userDelegeeRoleName))
            {
                var delegeeUserRole = Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.UserDelegeeRoleID);
                userDelegeeRoleName = (delegeeUserRole != null ? delegeeUserRole.RoleName : string.Empty);
            }
        }


        //
        // Initialize the global Application URL
        private void InitializeTheGlobalApplicationUrl()
        {
            Uri originalUri;

            if(string.IsNullOrEmpty(Global.APPLICATION_URL))
            { 
                originalUri = System.Web.HttpContext.Current.Request.Url;
                Global.APPLICATION_URL = String.Format("{0}://{1}", originalUri.Scheme, originalUri.Authority);
            }
        }


        //
        // Get the user displayname.
        private string GetEffectiveDisplayName()
        {
            string userDisplayName = string.Empty;
            UserSession session = (UserSession)HttpContext.Current.Session.Contents["UserData"];

            if (session != null)
            {
                //If the user is a normal one, just return the normal user sipaccount.
                if (session.ActiveRoleName == normalUserRoleName)
                {
                    userDisplayName = session.User.DisplayName;
                }
                //if the user is a user-delegee return the delegate sipaccount.
                else if (session.ActiveRoleName == userDelegeeRoleName)
                {
                    userDisplayName = session.DelegeeUserAccount.User.DisplayName;
                }
            }

            return userDisplayName;
        }
    }
}