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
    public partial class TelephonyRates : System.Web.UI.Page
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


        public List<Gateway> GetGateways(int siteID)
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
                    FilterRatesByGateway.SetValueAndFireSelect(gateways[0].Id);
                    FilterRatesByGateway.ReadOnly = true;
                }
                else
                {
                    FilterRatesByGateway.ReadOnly = false;
                }
            }
        }


        public string GetRatesTableName(int siteID)
        {
            if (gateways.Count < 1)
                gateways = GetGateways(siteID);

            string rateTable = string.Empty;

            if (FilterRatesByGateway.SelectedItem.Index == -1)
            {
                return rateTable;
            }
            else
            {
                int gatewayID = Convert.ToInt32(FilterRatesByGateway.SelectedItem.Value);
                rateTable = Global.DATABASE.GatewaysRates.GetByGatewayId(gatewayID).First(item => item.EndingDate == DateTime.MinValue).RatesTableName;
                return rateTable;
            }

        }


        protected void GetRates(object sender, DirectEventArgs e)
        {
            int gatewayId = 0;
            List<GatewayRate> gatewayRates = new List<GatewayRate>();

            if (FilterGatewaysBySite.SelectedItem.Index > -1 && FilterRatesByGateway.SelectedItem.Index > -1)
            {
                gatewayId = Convert.ToInt32(FilterRatesByGateway.SelectedItem.Value);

                //Clear Store
                ManageRatesGrid.GetStore().RemoveAll();

                ManageRatesGrid.GetStore().DataSource = Global.DATABASE.Rates.GetInternationalRatesByGatewayId(gatewayId) ?? (new List<RatesInternational>());
                ManageRatesGrid.GetStore().DataBind();
            }
        }

        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ManageRatesGrid.GetStore().RejectChanges();
        }


        protected void UpdateEdited_DirectEvent(object sender, DirectEventArgs e)
        {
            int gatewayId = 0;
            string json = e.ExtraParams["Values"];

            //ChangeRecords<Rate> toBeUpdated = new StoreDataHandler(e.ExtraParams["Values"]).BatchObjectData<Rate>();
            ChangeRecords<RatesInternational> toBeUpdated = new StoreDataHandler(e.ExtraParams["Values"]).BatchObjectData<RatesInternational>();

            if (toBeUpdated.Updated.Count > 0)
            {
                gatewayId = Convert.ToInt32(FilterRatesByGateway.SelectedItem.Value);

                foreach (RatesInternational countryRate in toBeUpdated.Updated)
                {
                    //List<DialingPrefixRate> dialingPrefixsRates = DialingPrefixsRates.GetRates(ratesTableName, countryRate.Iso3CountryCode);
                    List<RatesNational> dialingPrefixsRates = Global.DATABASE.Rates.GetNationalRatesForCountryByGatewayId(gatewayId, countryRate.Iso3CountryCode).ToList();

                    //The Rates Table doesnt have this country codes
                    if (dialingPrefixsRates.Where(item => item.RateId == 0).Count() == dialingPrefixsRates.Count())
                    {
                        foreach (RatesNational dialingPrefixRate in dialingPrefixsRates)
                        {
                            if (dialingPrefixRate.TypeOfService.ToLower() == "gsm")
                                dialingPrefixRate.Rate = countryRate.MobileLineRate;
                            else
                                dialingPrefixRate.Rate = countryRate.FixedLineRate;

                            int rateID = Global.DATABASE.Rates.Insert(dialingPrefixRate, gatewayId);
                        }

                        ManageRatesStore.Find("Iso3CountryCode", countryRate.Iso3CountryCode).Commit();
                    }
                    else if (dialingPrefixsRates.Where(item => item.RateId > 0).Count() == dialingPrefixsRates.Count())
                    {
                        foreach (RatesNational dialingPrefixRate in dialingPrefixsRates)
                        {
                            if (dialingPrefixRate.TypeOfService.ToLower() == "gsm" && dialingPrefixRate.Rate != countryRate.MobileLineRate)
                            {
                                dialingPrefixRate.Rate = countryRate.MobileLineRate;
                                Global.DATABASE.Rates.Update(dialingPrefixRate, gatewayId);
                            }
                            else if (dialingPrefixRate.TypeOfService.ToLower() != "gsm" && dialingPrefixRate.Rate != countryRate.FixedLineRate)
                            {
                                dialingPrefixRate.Rate = countryRate.FixedLineRate;
                                Global.DATABASE.Rates.Update(dialingPrefixRate, gatewayId);
                            }
                        }

                        ManageRatesStore.Find("Iso3CountryCode", countryRate.Iso3CountryCode).Commit();
                    }
                    else
                    {
                        // Some needs to be updated and some needs to be inserted
                        foreach (RatesNational dialingPrefixRate in dialingPrefixsRates)
                        {
                            if (dialingPrefixRate.RateId == 0)
                            {
                                if (dialingPrefixRate.TypeOfService.ToLower() == "gsm")
                                {
                                    dialingPrefixRate.Rate = countryRate.MobileLineRate;
                                }
                                else
                                {
                                    dialingPrefixRate.Rate = countryRate.FixedLineRate;
                                }

                                int rateID = Global.DATABASE.Rates.Insert(dialingPrefixRate, gatewayId);
                            }
                            else if (dialingPrefixRate.RateId != 0)
                            {
                                if (dialingPrefixRate.TypeOfService.ToLower() == "gsm" && dialingPrefixRate.Rate != countryRate.MobileLineRate)
                                {
                                    dialingPrefixRate.Rate = countryRate.MobileLineRate;
                                    Global.DATABASE.Rates.Update(dialingPrefixRate, gatewayId);
                                }
                                else if (dialingPrefixRate.TypeOfService.ToLower() != "gsm" && dialingPrefixRate.Rate != countryRate.FixedLineRate)
                                {
                                    dialingPrefixRate.Rate = countryRate.FixedLineRate;
                                    Global.DATABASE.Rates.Update(dialingPrefixRate, gatewayId);
                                }
                            }
                            else
                            {
                                // Catcher should be implemented 
                            }
                        }

                        ManageRatesStore.Find("Iso3CountryCode", countryRate.Iso3CountryCode).Commit();
                    }
                }
            }

            if (toBeUpdated.Deleted.Count > 0)
            {

            }
        }

    }

}