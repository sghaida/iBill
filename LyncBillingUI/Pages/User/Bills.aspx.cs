using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Ext;
using Ext.Net;
using Newtonsoft.Json;

using CCC.ORM.Helpers;
using LyncBillingBase;
using LyncBillingBase.DataModels;
using LyncBillingBase.DataMappers;
using LyncBillingUI;
using LyncBillingUI.Helpers;
using LyncBillingUI.Helpers.Account;

namespace LyncBillingUI.Pages.User
{
    public partial class Bills : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;

        // This actually takes a copy of the current CurrentSession for some uses on the frontend.
        public UserSession CurrentSession { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            // 
            // If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string RedirectTo = String.Format(@"{0}/User/Bills", Global.APPLICATION_URL);
                string Url = String.Format(@"{0}/Login?RedirectTo={1}", Global.APPLICATION_URL, RedirectTo);
                Response.Redirect(Url);
            }
            else
            {
                CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (CurrentSession.ActiveRoleName != Functions.NormalUserRoleName && CurrentSession.ActiveRoleName != Functions.UserDelegeeRoleName)
                {
                    string url = String.Format(@"{0}/Authorize?access={1}", Global.APPLICATION_URL, CurrentSession.ActiveRoleName);
                    Response.Redirect(url);
                }
            }

            sipAccount = CurrentSession.GetEffectiveSipAccount();
        }


        protected void BillsStore_Load(object sender, EventArgs e)
        {
            List<CallsSummaryForUser> UserSummariesList = new List<CallsSummaryForUser>();
            List<CallsSummaryForUser> BillsList = new List<CallsSummaryForUser>();

            DateTime fromDate = new DateTime(DateTime.Now.Year - 1, 1, 1);
            DateTime toDate = DateTime.Now;

            UserSummariesList = Global.DATABASE.UsersCallsSummaries.GetBySipAccount(sipAccount, fromDate, toDate);

            foreach (var summary in UserSummariesList)
            {
                BillsList.Add(summary);
            }

            BillsStore.DataSource = BillsList;
            BillsStore.DataBind();
        }

    }

}