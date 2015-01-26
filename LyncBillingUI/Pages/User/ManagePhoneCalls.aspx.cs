using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext;
using Ext.Net;

using CCC.ORM.Helpers;
using LyncBillingUI;
using LyncBillingBase;
using LyncBillingBase.DataModels;
using LyncBillingBase.DataMappers;

namespace LyncBillingUI.Pages.User
{
    public partial class ManagePhoneCalls : System.Web.UI.Page
    {
        private static List<PhoneCall> phoneCalls;
        private static List<Country> countries;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                phoneCalls = Global.DATABASE.PhoneCalls.GetChargableCallsPerUser("aalhour@ccc.gr").ToList();
                countries = Global.DATABASE.Countries.GetAll().ToList();

                phoneCalls.ForEach((phoneCall) =>
                    {
                        var country = countries.Find(item => item.Iso3Code == phoneCall.MarkerCallToCountry);
                        phoneCall.MarkerCallToCountry = (country != null ? country.Name : phoneCall.MarkerCallToCountry);
                    });
            }
        }


        //STORE LOADERS
        protected void PhoneCallsStore_Load(object sender, EventArgs e)
        {
            this.PhoneCallsStore.DataSource = phoneCalls;
            this.PhoneCallsStore.DataBind();
        }


        //User Controls
        [DirectMethod]
        protected void PhoneCallsTypeFilter(object sender, DirectEventArgs e)
        {

        }

        [DirectMethod]
        protected void ShowUserHelpPanel(object sender, DirectEventArgs e)
        {

        }

        [DirectMethod]
        protected void PhoneCallsGridSelectDirectEvents(object sender, DirectEventArgs e)
        {

        }

        [DirectMethod]
        protected void AutomarkCalls_Clicked(object sender, DirectEventArgs e)
        {

        }


        //Phone Calls Allocation
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