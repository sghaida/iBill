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
    public partial class DIDs : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private static List<Site> usersSites;
        private static List<Did> allDIDs;

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

            // Get User Sites
            GetUserSitesAndDIDsData();
        }


        private void GetUserSitesAndDIDsData()
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                usersSites = Global.DATABASE.SystemRoles.GetSitesByRoles(CurrentSession.SystemRoles, Functions.SiteAdminRoleName);
            }

            //Get the DIDs of these sites
            allDIDs = Global.DATABASE.DiDs.GetAll()
                .AsParallel()
                .Where(item => usersSites.Find(targetSite => targetSite.Id == item.SiteId) != null)
                .Select(item => { item.SiteName = item.Site.Name; return item; })
                .ToList();
        }


        protected void Editor_SitesComboboxStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                Editor_SitesCombobox.GetStore().DataSource = usersSites;
                Editor_SitesCombobox.GetStore().DataBind();
            }
        }


        protected void NewDID_DIDSiteStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                NewDID_DIDSite.GetStore().DataSource = usersSites;
                NewDID_DIDSite.GetStore().DataBind();
            }
        }


        protected void ManageDIDsGridStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                ManageDIDsGrid.GetStore().DataSource = allDIDs;
                ManageDIDsGrid.GetStore().DataBind();
            }
        }


        protected void ShowAddNewDIDWindowPanel(object sender, DirectEventArgs e)
        {
            this.AddNewDIDWindowPanel.Show();
        }


        protected void CancelNewDIDButton_Click(object sender, DirectEventArgs e)
        {
            this.AddNewDIDWindowPanel.Hide();
        }


        protected void AddNewDIDWindowPanel_BeforeHide(object sender, DirectEventArgs e)
        {
            NewDID_DIDSite.Text = null;
            NewDID_DIDPattern.Text = null;
            NewDID_Description.Text = null;
            NewDID_StatusMessage.Text = null;
        }


        protected void SaveChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            bool statusFlag = false;
            string messageType = "error";
            string notificationMessage = string.Empty;

            string json = string.Empty;
            ChangeRecords<Did> storeChangedData;

            json = e.ExtraParams["Values"];

            if (!string.IsNullOrEmpty(json))
            {
                storeChangedData = new StoreDataHandler(json).BatchObjectData<Did>();

                //DELETE EXISTING DIDS
                if (storeChangedData.Deleted.Count > 0)
                {
                    foreach (Did storeDIDObject in storeChangedData.Deleted)
                    {
                        statusFlag = Global.DATABASE.DiDs.Delete(storeDIDObject);

                        if (statusFlag == false)
                        {
                            messageType = "error";
                            notificationMessage = String.Format("The DID(s) were NOT deleted successfully. An error has occured. Please try again.");

                            break;
                        }

                        messageType = "success";
                        notificationMessage = "DID(s) were updated successfully, changes were saved.";
                    }

                    ManageDIDsGrid.GetStore().CommitChanges();

                    Functions.Message("Update DIDs", notificationMessage, messageType, hideDelay: 10000, width: 200, height: 120);
                }


                //RESET STATUS FLAG
                statusFlag = false;


                //Update Existing DIDs
                if (storeChangedData.Updated.Count > 0)
                {
                    foreach (Did storeDIDObject in storeChangedData.Updated)
                    {
                        //Initialize the SiteID
                        //storeDIDObject.SiteId = ((Site)usersSites.Find(item => item.Name == storeDIDObject.Site.Name)).Id;
                        var site = ((Site)usersSites.Find(item => item.Name == storeDIDObject.SiteName));
                        storeDIDObject.Site = site;
                        storeDIDObject.SiteId = site.Id;

                        //Check the original version of the DID object
                        var originalDIDObject = (Did)allDIDs.Find(DID => DID.Id == storeDIDObject.Id);

                        //Check for duplicate site name
                        //If the DIDs Pattern was changed
                        if (storeDIDObject.Regex != originalDIDObject.Regex)
                        {
                            //If the changed (submitted) DIDs Pattern alread exists in the system, exit and display error message
                            if (allDIDs.Find(DID => DID.Regex == storeDIDObject.Regex && DID.SiteId == storeDIDObject.SiteId) != null)
                            {
                                messageType = "error";
                                notificationMessage = String.Format("DID(s) was not changed to due to a duplicate DIDs Pattern. Please enter a valide DIDs Pattern.");

                                break;
                            }
                        }

                        //Update it in the database
                        statusFlag = Global.DATABASE.DiDs.Update(storeDIDObject);

                        //If an error has occured during the Database Update, display error message
                        if (statusFlag == false)
                        {
                            messageType = "error";
                            notificationMessage = String.Format("The DID(s) were NOT updated successfully. An error has occured. Please try again.");
                        }
                        else
                        {
                            messageType = "success";
                            notificationMessage = "DID(s) were updated successfully, changes were saved.";

                            ManageDIDsGrid.GetStore().GetById(storeDIDObject.Id).Set(storeDIDObject);
                            ManageDIDsGrid.GetStore().CommitChanges();
                        }
                    }

                    Functions.Message("Update DIDs", notificationMessage, messageType, hideDelay: 10000, width: 200, height: 120);
                }//End if
            }//End if
        }


        protected void AddNewDIDButton_Click(object sender, DirectEventArgs e)
        {
            Did NewDID;

            int SiteID;
            string DIDPattern = string.Empty;
            string Description = string.Empty;

            string statusMessage = string.Empty;
            string successStatusMessage = string.Empty;

            if (!string.IsNullOrEmpty(NewDID_DIDPattern.Text) && !string.IsNullOrEmpty(NewDID_Description.Text) && NewDID_DIDSite.SelectedItem.Index > -1)
            {
                NewDID = new Did();

                SiteID = Convert.ToInt32(NewDID_DIDSite.SelectedItem.Value);
                DIDPattern = NewDID_DIDPattern.Text.ToString();
                Description = NewDID_Description.Text.ToString();

                //Check for duplicates
                if (allDIDs.Find(DID => DID.Regex == DIDPattern) != null)
                {
                    statusMessage = "Cannot add duplicate DIDs!";
                }
                //This Sites record doesn't exist, add it.
                else
                {
                    NewDID.SiteId = SiteID;
                    NewDID.Regex = DIDPattern;
                    NewDID.Description = Description;
                    NewDID.Site = ((Site)usersSites.Find(site => site.Id == SiteID));

                    //Insert the New Sites to the database
                    NewDID.Id = Global.DATABASE.DiDs.Insert(NewDID);

                    //Close the window
                    this.AddNewDIDWindowPanel.Hide();

                    //Add the New Sites record to the store and apply the filter
                    ManageDIDsGrid.GetStore().Add(new { SiteId = NewDID.SiteId, Regex = NewDID.Regex, Description = NewDID.Description, SiteName = NewDID.Site.Name });
                    ManageDIDsGrid.GetStore().CommitChanges();

                    successStatusMessage = String.Format("The DIDs was added successfully.");
                }
            }
            else
            {
                statusMessage = "Please provide all the required information!";
            }

            this.NewDID_StatusMessage.Text = statusMessage;

            if (!string.IsNullOrEmpty(successStatusMessage))
            {
                Functions.Message("New DIDs", successStatusMessage, "success", hideDelay: 10000, width: 200, height: 100);
            }
        }

    }

}