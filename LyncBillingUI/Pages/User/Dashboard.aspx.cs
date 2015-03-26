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

namespace LyncBillingUI.Pages.User
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        
        private static string normalUserRoleName { get; set; }
        private static string userDelegeeRoleName { get; set; }

        //public variables made available for the view
        public int unmarkedCallsCount = 0;
        public string DisplayName = string.Empty;

        public List<PhoneCall> phoneCalls;
        public Dictionary<string, PhoneBookContact> phoneBookEntries;
        public MailReport userMailStatistics;
        public List<CallsSummaryForDestinationNumbers> TopDestinationNumbersList;
        public List<CallsSummaryForDestinationCountries> TopDestinationCountriesList;

        public List<string> columns = new List<string>();
        public Dictionary<string, object> wherePart = new Dictionary<string, object>();

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
                string RedirectTo = String.Format(@"{0}/User/Dashboard", Global.APPLICATION_URL);
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

            //initialize the local copy of the current user's PrimarySipAccount
            sipAccount = CurrentSession.GetEffectiveSipAccount();

            //Get the display name for this user
            DisplayName = CurrentSession.GetEffectiveDisplayName();

            //Initialize the Address Book data.
            phoneBookEntries = Global.DATABASE.PhoneBooks.GetAddressBook(sipAccount);

            //Get this user's mail statistics
            userMailStatistics = Global.DATABASE.MailReports.GetTotalByUser(sipAccount, DateTime.Now.AddMonths(-1));

            //Try to auto-mark phonecalls using the user's addressbook
            //Return the number of still-unmarked phone calls
            unmarkedCallsCount = TryAutoMarkPhoneCalls();
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


        // 
        // Phone Calls Auto-Marking using Addressbook
        // Return #no of still unmarked phone calls
        private int TryAutoMarkPhoneCalls()
        {
            List<PhoneCall> userSessionPhoneCalls;
            Dictionary<string, PhoneBookContact> userSessionAddressbook;
            PhoneBookContact addressBookEntry;
            int numberOfRemainingUnmarked = 0;

            //Get the unmarked calls from the user session phonecalls container
            CurrentSession.FetchSessionPhonecallsAndAddressbookData(out userSessionPhoneCalls, out userSessionAddressbook);

            var unmarkedCalls = userSessionPhoneCalls.Where(phoneCall => string.IsNullOrEmpty(phoneCall.UiCallType)).ToList();
            numberOfRemainingUnmarked = unmarkedCalls.Count;

            //If the user has no addressbook contacts, skip the auto marking process
            if (numberOfRemainingUnmarked > 0 && userSessionAddressbook.Keys.Count > 0)
            {
                foreach (var phoneCall in unmarkedCalls)
                {
                    if (userSessionAddressbook.Keys.Contains(phoneCall.DestinationNumberUri))
                    {
                        addressBookEntry = (PhoneBookContact)userSessionAddressbook[phoneCall.DestinationNumberUri];

                        //Remove the object that represents the unmarked phone call
                        userSessionPhoneCalls.Remove(phoneCall);

                        if (!string.IsNullOrEmpty(addressBookEntry.Type))
                        {
                            phoneCall.UiCallType = addressBookEntry.Type;
                            phoneCall.UiUpdatedByUser = sipAccount;
                            phoneCall.UiMarkedOn = DateTime.Now;

                            //Update the phonecall record in the database
                            Global.DATABASE.PhoneCalls.Update(phoneCall, phoneCall.PhoneCallsTableName);

                            //IMPORTANT: Decrese number of unmarked calls
                            --numberOfRemainingUnmarked;
                        }

                        //Add the object of the phone call which was (most probably) marked
                        userSessionPhoneCalls.Add(phoneCall);
                    }
                }
            }

            CurrentSession.AssignSessionPhonecallsAndAddressbookData(
                userSessionAddressBook: userSessionAddressbook,
                userSessionPhoneCalls: userSessionPhoneCalls.OrderByDescending(phoneCall => phoneCall.SessionIdTime).ToList());

            return numberOfRemainingUnmarked;
        }


        // 
        // Automark phonecalls from addressbook
        // This returns the number of phonecalls which weren't marked
        private int GetNumberOfUnmarkedCalls()
        {
            List<PhoneCall> userSessionPhoneCalls;
            Dictionary<string, PhoneBookContact> userSessionAddressbook;

            //Get the unmarked calls from the user session phonecalls container
            CurrentSession.FetchSessionPhonecallsAndAddressbookData(out userSessionPhoneCalls, out userSessionAddressbook);

            return userSessionPhoneCalls.Where(phoneCall => string.IsNullOrEmpty(phoneCall.UiCallType)).ToList().Count;
        }


        private void FilterDestinationNumbersNames(ref List<CallsSummaryForDestinationNumbers> DestinationNumbersList)
        {
            foreach (var destination in DestinationNumbersList)
            {
                //if (GetUserNameBySip(destination.PhoneNumber) != string.Empty)
                //{
                //    destination.UserName = GetUserNameBySip(destination.PhoneNumber);
                //    continue;
                //}

                if (phoneBookEntries.ContainsKey(destination.PhoneNumber))
                {
                    string temporaryName = phoneBookEntries[destination.PhoneNumber].Name;
                    destination.DestinationContactName = (!string.IsNullOrEmpty(temporaryName)) ? temporaryName : "N/A";
                }
                else
                {
                    destination.DestinationContactName = "N/A";
                }
            }
        }


        //private string GetUserNameBySip(string sipAccount)
        //{
        //    AdLib adRoutines = new AdLib();
        //    ADUserInfo userInfo = adRoutines.GetUserAttributes(sipAccount);

        //    if (userInfo != null && userInfo.DisplayName != null)
        //        return userInfo.DisplayName;
        //    else
        //        return string.Empty;
        //}


        protected void CallsCostsChartStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                DateTime fromDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                DateTime toDate = DateTime.Now;

                var summaries = Global.DATABASE.UsersCallsSummaries.GetBySipAccount(sipAccount, fromDate, toDate);

                CallsCostsChartStore.DataSource = summaries;
                CallsCostsChartStore.DataBind();
            }
        }


        protected void TopDestinationNumbersStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                TopDestinationNumbersList = Global.DATABASE.TopDestinationNumbers.GetBySipAccount(sipAccount, 5);
                FilterDestinationNumbersNames(ref TopDestinationNumbersList);

                TopDestinationNumbersStore.DataSource = TopDestinationNumbersList;
                TopDestinationNumbersStore.DataBind();
            }
        }


        protected void TopDestinationCountriesStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                TopDestinationCountriesList = Global.DATABASE.TopDestinationCountries.GetByUser(sipAccount, 5);

                TopDestinationCountriesStore.DataSource = TopDestinationCountriesList;
                TopDestinationCountriesStore.DataBind();
            }
        }


        protected void CustomizeStats_YearStore_Load(object sender, EventArgs e)
        {
            //if (!Ext.Net.X.IsAjaxRequest)
            //{
            //    //Get years from the database
            //    List<SpecialDateTime> Years = UserCallsSummary.GetUserCallsSummaryYears(sipAccount, session.BundledAccountsList);

            //    //Add a custom year criteria
            //    SpecialDateTime CustomYear = SpecialDateTime.Get_OneYearAgoFromToday();

            //    Years.Reverse();        //i.e. 2015, 2014, 2013
            //    Years.Add(CustomYear);  //2015, 2014, 2013, "ONEYEARAGO..."
            //    Years.Reverse();        //"ONEYEARAGO...", 2013, 2014, 2015

            //    //Bind the data
            //    CustomizeStats_Years.GetStore().DataSource = Years;
            //    CustomizeStats_Years.GetStore().DataBind();
            //}
        }


        protected void CustomizeStats_Years_Select(object sender, DirectEventArgs e)
        {
            //if (CustomizeStats_Years.SelectedItem.Index > -1)
            //{
            //    int selectedValue = Convert.ToInt32(CustomizeStats_Years.SelectedItem.Value);

            //    if (selectedValue == Convert.ToInt32(Enums.GetValue(Enums.SpecialDateTime.OneYearAgoFromToday)))
            //    {
            //        CustomizeStats_Quarters.Disabled = true;
            //    }
            //    else
            //    {
            //        CustomizeStats_Quarters.Disabled = false;
            //    }
            //}
        }


        protected void CustomizeStats_QuartersStore_Load(object sender, EventArgs e)
        {
            //if (!Ext.Net.X.IsAjaxRequest)
            //{
            //    var allQuarters = SpecialDateTime.GetQuartersOfTheYear();

            //    CustomizeStats_Quarters.GetStore().DataSource = allQuarters;
            //    CustomizeStats_Quarters.GetStore().DataBind();
            //}
        }

        protected void SubmitCustomizeStatisticsBtn_Click(object sender, DirectEventArgs e)
        {
            //Submitted from the view
            //int filterYear, filterQuater;

            ////For DateTime handling
            //DateTime startingDate, endingDate;
            //string titleText = string.Empty;


            //if (CustomizeStats_Years.SelectedItem.Index > -1 && CustomizeStats_Quarters.SelectedItem.Index > -1)
            //{
            //    filterYear = Convert.ToInt32(CustomizeStats_Years.SelectedItem.Value);
            //    filterQuater = Convert.ToInt32(CustomizeStats_Quarters.SelectedItem.Value);


            //    //Construct the Date Range
            //    titleText = SpecialDateTime.ConstructDateRange(filterYear, filterQuater, out startingDate, out endingDate);


            //    //Re-bind TopDestinationCountries to match the filter dates criteria
            //    var topDestinationCountries_DATA = TopDestinationCountries.GetTopDestinationCountriesForUser(sipAccount, session.BundledAccountsList, 5, startingDate, endingDate);
                
            //    if (topDestinationCountries_DATA.Count > 0) 
            //        TopDestinationCountriesChart.GetStore().LoadData(topDestinationCountries_DATA);
            //    else
            //        TopDestinationCountriesChart.GetStore().RemoveAll();

            //    TopDestinationCountriesPanel.Title = String.Format("Most Called Countries - {0}", titleText);

            //    //Re-bind TopDestinationNumbers to match the filter dates criteria
            //    TopDestinationNumbersGrid.GetStore().LoadData(FilterDestinationNumbersNames(TopDestinationNumbers.GetTopDestinationNumbers(sipAccount, session.BundledAccountsList, 5, startingDate, endingDate)));
            //    TopDestinationNumbersGrid.Title = String.Format("Most Called Numbers - {0}", titleText);

            //    //Re-bind CallsCosts to match the filter dates criteria
            //    CallsCostsChart.GetStore().LoadData(UserCallsSummary.GetUsersCallsSummary(sipAccount, session.BundledAccountsList, startingDate, endingDate));
            //    CallsCostsChartPanel.Title = String.Format("Calls Cost Chart - {0}", titleText);

            //}//End-if

        }//End-Function

    }//End-class

}