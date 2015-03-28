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
using LyncBillingUI.Account;

namespace LyncBillingUI.Pages.User
{
    public partial class PhoneCallsHistory : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;

        private static string normalUserRoleName { get; set; }
        private static string userDelegeeRoleName { get; set; }

        private List<PhoneCalls> AutoMarkedPhoneCalls = new List<PhoneCalls>();

        // This actually takes a copy of the current CurrentSession for some uses on the frontend.
        public UserSession CurrentSession { get; set; }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            //
            // Set the role names of User and Delegee
            SetRolesNames();

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
                if (CurrentSession.ActiveRoleName != normalUserRoleName && CurrentSession.ActiveRoleName != userDelegeeRoleName)
                {
                    string url = String.Format(@"{0}/Authenticate?access={1}", Global.APPLICATION_URL, CurrentSession.ActiveRoleName);
                    Response.Redirect(url);
                }
            }

            sipAccount = CurrentSession.GetEffectiveSipAccount();
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