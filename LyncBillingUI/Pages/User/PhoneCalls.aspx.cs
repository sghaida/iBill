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
    public partial class PhoneCalls : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;

        // This actually takes a copy of the current session for some uses on the frontend.
        public UserSession CurrentSession { get; set; }

        private static List<PhoneCall> departmentPhoneCalls { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            //
            // If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string RedirectTo = String.Format(@"{0}/User/PhoneCalls", Global.APPLICATION_URL);
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


        private void DepartmentPhoneCallsGridManager(bool GetFreshData = true, bool BindData = true)
        {
            if(GetFreshData == true || departmentPhoneCalls == null)
            {
                //string SiteDepartment = session.NormalUserInfo.SiteName + "_" + session.NormalUserInfo.Departments;
                string SiteDepartment =
                    (CurrentSession.ActiveRoleName == Functions.UserDelegeeRoleName) ?
                    CurrentSession.DelegeeUserAccount.User.SiteName + "-" + CurrentSession.DelegeeUserAccount.User.DepartmentName :
                    CurrentSession.User.SiteName + "-" + CurrentSession.User.DepartmentName;

                departmentPhoneCalls = Global.DATABASE.PhoneCalls.GetChargeableCallsBySipAccount(SiteDepartment).ToList();
            }

            if(BindData == true)
            {
                DepartmentPhoneCallsGrid.GetStore().DataSource = departmentPhoneCalls;
                DepartmentPhoneCallsGrid.GetStore().DataBind();
            }
        }


        //
        // Data Binding/Re-binding function
        private void RebindDataToStore(List<PhoneCall> phoneCallsData)
        {
            MyPhoneCallsGrid.GetSelectionModel().DeselectAll();

            MyPhoneCallsGrid.GetStore().RemoveAll();

            //
            // Trigger the PhoneCallsTypeFilter OnSelect Event
            PhoneCallsTypeFilter(null, null);
        }


        //
        // STORE LOADERS
        protected void MyPhoneCallsStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                //Get use session and user phonecalls list.
                CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

                //Get user session phonecalls; handle normal user mode and delegee mode
                List<PhoneCall> userSessionPhoneCalls = CurrentSession.GetUserSessionPhoneCalls().Where(phoneCall => string.IsNullOrEmpty(phoneCall.UiCallType) == true).ToList();

                MyPhoneCallsGrid.GetStore().DataSource = userSessionPhoneCalls;
                MyPhoneCallsGrid.GetStore().DataBind();
            }
        }

        protected void DepartmentPhoneCallsStore_Load(object sender, EventArgs e)
        {
            DepartmentPhoneCallsGridManager(GetFreshData: false == X.IsAjaxRequest);
        }

        [DirectMethod]
        protected void PhoneCallsTabsPanel_TabChange(object sender, DirectEventArgs e)
        {
            MyPhoneCallsGrid.GetStore().Reload();
            DepartmentPhoneCallsGrid.GetStore().Reload();
        }

        //
        // User Controls
        [DirectMethod]
        protected void PhoneCallsTypeFilter(object sender, DirectEventArgs e)
        {
            //User session phonecalls container
            List<PhoneCall> userSessionPhoneCalls;

            //The phone calls type filter
            string phoneCallsTypeFilter = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(FilterTypeComboBox.SelectedItem.Value));


            if (phoneCallsTypeFilter != "Unallocated")
            {
                //Get user session phonecalls; handle normal user mode and delegee mode
                userSessionPhoneCalls = CurrentSession.GetUserSessionPhoneCalls().Where(phoneCall => phoneCall.UiCallType == phoneCallsTypeFilter).ToList();

                //Bind them to the Grid
                MyPhoneCallsGrid.GetStore().DataSource = userSessionPhoneCalls;
                MyPhoneCallsGrid.GetStore().DataBind();

                //Enable/Disable the context menu items
                PhoneBookNameEditorTextbox.ReadOnly = true;

                if (phoneCallsTypeFilter == "Personal")
                {
                    AllocatePhonecallsAsPersonal.Disabled = true;
                    AllocateDestinationsAsAlwaysPersonal.Disabled = true;

                    AllocatePhonecallsAsDispute.Disabled = false;
                    AllocatePhonecallsAsBusiness.Disabled = false;
                    AllocateDestinationsAsAlwaysBusiness.Disabled = false;
                }

                if (phoneCallsTypeFilter == "Business")
                {
                    AllocatePhonecallsAsBusiness.Disabled = true;
                    AllocateDestinationsAsAlwaysBusiness.Disabled = true;

                    AllocatePhonecallsAsDispute.Disabled = false;
                    AllocatePhonecallsAsPersonal.Disabled = false;
                    AllocateDestinationsAsAlwaysPersonal.Disabled = false;
                }

                if (phoneCallsTypeFilter == "Disputed")
                {
                    AllocatePhonecallsAsDispute.Disabled = true;

                    AllocatePhonecallsAsBusiness.Disabled = false;
                    AllocatePhonecallsAsPersonal.Disabled = false;
                    AllocateDestinationsAsAlwaysBusiness.Disabled = false;
                    AllocateDestinationsAsAlwaysPersonal.Disabled = false;
                }
            }
            else
            {
                //Get user session phonecalls; handle normal user mode and delegee mode
                userSessionPhoneCalls = CurrentSession.GetUserSessionPhoneCalls().Where(phoneCall => string.IsNullOrEmpty(phoneCall.UiCallType)).ToList();

                //Bind them to the Grid
                MyPhoneCallsGrid.GetStore().DataSource = userSessionPhoneCalls;
                MyPhoneCallsGrid.GetStore().DataBind();

                //Enable/Disable the context menu items
                PhoneBookNameEditorTextbox.ReadOnly = false;

                AllocatePhonecallsAsDispute.Disabled = false;
                AllocatePhonecallsAsBusiness.Disabled = false;
                AllocatePhonecallsAsPersonal.Disabled = false;

                AllocateDestinationsAsAlwaysPersonal.Disabled = false;
                AllocateDestinationsAsAlwaysBusiness.Disabled = false;
            }
        }

        [DirectMethod]
        protected void ShowUserHelpPanel(object sender, DirectEventArgs e)
        {
            this.UserHelpPanel.Show();
        }

        [DirectMethod]
        protected void PhoneCallsGridSelectDirectEvents(object sender, DirectEventArgs e)
        {
            string json = string.Empty;
            List<PhoneCall> submittedPhoneCalls;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();


            json = e.ExtraParams["Values"];
            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            var result = submittedPhoneCalls.Where(item => !string.IsNullOrEmpty(item.UiAssignedByUser)).ToList();

            if (submittedPhoneCalls.Count > 0 && submittedPhoneCalls.Count == result.Count)
                MoveToDepartmnet.Disabled = false;
            else
                MoveToDepartmnet.Disabled = true;
        }

        [DirectMethod]
        protected void AutomarkCalls_Clicked(object sender, DirectEventArgs e)
        {
            List<PhoneCall> userSessionPhoneCalls;
            Dictionary<string, PhoneBookContact> userSessionAddressbook;
            PhoneBookContact addressBookEntry;
            int numberOfRemainingUnmarked;

            //Get the unmarked calls from the user session phonecalls container
            CurrentSession.FetchSessionPhonecallsAndAddressbookData(out userSessionPhoneCalls, out userSessionAddressbook);

            numberOfRemainingUnmarked = userSessionPhoneCalls.Where(phoneCall => string.IsNullOrEmpty(phoneCall.UiCallType)).ToList().Count;

            //If the user has no addressbook contacts, skip the auto marking process
            if (userSessionAddressbook.Keys.Count > 0)
            {
                foreach (var phoneCall in userSessionPhoneCalls.Where(phoneCall => string.IsNullOrEmpty(phoneCall.UiCallType)))
                {
                    if (userSessionAddressbook.Keys.Contains(phoneCall.DestinationNumberUri))
                    {
                        addressBookEntry = (PhoneBookContact)userSessionAddressbook[phoneCall.DestinationNumberUri];

                        if (!string.IsNullOrEmpty(addressBookEntry.Type))
                        {
                            phoneCall.UiCallType = addressBookEntry.Type;
                            phoneCall.UiUpdatedByUser = sipAccount;
                            phoneCall.UiMarkedOn = DateTime.Now;

                            Global.DATABASE.PhoneCalls.Update(phoneCall, phoneCall.PhoneCallsTableName);

                            ModelProxy model = MyPhoneCallsGrid.GetStore().Find("SessionIdTime", phoneCall.SessionIdTime.ToString());
                            model.Set(phoneCall);
                            model.Commit();
                        }
                    }
                }
            }

            MyPhoneCallsGrid.GetSelectionModel().DeselectAll();
            MyPhoneCallsGrid.GetStore().LoadPage(1);

            CurrentSession.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls, userSessionAddressbook);
        }


        //
        // Phone Calls Allocation
        [DirectMethod]
        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            MyPhoneCallsGrid.GetStore().RejectChanges();
        }

        [DirectMethod]
        protected void AssignPersonal(object sender, DirectEventArgs e)
        {
            PhoneCall sessionPhoneCallRecord;
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> userSessionPhoneCalls;

            string json = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //Get the session and sip account of the current user
            //CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = CurrentSession.GetEffectiveSipAccount();

            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = CurrentSession.GetUserSessionPhoneCalls();

            json = e.ExtraParams["Values"];
            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                var sessionIdTime = phoneCall.SessionIdTime;
                
                sessionPhoneCallRecord = userSessionPhoneCalls.Find(
                    item => item.SessionIdTime.Year == sessionIdTime.Year
                        && item.SessionIdTime.Month == sessionIdTime.Month
                        && item.SessionIdTime.Day == sessionIdTime.Day
                        && item.SessionIdTime.Hour == sessionIdTime.Hour
                        && item.SessionIdTime.Minute == sessionIdTime.Minute
                        && item.SessionIdTime.Second == sessionIdTime.Second);

                //if (sessionPhoneCallRecord == null)
                //    continue;

                sessionPhoneCallRecord.UiCallType = "Personal";
                sessionPhoneCallRecord.UiMarkedOn = DateTime.Now;
                sessionPhoneCallRecord.UiUpdatedByUser = sipAccount;

                Global.DATABASE.PhoneCalls.Update(sessionPhoneCallRecord, sessionPhoneCallRecord.PhoneCallsTableName);
            }

            PhoneCallsAllocationToolsMenu.Hide();

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            CurrentSession.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls: userSessionPhoneCalls, userSessionAddressBook: null);

            //Rebind data to the grid store
            RebindDataToStore(userSessionPhoneCalls);
        }

        [DirectMethod]
        protected void AssignBusiness(object sender, DirectEventArgs e)
        {
            PhoneCall sessionPhoneCallRecord;
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> userSessionPhoneCalls;

            string json = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //Get the session and sip account of the current user
            //CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = CurrentSession.GetEffectiveSipAccount();

            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = CurrentSession.GetUserSessionPhoneCalls();

            json = e.ExtraParams["Values"];
            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                var sessionIdTime = phoneCall.SessionIdTime;

                sessionPhoneCallRecord = userSessionPhoneCalls.Find(
                    item => item.SessionIdTime.Year == sessionIdTime.Year
                        && item.SessionIdTime.Month == sessionIdTime.Month
                        && item.SessionIdTime.Day == sessionIdTime.Day
                        && item.SessionIdTime.Hour == sessionIdTime.Hour
                        && item.SessionIdTime.Minute == sessionIdTime.Minute
                        && item.SessionIdTime.Second == sessionIdTime.Second);

                sessionPhoneCallRecord.UiCallType = "Business";
                sessionPhoneCallRecord.UiMarkedOn = DateTime.Now;
                sessionPhoneCallRecord.UiUpdatedByUser = sipAccount;

                Global.DATABASE.PhoneCalls.Update(sessionPhoneCallRecord, sessionPhoneCallRecord.PhoneCallsTableName);
            }

            PhoneCallsAllocationToolsMenu.Hide();

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            CurrentSession.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls: userSessionPhoneCalls, userSessionAddressBook: null);

            //Rebind data to the grid store
            RebindDataToStore(userSessionPhoneCalls);
        }

        [DirectMethod]
        protected void AssignDispute(object sender, DirectEventArgs e)
        {
            PhoneCall sessionPhoneCallRecord;
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> userSessionPhoneCalls;

            string json = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //Get the session and sip account of the current user
            //CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = CurrentSession.GetEffectiveSipAccount();


            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = CurrentSession.GetUserSessionPhoneCalls();


            json = e.ExtraParams["Values"];
            settings.NullValueHandling = NullValueHandling.Ignore;

            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                var sessionIdTime = phoneCall.SessionIdTime;

                sessionPhoneCallRecord = userSessionPhoneCalls.Find(
                    item => item.SessionIdTime.Year == sessionIdTime.Year
                        && item.SessionIdTime.Month == sessionIdTime.Month
                        && item.SessionIdTime.Day == sessionIdTime.Day
                        && item.SessionIdTime.Hour == sessionIdTime.Hour
                        && item.SessionIdTime.Minute == sessionIdTime.Minute
                        && item.SessionIdTime.Second == sessionIdTime.Second);

                sessionPhoneCallRecord.UiCallType = "Disputed";
                sessionPhoneCallRecord.UiMarkedOn = DateTime.Now;
                sessionPhoneCallRecord.UiUpdatedByUser = sipAccount;

                Global.DATABASE.PhoneCalls.Update(sessionPhoneCallRecord, sessionPhoneCallRecord.PhoneCallsTableName);
            }

            PhoneCallsAllocationToolsMenu.Hide();

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            CurrentSession.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls: userSessionPhoneCalls, userSessionAddressBook: null);

            //Rebind data to the grid store
            RebindDataToStore(userSessionPhoneCalls);
        }

        [DirectMethod]
        protected void AssignAlwaysPersonal(object sender, DirectEventArgs e)
        {
            string json = string.Empty;
            RowSelectionModel selectiomModel;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //These are used for querying the filtering the submitted phonecalls and their destinations
            PhoneBookContact phoneBookEntry;
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> matchedDestinationCalls;
            List<PhoneBookContact> newOrUpdatedPhoneBookEntries = new List<PhoneBookContact>();

            //These would refer to either the the user's or the delegee's
            List<PhoneCall> userSessionPhoneCalls = new List<PhoneCall>();
            Dictionary<string, PhoneBookContact> userSessionAddressBook = new Dictionary<string, PhoneBookContact>();

            //Get user session and effective sip account
            //CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = CurrentSession.GetEffectiveSipAccount();

            //Get user phoneCalls, addressbook, and phoneCallsPerPage;
            //Handle user delegee mode and normal user mode
            CurrentSession.FetchSessionPhonecallsAndAddressbookData(out userSessionPhoneCalls, out userSessionAddressBook);

            //Get the submitted grid data
            json = e.ExtraParams["Values"];
            settings.NullValueHandling = NullValueHandling.Ignore;
            selectiomModel = this.MyPhoneCallsGrid.GetSelectionModel() as RowSelectionModel;
            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);
            
            //Start allocating the submitted phone calls
            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                //Create a Phonebook Entry
                phoneBookEntry = new PhoneBookContact();

                //Check if this entry Already exists by either destination number and destination name (in case it's edited)
                bool found = userSessionAddressBook.ContainsKey(phoneCall.DestinationNumberUri) &&
                             (userSessionAddressBook.Values.Select(phoneBookContact => phoneBookContact.Name == phoneCall.PhoneBookName) == null ? false : true);

                if (!found)
                {
                    phoneBookEntry.Name = phoneCall.PhoneBookName ?? string.Empty;
                    phoneBookEntry.DestinationCountry = phoneCall.MarkerCallToCountry;
                    phoneBookEntry.DestinationNumber = phoneCall.DestinationNumberUri;
                    phoneBookEntry.SipAccount = sipAccount;
                    phoneBookEntry.Type = "Personal";

                    //Add Phonebook entry to Session and to the list which will be written to database 
                    if (userSessionAddressBook.ContainsKey(phoneCall.DestinationNumberUri))
                        userSessionAddressBook[phoneCall.DestinationNumberUri] = phoneBookEntry;
                    else
                        userSessionAddressBook.Add(phoneCall.DestinationNumberUri, phoneBookEntry);

                    newOrUpdatedPhoneBookEntries.Add(phoneBookEntry);
                }

                matchedDestinationCalls = userSessionPhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri && (string.IsNullOrEmpty(o.UiCallType) || o.UiCallType == "Business")).ToList();

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    matchedDestinationCall.UiCallType = "Personal";
                    matchedDestinationCall.UiMarkedOn = DateTime.Now;
                    matchedDestinationCall.UiUpdatedByUser = sipAccount;
                    matchedDestinationCall.PhoneBookName = phoneCall.PhoneBookName ?? string.Empty;

                    Global.DATABASE.PhoneCalls.Update(matchedDestinationCall, matchedDestinationCall.PhoneCallsTableName);
                }
            }

            PhoneCallsAllocationToolsMenu.Hide();

            //Add To Users Addressbook Store
            Global.DATABASE.PhoneBooks.AddOrUpdatePhoneBookEntries(sipAccount, newOrUpdatedPhoneBookEntries);

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            CurrentSession.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls: userSessionPhoneCalls, userSessionAddressBook: userSessionAddressBook);

            //Rebind data to the grid store
            RebindDataToStore(userSessionPhoneCalls);
        }

        [DirectMethod]
        protected void AssignAlwaysBusiness(object sender, DirectEventArgs e)
        {
            string json = string.Empty;
            RowSelectionModel selectiomModel;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //These are used for querying the filtering the submitted phonecalls and their destinations
            PhoneBookContact phoneBookEntry;
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> matchedDestinationCalls;
            List<PhoneBookContact> newOrUpdatedPhoneBookEntries = new List<PhoneBookContact>();

            //These would refer to either the the user's or the delegee's
            List<PhoneCall> userSessionPhoneCalls = new List<PhoneCall>();
            Dictionary<string, PhoneBookContact> userSessionAddressBook = new Dictionary<string, PhoneBookContact>();

            //Get user session and effective sip account
            //CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = CurrentSession.GetEffectiveSipAccount();

            //Get user phoneCalls, addressbook, and phoneCallsPerPage;
            //Handle user delegee mode and normal user mode
            CurrentSession.FetchSessionPhonecallsAndAddressbookData(out userSessionPhoneCalls, out userSessionAddressBook);

            //Get the submitted grid data
            json = e.ExtraParams["Values"];
            settings.NullValueHandling = NullValueHandling.Ignore;
            selectiomModel = this.MyPhoneCallsGrid.GetSelectionModel() as RowSelectionModel;
            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            //Start allocating the submitted phone calls
            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                //Create a Phonebook Entry
                phoneBookEntry = new PhoneBookContact();

                //Check if this entry Already exists by either destination number and destination name (in case it's edited)
                bool found = userSessionAddressBook.ContainsKey(phoneCall.DestinationNumberUri) &&
                             (userSessionAddressBook.Values.Select(phoneBookContact => phoneBookContact.Name == phoneCall.PhoneBookName) == null ? false : true);

                if (!found)
                {
                    phoneBookEntry.Name = phoneCall.PhoneBookName ?? string.Empty;
                    phoneBookEntry.DestinationCountry = phoneCall.MarkerCallToCountry;
                    phoneBookEntry.DestinationNumber = phoneCall.DestinationNumberUri;
                    phoneBookEntry.SipAccount = sipAccount;
                    phoneBookEntry.Type = "Business";

                    //Add Phonebook entry to Session and to the list which will be written to database 
                    if (userSessionAddressBook.ContainsKey(phoneCall.DestinationNumberUri))
                        userSessionAddressBook[phoneCall.DestinationNumberUri] = phoneBookEntry;
                    else
                        userSessionAddressBook.Add(phoneCall.DestinationNumberUri, phoneBookEntry);

                    newOrUpdatedPhoneBookEntries.Add(phoneBookEntry);
                }

                matchedDestinationCalls = userSessionPhoneCalls.Where(o => o.DestinationNumberUri == phoneCall.DestinationNumberUri && (string.IsNullOrEmpty(o.UiCallType) || o.UiCallType == "Personal")).ToList();

                foreach (PhoneCall matchedDestinationCall in matchedDestinationCalls)
                {
                    matchedDestinationCall.UiCallType = "Business";
                    matchedDestinationCall.UiMarkedOn = DateTime.Now;
                    matchedDestinationCall.UiUpdatedByUser = sipAccount;
                    matchedDestinationCall.PhoneBookName = phoneCall.PhoneBookName ?? string.Empty;

                    Global.DATABASE.PhoneCalls.Update(matchedDestinationCall, matchedDestinationCall.PhoneCallsTableName);
                }
            }

            PhoneCallsAllocationToolsMenu.Hide();

            //Add To Users Addressbook Store
            Global.DATABASE.PhoneBooks.AddOrUpdatePhoneBookEntries(sipAccount, newOrUpdatedPhoneBookEntries);

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            CurrentSession.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls: userSessionPhoneCalls, userSessionAddressBook: userSessionAddressBook);

            //Rebind data to the grid store
            RebindDataToStore(userSessionPhoneCalls);
        }

        [DirectMethod]
        protected void MoveToDepartmnent(object sender, DirectEventArgs e)
        {
            PhoneCall sessionPhoneCallRecord;
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> userSessionPhoneCalls;
            string userSiteDepartment = string.Empty;

            string json = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //Get the session and sip account of the current user
            //CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = CurrentSession.GetEffectiveSipAccount();

            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = CurrentSession.GetUserSessionPhoneCalls();

            json = e.ExtraParams["Values"];
            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                var sessionIdTime = phoneCall.SessionIdTime;

                sessionPhoneCallRecord = userSessionPhoneCalls.Find(
                    item => item.SessionIdTime.Year == sessionIdTime.Year
                        && item.SessionIdTime.Month == sessionIdTime.Month
                        && item.SessionIdTime.Day == sessionIdTime.Day
                        && item.SessionIdTime.Hour == sessionIdTime.Hour
                        && item.SessionIdTime.Minute == sessionIdTime.Minute
                        && item.SessionIdTime.Second == sessionIdTime.Second);

                if (sessionPhoneCallRecord.UiAssignedToUser == sipAccount && !string.IsNullOrEmpty(sessionPhoneCallRecord.UiAssignedByUser))
                {
                    userSiteDepartment =
                        (CurrentSession.ActiveRoleName == Functions.UserDelegeeRoleName) ?
                        CurrentSession.DelegeeUserAccount.User.SiteName + "-" + CurrentSession.DelegeeUserAccount.User.DepartmentName :
                        CurrentSession.User.SiteName + "-" + CurrentSession.User.DepartmentName;

                    sessionPhoneCallRecord.UiCallType = string.Empty;
                    sessionPhoneCallRecord.UiAssignedToUser = userSiteDepartment;

                    Global.DATABASE.PhoneCalls.Update(sessionPhoneCallRecord, sessionPhoneCallRecord.PhoneCallsTableName);

                    ModelProxy model = MyPhoneCallsGrid.GetStore().Find("SessionIdTime", sessionPhoneCallRecord.SessionIdTime.ToString());

                    //Remove it from the MyPhoneCallsGrid.GetStore()
                    MyPhoneCallsGrid.GetStore().Remove(model);

                    //Add it to the Departments's phoneCalls Store
                    DepartmentPhoneCallsStore.Add(phoneCall);

                    //Remove from the user own session phoneCalls list.
                    userSessionPhoneCalls.Remove(sessionPhoneCallRecord);
                }
                else
                {
                    continue;
                }
            }

            //Deselect all the grid's records
            MyPhoneCallsGrid.GetSelectionModel().DeselectAll();

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            CurrentSession.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls: userSessionPhoneCalls, userSessionAddressBook: null);

            //Rebind data to the grid store
            RebindDataToStore(userSessionPhoneCalls);
        }

        [DirectMethod]
        protected void AssignSelectedPhonecallsToMe_DirectEvent(object sender, DirectEventArgs e)
        {
            List<PhoneCall> submittedPhoneCalls;
            List<PhoneCall> userSessionPhoneCalls;

            string json = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonSerializerSettings settings = new JsonSerializerSettings();

            //Get the session and sip account of the current user
            //CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = CurrentSession.GetEffectiveSipAccount();

            //Get user phonecalls from the session
            //Handle user delegee mode and normal user mode
            userSessionPhoneCalls = CurrentSession.GetUserSessionPhoneCalls();

            json = e.ExtraParams["Values"];
            submittedPhoneCalls = serializer.Deserialize<List<PhoneCall>>(json);

            foreach (PhoneCall phoneCall in submittedPhoneCalls)
            {
                //Assign the call to this user
                phoneCall.UiAssignedToUser = sipAccount;
                phoneCall.UiCallType = string.Empty;

                //Update this phonecall in the database
                Global.DATABASE.PhoneCalls.Update(phoneCall, phoneCall.PhoneCallsTableName);

                //Commit the changes to the grid and it's store
                ModelProxy model = DepartmentPhoneCallsStore.Find("SessionIdTime", phoneCall.SessionIdTime.ToString());

                //Remove from the Departments's phoneCalls Store
                DepartmentPhoneCallsStore.Remove(model);

                //Add it to the phonecalls store
                MyPhoneCallsGrid.GetStore().Add(phoneCall);

                //Add this new phonecall to the user session
                userSessionPhoneCalls.Add(phoneCall);
            }

            //Reload the department phonecalls grid
            DepartmentPhoneCallsGrid.GetSelectionModel().DeselectAll();

            //Reassign the user session data
            //Handle the normal user mode and user delegee mode
            CurrentSession.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls: userSessionPhoneCalls, userSessionAddressBook: null);

            //Rebind data to the grid store
            RebindDataToStore(userSessionPhoneCalls);
        }
    }
}