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
    public partial class UnallocatedCallsNotification : System.Web.UI.Page
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
        }


        private void GetUserSitesDate()
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                usersSites = Global.DATABASE.SystemRoles.GetSitesByRoles(CurrentSession.SystemRoles, Functions.SiteAdminRoleName);
            }
        }


        private List<CallsSummaryForUser> GetReport(int siteId, DateTime startingDate, DateTime endingDate)
        {
            string siteName = Global.DATABASE.Sites.GetNameById(siteId);
            var usersWithNotifications = Global.DATABASE.Users.GetBySiteId(siteId).Where(item => item.NotifyUser == "Y").ToList();

            var data = Global.DATABASE.UsersCallsSummaries.GetBySite(siteName, startingDate, endingDate, CCC.ORM.Globals.CallsSummaryForUser.GroupBy.UserAndInvoiceFlag)
                    .Where(summary =>
                            summary.UnallocatedCallsCount > 0 
                            && usersWithNotifications.Exists(user => user.SipAccount.ToLower() == summary.UserSipAccount.ToLower()))
                    .ToList();

            return data;
        }


        protected void FilterUsersBySiteStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                FilterUsersBySite.GetStore().DataSource = usersSites;
                FilterUsersBySite.GetStore().DataBind();
            }
        }


        protected void GetUnmarkedCallsForSite(object sender, DirectEventArgs e)
        {
            int siteId = Convert.ToInt32(FilterUsersBySite.SelectedItem.Value);

            UnmarkedCallsGrid.GetStore().DataSource = GetReport(siteId, DateTime.Now.AddYears(-1), DateTime.Now);
            UnmarkedCallsGrid.GetStore().DataBind();
        }


        protected void NotifyUsers(object sender, DirectEventArgs e)
        {
            string Body = string.Empty;
            string subject = string.Empty;

            string sipAccount = string.Empty;

            string json = e.ExtraParams["Values"];

            List<CallsSummaryForUser> usersSummary = JSON.Deserialize<List<CallsSummaryForUser>>(json);

            MailTemplate mailTemplate = Global.DATABASE.MailTemplates.GetById(1);

            subject = mailTemplate.Subject;
            Body = mailTemplate.TemplateBody;

            foreach (var userSummary in usersSummary)
            {
                sipAccount = userSummary.UserSipAccount;

                string RealBody =
                    string.Format(
                            Body,
                            userSummary.UserName,
                            userSummary.UnallocatedCallsCount,
                            userSummary.UnallocatedCallsCost,
                            HelperFunctions.ConvertSecondsToReadable(userSummary.UnallocatedCallsDuration));

                Mailer mailer = new Mailer(sipAccount, subject, RealBody);
            }

        }//end-function


    }

}