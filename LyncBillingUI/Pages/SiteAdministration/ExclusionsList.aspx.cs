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
    public partial class ExclusionsList : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private static List<Site> usersSites;

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
            GetUserSitesDate();

            InitializeGrid();
        }


        private void GetUserSitesDate()
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                usersSites = Global.DATABASE.SystemRoles.GetSitesByRoles(CurrentSession.SystemRoles, Functions.SiteAdminRoleName);
            }
        }


        private void InitializeGrid()
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                if (usersSites.Count > 0)
                {
                    var firstSite = (Site)usersSites.First();

                    FilterExceptionsBySite.SetValueAndFireSelect(firstSite.Id);

                    ManageExceptionsListGrid.GetStore().DataSource = Global.DATABASE.PhoneCallsExclusions.GetBySiteId(firstSite.Id);
                    ManageExceptionsListGrid.GetStore().DataBind();
                }
            }
        }


        protected void FilterExceptionsBySiteStore_Load(object sender, EventArgs e)
        {
            FilterExceptionsBySite.GetStore().DataSource = usersSites;
            FilterExceptionsBySite.DataBind();
        }


        protected void FilterExceptionsBySite_Selected(object sender, DirectEventArgs e)
        {
            int siteID;

            if (FilterExceptionsBySite.SelectedItem.Index > -1)
            {
                siteID = Convert.ToInt32(FilterExceptionsBySite.SelectedItem.Value);

                ManageExceptionsListGrid.GetStore().DataSource = Global.DATABASE.PhoneCallsExclusions.GetBySiteId(siteID);
                ManageExceptionsListGrid.GetStore().DataBind();
            }
        }


        protected void NewException_SitesListStore_Load(object sender, EventArgs e)
        {
            NewException_SitesList.GetStore().RemoveAll();
            NewException_SitesList.GetStore().LoadData(usersSites);
        }


        protected void AddExceptionButton_Click(object sender, DirectEventArgs e)
        {
            AddNewExceptionWindowPanel.Show();
        }


        protected void CancelNewExceptionButton_Click(object sender, DirectEventArgs e)
        {
            AddNewExceptionWindowPanel.Hide();
        }


        protected void AddNewExceptionWindowPanel_BeforeHide(object sender, DirectEventArgs e)
        {
            NewException_ExceptionEntity.Text = null;
            NewException_ExceptionType.Select(0);
            NewException_ZeroCost.Select(0);
            NewException_AutoMark.Select(0);
            NewException_Description.Text = null;
            NewException_SitesList.Value = null;

            NewException_StatusMessage.Text = string.Empty;
        }


        protected void SaveChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            bool statusFlag = false;
            string messageType = "error";
            string notificationMessage = string.Empty;

            int siteID;
            string json = string.Empty;
            ChangeRecords<PhoneCallExclusion> storeShangedData;
            List<PhoneCallExclusion> existingExceptions;

            json = e.ExtraParams["Values"];

            if (FilterExceptionsBySite.SelectedItem.Index > -1)
            {
                if (!string.IsNullOrEmpty(json))
                {
                    siteID = Convert.ToInt32(FilterExceptionsBySite.SelectedItem.Value);
                    storeShangedData = new StoreDataHandler(json).BatchObjectData<PhoneCallExclusion>();
                    existingExceptions = Global.DATABASE.PhoneCallsExclusions.GetBySiteId(siteID);


                    //Update existent Exceptions
                    if (storeShangedData.Updated.Count > 0)
                    {
                        foreach (var storeEexceptionObject in storeShangedData.Updated)
                        {
                            //Convert the entity to lowercase
                            storeEexceptionObject.ExclusionSubject = storeEexceptionObject.ExclusionSubject.ToString().ToLower();

                            //Check for duplicate Exceptions
                            var duplicateException = existingExceptions.Find(item =>
                                    item.ExclusionSubject == storeEexceptionObject.ExclusionSubject &&
                                    item.ExclusionType == storeEexceptionObject.ExclusionType &&
                                    item.SiteId == storeEexceptionObject.SiteId
                            );

                            if (duplicateException != null && duplicateException.Id != storeEexceptionObject.Id)
                            {
                                messageType = "error";
                                notificationMessage = String.Format("Couldn't save changes. The new changes would create duplicate exceptions.");
                            }
                            else if (storeEexceptionObject.ExclusionType == "Source" && false == HelperFunctions.IsValidEmail(storeEexceptionObject.ExclusionSubject))
                            {
                                messageType = "error";
                                notificationMessage = String.Format("The Exception(s) were NOT updated successfully. You must provide valid Emails for Source Exceptions.");
                            }
                            else
                            {
                                //Update the record in the database
                                statusFlag = Global.DATABASE.PhoneCallsExclusions.Update(storeEexceptionObject);

                                //Check database status flag
                                if (statusFlag == false)
                                {
                                    ManageExceptionsListGrid.GetStore().Filter("SiteID", storeEexceptionObject.SiteId.ToString());

                                    messageType = "error";
                                    notificationMessage = String.Format("The Exception(s) were NOT updated successfully. An error has occured. Please try again.");
                                }
                                else
                                {
                                    ManageExceptionsListGrid.GetStore().GetById(storeEexceptionObject.Id).Commit();
                                    ManageExceptionsListGrid.GetStore().Filter("SiteID", storeEexceptionObject.SiteId.ToString());

                                    messageType = "success";
                                    notificationMessage = "Exception(s) were updated successfully, changes were saved.";
                                }
                            }
                        }//end for-each

                        Functions.Message("Update Exceptions", notificationMessage, messageType, hideDelay: 10000, width: 200, height: 120);
                    }//end inner-if


                    //Delete Existing Exceptions
                    if (storeShangedData.Deleted.Count > 0)
                    {
                        foreach (var storeEexceptionObject in storeShangedData.Deleted)
                        {
                            statusFlag = Global.DATABASE.PhoneCallsExclusions.Delete(storeEexceptionObject);

                            if (statusFlag == false)
                                break;
                        }


                        if (statusFlag == true)
                        {
                            messageType = "success";
                            notificationMessage = "Exceptions(s) were deleted successfully, changes were saved.";
                        }
                        else
                        {
                            messageType = "error";
                            notificationMessage = "Some exceptions weren't deleted, please try again!";
                        }

                        Functions.Message("Delete Delegees", notificationMessage, messageType, hideDelay: 10000, width: 200, height: 100);

                        ManageExceptionsListGrid.GetStore().RemoveAll();
                        ManageExceptionsListGrid.GetStore().DataSource = Global.DATABASE.PhoneCallsExclusions.GetBySiteId(siteID);
                        ManageExceptionsListGrid.GetStore().DataBind();
                    }
                }
            }//end outer-if
        }


        protected void AddNewExceptionButton_Click(object sender, DirectEventArgs e)
        {
            PhoneCallExclusion NewPhoneException;
            List<PhoneCallExclusion> ExistingPhoneExceptions;

            int GridFilter_SelectdSiteID;

            int ExceptionSiteID;
            string ExceptionEntity = string.Empty;
            string ExceptionEntityType = string.Empty;
            string ExceptionZeroCost = string.Empty;
            string ExceptionAutoMark = string.Empty;
            string ExceptionDescription = string.Empty;

            string statusMessage = string.Empty;
            string successStatusMessage = string.Empty;

            if (!string.IsNullOrEmpty(NewException_ExceptionEntity.Text) &&
                NewException_ExceptionType.SelectedItem.Index > -1 &&
                NewException_ZeroCost.SelectedItem.Index > -1 &&
                NewException_AutoMark.SelectedItem.Index > -1 &&
                NewException_SitesList.SelectedItem.Index > -1 &&
                !string.IsNullOrEmpty(NewException_Description.Text))
            {
                NewPhoneException = new PhoneCallExclusion();

                ExceptionSiteID = Convert.ToInt32(NewException_SitesList.SelectedItem.Value);
                ExceptionEntity = Convert.ToString(NewException_ExceptionEntity.Text).ToLower();
                ExceptionEntityType = Convert.ToString(NewException_ExceptionType.SelectedItem.Value);
                ExceptionZeroCost = Convert.ToString(NewException_ZeroCost.SelectedItem.Value);
                ExceptionAutoMark = Convert.ToString(NewException_AutoMark.SelectedItem.Value);
                ExceptionDescription = Convert.ToString(NewException_Description.Text);


                //Check for duplicates
                ExistingPhoneExceptions = Global.DATABASE.PhoneCallsExclusions.GetAll().ToList();

                var duplicate = ExistingPhoneExceptions.Find(item =>
                        item.ExclusionSubject == ExceptionEntity &&
                        item.ExclusionType == ExceptionEntityType &&
                        item.SiteId == ExceptionSiteID);


                if (duplicate != null)
                {
                    statusMessage = "Cannot add duplicate Phone Exceptions!";
                }
                else if (ExceptionEntityType == "Source" && false == HelperFunctions.IsValidEmail(ExceptionEntity))
                {
                    statusMessage = "You can only add valid Emails as Source Exceptions!";
                }
                //This Phone Exception record doesn't exist, add it.
                else
                {
                    NewPhoneException.ExclusionSubject = ExceptionEntity;
                    NewPhoneException.ExclusionType = ExceptionEntityType;
                    NewPhoneException.ZeroCost = ExceptionZeroCost;
                    NewPhoneException.AutoMark = ExceptionAutoMark;
                    NewPhoneException.SiteId = ExceptionSiteID;
                    NewPhoneException.Description = ExceptionDescription;

                    //Insert the New Sites to the database
                    Global.DATABASE.PhoneCallsExclusions.Insert(NewPhoneException);

                    //Close the window
                    this.AddNewExceptionWindowPanel.Hide();

                    //Add the New Sites record to the store and apply the filter
                    if (FilterExceptionsBySite.SelectedItem.Index > -1)
                    {
                        GridFilter_SelectdSiteID = Convert.ToInt32(FilterExceptionsBySite.SelectedItem.Value);

                        if (GridFilter_SelectdSiteID == NewPhoneException.SiteId)
                        {
                            ManageExceptionsListGrid.GetStore().DataSource = Global.DATABASE.PhoneCallsExclusions.GetBySiteId(GridFilter_SelectdSiteID);
                            ManageExceptionsListGrid.GetStore().DataBind();
                        }
                    }

                    successStatusMessage = String.Format("The Phone Exception was added successfully.");
                }
            }
            else
            {
                statusMessage = "Please provide all the required information!";
            }

            this.NewException_StatusMessage.Text = statusMessage;

            if (!string.IsNullOrEmpty(successStatusMessage))
                Functions.Message("New Exceptions", successStatusMessage, "success", hideDelay: 10000, width: 200, height: 100);
        }//end-function
    }

}