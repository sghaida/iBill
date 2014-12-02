using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataModels;
using LyncBillingBase.DataAccess;
using LyncBillingBase.DataMappers;

namespace LyncBillingBase.SessionManagement
{
    public class UserSession
    {
        //The delegates roles data mapper - used for data access
        private DelegateRolesDataMapper DelegateRoleAccessor = new DelegateRolesDataMapper();

        private static List<UserSession> usersSessions = new List<UserSession>();
        private List<DelegateRole> userDelegees = new List<DelegateRole>();


        //Normal user data
        public User NormalUserInfo { get; set; }
        public string TelephoneNumber { set; get; }
        public string IPAddress { set; get; }
        public string UserAgent { set; get; }

        //Bundled Accounts List
        public List<string> BundledAccountsList { get; set; }

        //Roles Related
        public string ActiveRoleName { set; get; }
        public List<SystemRole> SystemRoles { set; get; }
        public List<DelegateRole> UserDelegateRoles { get; set; }
        public List<DelegateRole> SiteDelegateRoles { get; set; }
        public List<DelegateRole> DepartmentDelegateRoles { get; set; }

        public DelegeeAccountInfo DelegeeAccount { get; set; }

        //Phone Calls and Phone Book Related
        public string PhonecallsPerPage { set; get; }
        public List<PhoneCall> Phonecalls { set; get; }
        public List<PhoneCall> PhonecallsHistory { set; get; }
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

        public UserSession()
        {
            NormalUserInfo = new User();
            TelephoneNumber = string.Empty;
            IPAddress = string.Empty;
            UserAgent = string.Empty;

            BundledAccountsList = new List<string>();

            ActiveRoleName = string.Empty;
            SystemRoles = new List<SystemRole>();
            DelegeeAccount = null;

            //Initialized other containers
            PhonecallsPerPage = string.Empty;
            Phonecalls = new List<PhoneCall>();
            PhonecallsHistory = new List<PhoneCall>();
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


        private void InitializeSystemRoles(List<SystemRole> SystemRoles = null)
        {
            if (SystemRoles != null && SystemRoles.Count > 0)
            {
                this.SystemRoles = SystemRoles;

                foreach (SystemRole role in SystemRoles)
                {
                    //if (Role.IsDeveloper(role.RoleID)) IsDeveloper = true;
                    //else if (Role.IsSystemAdmin(role.RoleID)) IsSystemAdmin = true;
                    //else if (Role.IsSiteAdmin(role.RoleID)) IsSiteAdmin = true;
                    //else if (Role.IsSiteAccountant(role.RoleID)) IsSiteAccountant = true;
                }
            }
        }


        /***
         * Please note that this function depends on the state of the "PrimarySipAccount" variable
         * So, don't call this function before at least initializing these instance variables unless you pass the SipAccount directly
         */
        private void InitializeDelegeesInformation(string userSipAccount = "")
        {
            //This is a mandatory check, because we can't go on with the procedure without a SipAccount!!
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            else if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                userSipAccount = this.NormalUserInfo.SipAccount;
            }

            //Initialize the Delegees SystemRoles Flags
            //this.IsUserDelegate = DelegateRoleAccessor.IsUserDelegate(userSipAccount);
            //this.IsSiteDelegate = DelegateRoleAccessor.IsSiteDelegate(userSipAccount);
            //this.IsDepartmentDelegate = DelegateRoleAccessor.IsSiteDepartmentDelegate(userSipAccount);

            this.IsDelegee = this.IsUserDelegate || this.IsDepartmentDelegate || this.IsSiteDelegate;

            //Initialize the Delegees Information Lists
            //if (IsUserDelegate)
            //    this.UserDelegateRoles = DelegateRoleAccessor.GetDelegees(userSipAccount, Role.UserDelegeeTypeID);

            //if (IsDepartmentDelegate)
            //    this.DepartmentDelegateRoles = DelegateRoleAccessor.GetDelegees(userSipAccount, Role.DepartmentDelegeeTypeID);

            //if (IsSiteDelegate)
            //    this.SiteDelegateRoles = DelegateRoleAccessor.GetDelegees(userSipAccount, Role.SiteDelegeeTypeID);

        }


        /***
         * Please note that this function depends on the state of the "PrimarySipAccount" variable
         * So, don't call this function before at least initializing these instance variables unless you pass the SipAccount directly
         */
        private void InitializeDepartmentHeadRoles(string userSipAccount = "")
        {
            //This is a mandatory check, because we can't go on with the procedure without a SipAccount!!
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            else if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                userSipAccount = this.NormalUserInfo.SipAccount;
            }

           //IsDepartmentHead = DepartmentHeadRole.IsDepartmentHead(userSipAccount);
        }


        public void AddUserSession(UserSession userSession)
        {
            if (!usersSessions.Contains(userSession))
            {
                usersSessions.Add(userSession);
            }
        }


        public void RemoveUserSession(UserSession userSession)
        {
            if (!usersSessions.Contains(userSession))
            {
                usersSessions.Remove(userSession);
            }
        }


        //Initialize the BundledAccounts List
        public void InitializeBundledAccountsList(string userSipAccount)
        {
            //Make sure the parameter was given
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            else if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                userSipAccount = this.NormalUserInfo.SipAccount;
            }

           // this.BundledAccountsList = BundledAccounts.GetBundledAccountsForUser(userSipAccount);
        }


        public void InitializeAllRolesInformation(string userSipAccount)
        {
            //This is a mandatory check, because we can't go on with the procedure without a SipAccount!!
            if (string.IsNullOrEmpty(userSipAccount) && string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                throw new Exception("No SipAccount was assigned to this session instance!");
            }
            else if (string.IsNullOrEmpty(userSipAccount) && !string.IsNullOrEmpty(this.NormalUserInfo.SipAccount))
            {
                userSipAccount = this.NormalUserInfo.SipAccount;
            }

            //SystemRoles = SystemRole.GetSystemRolesPerSipAccount(userSipAccount);
            InitializeSystemRoles(SystemRoles);
            InitializeDelegeesInformation(userSipAccount);
            InitializeDepartmentHeadRoles(userSipAccount);

            //ActiveRoleName = Enums.GetDescription(Enums.ActiveRoleNames.NormalUser);
        }


        //Get the user sipaccount.
        public string GetEffectiveSipAccount()
        {
            List<string> DelegeesRoleNames = new List<string>()
            {
                //Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee),
                //Enums.GetDescription(Enums.ActiveRoleNames.DepartmentDelegee),
                //Enums.GetDescription(Enums.ActiveRoleNames.SiteDelegee)
            };

            //if the user is a user-delegee return the delegate sipaccount.
            if (DelegeesRoleNames.Contains(this.ActiveRoleName) && this.DelegeeAccount != null)
            {
                return (this.DelegeeAccount.DelegeeUserAccount.SipAccount);
            }
            //else then the user is a normal one, just return the normal user sipaccount.
            else
            {
                return (this.NormalUserInfo.SipAccount);
            }
        }


        //Get the user displayname.
        public string GetEffectiveDisplayName()
        {
            List<string> DelegeesRoleNames = new List<string>()
            {
                //Enums.GetDescription(Enums.ActiveRoleNames.UserDelegee),
                //Enums.GetDescription(Enums.ActiveRoleNames.DepartmentDelegee),
                //Enums.GetDescription(Enums.ActiveRoleNames.SiteDelegee)
            };

            //if the user is a user-delegee return the delegate sipaccount.
            if (DelegeesRoleNames.Contains(this.ActiveRoleName) && this.DelegeeAccount != null)
            {
                return (this.DelegeeAccount.DelegeeUserAccount.DisplayName);
            }
            //else then the user is a normal one, just return the normal user sipaccount.
            else
            {
                return (this.NormalUserInfo.DisplayName);
            }
        }

    }

}
