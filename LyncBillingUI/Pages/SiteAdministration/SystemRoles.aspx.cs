using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Newtonsoft.Json;

using CCC.UTILS.Libs;
using CCC.UTILS.Helpers;
using LyncBillingUI.Helpers;
using LyncBillingUI.Helpers.Account;
using LyncBillingBase.DataModels;


namespace LyncBillingUI.Pages.SiteAdministration
{
    public partial class SystemRoles : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private static List<Site> usersSites;
        private static List<SystemRole> assignedSystemRoles;

        // This actually takes a copy of the current session for some uses on the frontend.
        public UserSession CurrentSession { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            // 
            // If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string RedirectTo = String.Format(@"{0}/Site/Administration/Dashboard", Global.APPLICATION_URL);
                string Url = String.Format(@"{0}/Login?RedirectTo={1}", Global.APPLICATION_URL, RedirectTo);
                Response.Redirect(Url);
            }
            else
            {
                CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

                if (CurrentSession.ActiveRoleName != Functions.SiteAdminRoleName)
                {
                    string url = String.Format(@"{0}/Authorize?access={1}", Global.APPLICATION_URL, Functions.SiteAdminRoleName);
                    Response.Redirect(url);
                }
            }

            sipAccount = CurrentSession.GetEffectiveSipAccount();

            GetAllSitesAndSystemRolesRecords();
        }

        private void GetAllSitesAndSystemRolesRecords(bool forceRefreshSystemRoles = false)
        {
            // Initialize them if the request is not an ajax one
            if (!Ext.Net.X.IsAjaxRequest)
            {
                // Get user sites
                usersSites = Global.DATABASE.SystemRoles.GetSitesByRoles(CurrentSession.SystemRoles, Functions.SiteAdminRoleName);

                assignedSystemRoles = Global.DATABASE.SystemRoles.GetAll().ToList();
            }

            if (forceRefreshSystemRoles)
            {
                assignedSystemRoles = Global.DATABASE.SystemRoles.GetAll().ToList();
            }
        }

        private IEnumerable<object> GetUIFormattedSystemRoles(int siteId)
        {
            return assignedSystemRoles
                .Where(item => item.SiteId == siteId)
                .Select(item => new {
                    Id = item.Id,
                    RoleId = item.RoleId,
                    Description = item.Description,
                    SipAccount = item.SipAccount,
                    SiteId = item.SiteId,
                    RoleOwnerName = item.User.DisplayName,
                    SiteName = item.Site.Name
                });
        }


        [DirectMethod]
        protected void GetSystemRolesPerSite(object sender, DirectEventArgs e)
        {
            if (FilterSystemRolesBySite.SelectedItem.Index != -1)
            {
                int siteID = Convert.ToInt32(FilterSystemRolesBySite.SelectedItem.Value);

                ManageSystemRolesGrid.GetStore().DataSource = GetUIFormattedSystemRoles(siteID);
                ManageSystemRolesGrid.GetStore().DataBind();
            }
        }


        protected void FilterSystemRolesBySiteStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                FilterSystemRolesBySite.GetStore().DataSource = usersSites;
                FilterSystemRolesBySite.GetStore().DataBind();
            }
        }


        protected void NewSystemRole_UserSipAccount_BeforeQuery(object sender, DirectEventArgs e)
        {
            string searchTerm = string.Empty;
            List<LyncBillingBase.DataModels.User> matchedUsers;

            if (NewSystemRole_UserSipAccount.Value != null && NewSystemRole_UserSipAccount.Value.ToString().Length > 3)
            {
                searchTerm = NewSystemRole_UserSipAccount.Value.ToString();

                matchedUsers = Global.DATABASE.Users.GetBySearchTerm(searchTerm);

                NewSystemRole_UserSipAccount.GetStore().DataSource = matchedUsers;
                NewSystemRole_UserSipAccount.GetStore().DataBind();
            }
        }


        protected void SitesListStore_Load(object sender, EventArgs e)
        {
            NewSystemRole_SitesList.GetStore().DataSource = usersSites;
            NewSystemRole_SitesList.GetStore().LoadData(usersSites);
        }


        protected void ShowAddSystemRoleWindowPanel(object sender, DirectEventArgs e)
        {
            this.AddNewSystemRoleWindowPanel.Show();
        }


        protected void CancelNewSystemRoleButton_Click(object sender, DirectEventArgs e)
        {
            this.AddNewSystemRoleWindowPanel.Hide();
        }


        protected void AddNewSystemRoleWindowPanel_BeforeHide(object sender, DirectEventArgs e)
        {
            NewSystemRole_RoleTypeCombobox.Select(0);
            NewSystemRole_UserSipAccount.Clear();
            NewSystemRole_SitesList.Value = null;
            NewSystemRole_StatusMessage.Text = string.Empty;
        }


        protected void SaveChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            bool statusFlag = false;
            string successMessage = string.Empty;
            string errorMessage = string.Empty;

            string json = string.Empty;
            ChangeRecords<SystemRole> storeShangedData;

            json = e.ExtraParams["Values"];

            if (!string.IsNullOrEmpty(json))
            {
                storeShangedData = new StoreDataHandler(json).BatchObjectData<SystemRole>();

                //Delete existent delegees
                if (storeShangedData.Deleted.Count > 0)
                {
                    foreach (SystemRole systemRole in storeShangedData.Deleted)
                    {
                        statusFlag = Global.DATABASE.SystemRoles.Delete(systemRole);

                        assignedSystemRoles.RemoveAll(
                            item => 
                                item.Id == systemRole.Id 
                                && item.RoleId == systemRole.RoleId 
                                && item.SipAccount == systemRole.SipAccount 
                                && item.SiteId == systemRole.SiteId);
                    }

                    if (statusFlag == true)
                    {
                        successMessage = "System Role(s) were deleted successfully, changes were saved.";
                        Functions.Message("Delete System Roles", successMessage, "success", hideDelay: 10000, width: 200, height: 100);

                    }
                    else
                    {
                        errorMessage = "Some System Roles were NOT deleted, please try again!";
                        Functions.Message("Delete System Roles", errorMessage, "error", hideDelay: 10000, width: 200, height: 100);
                    }
                }
            }
        }


        protected void AddNewSystemRoleButton_Click(object sender, DirectEventArgs e)
        {
            SystemRole NewSystemRole;

            int siteId = 0;
            int roleId = 0;
            string userSipAccount = string.Empty;

            string statusMessage = string.Empty;
            string successStatusMessage = string.Empty;

            if (NewSystemRole_RoleTypeCombobox.SelectedItem.Index > -1 && NewSystemRole_UserSipAccount.SelectedItem.Index > -1 && NewSystemRole_SitesList.SelectedItem.Index > -1)
            {
                NewSystemRole = new SystemRole();

                siteId = Convert.ToInt32(NewSystemRole_SitesList.SelectedItem.Value);
                roleId = Convert.ToInt32(NewSystemRole_RoleTypeCombobox.SelectedItem.Value);
                userSipAccount = NewSystemRole_UserSipAccount.SelectedItem.Value.ToString();

                //Check for duplicates
                if (assignedSystemRoles.Find(item => item.SipAccount == userSipAccount && item.RoleId == roleId && item.SiteId == siteId) != null)
                {
                    statusMessage = "Cannot add duplicate system roles!";
                }
                //This system role record doesn't exist, add it.
                else
                {
                    var userAccount = Global.DATABASE.Users.GetBySipAccount(userSipAccount);

                    NewSystemRole.RoleId = roleId;
                    NewSystemRole.SipAccount = userSipAccount;
                    NewSystemRole.User = userAccount;
                    NewSystemRole.User.DisplayName = HelperFunctions.FormatUserDisplayName(userAccount.FullName, userAccount.SipAccount, returnNameIfExists: true, returnAddressPartIfExists: true);
                    //NewSystemRole.RoleOwnerName = HelperFunctions.FormatUserDisplayName(userAccount.FullName, userAccount.SipAccount, returnNameIfExists: true, returnAddressPartIfExists: true);
                    NewSystemRole.SiteId = siteId;
                    NewSystemRole.Site = ((Site)usersSites.Find(site => site.Id == siteId));
                    //NewSystemRole.SiteName = ((Site)usersSites.Find(site => site.SiteID == siteId)).SiteName;
                    var roleDescription = Global.DATABASE.Roles.GetByRoleId(roleId);
                    NewSystemRole.Description = String.Format("{0} - {1}", NewSystemRole.Site.Name, roleDescription.RoleDescription);

                    //Insert the delegee to the database
                    Global.DATABASE.SystemRoles.Insert(NewSystemRole);

                    GetAllSitesAndSystemRolesRecords(forceRefreshSystemRoles: true);

                    GetSystemRolesPerSite(null, null);

                    successStatusMessage = "System Role was added successfully, select their respective Sites from the menu for more information.";

                    //Close the window
                    this.AddNewSystemRoleWindowPanel.Hide();
                }//End else
            }
            else
            {
                statusMessage = "Please provide the required information!";
            }

            this.NewSystemRole_StatusMessage.Text = statusMessage;

            if (!string.IsNullOrEmpty(successStatusMessage))
            {
                Functions.Message("Add New System Role", successStatusMessage, "success", hideDelay: 10000, width: 200, height: 100);
            }
        }
        

    }

}