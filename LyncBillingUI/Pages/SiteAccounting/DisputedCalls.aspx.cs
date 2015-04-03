using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;
using System.Web.Script.Serialization;
using Ext.Net;

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
                string RedirectTo = String.Format(@"{0}/Site/Accounting/Dashboard", Global.APPLICATION_URL);
                string Url = String.Format(@"{0}/Login?RedirectTo={1}", Global.APPLICATION_URL, RedirectTo);
                Response.Redirect(Url);
            }
            else
            {
                CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (CurrentSession.ActiveRoleName != allowedRoleName)
                {
                    string url = String.Format(@"{0}/Authorize?access={1}", Global.APPLICATION_URL, CurrentSession.ActiveRoleName);
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


        private List<PhoneCall> GetDisputedPhoneCalls(int SiteID, string DisputedStatus)
        {
            List<PhoneCall> disputedPhoneCalls = new List<PhoneCall>();

            if (SiteID > 0 && !string.IsNullOrEmpty(DisputedStatus))
            {
                string siteName = Global.DATABASE.Sites.GetNameById(SiteID);

                switch (DisputedStatus)
                {
                    case "Pending":
                        disputedPhoneCalls = Global.DATABASE.PhoneCalls.GetPendingDisputedCallsBySite(siteName).ToList();
                        break;

                    case "Accepted":
                        disputedPhoneCalls = Global.DATABASE.PhoneCalls.GetEvaluatedDisputedCallsBySite(siteName).Where(phonecall => phonecall.AcDisputeStatus == "Accepted").ToList();
                        break;

                    case "Rejected":
                        disputedPhoneCalls = Global.DATABASE.PhoneCalls.GetEvaluatedDisputedCallsBySite(siteName).Where(phonecall => phonecall.AcDisputeStatus == "Rejected").ToList();
                        break;
                }
            }//end-if

            return disputedPhoneCalls;
        }


        protected void FilterDisputedCallsBySiteStore_Load(object sender, EventArgs e)
        {
            FilterDisputedCallsBySite.GetStore().DataSource = userSites;
            FilterDisputedCallsBySite.GetStore().DataBind();
        }


        protected void FilterDisputedCallsBySite_Selecting(object sender, DirectEventArgs e)
        {
            int siteID;
            string disputedCallsType = string.Empty;

            if (FilterDisputedCallsBySite.SelectedItem.Index > -1)
            {
                siteID = Convert.ToInt32(FilterDisputedCallsBySite.SelectedItem.Value);

                FilterDisputedCallsByType.Disabled = false;

                MarkAsAcceptedBtn.Disabled = false;
                MarkAsRejectedBtn.Disabled = false;

                if (FilterDisputedCallsByType.SelectedItem.Index > -1)
                {
                    disputedCallsType = Convert.ToString(FilterDisputedCallsByType.SelectedItem.Value);

                    ManageDisputedCallsGrid.GetStore().RemoveAll();
                    ManageDisputedCallsGrid.GetStore().DataSource = GetDisputedPhoneCalls(siteID, disputedCallsType);
                    ManageDisputedCallsGrid.DataBind();
                }
            }
            else
            {
                FilterDisputedCallsByType.Disabled = true;
            }
        }


        protected void FilterDisputedCallsByType_Selecting(object sender, DirectEventArgs e)
        {
            int siteID;
            string selectedFilterValue = string.Empty;

            if (FilterDisputedCallsBySite.SelectedItem.Index > -1)
            {
                if (FilterDisputedCallsByType.SelectedItem.Index > -1)
                {
                    MarkAsAcceptedBtn.Disabled = false;
                    MarkAsRejectedBtn.Disabled = false;

                    siteID = Convert.ToInt32(FilterDisputedCallsBySite.SelectedItem.Value);
                    selectedFilterValue = Convert.ToString(FilterDisputedCallsByType.SelectedItem.Value);

                    ManageDisputedCallsGrid.GetStore().RemoveAll();
                    ManageDisputedCallsGrid.GetStore().DataSource = GetDisputedPhoneCalls(siteID, selectedFilterValue);
                    ManageDisputedCallsGrid.DataBind();
                }
                else
                {
                    MarkAsAcceptedBtn.Disabled = true;
                    MarkAsRejectedBtn.Disabled = true;
                }
            }
            else
            {
                FilterDisputedCallsByType.Disabled = true;
            }
        }


        protected void DisputedCallsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            XmlNode xml = e.Xml;

            this.Response.Clear();
            this.Response.ContentType = "application/vnd.ms-excel";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
            XslCompiledTransform xtExcel = new XslCompiledTransform();
            xtExcel.Load(Server.MapPath("~/Resources/Excel.xsl"));
            xtExcel.Transform(xml, null, Response.OutputStream);
        }


        protected void AcceptDispute(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = this.ManageDisputedCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (var phoneCall in phoneCalls)
            {
                phoneCall.AcDisputeStatus = "Accepted";
                phoneCall.AcDisputeResolvedOn = DateTime.Now;
                phoneCall.UiCallType = "Business";
                phoneCall.UiMarkedOn = DateTime.Now;
                phoneCall.UiUpdatedByUser = sipAccount;

                Global.DATABASE.PhoneCalls.Update(phoneCall, phoneCall.PhoneCallsTableName);
            }

            ManageDisputedCallsGrid.GetSelectionModel().DeselectAll();

            //Rebind the data
            if (FilterDisputedCallsBySite.SelectedItem.Index > -1 && FilterDisputedCallsByType.SelectedItem.Index > -1)
            {
                var siteID = Convert.ToInt32(FilterDisputedCallsBySite.SelectedItem.Value);
                var selectedFilterValue = Convert.ToString(FilterDisputedCallsByType.SelectedItem.Value);

                ManageDisputedCallsGrid.GetStore().RemoveAll();
                ManageDisputedCallsGrid.GetStore().DataSource = GetDisputedPhoneCalls(siteID, selectedFilterValue);
                ManageDisputedCallsGrid.DataBind();
            }
        }


        protected void RejectDispute(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = this.ManageDisputedCallsGrid.GetSelectionModel() as RowSelectionModel;

            string json = e.ExtraParams["Values"];
            List<PhoneCall> phoneCalls = new List<PhoneCall>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            phoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (var phoneCall in phoneCalls)
            {
                phoneCall.AcDisputeStatus = "Rejected";
                phoneCall.AcDisputeResolvedOn = DateTime.Now;
                phoneCall.UiCallType = "Personal";
                phoneCall.UiMarkedOn = DateTime.Now;
                phoneCall.UiUpdatedByUser = sipAccount;

                Global.DATABASE.PhoneCalls.Update(phoneCall, phoneCall.PhoneCallsTableName);
            }

            ManageDisputedCallsGrid.GetSelectionModel().DeselectAll();

            //Rebind the data
            if (FilterDisputedCallsBySite.SelectedItem.Index > -1 && FilterDisputedCallsByType.SelectedItem.Index > -1)
            {
                var siteID = Convert.ToInt32(FilterDisputedCallsBySite.SelectedItem.Value);
                var selectedFilterValue = Convert.ToString(FilterDisputedCallsByType.SelectedItem.Value);

                ManageDisputedCallsGrid.GetStore().RemoveAll();
                ManageDisputedCallsGrid.GetStore().DataSource = GetDisputedPhoneCalls(siteID, selectedFilterValue);
                ManageDisputedCallsGrid.DataBind();
            }
        }

    }

}