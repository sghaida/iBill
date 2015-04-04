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
    public partial class PhoneCallsHistory : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;

        private List<PhoneCalls> AutoMarkedPhoneCalls = new List<PhoneCalls>();

        // This actually takes a copy of the current CurrentSession for some uses on the frontend.
        public UserSession CurrentSession { get; set; }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            // 
            // If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string RedirectTo = String.Format(@"{0}/User/TelephonyRates", Global.APPLICATION_URL);
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

        
        protected void PhoneCallStore_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                var phoneCallsHistory = Global.DATABASE.PhoneCalls.GetChargeableCallsBySipAccount(sipAccount)
                    .Where(phoneCall => phoneCall.AcInvoiceDate != DateTime.MinValue && (!string.IsNullOrEmpty(phoneCall.AcIsInvoiced) && phoneCall.AcIsInvoiced == "YES"))
                    .ToList();

                PhoneCallsHistoryGrid.GetStore().DataSource = phoneCallsHistory;
                PhoneCallsHistoryGrid.GetStore().DataBind();
            }
        }


        [DirectMethod]
        protected void PhoneCallsHistoryFilter(object sender, DirectEventArgs e)
        {
            List<PhoneCall> phoneCallsHistory = new List<PhoneCall>();

            if (FilterTypeComboBox.SelectedItem.Index > -1)
            {
                int filterType = Convert.ToInt32(FilterTypeComboBox.SelectedItem.Value);

                phoneCallsHistory = Global.DATABASE.PhoneCalls.GetChargeableCallsBySipAccount(sipAccount)
                    .Where(phoneCall => phoneCall.AcInvoiceDate != DateTime.MinValue && (!string.IsNullOrEmpty(phoneCall.AcIsInvoiced) && phoneCall.AcIsInvoiced == "YES"))
                    .ToList();

                //Business filter
                if (filterType == 2)
                {
                    phoneCallsHistory = phoneCallsHistory.Where(phonecall => phonecall.UiCallType == "Business").ToList();
                }
                //Personal filter
                else if (filterType == 3)
                {
                    phoneCallsHistory = phoneCallsHistory.Where(phonecall => phonecall.UiCallType == "Personal").ToList();
                }
            }

            PhoneCallsHistoryGrid.GetStore().LoadData(phoneCallsHistory);
        }

    }

}