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
        public UserSession CurrentSession { get; set; }
        public string HTML_SELECTED = string.Empty;
        public string PAGE_NAME = string.Empty;

        public static string normalUserRoleName {get; set; }
        public static string userDelegeeRoleName { get; set; }

        //public variable made available for the view
        public string DisplayName = string.Empty;

        public string UiElevateAccessDropdown = string.Empty;
        public string UiSwtichToDelegeeDropdown = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            //
            // Set the roles names
            SetRolesNames();

            //
            // Initialize the global Application URL
            InitializeTheGlobalApplicationUrl();

            // get the current session
            CurrentSession = (UserSession)HttpContext.Current.Session.Contents["UserData"];

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

            //
            // Get the Ui Dropdown Menus
            ConstructUiDropdownMenus();
        }


        //
        // Constructs the UiElevateAccessDropdown and UiSwtichToDelegeeDropdown Menus for the Navbar.
        private void ConstructUiDropdownMenus()
        {
            //
            // THE SWTICH TO DELEGEE DROPDOWN
            bool swtichToDelegee = 
                CurrentSession != null && 
                CurrentSession.IsDelegee && 
                (CurrentSession.ActiveRoleName == normalUserRoleName);

            if (true == swtichToDelegee)
            {
                //
                // The opening tags of the dropdown
                UiSwtichToDelegeeDropdown = String.Format(
                    "<li id='swtich-delegee-menu-tab' class='dropdown'>" + 
                    "<a href='#' class='dropdown-toggle ibill-nav-dropdown' data-toggle='dropdown' role='button' aria-expanded='false'>Switch to User&nbsp;<span class='caret'></span></a>" + 
                    "<ul class='dropdown-menu' role='menu'>");
                
                //
                // User Delegees
                if(CurrentSession.UserDelegateRoles.Any())
                {
                    UiSwtichToDelegeeDropdown = String.Format(
                            "{0}" + 
                            "<li class='dropdown-header'>Users</li>"
                        , UiSwtichToDelegeeDropdown);

                    foreach (var userDelegeeRecord in CurrentSession.UserDelegateRoles)
                    {
                        UiSwtichToDelegeeDropdown = String.Format(
                            "{0}" + 
                            "<li>" + 
                            "<a href='{1}/Authorize?access=userdelegee&identity={2}'>" + 
                            "{3}" + 
                            "</a>" + 
                            "</li>"
                        , UiSwtichToDelegeeDropdown
                        , Global.APPLICATION_URL
                        , userDelegeeRecord.ManagedUserSipAccount
                        , userDelegeeRecord.ManagedUser.DisplayName);
                    }
                }

                //
                // Divider
                if (CurrentSession.UserDelegateRoles.Any() && CurrentSession.DepartmentDelegateRoles.Any()) 
                {
                    UiSwtichToDelegeeDropdown = String.Format(
                            "{0}" + 
                            "<li class='divider'></li>"
                        , UiSwtichToDelegeeDropdown);
                }
                
                //
                // Department Delegees
                if(CurrentSession.DepartmentDelegateRoles.Any())
                {
                    UiSwtichToDelegeeDropdown = String.Format(
                            "{0}" +
                            "<li class='dropdown-header'>Departments</li>"
                        , UiSwtichToDelegeeDropdown);

                    foreach (var departmentDelegeeRecord in CurrentSession.DepartmentDelegateRoles)
                    {
                        UiSwtichToDelegeeDropdown = String.Format(
                                "{0}" +
                                "<li>" + 
                                "<a href='{1}/Authorize?access=depdelegee&identity={2}_{3}'>" + 
                                "{2} - {3}" + 
                                "</a>" + 
                                "</li>"
                            , UiSwtichToDelegeeDropdown
                            , Global.APPLICATION_URL
                            , departmentDelegeeRecord.ManagedSiteDepartment.Site.Name
                            , departmentDelegeeRecord.ManagedSiteDepartment.Department.Name);
                    }
                }

                //
                // Divider
                if (CurrentSession.DepartmentDelegateRoles.Any() && CurrentSession.SiteDelegateRoles.Any())
                {
                    UiSwtichToDelegeeDropdown = String.Format(
                            "{0}" +
                            "<li class='divider'></li>"
                        , UiSwtichToDelegeeDropdown);
                }

                //
                // Site Delegees
                if (CurrentSession.SiteDelegateRoles.Any())
                {
                    UiSwtichToDelegeeDropdown = String.Format(
                            "{0}" +
                            "<li class='dropdown-header'>Sites</li>"
                        , UiSwtichToDelegeeDropdown);

                    foreach (var siteDelegeeRecord in CurrentSession.SiteDelegateRoles)
                    {
                        UiSwtichToDelegeeDropdown = String.Format(
                                "{0}" +
                                "<li>" + 
                                "<a href='{1}/Authorize?access=sitedelegee&identity={2}'>" + 
                                "{2}" + 
                                "</a>" + 
                                "</li>"
                            , UiSwtichToDelegeeDropdown
                            , Global.APPLICATION_URL
                            , siteDelegeeRecord.ManagedSite.Name);
                    }
                }
                
                //
                // The closing tags of the dropdown
                UiSwtichToDelegeeDropdown = String.Format(
                        "{0}" +
                        "</ul>" + 
                        "</li>"
                    , UiSwtichToDelegeeDropdown);
            }


            //
            // THE ELEVATED ACCESS MENU
            bool elevateAccess =
                (CurrentSession != null) &&
                (
                    CurrentSession.IsDeveloper || 
                    CurrentSession.IsSystemAdmin || 
                    CurrentSession.IsSiteAdmin || 
                    CurrentSession.IsSiteAccountant || 
                    CurrentSession.IsDepartmentHead
                ) && 
                (CurrentSession.ActiveRoleName == normalUserRoleName);

            if (true == elevateAccess)
            {
                //
                // The opening tags of the dropdown menu
                UiElevateAccessDropdown = String.Format(
                    "<li id='elevate-access-menu-tab' class='dropdown'>" +
                    "<a href='#' class='dropdown-toggle ibill-nav-dropdown' data-toggle='dropdown' role='button' aria-expanded='false'>Elevate Access&nbsp;<span class='caret'></span></a>" +
                    "<ul class='dropdown-menu' role='menu'>");

                //
                // System Admin role
                if (CurrentSession.IsSystemAdmin || CurrentSession.IsDeveloper)
                {
                    UiElevateAccessDropdown = String.Format(
                            "{0}" +
                            "<li class='dropdown-header'>System Level</li>" +
                            "<li><a title='Elevate Access to System Admin Role' href='{1}/Authorize?access=sysadmin'>System Administrator</a></li>"
                        , UiElevateAccessDropdown
                        , Global.APPLICATION_URL);
                }

                //
                // Divider
                if ((CurrentSession.IsSystemAdmin && CurrentSession.IsSiteAdmin) || CurrentSession.IsDeveloper)
                {
                    UiElevateAccessDropdown = String.Format(
                            "{0}" +
                            "<li class='divider'></li>"
                        , UiElevateAccessDropdown);
                }

                //
                // List section header for the Site Level
                if ((CurrentSession.IsSiteAdmin || CurrentSession.IsSiteAccountant) || CurrentSession.IsDeveloper)
                {
                    UiElevateAccessDropdown = String.Format(
                            "{0}" +
                            "<li class='dropdown-header'>Site Level</li>"
                        , UiElevateAccessDropdown);
                }

                //
                // Site Admin
                if (CurrentSession.IsSiteAdmin || CurrentSession.IsDeveloper)
                {
                    UiElevateAccessDropdown = String.Format(
                            "{0}" +
                            "<li><a title='Elevate Access to Site Administrator Role' href='{1}/Authorize?access=admin'>Site Administrator</a></li>"
                        , UiElevateAccessDropdown
                        , Global.APPLICATION_URL);
                }

                //
                // Site Admin
                if (CurrentSession.IsSiteAccountant || CurrentSession.IsDeveloper)
                {
                    UiElevateAccessDropdown = String.Format(
                            "{0}" +
                            "<li><a title='Elevate Access to Site Accountant Role' href='{1}/Authorize?access=accounting'>Site Accountant</a></li>"
                        , UiElevateAccessDropdown
                        , Global.APPLICATION_URL);
                }

                //
                // Divider
                if ((CurrentSession.IsSiteAccountant && CurrentSession.IsDepartmentHead) || CurrentSession.IsDeveloper)
                {
                    UiElevateAccessDropdown = String.Format(
                            "{0}" +
                            "<li class='divider'></li>"
                        , UiElevateAccessDropdown);
                }

                if (CurrentSession.IsDepartmentHead || CurrentSession.IsDeveloper)
                {
                    UiElevateAccessDropdown = String.Format(
                            "{0}" +
                            "<li class='dropdown-header'>Department Level</li>" +
                            "<li><a title='Elevate Access to Department Head Role' href='{1}/Authorize?access=dephead'>Department Head</a></li>"
                        , UiElevateAccessDropdown
                        , Global.APPLICATION_URL);
                }

                //
                // Closing tags of the dropdown menu
                UiElevateAccessDropdown = String.Format(
                        "{0}" +
                        "</ul>" +
                        "</li>"
                    , UiElevateAccessDropdown);
            }//end-elevated-access-menu
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