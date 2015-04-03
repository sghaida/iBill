using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext;
using Ext.Net;

using CCC.ORM.Helpers;
using LyncBillingBase;
using LyncBillingBase.DataModels;
using LyncBillingBase.DataMappers;
using LyncBillingUI.Account;
using CCC.UTILS.Libs;

namespace LyncBillingUI.Pages.User
{
    public partial class Statistics : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private static string normalUserRoleName { get; set; }
        private static string userDelegeeRoleName { get; set; }

        // This actually takes a copy of the current session for some uses on the frontend.
        public UserSession CurrentSession { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            //
            // Set the roles names
            SetRolesNames();

            // 
            // If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string RedirectTo = String.Format(@"{0}/User/Statistics", Global.APPLICATION_URL);
                string Url = String.Format(@"{0}/Login?RedirectTo={1}", Global.APPLICATION_URL, RedirectTo);
                Response.Redirect(Url);
            }
            else
            {
                CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (CurrentSession.ActiveRoleName != normalUserRoleName && CurrentSession.ActiveRoleName != userDelegeeRoleName)
                {
                    string url = String.Format(@"{0}/Authorize?access={1}", Global.APPLICATION_URL, CurrentSession.ActiveRoleName);
                    Response.Redirect(url);
                }
            }

            //initialize the local copy of the current user's PrimarySipAccount
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

        private List<ChartReport> GetChartData()
        {
            DateTime fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
            DateTime toDate = DateTime.Now;

            return Global.DATABASE.ChartsReports.GetByUser(sipAccount, fromDate, toDate);
        }

        protected void PhoneCallsDuartionChartStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                PhoneCallsDuartionChartStore.DataSource = GetChartData();
                PhoneCallsDuartionChartStore.DataBind();
            }
        }

        protected void PhoneCallsCostChartStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                PhoneCallsCostChartStore.DataSource = GetChartData();
                PhoneCallsCostChartStore.DataBind();
            }
        }

        protected void DurationCostChartStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                DateTime fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                DateTime toDate = DateTime.Now;

                DurationCostChartStore.DataSource = Global.DATABASE.UsersCallsSummaries.GetBySipAccount(sipAccount, fromDate, toDate);
                DurationCostChartStore.DataBind();
            }
        }

        protected void CustomizeStats_YearStore_Load(object sender, EventArgs e)
        {
            //Get years from the database
            List<SpecialDateTime> Years = Global.DATABASE.UsersCallsSummaries.GetYearsBySipAccount(sipAccount);

            //Add a custom year criteria
            SpecialDateTime CustomYear = SpecialDateTime.Get_OneYearAgoFromToday();

            Years.Reverse();        //i.e. 2015, 2014, 2013
            Years.Add(CustomYear);  //2015, 2014, 2013, "ONEYEARAGO..."
            Years.Reverse();        //"ONEYEARAGO...", 2013, 2014, 2015

            //Bind the data
            CustomizeStats_Years.GetStore().DataSource = Years;
            CustomizeStats_Years.GetStore().DataBind();
        }

        protected void CustomizeStats_QuartersStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                CustomizeStats_Quarters.GetStore().DataSource = SpecialDateTime.GetQuartersOfTheYear();
                CustomizeStats_Quarters.GetStore().DataBind();
            }
        }

        protected void CustomizeStats_Years_Select(object sender, DirectEventArgs e)
        {
            var oneYearAgoFromToday = Convert.ToInt32(CCC.ORM.Globals.SpecialDateTime.OneYearAgoFromToday.Value());

            if (CustomizeStats_Years.SelectedItem.Index > -1)
            {
                int selectedValue = Convert.ToInt32(CustomizeStats_Years.SelectedItem.Value);

                if (selectedValue == oneYearAgoFromToday)
                {
                    CustomizeStats_Quarters.Disabled = true;
                }
                else
                {
                    CustomizeStats_Quarters.Disabled = false;
                }
            }
        }

        protected void SubmitCustomizeStatisticsBtn_Click(object sender, DirectEventArgs e)
        {
            //Submitted from the view
            int filterYear, filterQuater;

            //For DateTime handling
            DateTime startingDate, endingDate;
            string titleText = string.Empty;


            if (CustomizeStats_Years.SelectedItem.Index > -1 && CustomizeStats_Quarters.SelectedItem.Index > -1)
            {
                filterYear = Convert.ToInt32(CustomizeStats_Years.SelectedItem.Value);
                filterQuater = Convert.ToInt32(CustomizeStats_Quarters.SelectedItem.Value);

                //Construct the Date Range
                titleText = SpecialDateTime.ConstructDateRange(filterYear, filterQuater, out startingDate, out endingDate);

                //Re-bind DurationCostChart to match the filter dates criteria
                DurationCostChart.GetStore().LoadData(Global.DATABASE.UsersCallsSummaries.GetBySipAccount(sipAccount, startingDate, endingDate));
                DurationCostChartPanel.Title = String.Format("Business/Personal Calls - {0}", titleText);

                var chartData = Global.DATABASE.ChartsReports.GetByUser(sipAccount, startingDate, endingDate);

                if (chartData.Count > 0)
                {
                    //Re-bind PhoneCallsDuartionChart to match the filter dates criteria
                    PhoneCallsDuartionChart.GetStore().LoadData(chartData);
                    PhoneCallsDuartionChartPanel.Title = String.Format("Calls Duration - {0}", titleText);

                    //Re-bind PhoneCallsCostChart to match the filter dates criteria
                    PhoneCallsCostChart.GetStore().LoadData(chartData);
                    PhoneCallsCostChartPanel.Title = String.Format("Calls Costs - {0}", titleText);
                }
                else
                {
                    PhoneCallsDuartionChart.GetStore().RemoveAll();
                    PhoneCallsCostChart.GetStore().RemoveAll();
                }

            }//End-if

        }//End-Function

    }

}