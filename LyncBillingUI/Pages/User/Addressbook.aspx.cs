using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext;
using Ext.Net;
using Newtonsoft.Json;

using CCC.ORM.Helpers;
using LyncBillingBase;
using LyncBillingBase.DataModels;
using LyncBillingBase.DataMappers;
using LyncBillingUI;
using LyncBillingUI.Account;
using System.Web.Script.Serialization;
using CCC.UTILS.Outlook;

namespace LyncBillingUI.Pages.User
{
    public partial class Addressbook : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private string normalUserRoleName { get; set; }
        private string userDelegeeRoleName { get; set; }

        private static List<PhoneBookContact> AddressBookData = new List<PhoneBookContact>();
        private static List<PhoneBookContact> HistoryDestinationNumbers = new List<PhoneBookContact>();

        // This actually takes a copy of the current CurrentSession for some uses on the frontend.
        public UserSession CurrentSession { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            //
            // Set the role names of User and Delegee
            SetRolesNames();

            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string RedirectTo = @"/User/Addressbook";
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

        private void UpdateSessionRelatedInformation(PhoneBookContact phoneBookObj = null)
        {
            List<PhoneCall> phoneCalls;
            Dictionary<string, PhoneBookContact> addressBook;

            CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);
            sipAccount = CurrentSession.GetEffectiveSipAccount();

            //Get user addressbook
            addressBook = Global.DATABASE.PhoneBooks.GetAddressBook(sipAccount);

            //Get userphonecalls
            phoneCalls = CurrentSession.GetUserSessionPhoneCalls();

            if (phoneBookObj != null)
            {
                if (phoneCalls.Find(item => item.DestinationNumberUri == phoneBookObj.DestinationNumber) != null)
                {
                    //Update user phonecalls
                    foreach (var phoneCall in phoneCalls)
                    {
                        if (addressBook.ContainsKey(phoneCall.DestinationNumberUri))
                        {
                            phoneCall.PhoneBookName = ((PhoneBookContact)addressBook[phoneCall.DestinationNumberUri]).Name;
                        }
                        else
                        {
                            phoneCall.PhoneBookName = string.Empty;
                        }
                    }
                }
            }
            else
            {
                foreach (var phoneCall in phoneCalls)
                {
                    if (addressBook.ContainsKey(phoneCall.DestinationNumberUri))
                    {
                        phoneCall.PhoneBookName = ((PhoneBookContact)addressBook[phoneCall.DestinationNumberUri]).Name;
                    }
                    else
                    {
                        phoneCall.PhoneBookName = string.Empty;
                    }
                }
            }


            //Allocate the phonecalls and addressbook to the CurrentSession
            //Handle normal user mode, and user delegee mode
            CurrentSession.AssignSessionPhonecallsAndAddressbookData(phoneCalls, addressBook);

        }

        private void GridsDataManager(bool GetFreshData = false, bool BindData = true)
        {
            sipAccount = CurrentSession.GetEffectiveSipAccount();

            if (GetFreshData == true)
            {
                List<PhoneBookContact> TempHistoryData = new List<PhoneBookContact>();
                Dictionary<string, PhoneBookContact> TempAddressBookData = new Dictionary<string, PhoneBookContact>();

                TempAddressBookData = Global.DATABASE.PhoneBooks.GetAddressBook(sipAccount);
                TempHistoryData = Global.DATABASE.TopDestinationNumbers.GetBySipAccount(sipAccount, 200).Select(
                    number => {
                        return new PhoneBookContact() {
                            DestinationNumber = number.PhoneNumber,
                            DestinationCountry = number.Country
                        };
                    })
                    .ToList();

                //Always clear the contents of the data containers
                AddressBookData.Clear();
                HistoryDestinationNumbers.Clear();

                //Normalize the Address Book Data: Convert it from Dictionary to List.
                foreach (KeyValuePair<string, PhoneBookContact> entry in TempAddressBookData)
                {
                    AddressBookData.Add(entry.Value);
                }

                //Normalize the History: Remove AddressBooks entries.
                foreach (PhoneBookContact entry in TempHistoryData)
                {
                    if (!TempAddressBookData.ContainsKey(entry.DestinationNumber))
                    {
                        HistoryDestinationNumbers.Add(entry);
                    }
                }

                TempHistoryData.Clear();
                TempAddressBookData.Clear();
            }

            if (BindData == true)
            {
                AddressBookStore.DataSource = AddressBookData;
                AddressBookStore.DataBind();

                ImportContactsStore.DataSource = HistoryDestinationNumbers;
                ImportContactsStore.DataBind();
            }
        }

        /// <summary>
        /// Reset the Form's Values.
        /// </summary>
        private void ResetFormFields()
        {
            this.ContactDetails_ContactID.Value = "";
            this.ContactDetails_ContactName.Value = "";
            this.ContactDetails_ContactType.Value = "";
            this.ContactDetails_Country.Value = "";
            this.ContactDetails_Number.Value = "";
            this.ContactDetails_SipAccount.Value = "";
        }

        /// <summary>
        /// Set the Form's Values.
        /// </summary>
        private void SetFormFields(PhoneBookContact contact)
        {
            this.ContactDetails_ContactID.Value = contact.Id;
            this.ContactDetails_ContactName.Value = contact.Name;
            this.ContactDetails_ContactType.Value = contact.Type;
            this.ContactDetails_Country.Value = contact.DestinationCountry;
            this.ContactDetails_Number.Value = contact.DestinationNumber;
            this.ContactDetails_SipAccount.Value = contact.SipAccount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact"></param>
        private void GetFormFieldsValues(out PhoneBookContact contact)
        {
            contact = new PhoneBookContact()
            {
                Id = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(this.ContactDetails_ContactID.Value)),
                Name = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(this.ContactDetails_ContactName.Value)),
                Type = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(this.ContactDetails_ContactType.Value)),
                DestinationCountry = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(this.ContactDetails_Country.Value)),
                DestinationNumber = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(this.ContactDetails_Number.Value)),
                SipAccount = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(this.ContactDetails_SipAccount.Value))
            };
        }

        /*
         * AddressBook Data Binding
         */
        protected void AddressBookStore_Load(object sender, EventArgs e)
        {
            bool getFreshData = false;

            if (!X.IsAjaxRequest)
            {
                getFreshData = true;
            }

            GridsDataManager(getFreshData);
            
        }

        protected void AddressBookStore_Refresh(object sender, StoreReadDataEventArgs e)
        {
            this.AddressBookStore.DataBind();
        }

        protected void OutlookContactsStore_Load(object sender, EventArgs e)
        {
            ExchangeWebServices exchange;
            List<PhoneBookContact> contacts;
            List<OutlookContact> outlookContacts;

            if (!X.IsAjaxRequest)
            {
                exchange = new ExchangeWebServices("aalhour", "!_aa_2015", "cccgr");
                outlookContacts = exchange.OutlookContacts;
                contacts = new List<PhoneBookContact>();

                foreach(var outlookContact in outlookContacts)
                {
                    //Add the business phone 1
                    if(!string.IsNullOrEmpty(outlookContact.BusinessPhone1)) 
                        contacts.Add((new PhoneBookContact(0, sipAccount, "Personal", outlookContact.Name, outlookContact.BusinessPhone1, "N/A")));

                    //Add the business phone 2
                    if (!string.IsNullOrEmpty(outlookContact.BusinessPhone2))
                        contacts.Add((new PhoneBookContact(0, sipAccount, "Personal", outlookContact.Name, outlookContact.BusinessPhone2, "N/A")));

                    //Add the home phone 1
                    if (!string.IsNullOrEmpty(outlookContact.HomePhone1))
                        contacts.Add((new PhoneBookContact(0, sipAccount, "Personal", outlookContact.Name, outlookContact.HomePhone1, "N/A")));

                    //Add the home phone 2
                    if (!string.IsNullOrEmpty(outlookContact.HomePhone2))
                        contacts.Add((new PhoneBookContact(0, sipAccount, "Personal", outlookContact.Name, outlookContact.HomePhone2, "N/A")));

                    //Add the mobile phone
                    if (!string.IsNullOrEmpty(outlookContact.MobilePhone))
                        contacts.Add((new PhoneBookContact(0, sipAccount, "Personal", outlookContact.Name, outlookContact.MobilePhone, "N/A")));
                }

                if (contacts != null && contacts.Count > 0)
                {
                    this.OutlookContactsStore.DataSource = contacts;
                    this.OutlookContactsStore.DataBind();
                }
            }
        }

        protected void OutlookContactsStore_Refresh(object sender, StoreReadDataEventArgs e)
        {
            this.OutlookContactsGrid.GetStore().DataBind();
        }

        /*
         * ImportContacts Data Binding
         */
        protected void ImportContactsStore_Load(object sender, EventArgs e)
        {
            bool getFreshData = false;

            if (!X.IsAjaxRequest)
            {
                getFreshData = true;
            }

            GridsDataManager(getFreshData);
        }

        [DirectMethod]
        protected void AddressBookContactSelect(object sender, DirectEventArgs e)
        {
            string destinationNumber = e.ExtraParams["DestinationNumber"];

            var contact = AddressBookData.Find(item => item.DestinationNumber == destinationNumber);

            if (contact != null)
            {
                SetFormFields(contact);
            }
            else
            {
                GridsDataManager(GetFreshData: true);
                ResetFormFields();
            }
        }

        [DirectMethod]
        protected void RejectImportChanges_DirectEvent(object sender, DirectEventArgs e)
        {
            ImportContactsGrid.GetStore().RejectChanges();
        }

        [DirectMethod]
        protected void ImportContactsFromHistory(object sender, DirectEventArgs e)
        {
            sipAccount = CurrentSession.GetEffectiveSipAccount();

            string json = e.ExtraParams["Values"];

            List<PhoneBookContact> allAddressBookContacts = new List<PhoneBookContact>();
            List<PhoneBookContact> filteredAddressBookContacts = new List<PhoneBookContact>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            allAddressBookContacts = serializer.Deserialize<List<PhoneBookContact>>(json);

            foreach (var contact in allAddressBookContacts)
            {
                if (!string.IsNullOrEmpty(contact.Type) && (contact.Type == "Personal" || contact.Type == "Business"))
                {
                    contact.SipAccount = sipAccount;
                    filteredAddressBookContacts.Add(contact);
                }
            }

            if (filteredAddressBookContacts.Count > 0)
            {
                Global.DATABASE.PhoneBooks.InsertMany(filteredAddressBookContacts);
                GridsDataManager(true);

                AddressBookGrid.GetStore().Reload();
                ImportContactsGrid.GetStore().Reload();

                //Update the session's phonebook dictionary and phonecalls list.
                UpdateSessionRelatedInformation();
            }
        }

        [DirectMethod]
        protected void UpdateAddressBook_DirectEvent(object sender, DirectEventArgs e)
        {
            sipAccount = CurrentSession.GetEffectiveSipAccount();

            string json = e.ExtraParams["Values"];

            List<PhoneBookContact> recordsToUpate = new List<PhoneBookContact>();
            List<PhoneBookContact> filteredContactsForUpdate = new List<PhoneBookContact>();
            ChangeRecords<PhoneBookContact> toBeUpdated = new StoreDataHandler(e.ExtraParams["Values"]).BatchObjectData<PhoneBookContact>();

            if (toBeUpdated.Updated.Count > 0)
            {
                foreach (var contact in toBeUpdated.Updated)
                {
                    if (!string.IsNullOrEmpty(contact.Type) && (contact.Type == "Personal" || contact.Type == "Business"))
                    {
                        if (!string.IsNullOrEmpty(contact.SipAccount))
                        {
                            contact.SipAccount = sipAccount;
                        }

                        filteredContactsForUpdate.Add(contact);
                    }
                }

                if (filteredContactsForUpdate.Count > 0)
                {
                    //foreach (var contact in filteredContactsForUpdate)
                    //{
                    //    Global.DATABASE.PhoneBooks.Update(contact);
                    //}
                    Global.DATABASE.PhoneBooks.UpdateMany(filteredContactsForUpdate);

                    GridsDataManager(true);

                    AddressBookGrid.GetStore().Reload();
                    ImportContactsGrid.GetStore().Reload();
                }
            }

            if (toBeUpdated.Deleted.Count > 0)
            {
                Global.DATABASE.PhoneBooks.DeleteMany(toBeUpdated.Deleted.ToList<PhoneBookContact>());
                GridsDataManager(true);

                AddressBookGrid.GetStore().Reload();
                ImportContactsGrid.GetStore().Reload();
            }

            //Update the session's phonebook dictionary and phonecalls list.
            UpdateSessionRelatedInformation();
        }

        [DirectMethod]
        protected void SaveChangesButton_DirectEvent(object sender, DirectEventArgs e)
        {
            int contactId;
            string contactIdString;
            bool doesExist;
            PhoneBookContact contact;

            contactIdString = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(this.ContactDetails_ContactID.Value));

            if (!string.IsNullOrEmpty(contactIdString))
            {
                contactId = Convert.ToInt32(contactIdString);

                doesExist = AddressBookData.Exists(item => item.Id == contactId);

                if (doesExist == true)
                {
                    GetFormFieldsValues(out contact);

                    if (!string.IsNullOrEmpty(contact.SipAccount) && !string.IsNullOrEmpty(contact.Type) && !string.IsNullOrEmpty(contact.DestinationNumber))
                    {
                        Global.DATABASE.PhoneBooks.Update(contact);
                        GridsDataManager(true);

                        AddressBookGrid.GetStore().Reload();
                        ImportContactsGrid.GetStore().Reload();
                    }
                }

                //Reset the form fields.
                ResetFormFields();

                //Update the session's phonebook dictionary and phonecalls list.
                UpdateSessionRelatedInformation();
            }
        }

        [DirectMethod]
        protected void CancelChangesButton_DirectEvent(object sender, DirectEventArgs e)
        {
            int contactId = Convert.ToInt32(this.ContactDetails_ContactID.Value);

            if (contactId > 0)
            {
                var contact = AddressBookData.Find(item => item.Id == contactId);

                SetFormFields(contact);
            }
        }

        [DirectMethod]
        protected void DeleteContactButton_DirectEvent(object sender, DirectEventArgs e)
        {
            int contactId;
            string contactIdString;
            PhoneBookContact contact;

             contactIdString = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(this.ContactDetails_ContactID.Value));

             if (!string.IsNullOrEmpty(contactIdString))
             {
                 contactId = Convert.ToInt32(contactIdString);

                 contact = AddressBookData.Find(item => item.Id == contactId);

                 if (contact != null)
                 {
                     Global.DATABASE.PhoneBooks.Delete(contact);
                     GridsDataManager(true);

                     AddressBookGrid.GetStore().Reload();
                     ImportContactsGrid.GetStore().Reload();
                 }

                 //Reset the form fields.
                 ResetFormFields();

                 //Update the session's phonebook dictionary and phonecalls list.
                 UpdateSessionRelatedInformation();
             }
        }


        /***
         * ADD NEW COTNACT WINDOW - EVENTS
         */
        [DirectMethod]
        protected void AddNewAddressBookContact_Click(object sender, DirectEventArgs e)
        {
            AddNewContactWindowPanel.Show();
        }

        [DirectMethod]
        protected void CancelNewContactButton_Click(object sender, DirectEventArgs e)
        {
            AddNewContactWindowPanel.Hide();
        }

        [DirectMethod]
        protected void AddNewContactWindowPanel_BeforeHide(object sender, DirectEventArgs e)
        {
            NewContact_ContactName.Text = null;
            NewContact_ContactNumber.Text = null;
            NewContact_ContactType.Select(0);
        }

        [DirectMethod]
        protected void AddNewContactButton_Click(object sender, DirectEventArgs e)
        {
            PhoneBookContact NewContact;
            string statusMessage = string.Empty;

            if (!string.IsNullOrEmpty(NewContact_ContactNumber.Text) && NewContact_ContactType.SelectedItem.Index > -1)
            {
                string telephoneNumber = NewContact_ContactNumber.Text;
                string detectedCountry = Global.DATABASE.NumberingPlans.GetIso3CountryCodeByNumber(telephoneNumber);

                if (telephoneNumber.Length < 9 || string.IsNullOrEmpty(detectedCountry))
                {
                    statusMessage = "Number is invalid.";
                }
                else if (CurrentSession.Addressbook.ContainsKey(telephoneNumber))
                {
                    statusMessage = "Cannot add duplicate contacts.";
                }
                else
                {
                    NewContact = new PhoneBookContact();

                    NewContact.DestinationNumber = telephoneNumber;
                    NewContact.SipAccount = sipAccount;
                    NewContact.Type = Convert.ToString(NewContact_ContactType.SelectedItem.Value);
                    NewContact.Name = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(NewContact_ContactName.Text));
                    NewContact.DestinationCountry = detectedCountry;

                    Global.DATABASE.PhoneBooks.Insert(NewContact);

                    GridsDataManager(true);

                    //Update the session's phonebook dictionary and phonecalls list.
                    UpdateSessionRelatedInformation(NewContact);

                    AddNewContactWindowPanel.Hide();
                }
            }
            else
            {
                statusMessage = "Please provide all the information.";
            }

            NewContact_StatusMessage.Text = statusMessage;
        }

    }

}