using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CCC.UTILS.Libs;
using CCC.UTILS.Helpers;
using LyncBillingBase.DataModels;
using LyncBillingUI.Account;

namespace LyncBillingUI.Pages.Account
{
    public partial class Login : System.Web.UI.Page
    {
        public AdLib ADConnector = new AdLib();
        public string AuthenticationMessage { get; set; }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            //If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session != null && HttpContext.Current.Session.Contents["UserData"] != null)
            {
                Response.Redirect(String.Format(@"{0}/User/Dashboard", Global.APPLICATION_URL));
            }

            //Check if a RedirectTo value has been passed and validate it's link
            if (Request.QueryString["RedirectTo"] != null && !string.IsNullOrEmpty(Request.QueryString["RedirectTo"].ToString()))
            {
                this.RedirectToUrl.Value = Request.QueryString["RedirectTo"];
            }

            AuthenticationMessage = string.Empty;
        }


        /// <summary>
        /// Session managemenet routine. This is called from the SignButton_Click procedure.
        /// </summary>
        /// <param name="session">The current user session, sent by reference.</param>
        /// <param name="userInfo">The current user info</param>
        private void SetUserSessionFields(ref UserSession session, AdUserInfo userInfo)
        {
            //First and foremost initialize the user's most basic and mandatory fields
            session.User = Global.DATABASE.Users.GetBySipAccount(userInfo.SipAccount.Replace("sip:", ""));
            session.User.DisplayName = HelperFunctions.FormatUserDisplayName(userInfo.DisplayName, userInfo.SipAccount);

            session.DelegeeUserAccount = null;

            //Initialize his/her ROLES AND THEN DELEGEES information
            session.InitializeAllRolesInformation(session.User.SipAccount);

            //Initialize the Bundled Accounts List for the user
            session.InitializeBundledAccountsList(session.User.SipAccount);

            //Lastly, get some additional information about the user.
            session.IpAddress = HttpContext.Current.Request.UserHostAddress;
            session.UserAgent = HttpContext.Current.Request.UserAgent;
        }


        protected void LogIn(object sender, EventArgs e)
        {
            UserSession session = new UserSession();
            AdUserInfo userInfo = new AdUserInfo();
            List<SystemRole> userSystemRoles = new List<SystemRole>();
            LyncBillingBase.DataModels.User existingiBillUser;
            LyncBillingBase.DataModels.User iBillUser = new LyncBillingBase.DataModels.User();

            //START
            bool status = false;
            string msg = string.Empty;

            if (IsValid)
            {
                status = ADConnector.AuthenticateUser(Email.Text, Password.Text, out msg);
                AuthenticationMessage = msg;

                // Impersonation example
                // email.Text = "user@ccc-domain.xyz";
                // status = true;

                if (status == true)
                {
                    userInfo = ADConnector.GetUserAttributes(Email.Text);

                    // Users Information was found in active directory
                    if (userInfo != null)
                    {
                        //Try to get user from the database
                        existingiBillUser = Global.DATABASE.Users.GetBySipAccount(userInfo.SipAccount.Replace("sip:", ""));

                        //Update the user, if exists and if his/her info has changed... Insert te Users if s/he doesn't exist
                        if (existingiBillUser != null)
                        {
                            //Make sure the user record was updated by ActiveDirectory and not by the System Admin
                            //If the system admin has updated this user then you cannot update his record from Active Directory
                            if (existingiBillUser.UpdatedByAd == Convert.ToByte(true))
                            {

                                //If user information from Active directory doesnt match the one in Users Table : update user table 
                                if (existingiBillUser.EmployeeId.ToString() != userInfo.EmployeeId ||
                                    existingiBillUser.FullName != String.Format("{0} {1}", userInfo.FirstName, userInfo.LastName) ||
                                    existingiBillUser.SiteName != userInfo.PhysicalDeliveryOfficeName ||
                                    existingiBillUser.DepartmentName != userInfo.department ||
                                    existingiBillUser.TelephoneNumber != HelperFunctions.FormatUserTelephoneNumber(userInfo.Telephone))
                                {
                                    int employeeID = 0;

                                    // Validate employeeID if it could be parsed as integer or not
                                    bool result = Int32.TryParse(userInfo.EmployeeId, out employeeID);

                                    if (result)
                                        iBillUser.EmployeeId = employeeID;
                                    else
                                        iBillUser.EmployeeId = 0;

                                    iBillUser.SipAccount = userInfo.SipAccount.Replace("sip:", "");
                                    iBillUser.FullName = String.Format("{0} {1}", userInfo.FirstName, userInfo.LastName);
                                    iBillUser.TelephoneNumber = HelperFunctions.FormatUserTelephoneNumber(userInfo.Telephone);
                                    iBillUser.DepartmentName = userInfo.department;
                                    iBillUser.SiteName = userInfo.PhysicalDeliveryOfficeName;
                                    iBillUser.UpdatedByAd = Convert.ToByte(true);

                                    Global.DATABASE.Users.Update(iBillUser);
                                }
                            }
                        }
                        else
                        {
                            // If user not found in Users tables that means this is his first login : insert his information into Users table
                            int employeeID = 0;

                            bool result = Int32.TryParse(userInfo.EmployeeId, out employeeID);

                            if (result)
                                iBillUser.EmployeeId = employeeID;
                            else
                                iBillUser.EmployeeId = 0;

                            iBillUser.SipAccount = userInfo.SipAccount.Replace("sip:", "");
                            iBillUser.FullName = String.Format("{0} {1}", userInfo.FirstName, userInfo.LastName);
                            iBillUser.TelephoneNumber = HelperFunctions.FormatUserTelephoneNumber(userInfo.Telephone);
                            iBillUser.DepartmentName = userInfo.department;
                            iBillUser.SiteName = userInfo.PhysicalDeliveryOfficeName;
                            iBillUser.UpdatedByAd = Convert.ToByte(true);

                            Global.DATABASE.Users.Insert(iBillUser);
                        }

                        //
                        //Assign the current userInfo to the UserSession fields.
                        SetUserSessionFields(ref session, userInfo);

                        //
                        // Encrypt the password and assign it to the session
                        session.EncryptedPassword = Global.ENCRYPTION.EncryptRijndael(Password.Text);

                        Session.Add("UserData", session);

                        if (this.RedirectToUrl != null && !string.IsNullOrEmpty(this.RedirectToUrl.Value))
                        {
                            Response.Redirect(this.RedirectToUrl.Value);
                        }
                        else
                        {
                            Response.Redirect(String.Format(@"{0}/User/Dashboard", Global.APPLICATION_URL));
                        }
                    }//end-if-userInfo-noteq-null

                }//end-if-status-is-true

                if (AuthenticationMessage.ToString() != string.Empty)
                {
                    AuthenticationMessage = "* " + AuthenticationMessage;
                }

            }//end-if-valid

        }//end-function

    }

}