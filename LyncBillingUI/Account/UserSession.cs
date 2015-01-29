using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq.Expressions;

using LyncBillingUI;
using LyncBillingBase.DataMappers;
using LyncBillingBase.DataModels;
using LyncBillingBase.Helpers;

namespace LyncBillingUI.Account
{
    public class UserSession
    {
        private static readonly List<UserSession> UsersSessions = new List<UserSession>();

        public UserSession()
        {
            User = new User();
            IpAddress = string.Empty;
            UserAgent = string.Empty;

            BundledAccountsList = new List<string>();

            ActiveRoleName = string.Empty;
            SystemRoles = new List<SystemRole>();
            DelegeeUserAccount = null;

            //Initialized other containers
            Phonecalls = new List<PhoneCall>();
            Addressbook = new Dictionary<string, PhoneBookContact>();

            //By default the roles are set to false unless initialized as otherwise!
            IsDeveloper = false;
            IsSystemAdmin = false;
            IsSiteAdmin = false;
            IsSiteAccountant = false;
            IsDepartmentHead = false;

            IsDelegee = false;
            IsUserDelegate = false;
            IsDepartmentDelegate = false;
            IsSiteDelegate = false;

            //Initialize the lists
            UserDelegateRoles = new List<DelegateRole>();
            DepartmentDelegateRoles = new List<DelegateRole>();
            SiteDelegateRoles = new List<DelegateRole>();
        }

        //Normal user data
        public User User { get; set; }
        public string IpAddress { set; get; }
        public string UserAgent { set; get; }

        //Bundled Accounts List
        public List<string> BundledAccountsList { get; set; }
        
        //Roles Related
        public string ActiveRoleName { set; get; }
        public List<SystemRole> SystemRoles { set; get; }
        public List<DelegateRole> UserDelegateRoles { get; set; }
        public List<DelegateRole> SiteDelegateRoles { get; set; }
        public List<DelegateRole> DepartmentDelegateRoles { get; set; }
        public DelegeeUserAccount DelegeeUserAccount { get; set; }
        
        //Phone Calls and Phone Book Related
        public List<PhoneCall> Phonecalls { set; get; }
        public Dictionary<string, PhoneBookContact> Addressbook { set; get; }
        
        //Generic Users SystemRoles
        public bool IsDeveloper { set; get; }
        
        //System Roles
        public bool IsSystemAdmin { set; get; }
        public bool IsSiteAdmin { set; get; }
        public bool IsSiteAccountant { set; get; }
        
        //Departments Head Role
        public bool IsDepartmentHead { get; set; }
        
        //Delegates Roles
        public bool IsDelegee { set; get; }
        public bool IsUserDelegate { set; get; }
        public bool IsDepartmentDelegate { set; get; }
        public bool IsSiteDelegate { set; get; }

        private void InitializeSystemRoles(List<SystemRole> systemRoles = null)
        {
            if (systemRoles != null && systemRoles.Count > 0)
            {
                this.SystemRoles = systemRoles;

                foreach (var role in systemRoles)
                {
                    if (Global.DATABASE.Roles.DeveloperRoleID == role.RoleId) IsDeveloper = true;
                    else if (Global.DATABASE.Roles.SystemAdminRoleID == role.RoleId) IsSystemAdmin = true;
                    else if (Global.DATABASE.Roles.SiteAdminRoleID == role.RoleId) IsSiteAdmin = true;
                    else if (Global.DATABASE.Roles.SiteAccountantRoleID == role.RoleId) IsSiteAccountant = true;
                }
            }
        }

        /***
         * Please note that this function depends on the state of the "PrimarySipAccount" variable
         * So, don't call this function before at least initializing these instance variables unless you pass the SipAccount directly
         */

        private void InitializeDelegeesInformation(string userSipAccount = "")
        {
            //
            // Delegees list.
            List<DelegateRole> delegees;

            //This is a mandatory check, because we can't go on with the procedure without a SipAccount!!
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(User.SipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(User.SipAccount))
            {
                userSipAccount = User.SipAccount;
            }

            //Initialize the Delegees SystemRoles Flags
            delegees = Global.DATABASE.DelegateRoles.GetByDelegeeSipAccount(userSipAccount);

            if(delegees != null && delegees.Any())
            { 
                this.IsUserDelegate = delegees.Exists(item => item.DelegationType == 1);
                this.IsDepartmentDelegate = delegees.Exists(item => item.DelegationType == 2);
                this.IsSiteDelegate = delegees.Exists(item => item.DelegationType == 3);

                //Initialize the Delegees Information Lists
                if (IsSiteDelegate)
                    this.SiteDelegateRoles = delegees.Where(item => item.DelegationType == 1).ToList();

                if (IsDepartmentDelegate)
                    this.DepartmentDelegateRoles = delegees.Where(item => item.DelegationType == 2).ToList();

                if (IsUserDelegate)
                    this.UserDelegateRoles = delegees.Where(item => item.DelegationType == 3).ToList();

                // Decide the general IsDelegee flag value
                IsDelegee = IsUserDelegate || IsDepartmentDelegate || IsSiteDelegate;
            }
        }


        /***
         * Please note that this function depends on the state of the "PrimarySipAccount" variable
         * So, don't call this function before at least initializing these instance variables unless you pass the SipAccount directly
         */
        private void InitializeDepartmentHeadRoles(string userSipAccount = "")
        {
            //This is a mandatory check, because we can't go on with the procedure without a SipAccount!!
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(User.SipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(User.SipAccount))
            {
                userSipAccount = User.SipAccount;
            }

            IsDepartmentHead = Global.DATABASE.DepartmentHeads.IsDepartmentHead(userSipAccount);
        }

        public void AddUserSession(UserSession userSession)
        {
            if (!UsersSessions.Contains(userSession))
            {
                UsersSessions.Add(userSession);
            }
        }

        public void RemoveUserSession(UserSession userSession)
        {
            if (!UsersSessions.Contains(userSession))
            {
                UsersSessions.Remove(userSession);
            }
        }

        // 
        // Initialize the BundledAccounts List
        public void InitializeBundledAccountsList(string userSipAccount)
        {
            //Make sure the parameter was given
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(User.SipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(User.SipAccount))
            {
                userSipAccount = User.SipAccount;
            }

            this.BundledAccountsList = Global.DATABASE.BundledAccounts.GetAssociatedSipAccounts(userSipAccount);
        }

        public void InitializeAllRolesInformation(string userSipAccount)
        {
            //This is a mandatory check, because we can't go on with the procedure without a SipAccount!!
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(User.SipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(User.SipAccount))
            {
                userSipAccount = User.SipAccount;
            }

            SystemRoles = Global.DATABASE.SystemRoles.GetBySipAccount(userSipAccount);

            InitializeSystemRoles(SystemRoles);
            InitializeDelegeesInformation(userSipAccount);
            InitializeDepartmentHeadRoles(userSipAccount);

            var normalUser = Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.UserRoleID);
            ActiveRoleName = (normalUser != null ? normalUser.RoleName : string.Empty);
        }

        //
        // Get the user sipaccount.
        public string GetEffectiveSipAccount()
        {
            var delegeesRoleNames = new List<string>();

            //if the user is a user-delegee return the delegate sipaccount.
            if (delegeesRoleNames.Contains(ActiveRoleName) && DelegeeUserAccount != null)
            {
                return (DelegeeUserAccount.User.SipAccount);
            }
            
            //else then the user is a normal one, just return the normal user sipaccount.
            return (User.SipAccount);
        }

        //
        // Get the user displayname.
        public string GetEffectiveDisplayName()
        {
            var delegeesRoleNames = new List<string>();

            //if the user is a user-delegee return the delegate sipaccount.
            if (delegeesRoleNames.Contains(ActiveRoleName) && DelegeeUserAccount != null)
            {
                return (DelegeeUserAccount.User.DisplayName);
            }

            //else then the user is a normal one, just return the normal user sipaccount.
            return (User.DisplayName);
        }


        //
        // Get the user session phonecalls
        // Handle normal user mode and user delegee mode
        public List<PhoneCall> GetUserSessionPhoneCalls(bool force = false)
        {
            string sipAccount = this.GetEffectiveSipAccount();

            List<string> DelegeesRoleNames = new List<string>()
            {
                Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.UserDelegeeRoleID).RoleName,
                Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.DepartmentDelegeeRoleID).RoleName,
                Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.SiteDelegeeRoleID).RoleName,
            };

            //
            // Delegee Mode
            if (DelegeesRoleNames.Contains(this.ActiveRoleName))
            {
                //Initialize the addressbook if it was not initialized already
                if (this.DelegeeUserAccount.DelegeeUserAddressbook == null || this.DelegeeUserAccount.DelegeeUserAddressbook.Count == 0)
                {
                    this.DelegeeUserAccount.DelegeeUserAddressbook = Global.DATABASE.PhoneBooks.GetAddressBook(sipAccount);
                }

                //Initialize and/or return phonecalls.
                if (this.DelegeeUserAccount.DelegeeUserPhonecalls == null || this.DelegeeUserAccount.DelegeeUserPhonecalls.Count == 0 || force == true)
                {
                    var userPhoneCalls = Global.DATABASE.PhoneCalls.GetChargableCallsPerUser(sipAccount)
                        .Where(item => item.AcIsInvoiced == "NO" || string.IsNullOrEmpty(item.AcIsInvoiced) == true);

                    var addressbook = this.DelegeeUserAccount.DelegeeUserAddressbook;

                    //Skip the adding addressbook contact names to the phonecalls if there are no entries in the addressbook
                    if (addressbook.Count > 0)
                    {
                        foreach (var phoneCall in userPhoneCalls)
                        {
                            if (addressbook.Keys.Contains(phoneCall.DestinationNumberUri))
                            {
                                phoneCall.PhoneBookName = ((PhoneBookContact)addressbook[phoneCall.DestinationNumberUri]).Name;
                            }
                        }
                    }

                    this.DelegeeUserAccount.DelegeeUserPhonecalls = userPhoneCalls.ToList();
                }

                return this.DelegeeUserAccount.DelegeeUserPhonecalls;
            }
            //
            // Normal Users Mode
            else
            {
                //Initialize the addressbook if it was not initialized already
                if (this.Addressbook == null || this.Addressbook.Count == 0)
                {
                    this.Addressbook = Global.DATABASE.PhoneBooks.GetAddressBook(sipAccount);
                }

                //Initialize and/or return phonecalls.
                if (this.Phonecalls == null || this.Phonecalls.Count == 0 || force == true)
                {
                    //var userPhoneCalls = PhoneCalls.GetPhoneCalls(sipAccount).Where(item => item.AC_IsInvoiced == "NO" || item.AC_IsInvoiced == string.Empty || item.AC_IsInvoiced == null);
                    var userPhoneCalls = Global.DATABASE.PhoneCalls.GetChargableCallsPerUser(sipAccount)
                        .Where(item => item.AcIsInvoiced == "NO" || string.IsNullOrEmpty(item.AcIsInvoiced) == true);

                    var addressbook = this.Addressbook;

                    //Skip the adding addressbook contact names to the phonecalls if there are no entries in the addressbook
                    if (addressbook.Count > 0)
                    {
                        foreach (var phoneCall in userPhoneCalls)
                        {
                            if (phoneCall.DestinationNumberUri != null && addressbook.Keys.Contains(phoneCall.DestinationNumberUri))
                            {
                                phoneCall.PhoneBookName = ((PhoneBookContact)addressbook[phoneCall.DestinationNumberUri]).Name;
                            }
                        }
                    }

                    this.Phonecalls = userPhoneCalls.ToList();
                }

                return this.Phonecalls;
            }
        }


        // 
        // Get the user session phonecalls data, such as: phoneCalls list, AddressBook and PhoneCallsPerPage JSON String
        // Handle normal user mode and user delegee mode
        public void FetchSessionPhonecallsAndAddressbookData(out List<PhoneCall> userSessionPhoneCalls, out Dictionary<string, PhoneBookContact> userSessionAddressBook)
        {
            //Initialize the passed varaibles
            userSessionPhoneCalls = new List<PhoneCall>();
            userSessionAddressBook = new Dictionary<string, PhoneBookContact>();

            List<string> DelegeesRoleNames = new List<string>()
            {
                Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.UserDelegeeRoleID).RoleName,
                Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.DepartmentDelegeeRoleID).RoleName,
                Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.SiteDelegeeRoleID).RoleName,
            };

            //This part is off-loaded to another procedure due to size of code
            userSessionPhoneCalls = GetUserSessionPhoneCalls();

            if (DelegeesRoleNames.Contains(this.ActiveRoleName))
            {
                userSessionAddressBook = this.DelegeeUserAccount.DelegeeUserAddressbook;
            }
            else
            {
                userSessionAddressBook = this.Addressbook;
            }
        }


        // 
        // Pass any of the three variables to this function and it will assign it's data to the respective UserSession container
        // The functions respectively stand for the the list of user phonecalls, his/her addressbook contacts, and the phonecalls grid json string
        // This handles the normal user mode and the user delegee mode.
        public void AssignSessionPhonecallsAndAddressbookData(List<PhoneCall> userSessionPhoneCalls, Dictionary<string, PhoneBookContact> userSessionAddressBook)
        {
            List<string> DelegeesRoleNames = new List<string>()
            {
                Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.UserDelegeeRoleID).RoleName,
                Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.DepartmentDelegeeRoleID).RoleName,
                Global.DATABASE.Roles.GetById(Global.DATABASE.Roles.SiteDelegeeRoleID).RoleName,
            };

            if (DelegeesRoleNames.Contains(this.ActiveRoleName))
            {
                if (userSessionPhoneCalls != null && userSessionPhoneCalls.Count > 0)
                    this.DelegeeUserAccount.DelegeeUserPhonecalls = userSessionPhoneCalls;

                if (userSessionAddressBook != null && userSessionAddressBook.Count > 0)
                    this.DelegeeUserAccount.DelegeeUserAddressbook = userSessionAddressBook;
            }
            else
            {
                if (userSessionPhoneCalls != null && userSessionPhoneCalls.Count > 0)
                    this.Phonecalls = userSessionPhoneCalls;

                if (userSessionAddressBook != null && userSessionAddressBook.Count > 0)
                    this.Addressbook = userSessionAddressBook;
            }
        }

    }
}