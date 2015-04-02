using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LyncBillingBase.DataModels;
using LyncBillingUI.Account;

namespace LyncBillingUI.Pages.SiteAccounting
{
    public partial class DisputedCalls : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;

        private static string normalUserRoleName { get; set; }
        private static string userDelegeeRoleName { get; set; }
        private static string allowedRoleName { get; set; }

        // This actually takes a copy of the current session for some uses on the frontend.
        public UserSession CurrentSession { get; set; }

        private static List<Site> userSites;


        protected void Page_Load(object sender, EventArgs e)
        {
            //
            // Set the role names of User and Delegee
            SetRolesNames();

            // 
            // If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string RedirectTo = String.Format(@"{0}/User/PhoneCalls", Global.APPLICATION_URL);
                string Url = String.Format(@"{0}/Login?RedirectTo={1}", Global.APPLICATION_URL, RedirectTo);
                Response.Redirect(Url);
            }
            else
            {
                CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (CurrentSession.ActiveRoleName != normalUserRoleName && CurrentSession.ActiveRoleName != userDelegeeRoleName)
                {
                    string url = String.Format(@"{0}/Authenticate?access={1}", Global.APPLICATION_URL, CurrentSession.ActiveRoleName);
                    Response.Redirect(url);
                }
            }

            sipAccount = CurrentSession.GetEffectiveSipAccount();

            GetUserSitesData();
        }


        //
        // Set the role names of User and Delegee
        private void SetRolesNames()
        {
            if (string.IsNullOrEmpty(normalUserRoleName))
            {
                normalUserRoleName = Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.UserRoleID);
            }

            if (string.IsNullOrEmpty(userDelegeeRoleName))
            {
                userDelegeeRoleName = Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.UserDelegeeRoleID);
            }

            if (string.IsNullOrEmpty(allowedRoleName))
            {
                allowedRoleName = Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SiteAccountantRoleID);
            }
        }


        //
        // Get the sites data for this user
        private void GetUserSitesData()
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                userSites = Global.DATABASE.SystemRoles.GetSitesByRoles(CurrentSession.SystemRoles, allowedRoleName);
            }
        }

    }

}