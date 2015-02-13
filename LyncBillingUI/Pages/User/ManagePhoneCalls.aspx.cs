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
using LyncBillingUI;
using LyncBillingUI.Account;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace LyncBillingUI.Pages.User
{
    public partial class ManagePhoneCalls : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private string normalUserRoleName { get; set; }
        private string userDelegeeRoleName { get; set; }

        private static List<PhoneCall> phoneCalls;
        private static List<Country> countries;

        // This actually takes a copy of the current session for some uses on the frontend.
        public UserSession CurrentSession { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            SetRolesNames();

            // If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string RedirectTo = @"/User/Manage/PhoneCalls";
                string Url = @"/Login?RedirectTo=" + RedirectTo;
                Response.Redirect(Url);
            }
            else
            {
                CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
                if (CurrentSession.ActiveRoleName != normalUserRoleName && CurrentSession.ActiveRoleName != userDelegeeRoleName)
                {
                    string url = @"/Authenticate?access=" + CurrentSession.ActiveRoleName;
                    Response.Redirect(url);
                }
            }

            sipAccount = CurrentSession.GetEffectiveSipAccount();

            //
            // Handle user delegee mode and normal user mode
            if (CurrentSession.ActiveRoleName == userDelegeeRoleName)
                CurrentSession.DelegeeUserAccount.DelegeeUserAddressbook = Global.DATABASE.PhoneBooks.GetAddressBook(sipAccount);
            else
                CurrentSession.Addressbook = Global.DATABASE.PhoneBooks.GetAddressBook(sipAccount);
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
        // Data Binding/Re-binding function
        private void RebindDataToStore(List<PhoneCall> phoneCallsData)
        {
            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();

            ManagePhoneCallsGrid.GetStore().RemoveAll();

            //
            // Trigger the PhoneCallsTypeFilter OnSelect Event
            PhoneCallsTypeFilter(null, null);
        }


        //
        // STORE LOADERS
        protected void PhoneCallsStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                //Get use session and user phonecalls list.
                CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

                //Get user session phonecalls; handle normal user mode and delegee mode
                List<PhoneCall> userSessionPhoneCalls = CurrentSession.GetUserSessionPhoneCalls().Where(phoneCall => string.IsNullOrEmpty(phoneCall.UiCallType) == true).ToList();

                ManagePhoneCallsGrid.GetStore().DataSource = userSessionPhoneCalls;
                ManagePhoneCallsGrid.GetStore().DataBind();
            }
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
                ManagePhoneCallsGrid.GetStore().DataSource = userSessionPhoneCalls;
                ManagePhoneCallsGrid.GetStore().DataBind();

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
                ManagePhoneCallsGrid.GetStore().DataSource = userSessionPhoneCalls;
                ManagePhoneCallsGrid.GetStore().DataBind();

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

                            ModelProxy model = PhoneCallsStore.Find("SessionIdTime", phoneCall.SessionIdTime.ToString());
                            model.Set(phoneCall);
                            model.Commit();
                        }
                    }
                }
            }

            ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();
            PhoneCallsStore.LoadPage(1);

            CurrentSession.AssignSessionPhonecallsAndAddressbookData(userSessionPhoneCalls, userSessionAddressbook);
        }


        //
        // Phone Calls Allocation
        [DirectMethod]
        protected void RejectChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ManagePhoneCallsGrid.GetStore().RejectChanges();
        }

        [DirectMethod]
        protected void AssignPersonal(object sender, DirectEventArgs e)
        {
            //PhoneCalls sessionPhoneCallRecord;
            //List<PhoneCalls> submittedPhoneCalls;
            //List<PhoneCalls> userSessionPhoneCalls;

            //string json = string.Empty;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //JsonSerializerSettings settings = new JsonSerializerSettings();

            ////Get the session and sip account of the current user
            //session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            //sipAccount = session.GetEffectiveSipAccount();

            ////Get user phonecalls from the session
            ////Handle user delegee mode and normal user mode
            //userSessionPhoneCalls = session.GetUserSessionPhoneCalls();

            //json = e.ExtraParams["Values"];
            //submittedPhoneCalls = serializer.Deserialize<List<PhoneCalls>>(json);

            //foreach (PhoneCalls phoneCall in submittedPhoneCalls)
            //{
            //    sessionPhoneCallRecord = userSessionPhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();

            //    sessionPhoneCallRecord.UI_CallType = "Personal";
            //    sessionPhoneCallRecord.UI_MarkedOn = DateTime.Now;
            //    sessionPhoneCallRecord.UI_UpdatedByUser = sipAccount;

            //    PhoneCalls.UpdatePhoneCall(sessionPhoneCallRecord);
            //}

            //PhoneCallsAllocationToolsMenu.Hide();

            ////Reassign the user session data
            ////Handle the normal user mode and user delegee mode
            //session.AssignSessionPhonecallsAndAddressbookData(
            //    userSessionPhoneCalls: userSessionPhoneCalls,
            //    userSessionAddressBook: null);

            ////Rebind data to the grid store
            //RebindDataToStore(userSessionPhoneCalls);
        }

        [DirectMethod]
        protected void AssignBusiness(object sender, DirectEventArgs e)
        {
            //PhoneCalls sessionPhoneCallRecord;
            //List<PhoneCalls> submittedPhoneCalls;
            //List<PhoneCalls> userSessionPhoneCalls;

            //string json = string.Empty;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //JsonSerializerSettings settings = new JsonSerializerSettings();

            ////Get the session and sip account of the current user
            //session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            //sipAccount = session.GetEffectiveSipAccount();

            ////Get user phonecalls from the session
            ////Handle user delegee mode and normal user mode
            //userSessionPhoneCalls = session.GetUserSessionPhoneCalls();

            //json = e.ExtraParams["Values"];
            //submittedPhoneCalls = serializer.Deserialize<List<PhoneCalls>>(json);

            //foreach (PhoneCalls phoneCall in submittedPhoneCalls)
            //{
            //    sessionPhoneCallRecord = userSessionPhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();

            //    sessionPhoneCallRecord.UI_CallType = "Business";
            //    sessionPhoneCallRecord.UI_MarkedOn = DateTime.Now;
            //    sessionPhoneCallRecord.UI_UpdatedByUser = sipAccount;

            //    PhoneCalls.UpdatePhoneCall(sessionPhoneCallRecord);
            //}

            //PhoneCallsAllocationToolsMenu.Hide();

            ////Reassign the user session data
            ////Handle the normal user mode and user delegee mode
            //session.AssignSessionPhonecallsAndAddressbookData(
            //    userSessionPhoneCalls: userSessionPhoneCalls,
            //    userSessionAddressBook: null);

            ////Rebind data to the grid store
            //RebindDataToStore(userSessionPhoneCalls);
        }

        [DirectMethod]
        protected void AssignDispute(object sender, DirectEventArgs e)
        {
            //List<PhoneCalls> submittedPhoneCalls;
            //List<PhoneCalls> userSessionPhoneCalls;

            //string json = string.Empty;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //JsonSerializerSettings settings = new JsonSerializerSettings();

            ////Get the session and sip account of the current user
            //session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            //sipAccount = session.GetEffectiveSipAccount();


            ////Get user phonecalls from the session
            ////Handle user delegee mode and normal user mode
            //userSessionPhoneCalls = session.GetUserSessionPhoneCalls();


            //json = e.ExtraParams["Values"];
            //settings.NullValueHandling = NullValueHandling.Ignore;

            //submittedPhoneCalls = serializer.Deserialize<List<PhoneCalls>>(json);

            //foreach (PhoneCalls phoneCall in submittedPhoneCalls)
            //{
            //    PhoneCalls matchedDestinationCalls = userSessionPhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();

            //    matchedDestinationCalls.UI_CallType = "Disputed";
            //    matchedDestinationCalls.UI_MarkedOn = DateTime.Now;
            //    matchedDestinationCalls.UI_UpdatedByUser = sipAccount;

            //    PhoneCalls.UpdatePhoneCall(matchedDestinationCalls);
            //}

            //PhoneCallsAllocationToolsMenu.Hide();

            ////Reassign the user session data
            ////Handle the normal user mode and user delegee mode
            //session.AssignSessionPhonecallsAndAddressbookData(
            //    userSessionPhoneCalls: userSessionPhoneCalls,
            //    userSessionAddressBook: null);

            ////Rebind data to the grid store
            //RebindDataToStore(userSessionPhoneCalls);
        }

        [DirectMethod]
        protected void AssignAlwaysPersonal(object sender, DirectEventArgs e)
        {
            //string json = string.Empty;
            //RowSelectionModel selectiomModel;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //JsonSerializerSettings settings = new JsonSerializerSettings();

            ////These are used for querying the filtering the submitted phonecalls and their destinations
            //PhoneBook phoneBookEntry;
            //List<PhoneCalls> submittedPhoneCalls;
            //List<PhoneCalls> matchedDestinationCalls;
            //List<PhoneBook> newOrUpdatedPhoneBookEntries = new List<PhoneBook>();

            ////These would refer to either the the user's or the delegee's
            //List<PhoneCalls> userSessionPhoneCalls = new List<PhoneCalls>();
            //Dictionary<string, PhoneBook> userSessionAddressBook = new Dictionary<string, PhoneBook>();

            ////Get user session and effective sip account
            //session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            //sipAccount = session.GetEffectiveSipAccount();

            ////Get user phoneCalls, addressbook, and phoneCallsPerPage;
            ////Handle user delegee mode and normal user mode
            //session.FetchSessionPhonecallsAndAddressbookData(out userSessionPhoneCalls, out userSessionAddressBook);


            ////Get the submitted grid data
            //json = e.ExtraParams["Values"];
            //settings.NullValueHandling = NullValueHandling.Ignore;

            //selectiomModel = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;

            //submittedPhoneCalls = serializer.Deserialize<List<PhoneCalls>>(json);
            ////userSessionPhoneCallsPerPageJson = json;

            ////Start allocating the submitted phone calls
            //foreach (PhoneCalls phoneCall in submittedPhoneCalls)
            //{
            //    //Create a Phonebook Entry
            //    phoneBookEntry = new PhoneBook();

            //    //Check if this entry Already exists by either destination number and destination name (in case it's edited)
            //    bool found = userSessionAddressBook.ContainsKey(phoneCall.DestinationNumberUri) &&
            //                 (userSessionAddressBook.Values.Select(phoneBookContact => phoneBookContact.Name == phoneCall.PhoneBookName) == null ? false : true);

            //    if (!found)
            //    {
            //        phoneBookEntry.Name = phoneCall.PhoneBookName ?? string.Empty;
            //        phoneBookEntry.DestinationCountry = phoneCall.Marker_CallToCountry;
            //        phoneBookEntry.DestinationNumber = phoneCall.DestinationNumberUri;
            //        phoneBookEntry.SipAccount = sipAccount;
            //        phoneBookEntry.Type = "Personal";

            //        //Add Phonebook entry to Session and to the list which will be written to database 
            //        if (userSessionAddressBook.ContainsKey(phoneCall.DestinationNumberUri))
            //            userSessionAddressBook[phoneCall.DestinationNumberUri] = phoneBookEntry;
            //        else
            //            userSessionAddressBook.Add(phoneCall.DestinationNumberUri, phoneBookEntry);

            //        newOrUpdatedPhoneBookEntries.Add(phoneBookEntry);
            //    }

            //    matchedDestinationCalls = userSessionPhoneCalls.Where(
            //        o => o.DestinationNumberUri == phoneCall.DestinationNumberUri && (string.IsNullOrEmpty(o.UI_CallType) || o.UI_CallType == "Business")
            //    ).ToList();


            //    foreach (PhoneCalls matchedDestinationCall in matchedDestinationCalls)
            //    {
            //        matchedDestinationCall.UI_CallType = "Personal";
            //        matchedDestinationCall.UI_MarkedOn = DateTime.Now;
            //        matchedDestinationCall.UI_UpdatedByUser = sipAccount;
            //        matchedDestinationCall.PhoneBookName = phoneCall.PhoneBookName ?? string.Empty;

            //        PhoneCalls.UpdatePhoneCall(matchedDestinationCall);
            //    }
            //}

            //PhoneCallsAllocationToolsMenu.Hide();

            ////Add To Users Addressbook Store
            //PhoneBook.AddOrUpdatePhoneBookEntries(sipAccount, newOrUpdatedPhoneBookEntries);

            ////Reassign the user session data
            ////Handle the normal user mode and user delegee mode
            //session.AssignSessionPhonecallsAndAddressbookData(
            //    userSessionPhoneCalls: userSessionPhoneCalls,
            //    userSessionAddressBook: userSessionAddressBook);

            ////Rebind data to the grid store
            //RebindDataToStore(userSessionPhoneCalls);
        }

        [DirectMethod]
        protected void AssignAlwaysBusiness(object sender, DirectEventArgs e)
        {
            //string json = string.Empty;
            //RowSelectionModel selectiomModel;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //JsonSerializerSettings settings = new JsonSerializerSettings();

            ////These are used for querying the filtering the submitted phonecalls and their destinations
            //PhoneBook phoneBookEntry;
            //List<PhoneCalls> submittedPhoneCalls;
            //IEnumerable<PhoneCalls> matchedDestinationCalls;
            //List<PhoneBook> newOrUpdatedPhoneBookEntries = new List<PhoneBook>();

            ////These would refer to either the the user's or the delegee's
            //List<PhoneCalls> userSessionPhoneCalls = new List<PhoneCalls>();
            //Dictionary<string, PhoneBook> userSessionAddressBook = new Dictionary<string, PhoneBook>();

            ////Get user session and effective sip account
            //session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            //sipAccount = session.GetEffectiveSipAccount();

            ////Get user phoneCalls, addressbook, and phoneCallsPerPage;
            ////Handle user delegee mode and normal user mode
            //session.FetchSessionPhonecallsAndAddressbookData(out userSessionPhoneCalls, out userSessionAddressBook);


            ////Get the submitted grid data
            //json = e.ExtraParams["Values"];
            //settings.NullValueHandling = NullValueHandling.Ignore;
            //selectiomModel = this.ManagePhoneCallsGrid.GetSelectionModel() as RowSelectionModel;
            //submittedPhoneCalls = serializer.Deserialize<List<PhoneCalls>>(json);

            ////Start allocating the submitted phone calls
            //foreach (PhoneCalls phoneCall in submittedPhoneCalls)
            //{
            //    //Create a Phonebook Entry
            //    phoneBookEntry = new PhoneBook();

            //    //Check if this entry Already exists by either destination number and destination name (in case it's edited)
            //    bool found = userSessionAddressBook.ContainsKey(phoneCall.DestinationNumberUri) &&
            //                 (userSessionAddressBook.Values.Select(phoneBookContact => phoneBookContact.Name == phoneCall.PhoneBookName) == null ? false : true);

            //    if (!found)
            //    {
            //        phoneBookEntry.Name = phoneCall.PhoneBookName ?? string.Empty;
            //        phoneBookEntry.DestinationCountry = phoneCall.Marker_CallToCountry;
            //        phoneBookEntry.DestinationNumber = phoneCall.DestinationNumberUri;
            //        phoneBookEntry.SipAccount = sipAccount;
            //        phoneBookEntry.Type = "Business";

            //        //Add Phonebook entry to Session and to the list which will be written to database 
            //        if (userSessionAddressBook.ContainsKey(phoneCall.DestinationNumberUri))
            //            userSessionAddressBook[phoneCall.DestinationNumberUri] = phoneBookEntry;
            //        else
            //            userSessionAddressBook.Add(phoneCall.DestinationNumberUri, phoneBookEntry);

            //        newOrUpdatedPhoneBookEntries.Add(phoneBookEntry);
            //    }


            //    matchedDestinationCalls = userSessionPhoneCalls.Where(
            //        o => o.DestinationNumberUri == phoneCall.DestinationNumberUri && (string.IsNullOrEmpty(o.UI_CallType) || o.UI_CallType == "Personal")
            //    ).ToList();

            //    foreach (PhoneCalls matchedDestinationCall in matchedDestinationCalls)
            //    {
            //        matchedDestinationCall.UI_CallType = "Business";
            //        matchedDestinationCall.UI_MarkedOn = DateTime.Now;
            //        matchedDestinationCall.UI_UpdatedByUser = sipAccount;
            //        matchedDestinationCall.PhoneBookName = phoneCall.PhoneBookName ?? string.Empty;

            //        PhoneCalls.UpdatePhoneCall(matchedDestinationCall);
            //    }
            //}

            //PhoneCallsAllocationToolsMenu.Hide();

            ////Add To Users Addressbook Store
            //PhoneBook.AddOrUpdatePhoneBookEntries(sipAccount, newOrUpdatedPhoneBookEntries);

            ////Reassign the user session data
            ////Handle the normal user mode and user delegee mode
            //session.AssignSessionPhonecallsAndAddressbookData(
            //    userSessionPhoneCalls: userSessionPhoneCalls,
            //    userSessionAddressBook: userSessionAddressBook);

            ////Rebind data to the grid store
            //RebindDataToStore(userSessionPhoneCalls);
        }

        [DirectMethod]
        protected void MoveToDepartmnent(object sender, DirectEventArgs e)
        {
            //PhoneCalls sessionPhoneCallRecord;
            //List<PhoneCalls> submittedPhoneCalls;
            //List<PhoneCalls> userSessionPhoneCalls;
            //string userSiteDepartment = string.Empty;

            //string json = string.Empty;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //JsonSerializerSettings settings = new JsonSerializerSettings();

            ////Get the session and sip account of the current user
            //session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            //sipAccount = session.GetEffectiveSipAccount();

            ////Get user phonecalls from the session
            ////Handle user delegee mode and normal user mode
            //userSessionPhoneCalls = session.GetUserSessionPhoneCalls();

            //json = e.ExtraParams["Values"];
            //submittedPhoneCalls = serializer.Deserialize<List<PhoneCalls>>(json);

            //foreach (PhoneCalls phoneCall in submittedPhoneCalls)
            //{
            //    sessionPhoneCallRecord = userSessionPhoneCalls.Where(o => o.SessionIdTime == phoneCall.SessionIdTime).First();

            //    if (sessionPhoneCallRecord.UI_AssignedToUser == sipAccount && !string.IsNullOrEmpty(sessionPhoneCallRecord.UI_AssignedByUser))
            //    {
            //        userSiteDepartment =
            //            (session.ActiveRoleName == userDelegeeRoleName) ?
            //            session.DelegeeAccount.DelegeeUserAccount.SiteName + "-" + session.DelegeeAccount.DelegeeUserAccount.Department :
            //            session.NormalUserInfo.SiteName + "-" + session.NormalUserInfo.Department;

            //        sessionPhoneCallRecord.UI_AssignedToUser = userSiteDepartment;

            //        PhoneCalls.UpdatePhoneCall(sessionPhoneCallRecord, FORCE_RESET_UI_CallType: true);

            //        ModelProxy model = PhoneCallsStore.Find(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), sessionPhoneCallRecord.SessionIdTime.ToString());

            //        //Remove it from the PhoneCallsStore
            //        PhoneCallsStore.Remove(model);

            //        //Add it to the Departments's phoneCalls Store
            //        DepartmentPhoneCallsStore.Add(phoneCall);

            //        //Remove from the user own session phoneCalls list.
            //        userSessionPhoneCalls.Remove(sessionPhoneCallRecord);
            //    }
            //    else
            //    {
            //        continue;
            //    }
            //}

            //ManagePhoneCallsGrid.GetSelectionModel().DeselectAll();

            ////Reassign the user session data
            ////Handle the normal user mode and user delegee mode
            //session.AssignSessionPhonecallsAndAddressbookData(
            //    userSessionPhoneCalls: userSessionPhoneCalls,
            //    userSessionAddressBook: null);

            ////Rebind data to the grid store
            //RebindDataToStore(userSessionPhoneCalls);
        }

        [DirectMethod]
        protected void AssignSelectedPhonecallsToMe_DirectEvent(object sender, DirectEventArgs e)
        {
            //List<PhoneCalls> submittedPhoneCalls;
            //List<PhoneCalls> userSessionPhoneCalls;

            //string json = string.Empty;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //JsonSerializerSettings settings = new JsonSerializerSettings();

            ////Get the session and sip account of the current user
            //session = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            //sipAccount = session.GetEffectiveSipAccount();

            ////Get user phonecalls from the session
            ////Handle user delegee mode and normal user mode
            //userSessionPhoneCalls = session.GetUserSessionPhoneCalls();

            //json = e.ExtraParams["Values"];
            //submittedPhoneCalls = serializer.Deserialize<List<PhoneCalls>>(json);

            //foreach (PhoneCalls phoneCall in submittedPhoneCalls)
            //{
            //    //Assign the call to this user
            //    phoneCall.UI_AssignedToUser = sipAccount;

            //    //Update this phonecall in the database
            //    PhoneCalls.UpdatePhoneCall(phoneCall, FORCE_RESET_UI_CallType: true);

            //    //Commit the changes to the grid and it's store
            //    ModelProxy model = DepartmentPhoneCallsStore.Find(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), phoneCall.SessionIdTime.ToString());

            //    //Remove from the Departments's phoneCalls Store
            //    DepartmentPhoneCallsStore.Remove(model);

            //    //Add it to the phonecalls store
            //    PhoneCallsStore.Add(phoneCall);

            //    //Add this new phonecall to the user session
            //    userSessionPhoneCalls.Add(phoneCall);
            //}

            ////Reload the department phonecalls grid
            //DepartmentPhoneCallsGrid.GetSelectionModel().DeselectAll();

            ////Reassign the user session data
            ////Handle the normal user mode and user delegee mode
            //session.AssignSessionPhonecallsAndAddressbookData(
            //    userSessionPhoneCalls: userSessionPhoneCalls,
            //    userSessionAddressBook: null);

            ////Rebind data to the grid store
            //RebindDataToStore(userSessionPhoneCalls);
        }
    }
}