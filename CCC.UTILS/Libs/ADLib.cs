using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace CCC.UTILS.Libs
{
    public class ADLib
    {
        private static readonly string ADSearchFilter = ConfigurationManager.AppSettings["ADSearchFilter"];
        //INIT RESOURCE SEARCHER
        private readonly DirectorySearcher resourceSearcher = new DirectorySearcher(forestResource);
        //INIT LOCAL SEARCHER
        private DirectorySearcher localSearcher = new DirectorySearcher(forestlocal);

        /// <summary>
        ///     Get Lync Server Pools FQDN
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ADUserInfo setLyncPool(ADUserInfo userInfo)
        {
            var poolFilter = string.Format(@"(distinguishedName={0})", userInfo.PrimaryHomeServerDN);
            resourceSearcher.Filter = poolFilter;
            var resourceForestPoolResult = resourceSearcher.FindOne();

            if ((string) resourceForestPoolResult.Properties["dnshostname"][0] != null)
                userInfo.PoolName = (string) resourceForestPoolResult.Properties["dnshostname"][0];

            return userInfo;
        }

        /// <summary>
        ///     Authenticate user
        /// </summary>
        /// <param name="EmailAddress">Email Address </param>
        /// <param name="password">Domain Controller Password</param>
        /// <returns></returns>
        public bool AuthenticateUser(string EmailAddress, string password, out string msg)
        {
            msg = string.Empty;

            if (password == null || password == string.Empty || EmailAddress == null || EmailAddress == string.Empty)
            {
                msg = "Email and/or password can't be empty!";
                return false;
            }

            try
            {
                var userInfo = GetUserAttributes(EmailAddress);

                if (userInfo == null)
                {
                    msg = "Error: Couldn't fetch user information!";
                    return false;
                }
                var directoryEntry = new DirectoryEntry(LocalGCUri, userInfo.Upn, password);
                directoryEntry.AuthenticationType = AuthenticationTypes.None;
                var localFilter = string.Format(ADSearchFilter, EmailAddress);


                var localSearcher = new DirectorySearcher(directoryEntry);
                localSearcher.PropertiesToLoad.Add("mail");
                localSearcher.Filter = localFilter;

                var result = localSearcher.FindOne();


                if (result != null)
                {
                    msg = "You have logged in successfully!";
                    return true;
                }
                msg = "Login failed, please try again.";
                return false;
            }
            catch (Exception)
            {
                //System.ArgumentException argEx = new System.ArgumentException("Logon failure: unknown user name or bad password");
                //throw argEx;
                msg = "Wrong Email and/or Password!";
                return false;
            }
        }

        /// <summary>
        ///     Get a list of all Domain Controllers IP Address and DNS Suffixes
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetLocalDomainController()
        {
            try
            {
                var domainControllerInfo = new Dictionary<string, string>();
                var obj = Forest.GetCurrentForest();
                var collection = obj.Domains;
                foreach (Domain domain in collection)
                {
                    var domainControllers = domain.FindAllDiscoverableDomainControllers();
                    foreach (DomainController domainController in domainControllers)
                    {
                        if (!domainControllerInfo.ContainsKey(domain.Name))
                            domainControllerInfo.Add(domain.Name, domainController.IPAddress);
                    }
                }
                return domainControllerInfo;
            }
            catch (Exception ex)
            {
                var argEx = new ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
        }

        /// <summary>
        ///     Get All Users Related Attributes from Active Directory by Quering two forests
        ///     1. for Users Related Information
        ///     2. Sip Related Information
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <returns></returns>
        public ADUserInfo GetUserAttributes(string mailAddress)
        {
            var userInfo = new ADUserInfo();

            var resourceFilter = string.Format(ADSearchFilter, mailAddress);

            resourceSearcher.Filter = resourceFilter;

            try
            {
                var resourceForestResult = resourceSearcher.FindOne();

                if (resourceForestResult != null)
                {
                    if (resourceForestResult.Properties.Contains("title"))
                        userInfo.Title = (string) resourceForestResult.Properties["title"][0];

                    if (resourceForestResult.Properties.Contains("givenName"))
                        userInfo.FirstName = (string) resourceForestResult.Properties["givenName"][0];

                    if (resourceForestResult.Properties.Contains("sn"))
                        userInfo.LastName = (string) resourceForestResult.Properties["sn"][0];

                    if (resourceForestResult.Properties.Contains("cn"))
                        userInfo.DisplayName = (string) resourceForestResult.Properties["cn"][0];

                    if (resourceForestResult.Properties.Contains("samaccountname"))
                        userInfo.SamAccountName = (string) resourceForestResult.Properties["samaccountname"][0];

                    if (resourceForestResult.Properties.Contains("department"))
                        userInfo.department = (string) resourceForestResult.Properties["department"][0];

                    if (resourceForestResult.Properties.Contains("mail"))
                        userInfo.EmailAddress = (string) resourceForestResult.Properties["mail"][0];

                    if (resourceForestResult.Properties.Contains("employeeid"))
                        userInfo.EmployeeID = (string) resourceForestResult.Properties["employeeid"][0];

                    if (resourceForestResult.Properties.Contains("telephonenumber"))
                        userInfo.BusinessPhone = (string) resourceForestResult.Properties["telephonenumber"][0];

                    if (resourceForestResult.Properties.Contains("physicalDeliveryOfficeName"))
                        userInfo.physicalDeliveryOfficeName =
                            (string) resourceForestResult.Properties["physicalDeliveryOfficeName"][0];

                    if (resourceForestResult.Properties.Contains("msrtcsip-primaryuseraddress"))
                        userInfo.SipAccount = (string) resourceForestResult.Properties["msrtcsip-primaryuseraddress"][0];

                    if (resourceForestResult.Properties.Contains("msrtcsip-line"))
                        userInfo.Telephone = (string) resourceForestResult.Properties["msrtcsip-line"][0];

                    if (resourceForestResult.Properties.Contains("msrtcsip-primaryhomeserver"))
                        userInfo.PrimaryHomeServerDN =
                            ((string) resourceForestResult.Properties["msrtcsip-primaryhomeserver"][0]).Replace(
                                "CN=Lc Services,CN=Microsoft,", "");

                    //AUTHENTICATION
                    if (resourceForestResult.Properties.Contains("extensionAttribute3"))
                        userInfo.Upn = (string) resourceForestResult.Properties["extensionAttribute3"][0];
                    else if (resourceForestResult.Properties.Contains("userprincipalname"))
                        userInfo.Upn = (string) resourceForestResult.Properties["userprincipalname"][0];

                    return userInfo;
                }
                return null;
            }
            catch (Exception ex)
            {
                var argEx = new ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
        }

        public List<ADUserInfo> GetSipAccounts(string fullName)
        {
            var resourceFilter = string.Format("(&(objectClass=user)(objectCategory=person)(cn={0}))", fullName);

            resourceSearcher.Filter = resourceFilter;

            var listOfUsers = new List<ADUserInfo>();
            ADUserInfo userInfo;

            try
            {
                var resourceForestResult = resourceSearcher.FindAll();

                foreach (SearchResult result in resourceForestResult)
                {
                    userInfo = new ADUserInfo();

                    if (result.Properties.Contains("mail"))
                        userInfo.EmailAddress = (string) result.Properties["mail"][0];

                    if (result.Properties.Contains("cn"))
                        userInfo.DisplayName = (string) result.Properties["cn"][0];

                    if (result.Properties.Contains("employeeid"))
                        userInfo.EmployeeID = (string) result.Properties["employeeid"][0];

                    listOfUsers.Add(userInfo);
                }

                return listOfUsers;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     Get Users Attributes From Phone Number
        /// </summary>
        /// <param name="phoneNumber">Business Phone Number</param>
        /// <returns>ADUserInfo Object</returns>
        public ADUserInfo getUsersAttributesFromPhone(string phoneNumber)
        {
            var userInfo = new ADUserInfo();

            var searchFilter = "(&(objectClass=user)(objectCategory=person)(msrtcsip-line=Tel:{0}))";
            var resourceFilter = string.Format(searchFilter, phoneNumber);

            resourceSearcher.Filter = resourceFilter;

            try
            {
                var resourceForestResult = resourceSearcher.FindOne();

                if (resourceForestResult != null)
                {
                    userInfo.Title = (string) resourceForestResult.Properties["title"][0];
                    userInfo.FirstName = (string) resourceForestResult.Properties["givenName"][0];
                    userInfo.LastName = (string) resourceForestResult.Properties["sn"][0];
                    userInfo.DisplayName = (string) resourceForestResult.Properties["cn"][0];
                    userInfo.Telephone = (string) resourceForestResult.Properties["msrtcsip-line"][0];
                }
                return userInfo;
            }
            catch (Exception ex)
            {
                var argEx = new ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
        }

        /// <summary>
        ///     Get AD Domain NetBios Name
        /// </summary>
        /// <param name="dnsDomainName">DNS Suffix Name</param>
        /// <returns>Domain NetBios Name</returns>
        public string GetNetbiosDomainName(string dnsDomainName)
        {
            var netbiosDomainName = string.Empty;

            try
            {
                var rootDSE = new DirectoryEntry(string.Format("LDAP://{0}/RootDSE", dnsDomainName));

                var configurationNamingContext = rootDSE.Properties["configurationNamingContext"][0].ToString();

                var searchRoot = new DirectoryEntry("LDAP://cn=Partitions," + configurationNamingContext);

                var searcher = new DirectorySearcher(searchRoot);
                searcher.SearchScope = SearchScope.OneLevel;
                // searcher.PropertiesToLoad.Add("netbiosname");
                searcher.Filter = string.Format("(&(objectcategory=Crossref)(dnsRoot={0})(netBIOSName=*))",
                    dnsDomainName);

                var result = searcher.FindOne();

                if (result != null)
                {
                    netbiosDomainName = result.Properties["netbiosname"][0].ToString();
                }

                return netbiosDomainName;
            }
            catch (Exception ex)
            {
                var argEx = new ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
        }

        /// <summary>
        ///     Get DNS Name from AD Netbios Name
        /// </summary>
        /// <param name="netBiosName">AD Netbios Name</param>
        /// <returns>Domain FQDN</returns>
        public string GetFqdnFromNetBiosName(string netBiosName)
        {
            var FQDN = string.Empty;

            try
            {
                var rootDSE = new DirectoryEntry(string.Format("LDAP://{0}/RootDSE", netBiosName));

                var configurationNamingContext = rootDSE.Properties["configurationNamingContext"][0].ToString();

                var searchRoot = new DirectoryEntry("LDAP://cn=Partitions," + configurationNamingContext);

                var searcher = new DirectorySearcher(searchRoot);
                searcher.SearchScope = SearchScope.OneLevel;
                //searcher.PropertiesToLoad.Add("dnsroot");
                searcher.Filter = string.Format("(&(objectcategory=Crossref)(netbiosname={0}))", netBiosName);

                var result = searcher.FindOne();
                if (result != null)
                    FQDN = result.Properties["dnsroot"][0].ToString();

                return FQDN;
            }
            catch (Exception ex)
            {
                var argEx = new ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
        }

        public List<string> GetAllUsers()
        {
            var users = new List<string>();

            resourceSearcher.SizeLimit = 10000;
            resourceSearcher.PageSize = 250;

            var localFilter =
                string.Format(
                    @"(&(objectClass=user)(objectCategory=person)(!(objectClass=contact))(msRTCSIP-PrimaryUserAddress=*))");

            resourceSearcher.Filter = localFilter;

            SearchResultCollection resourceForestResult;

            try
            {
                resourceForestResult = resourceSearcher.FindAll();

                if (resourceForestResult != null)
                {
                    foreach (SearchResult result in resourceForestResult)
                    {
                        if (result.Properties.Contains("mail"))
                            users.Add((string) result.Properties["mail"][0]);
                    }
                }
            }
            catch (Exception)
            {
            }

            return users;
        }

        //WEB.CONF AD RELATED FIELDS
        private static readonly string LocalGCUri = ConfigurationManager.AppSettings["LocalDomainURI"];
        private static readonly string LocalGCUsername = ConfigurationManager.AppSettings["LocalDomainUser"];
        private static readonly string LocalGCPassword = ConfigurationManager.AppSettings["LocalDomainPassword"];
        private static readonly string ResourceGCUri = ConfigurationManager.AppSettings["ResourceDomainURI"];
        private static readonly string ResourceGCUsername = ConfigurationManager.AppSettings["ResourceDomainUser"];
        private static readonly string ResourceGCPassword = ConfigurationManager.AppSettings["ResourceDomainPassword"];
        //INIT LOCAL GC
        private static readonly DirectoryEntry forestResource = new DirectoryEntry(ResourceGCUri, ResourceGCUsername,
            ResourceGCPassword);

        //INIT RESOURCE GC
        private static readonly DirectoryEntry forestlocal = new DirectoryEntry(LocalGCUri, LocalGCUsername,
            LocalGCPassword);
    }

    public class ADUserInfo
    {
        public string Title { set; get; }
        public string DisplayName { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string SamAccountName { set; get; }
        public string Upn { set; get; }
        public string EmailAddress { set; get; }
        public string EmployeeID { set; get; }
        public string Department { set; get; }
        public string BusinessPhone { get; set; }
        public string department { set; get; }
        public string Telephone { get; set; }
        public string SipAccount { set; get; }
        public string PrimaryHomeServerDN { get; set; }
        public string PoolName { set; get; }
        public string physicalDeliveryOfficeName { set; get; }
    }
}