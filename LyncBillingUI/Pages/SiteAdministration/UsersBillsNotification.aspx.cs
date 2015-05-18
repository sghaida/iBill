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
using System.Globalization;


namespace LyncBillingUI.Pages.SiteAdministration
{
    public partial class UsersBillsNotification : System.Web.UI.Page
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


        private List<CallsSummaryForUser> GetUsersBills(int siteId, DateTime startingDate, DateTime endingDate)
        {
            string siteName = Global.DATABASE.Sites.GetNameById(siteId);
            List<CallsSummaryForUser> tmp = new List<CallsSummaryForUser>();
            tmp.AddRange(Global.DATABASE.UsersCallsSummaries.GetBySite(siteName, startingDate, endingDate).AsEnumerable<CallsSummaryForUser>());

            var usersWithNotifications = Global.DATABASE.Users.GetBySiteId(siteId).Where(item => item.NotifyUser == "Y").ToList();

            //Construct Users Bills List
            var UserBills = (from data in tmp.AsEnumerable()
                group data by new { data.UserSipAccount, data.UserId, data.UserName, MonthDate = data.Date } into res
                select new CallsSummaryForUser {
                    UserId = res.Key.UserId,
                    UserName = res.Key.UserName,
                    UserSipAccount = res.Key.UserSipAccount,
                    Date = endingDate,
                    PersonalCallsCost = res.Sum(x => x.PersonalCallsCost),
                    PersonalCallsDuration = res.Sum(x => x.PersonalCallsDuration),
                    PersonalCallsCount = res.Sum(x => x.PersonalCallsCount),
                }).Where(userBill =>
                    userBill.PersonalCallsCount > 0 &&
                    usersWithNotifications.Find(user => user.SipAccount.ToLower() == userBill.UserSipAccount.ToLower()) != null)
                .ToList();


            return UserBills;
        }


        protected void FilterUsersBySiteStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                FilterUsersBySite.GetStore().DataSource = usersSites;
                FilterUsersBySite.GetStore().DataBind();
            }
        }


        protected void GetUsersBillsForSite(object sender, DirectEventArgs e)
        {
            if (BillDateField.SelectedValue != null && FilterUsersBySite.SelectedItem != null)
            {
                int siteId = Convert.ToInt32(FilterUsersBySite.SelectedItem.Value);

                DateTime beginningOfTheMonth = new DateTime(BillDateField.SelectedDate.Year, BillDateField.SelectedDate.Month, 1);
                DateTime endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);

                UsersBillsGrid.GetStore().DataSource = GetUsersBills(siteId, beginningOfTheMonth, endOfTheMonth);
                UsersBillsGrid.GetStore().DataBind();
            }
        }


        protected void FilterUsersBySite_Selected(object sender, DirectEventArgs e)
        {
            if (FilterUsersBySite.SelectedItem.Index != -1 && !string.IsNullOrEmpty(FilterUsersBySite.SelectedItem.Value))
            {
                BillDateField.Disabled = false;
                if (BillDateField.SelectedValue != null)
                {
                    int siteId = Convert.ToInt32(FilterUsersBySite.SelectedItem.Value);

                    DateTime beginningOfTheMonth = new DateTime(BillDateField.SelectedDate.Year, BillDateField.SelectedDate.Month, 1);
                    DateTime endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);

                    UsersBillsGrid.GetStore().DataSource = GetUsersBills(siteId, beginningOfTheMonth, endOfTheMonth);
                    UsersBillsGrid.GetStore().DataBind();
                }
            }
            else
            {
                BillDateField.Disabled = true;
                //BillDateField.Clear();
                //UsersBillsGrid.ClearContent();
            }
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
                            userSummary.Date.ToString("MMM", CultureInfo.InvariantCulture) + " " + userSummary.Date.Year,
                            userSummary.PersonalCallsCount,
                            HelperFunctions.ConvertSecondsToReadable(userSummary.PersonalCallsDuration),
                            userSummary.PersonalCallsCost);

                Mailer mailer = new Mailer(sipAccount, subject, RealBody);
            }

        }//end-function
    }

}