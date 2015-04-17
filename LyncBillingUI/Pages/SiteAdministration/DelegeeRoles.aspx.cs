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
    public partial class DelegeeRoles : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private static List<Site> usersSites;
        private static List<DelegateRole> delegeesList;

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

            //Get the User Authorized List of Sites
            GetSitesDepartmentsAndDelegeesData();
        }


        //
        // Get the sites data for this user alongside site-departments, and delegee-users
        private void GetSitesDepartmentsAndDelegeesData(bool forceRefreshDelegees = false)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                //if(sitesList == null || sitesList.Count < 1)
                usersSites = Global.DATABASE.SystemRoles.GetSitesByRoles(CurrentSession.SystemRoles, Functions.SiteAdminRoleName);

                //if(delegeesList == null || delegeesList.Count < 1)
                delegeesList = Global.DATABASE.DelegateRoles.GetAll().ToList();
            }

            if (forceRefreshDelegees == true)
            {
                delegeesList = Global.DATABASE.DelegateRoles.GetAll().ToList();
            }
        }


        private List<string> GetUsersPerSite(int siteID)
        {
            List<LyncBillingBase.DataModels.User> users = new List<LyncBillingBase.DataModels.User>();
            List<string> usersList = new List<string>();

            var siteObject = usersSites.Find(site => site.Id == siteID);

            users = Global.DATABASE.Users.GetBySiteId(siteID);

            if (users.Count > 0)
            {
                foreach (var user in users)
                {
                    usersList.Add(user.SipAccount);
                }
            }

            return usersList;
        }

        [DirectMethod]
        protected void GetDelegates(object sender, DirectEventArgs e)
        {
            if (FilterDelegatesBySite.SelectedItem.Index != -1)
            {
                int siteId = Convert.ToInt32(FilterDelegatesBySite.SelectedItem.Value);

                var usersPerSite = GetUsersPerSite(siteId);

                //List<DelegateRole> selectedSitesDelegees = delegeesList
                //    .Where(delegee =>
                //        siteId == delegee.ManagedSiteId
                //        || siteId == delegee.ManagedSiteDepartment.SiteId
                //        || usersPerSite.Contains(delegee.ManagedUserSipAccount)
                //        || usersPerSite.Contains(delegee.DelegeeSipAccount))
                //    .AsParallel()
                //    .ToList();

                var uiReadyData = delegeesList
                    .Where(delegee =>
                        siteId == delegee.ManagedSiteId
                        || siteId == delegee.ManagedSiteDepartment.SiteId
                        || usersPerSite.Contains(delegee.ManagedUserSipAccount)
                        || usersPerSite.Contains(delegee.DelegeeSipAccount)
                    ).Select(item => new {
                        Id = item.Id,
                        DelegationType = item.DelegationType,
                        DelegeeSipAccount = item.DelegeeSipAccount,
                        ManagedUserSipAccount = item.ManagedUserSipAccount ?? string.Empty,
                        ManagedSiteId = item.ManagedSiteId,
                        ManagedSiteDepartmentId = item.ManagedSiteDepartmentId,
                        Description = item.Description ?? string.Empty,
                        ManagedSiteName = (item.ManagedSite != null ? item.ManagedSite.Name : string.Empty),
                        ManagedSiteDepartmentName = (item.ManagedSiteDepartment != null && item.ManagedSiteDepartment.Department != null ? item.ManagedSiteDepartment.Department.Name : string.Empty)
                    }).AsParallel()
                    .ToList();

                ManageDelegatesGrid.GetStore().ClearFilter();
                ManageDelegatesGrid.GetStore().DataSource = uiReadyData;
                ManageDelegatesGrid.GetStore().DataBind();
            }
        }

        [DirectMethod]
        protected void ShowAddDelegeePanel(object sender, DirectEventArgs e)
        {
            this.AddNewDelegeeWindowPanel.Show();
        }

        [DirectMethod]
        protected void CancelNewDelegeeButton_Click(object sender, DirectEventArgs e)
        {
            this.AddNewDelegeeWindowPanel.Hide();
        }

        [DirectMethod]
        protected void AddNewDelegeeWindowPanel_BeforeHide(object sender, DirectEventArgs e)
        {
            NewDelegee_DelegeeTypeCombobox.Select(0);
            NewDelegee_UserSipAccount.Clear();
            NewDelegee_DelegeeSipAccount.Clear();
            NewDelegee_DepartmentsList.Value = null;
            NewDelegee_SitesList.Value = null;
            NewDelegee_DepartmentsList.Hidden = true;
            NewDelegee_SitesList.Hidden = true;
            NewDelegee_StatusMessage.Text = string.Empty;
        }

        [DirectMethod]
        protected void DelegeeTypeMenu_Selected(object sender, DirectEventArgs e)
        {
            if (NewDelegee_DelegeeTypeCombobox.SelectedItem.Index != -1 && NewDelegee_DelegeeTypeCombobox.SelectedItem.Value != null)
            {
                var selected = Convert.ToInt32(NewDelegee_DelegeeTypeCombobox.SelectedItem.Value);

                if (selected == Global.DATABASE.Roles.SiteDelegeeRoleID)
                {
                    NewDelegee_SitesList.Hidden = false;
                    NewDelegee_DepartmentsList.Hidden = true;
                    NewDelegee_UserSipAccount.Hidden = true;
                }
                else if (selected == Global.DATABASE.Roles.DepartmentDelegeeRoleID)
                {
                    NewDelegee_SitesList.Hidden = false;
                    NewDelegee_DepartmentsList.Hidden = false;
                    NewDelegee_UserSipAccount.Hidden = true;
                }
                else
                {
                    NewDelegee_SitesList.Hidden = true;
                    NewDelegee_DepartmentsList.Hidden = true;
                    NewDelegee_UserSipAccount.Hidden = false;
                }
            }
        }


        protected void NewDelegee_UserSipAccount_BeforeQuery(object sender, DirectEventArgs e)
        {
            string searchTerm = string.Empty;
            List<LyncBillingBase.DataModels.User> matchedUsers;
            var sitesNames = usersSites.Select<Site, string>(site => site.Name).ToList<string>();

            if (NewDelegee_UserSipAccount.Value != null && NewDelegee_UserSipAccount.Value.ToString().Length > 3)
            {
                searchTerm = NewDelegee_UserSipAccount.Value.ToString();

                matchedUsers = Global.DATABASE.Users.GetBySearchTerm(searchTerm);

                //Return only the users in this site who match the query
                if (matchedUsers.Count > 0)
                    matchedUsers = matchedUsers.Where(user => sitesNames.Contains(user.SiteName)).ToList();

                NewDelegee_UserSipAccount.GetStore().DataSource = matchedUsers;
                NewDelegee_UserSipAccount.GetStore().LoadData(matchedUsers);
            }
        }


        protected void NewDelegee_DelegeeSipAccount_BeforeQuery(object sender, DirectEventArgs e)
        {
            string searchTerm = string.Empty;
            List<LyncBillingBase.DataModels.User> matchedUsers;
            var sitesNames = usersSites.Select<Site, string>(site => site.Name).ToList<string>();

            if (NewDelegee_DelegeeSipAccount.Value != null && NewDelegee_DelegeeSipAccount.Value.ToString().Length > 3)
            {
                searchTerm = NewDelegee_DelegeeSipAccount.Value.ToString();

                matchedUsers = Global.DATABASE.Users.GetBySearchTerm(searchTerm);

                //Return only the users in this site who match the query
                if (matchedUsers.Count > 0)
                    matchedUsers = matchedUsers.Where(user => sitesNames.Contains(user.SiteName)).ToList();

                NewDelegee_DelegeeSipAccount.GetStore().DataSource = matchedUsers;
                NewDelegee_DelegeeSipAccount.GetStore().LoadData(matchedUsers);
            }
        }


        protected void SitesListStore_Load(object sender, EventArgs e)
        {
            NewDelegee_SitesList.GetStore().DataSource = usersSites;
            NewDelegee_SitesList.GetStore().LoadData(usersSites);
        }


        protected void DelegatesSitesStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                FilterDelegatesBySite.GetStore().DataSource = usersSites;
                FilterDelegatesBySite.GetStore().LoadData(usersSites);
            }
        }


        protected void NewDelegee_SitesList_Selected(object sender, EventArgs e)
        {
            int siteId = Convert.ToInt32(NewDelegee_SitesList.SelectedItem.Value);
            var selectedSiteDepartments = Global.DATABASE.SitesDepartments.GetBySiteId(siteId);

            NewDelegee_DepartmentsList.ClearValue();
            NewDelegee_DepartmentsList.Clear();

            NewDelegee_DepartmentsList.GetStore().DataSource = selectedSiteDepartments;
            NewDelegee_DepartmentsList.GetStore().LoadData(selectedSiteDepartments);
        }

        [DirectMethod]
        protected void SaveChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            bool statusFlag = false;
            string successMessage = string.Empty;
            string errorMessage = string.Empty;

            string json = string.Empty;
            ChangeRecords<DelegateRole> storeShangedData;

            json = e.ExtraParams["Values"];

            if (!string.IsNullOrEmpty(json))
            {
                storeShangedData = new StoreDataHandler(json).BatchObjectData<DelegateRole>();

                //Delete existent delegees
                if (storeShangedData.Deleted.Count > 0)
                {
                    foreach (DelegateRole userDelgate in storeShangedData.Deleted)
                    {
                        statusFlag = Global.DATABASE.DelegateRoles.Delete(userDelgate);
                    }

                    if (statusFlag == true)
                    {
                        successMessage = "Delegee(s) were deleted successfully, changes were saved.";
                        Functions.Message("Delete Delegees", successMessage, "success", hideDelay: 10000, width: 200, height: 100);
                    }
                    else
                    {
                        errorMessage = "Some delegees weren't deleted, please try again!";
                        Functions.Message("Delete Delegees", errorMessage, "error", hideDelay: 10000, width: 200, height: 100);
                    }
                }
            }
        }


        protected void AddNewDelegeeButton_Click(object sender, DirectEventArgs e)
        {
            DelegateRole newDelegee;

            int selectedType = 0;
            string userSipAccount = string.Empty;
            string delegeeSipAccount = string.Empty;
            int delegeeSiteID = 0;
            int delegeeDepartmentID = 0;

            string statusMessage = string.Empty;
            string successStatusMessage = string.Empty;

            if (NewDelegee_DelegeeTypeCombobox.SelectedItem.Index > -1 && NewDelegee_DelegeeSipAccount.SelectedItem.Index > -1)
            {
                newDelegee = new DelegateRole();

                selectedType = Convert.ToInt32(NewDelegee_DelegeeTypeCombobox.SelectedItem.Value);
                delegeeSipAccount = NewDelegee_DelegeeSipAccount.SelectedItem.Value.ToString();

                var delegates = Global.DATABASE.DelegateRoles.GetByDelegeeSipAccount(delegeeSipAccount);
                var delegeeAccount = Global.DATABASE.Users.GetBySipAccount(delegeeSipAccount);

                if (selectedType == Global.DATABASE.Roles.UserDelegeeRoleID)
                {
                    if (NewDelegee_UserSipAccount.SelectedItem.Index > -1)
                    {
                        userSipAccount = Convert.ToString(NewDelegee_UserSipAccount.SelectedItem.Value.ReturnEmptyIfNull());
                        var userAccount = Global.DATABASE.Users.GetBySipAccount(userSipAccount);

                        //Check for duplicates
                        if (delegates.Find(item => item.ManagedUserSipAccount == userSipAccount) != null)
                        {
                            statusMessage = "Cannot add duplicate delegees!";
                        }
                        else
                        {
                            //Close the window
                            this.AddNewDelegeeWindowPanel.Hide();

                            newDelegee.DelegationType = Global.DATABASE.Roles.UserDelegeeRoleID;
                            newDelegee.ManagedUserSipAccount = userSipAccount;
                            newDelegee.DelegeeSipAccount = delegeeSipAccount;
                            newDelegee.Description = ((Role)Global.DATABASE.Roles.GetByRoleId(Global.DATABASE.Roles.UserDelegeeRoleID)).RoleDescription;

                            //Insert the delegee to the database
                            Global.DATABASE.DelegateRoles.Insert(newDelegee);

                            GetSitesDepartmentsAndDelegeesData(forceRefreshDelegees: true);

                            GetDelegates(null, null);

                            successStatusMessage = "Delegee was added successfully, select their respective Sites from the menu for more information.";
                        }
                    }
                    else
                    {
                        statusMessage = "Please select a user!";
                    }
                }

                else if (selectedType == Global.DATABASE.Roles.DepartmentDelegeeRoleID)
                {
                    if (NewDelegee_SitesList.SelectedItem.Index > -1 && NewDelegee_DepartmentsList.SelectedItem.Index > -1)
                    {
                        //Close the window
                        this.AddNewDelegeeWindowPanel.Hide();

                        delegeeSiteID = Convert.ToInt32(NewDelegee_SitesList.SelectedItem.Value);
                        delegeeDepartmentID = Convert.ToInt32(NewDelegee_DepartmentsList.SelectedItem.Value);

                        newDelegee.DelegationType = Global.DATABASE.Roles.DepartmentDelegeeRoleID;
                        newDelegee.ManagedUserSipAccount = userSipAccount;
                        newDelegee.DelegeeSipAccount = delegeeSipAccount;
                        newDelegee.ManagedSiteId = delegeeSiteID;
                        newDelegee.ManagedSite = ((Site)usersSites.Find(site => site.Id == delegeeSiteID));
                        newDelegee.ManagedSiteDepartmentId = delegeeDepartmentID;
                        newDelegee.ManagedSiteDepartment = ((SiteDepartment)Global.DATABASE.SitesDepartments.GetById(delegeeDepartmentID));
                        newDelegee.Description = ((Role)Global.DATABASE.Roles.GetByRoleId(Global.DATABASE.Roles.DepartmentDelegeeRoleID)).RoleDescription;

                        //Insert the delegee to the database
                        Global.DATABASE.DelegateRoles.Insert(newDelegee);

                        GetSitesDepartmentsAndDelegeesData(forceRefreshDelegees: true);

                        GetDelegates(null, null);

                        successStatusMessage = "Delegee was added successfully, select their respective Sites from the menu for more information.";
                    }
                    else
                    {
                        statusMessage = "Please select a Sites and a Departments!";
                    }
                }

                else if (selectedType == Global.DATABASE.Roles.SiteDelegeeRoleID)
                {
                    if (NewDelegee_SitesList.SelectedItem.Index > -1)
                    {
                        //Close the window
                        this.AddNewDelegeeWindowPanel.Hide();

                        delegeeSiteID = Convert.ToInt32(NewDelegee_SitesList.SelectedItem.Value);

                        newDelegee.DelegationType = Global.DATABASE.Roles.SiteDelegeeRoleID;
                        newDelegee.ManagedUserSipAccount = userSipAccount;
                        newDelegee.DelegeeSipAccount = delegeeSipAccount;
                        newDelegee.ManagedSiteId = delegeeSiteID;
                        newDelegee.ManagedSite = ((Site)usersSites.Find(site => site.Id == delegeeSiteID));
                        newDelegee.Description = ((Role)Global.DATABASE.Roles.GetByRoleId(Global.DATABASE.Roles.SiteDelegeeRoleID)).RoleDescription;

                        //Insert the delegee to the database
                        Global.DATABASE.DelegateRoles.Insert(newDelegee);

                        GetSitesDepartmentsAndDelegeesData(forceRefreshDelegees: true);

                        GetDelegates(null, null);

                        successStatusMessage = "Delegee was added successfully, select their respective Sites from the menu for more information.";
                    }
                    else
                    {
                        statusMessage = "Please select a Sites!";
                    }
                }
            }
            else
            {
                statusMessage = "Please provide the required information!";
            }

            this.NewDelegee_StatusMessage.Text = statusMessage;

            if (!string.IsNullOrEmpty(successStatusMessage))
            {
                Functions.Message("Add New Delegee", successStatusMessage, "success", hideDelay: 10000, width: 200, height: 100);
            }
        }

    }

}