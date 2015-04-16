using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;

using CCC.UTILS.Libs;
using LyncBillingUI.Helpers;
using LyncBillingUI.Helpers.Account;
using LyncBillingBase.DataModels;
using System.Web.Script.Serialization;


namespace LyncBillingUI.Pages.SiteAccounting
{
    public partial class BillingCycleNotifications : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private static List<Site> userSites = new List<Site>();

        // This actually takes a copy of the current session for some uses on the frontend.
        public UserSession CurrentSession { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
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

                if (CurrentSession.ActiveRoleName != Functions.SiteAccountantRoleName)
                {
                    string url = String.Format(@"{0}/Authorize?access={1}", Global.APPLICATION_URL, Functions.SiteAccountantRoleName);
                    Response.Redirect(url);
                }
            }

            sipAccount = CurrentSession.GetEffectiveSipAccount();

            //Get the User Authorized List of Sites
            GetUserSitesData();
        }


        //
        // Get the sites data for this user
        private void GetUserSitesData()
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                userSites = Global.DATABASE.SystemRoles.GetSitesByRoles(CurrentSession.SystemRoles, Functions.SiteAccountantRoleName);
            }
        }


        private List<CallsSummaryForUser> UsersReport(string siteName, DateTime startingDate, DateTime endingDate)
        {
            var users = Global.DATABASE.Users
                .GetAll()
                .Where(user => user.NotifyUser == "Y" && user.SiteName == siteName)
                .ToList();

            var report = Global.DATABASE.UsersCallsSummaries
                .GetBySite(siteName, startingDate, endingDate, CCC.ORM.Globals.CallsSummaryForUser.GroupBy.UserAndInvoiceFlag)
                .Where(
                    user =>
                        users.Exists(item => item.SipAccount.ToLower() == user.UserSipAccount.ToLower())
                        && user.IsInvoiced == "NO"
                        && (
                            user.BusinessCallsCost > Convert.ToDecimal(0) ||
                            user.PersonalCallsCost > Convert.ToDecimal(0) ||
                            user.UnallocatedCallsCost > Convert.ToDecimal(0)
                        )
                ).ToList<CallsSummaryForUser>();

            return report;
        }


        protected void FilterUsersBySiteStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                FilterUsersBySite.GetStore().DataSource = userSites;
                FilterUsersBySite.GetStore().DataBind();

                if (userSites.Count == 1)
                {
                    FilterUsersBySite.SetValueAndFireSelect(userSites.First().Name);
                }
            }
        }


        protected void GetUsersForForSite(object sender, DirectEventArgs e)
        {
            int siteId = Convert.ToInt32(FilterUsersBySite.SelectedItem.Value);
            string siteName = Global.DATABASE.Sites.GetNameById(siteId);

            BillingCycleNotificationGrid.GetStore().DataSource = UsersReport(siteName, DateTime.Now.AddYears(-1), DateTime.Now);
            BillingCycleNotificationGrid.GetStore().DataBind();
        }


        protected void BillingCycleNotificationStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            string Body = string.Empty;
            string subject = string.Empty;
            string sipAccount = string.Empty;

            var report = (new JavaScriptSerializer()).Deserialize<List<CallsSummaryForUser>>(e.Json);

            MailTemplate mailTemplate = Global.DATABASE.MailTemplates.GetById(3);

            subject = mailTemplate.Subject;
            Body = mailTemplate.TemplateBody;

            foreach (CallsSummaryForUser userSummary in report)
            {
                //sipAccount = userSummary.UserSipAccount;
                sipAccount = "aalhour@ccc.gr";

                string RealBody = String.Format(Body, userSummary.UserName);
                Mailer mailer = new Mailer(sipAccount, subject, RealBody);
            }
        }//end-function

    }

}