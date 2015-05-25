using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext;
using Ext.Net;

using CCC.UTILS.Libs;
using CCC.ORM.Helpers;
using LyncBillingBase;
using LyncBillingBase.DataModels;
using LyncBillingBase.DataMappers;
using LyncBillingUI.Helpers;
using LyncBillingUI.Helpers.Account;

namespace LyncBillingUI.Pages.User
{
    public partial class Statistics : System.Web.UI.Page
    {
        /// <summary>
        /// Used in exporting customized reports to the line chart.
        /// Check the GetDurationChartReport method.
        /// </summary>
        internal class DurationChartDataRecord
        {
            public DateTime Date { get; set; }
            public long BusinessCallsDuration { get; set; }
            public long PersonalCallsDuration { get; set; }
            public long TotalCallsDuration { get; set; }
            public decimal BusinessCallsCost { get; set; }
            public decimal PersonalCallsCost { get; set; }
            public decimal TotalCallsCost { get; set; }
        }


        private string sipAccount = string.Empty;

        // This actually takes a copy of the current session for some uses on the frontend.
        public UserSession CurrentSession { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
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
                if (CurrentSession.ActiveRoleName != Functions.NormalUserRoleName && CurrentSession.ActiveRoleName != Functions.UserDelegeeRoleName)
                {
                    string url = String.Format(@"{0}/Authorize?access={1}", Global.APPLICATION_URL, CurrentSession.ActiveRoleName);
                    Response.Redirect(url);
                }
            }

            //initialize the local copy of the current user's PrimarySipAccount
            sipAccount = CurrentSession.GetEffectiveSipAccount();
        }


        private List<DurationChartDataRecord> GetDurationChartData(DateTime startDate, DateTime endDate)
        {
            // Get chart data from the database for this user
            var chartData = Global.DATABASE.UsersCallsSummaries.GetBySipAccount(sipAccount, startDate, endDate);

            var report = (from dataRecord in chartData
                          group dataRecord by new { dataRecord.Date }
                              into result
                              select new DurationChartDataRecord
                              {
                                  Date = result.Key.Date,
                                  BusinessCallsCost = result.Sum(x => x.BusinessCallsCost),
                                  PersonalCallsCost = result.Sum(x => x.PersonalCallsCost),
                                  BusinessCallsDuration = result.Sum(x => x.BusinessCallsDuration),
                                  PersonalCallsDuration = result.Sum(x => x.PersonalCallsDuration),
                                  TotalCallsCost = result.Sum(x => x.BusinessCallsCost) + result.Sum(x => x.PersonalCallsCost),
                                  TotalCallsDuration = result.Sum(x => x.BusinessCallsDuration) + result.Sum(x => x.PersonalCallsDuration)
                              }).ToList<DurationChartDataRecord>();

            return report;
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

                var chartData = GetDurationChartData(fromDate, toDate);

                // Bind the "report" to to the Chart's store.
                DurationCostChart.GetStore().DataSource = chartData;
                DurationCostChart.GetStore().DataBind();
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
                DurationCostChart.GetStore().LoadData(this.GetDurationChartData(startingDate, endingDate));
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