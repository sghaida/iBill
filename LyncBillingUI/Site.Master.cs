using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LyncBillingUI.Helpers.Account;


namespace LyncBillingUI
{
    public partial class SiteMaster : MasterPage
    {
        //System Roles Names - Lookup variables
        protected static string systemAdminRoleName { get; set; }
        protected static string siteAdminRoleName { get; set; }
        protected static string siteAccountantRoleName { get; set; }
        protected static string departmentHeadRoleName { get; set; }

        //Delegee Roles Names - Lookup variables
        protected static string userDelegeeRoleName { get; set; }
        protected static string departmentDelegeeRoleName { get; set; }
        protected static string siteDelegeeRoleName { get; set; }

        //Normal User Role - Lookup variable
        protected static string normalUserRoleName { get; set; }


        //public variable made available for the view
        public UserSession CurrentSession { get; set; }
        public string HTML_SELECTED = string.Empty;
        public string PAGE_NAME = string.Empty;

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
                else if (CurrentSession.UserDelegateRoles.Any() && CurrentSession.SiteDelegateRoles.Any())
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
            // Normal User Role
            if (string.IsNullOrEmpty(normalUserRoleName))
            {
                normalUserRoleName = Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.UserRoleID);
            }

            // User Delegee Role
            if (string.IsNullOrEmpty(userDelegeeRoleName))
            {
                userDelegeeRoleName = Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.UserDelegeeRoleID);
            }

            // Department Delegee Role
            if (string.IsNullOrEmpty(departmentDelegeeRoleName))
            {
                departmentDelegeeRoleName = Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.DepartmentDelegeeRoleID);
            }

            // Site Delegee Role
            if (string.IsNullOrEmpty(siteDelegeeRoleName))
            {
                siteDelegeeRoleName = Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SiteDelegeeRoleID);
            }

            // System Admin Role
            if (string.IsNullOrEmpty(systemAdminRoleName))
            {
                systemAdminRoleName = Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SystemAdminRoleID);
            }

            // Site Admin Role
            if (string.IsNullOrEmpty(siteAdminRoleName))
            {
                siteAdminRoleName = Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SiteAdminRoleID);
            }

            // Site Accountant Role
            if (string.IsNullOrEmpty(siteAccountantRoleName))
            {
                siteAccountantRoleName = Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SiteAccountantRoleID);
            }

            // Department Head Role
            if (string.IsNullOrEmpty(departmentHeadRoleName))
            {
                departmentHeadRoleName = Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.DepartmentHeadRoleID);
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


        //
        //
        protected string GetUiElevatedAccessDropDownMenu(string activeRoleName)
        {
            string uiElevatedAccessDropdownMenu = string.Empty;

            if(activeRoleName == siteAccountantRoleName)
            {
                uiElevatedAccessDropdownMenu = String.Format(
                    "<li id='site-accountant-menu-tab' class='dropdown'>" + 
                        "<a href='#' class='dropdown-toggle ibill-nav-dropdown' data-toggle='dropdown' role='button' aria-expanded='false'>Navigation&nbsp;<span class='caret'></span></a> " + 
                        "<ul class='dropdown-menu' role='menu'> " + 
                            "<li><a href='{0}/Site/Accounting/Dashboard'>Dashboard</a></li> " + 
                            "<li class='divider'></li> " +
                            "<li class='dropdown-header'>Manage</li> " + 
                            "<li><a href='{0}/Site/Accounting/DisputedCalls'>Disputed Phone Calls</a></li> " + 
                            "<li class='divider'></li> " + 
                            "<li class='dropdown-header'>Notifications</li> " + 
                            "<li><a href='{0}/Site/Accounting/BillingCycle'>Billing Cycles</a></li> " + 
                            "<li class='divider'></li> " + 
                            "<li class='dropdown-header'>Reports</li> " + 
                            "<li><a href='{0}/Site/Accounting/MonthlyReports'>Monthly Reports</a></li> " + 
                            "<li><a href='{0}/Site/Accounting/PeriodicalReports'>Periodical Reports</a></li> " + 
                        "</ul> " + 
                    "</li>"
                    , Global.APPLICATION_URL
                );
            }

            else if(activeRoleName == siteAdminRoleName)
            {
                uiElevatedAccessDropdownMenu = String.Format(
                    "<li id='site-admin-menu-tab' class='dropdown'>" +
                        "<a href='#' class='dropdown-toggle ibill-nav-dropdown' data-toggle='dropdown' role='button' aria-expanded='false'>Navigation&nbsp;<span class='caret'></span></a> " +
                        "<ul class='dropdown-menu' role='menu'> " +
                            "<li><a href='{0}/Site/Administration/Dashboard'>Dashboard</a></li> " +
                            "<li class='divider'></li> " +
                            "<li class='dropdown-header'>User Roles</li> " +
                            "<li><a href='{0}/Site/Administration/SystemRoles'>System Roles</a></li> " +
                            "<li><a href='{0}/Site/Administration/DelegeeRoles'>User Delegee</a></li> " +
                            //"<li><a href='{0}/Site/Administration/DepartmentHeadRoles'>Department Heads</a></li> " +
                            "<li class='divider'></li> " +
                            "<li class='dropdown-header'>Telephony Rates</li> " +
                            "<li><a href='{0}/Site/Administration/TelephonyRates'>Telephony Rates</a></li> " +
                            "<li><a href='{0}/Site/Administration/NGNRates'>NGNs Rates</a></li> " +
                            "<li class='divider'></li> " +
                            "<li class='dropdown-header'>Email Notifications</li> " +
                            "<li><a href='{0}/Site/Administration/UsersBills'>User Bills</a></li> " +
                            "<li><a href='{0}/Site/Administration/UnallocatedCalls'>Unallocated Calls</a></li> " +
                            "<li class='divider'></li> " +
                            "<li class='dropdown-header'>Statistics</li> " +
                            "<li><a href='{0}/Site/Administration/Statistics/GeneralUsage'>General Usage</a></li> " +
                            "<li class='divider'></li> " +
                            "<li class='dropdown-header'>Reports</li> " +
                            "<li><a href='{0}/Site/Administration/Reports/GeneralUsage'>General Usage</a></li> " +
                            "<li><a href='{0}/Site/Administration/Reports/UsersUsage'>Users Usage</a></li> " +
                            "<li><a href='{0}/Site/Administration/Reports/DepartmentsUsage'>Departments Usage</a></li> " +
                            "<li class='divider'></li> " +
                            "<li><a href='{0}/Site/Administration/DIDS'>DIDs</a></li> " +
                            "<li><a href='{0}/Site/Administration/Exclusions'>Exclusions List</a></li> " +
                            "<li><a href='{0}/Site/Administration/NGNNumberingPlan'>NGN Numbering Plan</a></li> " +
                        "</ul> " +
                    "</li>"
                    , Global.APPLICATION_URL
                );
            }

            else if (activeRoleName == systemAdminRoleName)
            {
                uiElevatedAccessDropdownMenu = String.Format("");
            }

            return uiElevatedAccessDropdownMenu;
        }
    
    }

}