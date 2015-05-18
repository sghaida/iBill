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
    public partial class NgnRates : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private static List<Site> usersSites;
        private List<Gateway> gateways = new List<Gateway>();
        private List<Gateway> filteredGateways = new List<Gateway>();

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


        private List<Gateway> GetGateways(int siteID)
        {
            gateways = Global.DATABASE.Gateways.GetAll().ToList();

            //Get Related Gateways for that specific site
            List<GatewayInfo> gatewaysDetails = Global.DATABASE.GatewaysInfo.GetAll().Where(item => item.SiteId == siteID).ToList();

            foreach (int id in gatewaysDetails.Select(item => item.GatewayId))
            {
                filteredGateways.Add(gateways.First(item => item.Id == id));
            }

            return filteredGateways;
        }


        protected void FilterGatewaysBySiteStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                FilterGatewaysBySite.GetStore().DataSource = usersSites;
                FilterGatewaysBySite.GetStore().DataBind();
            }
        }


        protected void FilterRatesByGateway_Selected(object sender, DirectEventArgs e)
        {
            int gatewayID;

            if (FilterRatesByGateway.SelectedItem.Index > -1)
            {
                gatewayID = Convert.ToInt32(FilterRatesByGateway.SelectedItem.Value);

                //Clear Store
                ManageRatesGrid.GetStore().RemoveAll();

                var gatewayNGNRates = Global.DATABASE.RatesForNgn.GetByGatewayId(gatewayID)
                    .Select(item => new { 
                        Id = item.Id,
                        Rate = item.Rate,
                        DialingCodeId = item.DialingCodeId,
                        DialingCode = item.NumberingPlanForNgn.DialingCode,
                        Iso3CountryCode = item.NumberingPlanForNgn.Iso3CountryCode,
                        CountryName = item.NumberingPlanForNgn.Country.Name,
                        TypeOfService = item.NumberingPlanForNgn.TypeOfService.Name,
                        Description = item.NumberingPlanForNgn.Description
                    })
                    .ToList();

                ManageRatesGrid.GetStore().DataSource = gatewayNGNRates;
                ManageRatesGrid.GetStore().DataBind();

            }//end-if

        }


        protected void UpdateEdited_DirectEvent(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];
            bool status = false;

            int gatewayId = Convert.ToInt32(FilterRatesByGateway.SelectedItem.Value);
            ChangeRecords<RateForNgn> toBeUpdated = new StoreDataHandler(e.ExtraParams["Values"]).BatchObjectData<RateForNgn>();

            if (toBeUpdated.Updated.Count > 0)
            {
                foreach (RateForNgn dialingCodeRate in toBeUpdated.Updated)
                {
                    status = Global.DATABASE.RatesForNgn.Update(dialingCodeRate, gatewayId);

                    if (status)
                    {
                        ManageRatesGrid.GetStore().GetById(dialingCodeRate.Id).Commit();
                    }
                }
            }
        }


        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ManageRatesGrid.GetStore().RejectChanges();
        }


        protected void GetGatewaysForSite(object sender, DirectEventArgs e)
        {
            FilterRatesByGateway.Clear();
            FilterRatesByGateway.ReadOnly = false;

            if (FilterGatewaysBySite.SelectedItem != null && !string.IsNullOrEmpty(FilterGatewaysBySite.SelectedItem.Value))
            {
                List<Gateway> gateways = GetGateways(Convert.ToInt32(FilterGatewaysBySite.SelectedItem.Value));

                FilterRatesByGateway.Disabled = false;
                FilterRatesByGateway.GetStore().DataSource = gateways;
                FilterRatesByGateway.GetStore().DataBind();

                ManageRatesGrid.GetStore().RemoveAll();

                if (gateways.Count == 1)
                {
                    FilterRatesByGateway.SetValueAndFireSelect(gateways.First().Id);
                    FilterRatesByGateway.ReadOnly = true;
                }
                else
                {
                    FilterRatesByGateway.ReadOnly = false;
                }
            }
        }

    }

}