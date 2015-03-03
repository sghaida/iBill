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
    public partial class TelephonyRates : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private string normalUserRoleName { get; set; }
        private string userDelegeeRoleName { get; set; }

        private static List<Gateway> gateways = new List<Gateway>();
        private static List<Gateway> filteredGateways = new List<Gateway>();
        private static List<GatewayInfo> gatewaysInfo = new List<GatewayInfo>();

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

            GetGatewaysInfo();
        }


        //
        // Set the role names of User and Delegee
        private void SetRolesNames()
        {
            if (string.IsNullOrEmpty(normalUserRoleName))
            {
                var normalUserRole = Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.UserRoleID);
                normalUserRoleName = (normalUserRole != null ? normalUserRole.RoleName : string.Empty);
            }

            if (string.IsNullOrEmpty(userDelegeeRoleName))
            {
                var delegeeUserRole = Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.UserDelegeeRoleID);
                userDelegeeRoleName = (delegeeUserRole != null ? delegeeUserRole.RoleName : string.Empty);
            }
        }


        //
        // Makes sure the gateways info is always available
        private void GetGatewaysInfo()
        {
            var site = (CurrentSession.ActiveRoleName == normalUserRoleName) ? CurrentSession.User.Site : CurrentSession.DelegeeUserAccount.User.Site;

            if(!X.IsAjaxRequest)
            {
                gateways = new List<Gateway>();
                gatewaysInfo = new List<GatewayInfo>();
                filteredGateways = new List<Gateway>();

                if(site != null)
                {
                    gateways = Global.DATABASE.GatewaysInfo.GetGatewaysBySiteId(site.Id);
                    gatewaysInfo = Global.DATABASE.GatewaysInfo.GetAll().Where(item => item.SiteId == site.Id).ToList();

                    var activeGatewaysInfo = gatewaysInfo
                        .Where(item =>
                            item.GatewayRatesInfo != null &&
                            item.GatewayRatesInfo.EndingDate == DateTime.MinValue)
                        .ToList();

                    foreach (var gateway in activeGatewaysInfo)
                    {
                        var customGateway = new Gateway();
                        customGateway.Id = gateway.GatewayId;
                        customGateway.Name = gateway.GatewayRatesInfo.ProviderName;

                        filteredGateways.Add(customGateway);
                    }
                }
            }
        }


        protected void GatewaysFilterStore_Load(object sender, EventArgs e)
        {
            GatewaysFilter.GetStore().DataSource = filteredGateways;
            GatewaysFilter.GetStore().DataBind();

            if (gateways.Count == 1)
            {
                GatewaysFilter.SetValueAndFireSelect(filteredGateways[0].Id);
                GatewaysFilter.ReadOnly = true;
            }
        }


        protected void GatewaysFilter_Select(object sender, DirectEventArgs e)
        {
            List<GatewayRate> gatewayRates = new List<GatewayRate>();

            int selectedGatewayId = Convert.ToInt32(GatewaysFilter.SelectedItem.Value);

            //Clear Store
            ViewRatesGrid.GetStore().RemoveAll();

            //Fill Store
            ViewRatesGrid.GetStore().DataSource = Global.DATABASE.Rates.GetInternationalRatesByGatewayId(selectedGatewayId);
            ViewRatesGrid.GetStore().DataBind();

        }

    }

}