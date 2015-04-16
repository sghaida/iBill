using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Ext.Net;
using iTextSharp.text;

using CCC.UTILS.Libs;
using LyncBillingUI.Helpers;
using LyncBillingUI.Helpers.Account;
using LyncBillingBase.DataModels;

namespace LyncBillingUI.Pages.SiteAccounting
{
    public partial class MonthlyReport : System.Web.UI.Page
    {
        private string sipAccount = string.Empty;
        private static List<Site> userSites;
        private List<CallsSummaryForUser> listOfUsersCallsSummary = new List<CallsSummaryForUser>();

        // This actually takes a copy of the current session for some uses on the frontend.
        public UserSession CurrentSession { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            // 
            // If the user is not loggedin, redirect to Login page.
            if (HttpContext.Current.Session == null || HttpContext.Current.Session.Contents["UserData"] == null)
            {
                string RedirectTo = String.Format(@"{0}/Site/Accounting/Dashboard", Global.APPLICATION_URL);
                string Url = String.Format(@"{0}/Login?RedirectTo={1}", Global.APPLICATION_URL, RedirectTo);
                Response.Redirect(Url);
            }
            else
            {
                CurrentSession = ((UserSession)HttpContext.Current.Session.Contents["UserData"]);

                if (CurrentSession.ActiveRoleName != Functions.SiteAccountantRoleName)
                {
                    string url = String.Format(@"{0}/Authorize?access={1}", Global.APPLICATION_URL, Functions.SiteAccountantRoleName);
                    Response.Redirect(url);
                }
            }

            sipAccount = CurrentSession.GetEffectiveSipAccount();

            //Get the User Authorized List of Sites
            GetUserSitesData();
        }


        //
        // Get the sites data for this user
        private void GetUserSitesData()
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                userSites = Global.DATABASE.SystemRoles.GetSitesByRoles(CurrentSession.SystemRoles, Functions.SiteAccountantRoleName);
            }
        }


        private List<CallsSummaryForUser> MonthlyReports(int siteID, DateTime date)
        {
            Site site = Global.DATABASE.Sites.GetById(siteID);

            DateTime beginningOfTheMonth = new DateTime(date.Year, date.Month, 1);
            DateTime endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);

            listOfUsersCallsSummary = Global.DATABASE.UsersCallsSummaries.GetBySite(
                    site.Name, 
                    beginningOfTheMonth, 
                    endOfTheMonth, 
                    CCC.ORM.Globals.CallsSummaryForUser.GroupBy.UserAndInvoiceFlag)
                .Where(e => e.PersonalCallsCost > 0 || e.BusinessCallsCost > 0 || e.UnallocatedCallsCost > 0)
                .ToList();

            return listOfUsersCallsSummary;
        }


        private void BindDataToReportsGrid(bool siteHasChanged = false, bool dateHasChanged = false, bool callTypeHasChanged = false)
        {
            int callsType, siteID;
            string siteName = string.Empty;
            List<CallsSummaryForUser> gridData = new List<CallsSummaryForUser>();

            if (listOfUsersCallsSummary.Count == 0 || (siteHasChanged == true || dateHasChanged == true))
            {
                siteID = Convert.ToInt32(FilterReportsBySite.SelectedItem.Value);
                siteName = Global.DATABASE.Sites.GetNameById(siteID);
                listOfUsersCallsSummary = MonthlyReports(siteID, ReportDateField.SelectedDate);
            }

            callsType = Convert.ToInt32(CallsTypesComboBox.SelectedItem.Value);

            if (callsType == 1)
                gridData = listOfUsersCallsSummary.Where(summary => summary.IsInvoiced == "NO").ToList();
            else if (callsType == 2)
                gridData = listOfUsersCallsSummary.Where(summary => summary.IsInvoiced == "N/A").ToList();
            else if (callsType == 3)
                gridData = listOfUsersCallsSummary.Where(summary => summary.IsInvoiced == "YES").ToList();


            MonthlyReportsGrids.GetStore().DataSource = gridData;
            MonthlyReportsGrids.GetStore().LoadData(gridData);
        }


        private void DoInvoice(Site site, int callTypeId, string callTypeDesc, DateTime startingDate, DateTime endingDate)
        {
            Mailer email;
            bool status = false;
            string errorMessage = string.Empty;

            if (callTypeId == 1)
            {
                try
                {
                    Global.DATABASE.PhoneCalls.ChargeAllocatedPhoneCallsOfSite(site.Id, startingDate, endingDate);
                    Global.DATABASE.PhoneCalls.MarkUnallocatedPhoneCallsAsPending(site.Id, startingDate, endingDate);

                    status = true;
                }
                catch (Exception ex)
                {
                    errorMessage = string.Empty;

                    if (!string.IsNullOrEmpty(ex.Message)) { errorMessage += "<b>Exception Message:</b><br/>" + ex.Message + "<br /><br />"; }
                    if (ex.InnerException != null) { errorMessage += "<b>Exception InnerException:</b><br /><br />" + ex.InnerException.ToString() + "<br /><br />"; }
                    if (!string.IsNullOrEmpty(ex.StackTrace)) { errorMessage += "<b>Exception StackTrace:</b><br /><br />" + ex.StackTrace + "<br /><br />"; }

                    status = false;
                }
            }
            else if (callTypeId == 2)
            {
                try
                {
                    Global.DATABASE.PhoneCalls.ChargeUnallocatedPhoneCallsOfSite(site.Id, startingDate, endingDate);

                    status = true;
                }
                catch (Exception ex)
                {
                    errorMessage = string.Empty;

                    if (!string.IsNullOrEmpty(ex.Message)) { errorMessage += "<b>Exception Message:</b><br/>" + ex.Message + "<br /><br />"; }
                    if (ex.InnerException != null) { errorMessage += "<b>Exception InnerException:</b><br /><br />" + ex.InnerException.ToString() + "<br /><br />"; }
                    if (!string.IsNullOrEmpty(ex.StackTrace)) { errorMessage += "<b>Exception StackTrace:</b><br /><br />" + ex.StackTrace + "<br /><br />"; }

                    status = false;
                }
            }


            // Send notification email
            // Get all the users with either of the following: 
            // a> System Developer Role
            // b> Site Accountant Role on this Site

            List<SystemRole> developers = Global.DATABASE.SystemRoles.GetAll().Where(role => role.RoleId == Global.DATABASE.Roles.DeveloperRoleID).ToList();
            List<SystemRole> siteAccountants = Global.DATABASE.SystemRoles.GetAll().Where(role => role.RoleId == Global.DATABASE.Roles.SiteAccountantRoleID && role.SiteId == site.Id).ToList();

            if (status == true)
            {
                List<SystemRole> people = developers.Concat(siteAccountants).ToList();

                foreach (var person in people)
                {
                    email = new Mailer(
                        person.SipAccount,
                        "iBill Invoicing Operation",
                        String.Format("Notification:<br /><br />Kindly note the invoicing operation of \"{0}\" Phone Calls of {1} Site for the period: \"{2} - {3}\" has finished successfully.",
                            callTypeDesc,
                            site.Name,
                            startingDate.ToString(),
                            endingDate.ToString()));
                }
            }
            else
            {
                foreach (var superPerson in developers)
                {
                    email = new Mailer(
                        superPerson.SipAccount,
                        "iBill Invoicing Operation",
                        String.Format("<b>ERROR Notification:</b><br /><br />An error occured while performing the invoicing operation of \"{0}\" Phone Calls of {1} Site for the period: \"{2} - {3}\".<br /><br />The operation was not completed successfully.<br /><br /><hr /><b>Error Information:</b><br /><br />{4}",
                            callTypeDesc,
                            site.Name,
                            startingDate.ToString(),
                            endingDate.ToString(),
                            errorMessage));
                }

                foreach (var person in siteAccountants)
                {
                    email = new Mailer(
                        person.SipAccount,
                        "iBill Invoicing Operation",
                        String.Format("<b>ERROR Notification:</b><br /><br />An error occured while performing the invoicing operation of \"{0}\" Phone Calls of {1} Site for the period: \"{2} - {3}\".<br /><br />The operation was not completed successfully.<br /><br />Kindly contact the HelpDesk.",
                            callTypeDesc,
                            site.Name,
                            startingDate.ToString(),
                            endingDate.ToString()));
                }
            }
        }//end-do-invoice


        protected void SitesStore_Load(object sender, EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                FilterReportsBySite.GetStore().DataSource = userSites;
                FilterReportsBySite.GetStore().DataBind();
            }
        }


        [DirectMethod]
        protected void FilterReportsBySite_Selecting(object sender, DirectEventArgs e)
        {
            if (FilterReportsBySite.SelectedItem.Index != -1)
            {
                ReportDateField.Disabled = false;

                if (ReportDateField.SelectedValue != null)
                {
                    BindDataToReportsGrid(siteHasChanged: true);
                }
            }
            else
            {
                ReportDateField.Disabled = true;
            }
        }


        [DirectMethod]
        protected void ReportDateField_Selection(object sender, DirectEventArgs e)
        {
            if (FilterReportsBySite.SelectedItem.Index > -1)
            {
                CallsTypesComboBox.Disabled = false;
                AdvancedToolsMenu.Disabled = false;

                BindDataToReportsGrid(siteHasChanged: true, dateHasChanged: true);
            }
            else
            {
                CallsTypesComboBox.Disabled = true;
                AdvancedToolsMenu.Disabled = true;
            }
        }


        [DirectMethod]
        protected void FilterReportsByCallsTypes_Select(object sender, DirectEventArgs e)
        {
            int callsType;
            InvoiceUsers.Disabled = true;

            if (CallsTypesComboBox.SelectedItem.Index > -1)
            {
                callsType = Convert.ToInt32(CallsTypesComboBox.SelectedItem.Value);

                BindDataToReportsGrid(callTypeHasChanged: true);

                if (callsType == 1 || callsType == 2)
                {
                    InvoiceUsers.Disabled = false;
                }
            }
        }


        [DirectMethod]
        protected void InvoiceUsers_Button_Click(object sender, DirectEventArgs e)
        {
            X.Msg.Confirm("Message", "Are you sure you want to perform the invoicing operation?<br />Kindly note that this operation cannot be undone!", new MessageBoxButtonsConfig
            {
                Ok = new MessageBoxButtonConfig
                {
                    Handler = "ExtOperationsNamespace.ConfirmDoInvoice()",
                    Text = "Yes!"
                },
                Cancel = new MessageBoxButtonConfig
                {
                    //Handler = "CompanyX.DoNo()",
                    Text = "Cancel"
                }
            }).Show();
        }


        [DirectMethod]
        public void ConfirmDoInvoice()
        {
            // BEGIN PERFORMING THE OPERATION
            int siteID;
            int callTypeId;
            string callTypeDesc;
            DateTime startingDate;
            DateTime endingDate;

            siteID = Convert.ToInt32(FilterReportsBySite.SelectedItem.Value);
            callTypeId = Convert.ToInt32(CallsTypesComboBox.SelectedItem.Value);
            callTypeDesc = Convert.ToString(CallsTypesComboBox.SelectedItem.Text);
            startingDate = new DateTime(ReportDateField.SelectedDate.Year, ReportDateField.SelectedDate.Month, 1);
            endingDate = startingDate.AddMonths(1).AddDays(-1);

            var site = Global.DATABASE.Sites.GetById(siteID);

            if (callTypeId == 1 || callTypeId == 2)
            {
                Thread invoiceProcess = new Thread(() => DoInvoice(site, callTypeId, callTypeDesc, startingDate, endingDate));
                invoiceProcess.Start();
            }


            //Show a information message box
            X.Msg.Show(new MessageBoxConfig
            {
                Title = "Invoice Operation",
                Message = "The operation has started and it might take a while to finish. However, you will be notified via your email when the operation finishes. Feel free to close this window if you wish.",
                Buttons = MessageBox.Button.OK,
                Icon = MessageBox.Icon.INFO
            });
        }


        protected void MonthlyReportsStore_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            DateTime beginningOfTheMonth;
            DateTime endOfTheMonth;
            Document pdfDocument;
            Dictionary<string, string> pdfDocumentHeaders;
            Dictionary<string, Dictionary<string, object>> UsersCollection;
            JavaScriptSerializer jSerializer;
            int SiteID;
            Site SelectedSite;

            //These are created to hold the data submitted through the grid as JSON
            List<LyncBillingBase.DataModels.User> usersData;
            Dictionary<string, object> tempUserDataContainer;

            XmlNode xml = e.Xml;
            string format = this.FormatType.Value.ToString();
            string pdfReportFileName = string.Empty;


            SiteID = Convert.ToInt32(FilterReportsBySite.SelectedItem.Value);
            SelectedSite = Global.DATABASE.Sites.GetById(SiteID);

            int callsType = Convert.ToInt32(CallsTypesComboBox.SelectedItem.Value);
            string callsTypeString = string.Empty;


            // FIRST
            // CLEAR THE PAGE'S RESPONSE
            this.Response.Clear();


            switch (callsType)
            {
                case 1:
                    callsTypeString = "Not Charged";
                    break;
                case 2:
                    callsTypeString = "Pending Charges";
                    break;
                case 3:
                    callsTypeString = "Charged";
                    break;
                default:
                    callsTypeString = "Not Charged";
                    break;
            }


            switch (format)
            {
                // ---------------------------
                // CASE OF XLS SUMMARIZED REPORT
                case "xls":
                    this.Response.Clear();
                    this.Response.ContentType = "application/vnd.ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=MonthlyReport_Summary.xls");
                    XslCompiledTransform xtExcel = new XslCompiledTransform();
                    xtExcel.Load(Server.MapPath("~/Resources/excel.xsl"));
                    xtExcel.Transform(xml, null, Response.OutputStream);

                    break;


                // ---------------------------
                // CASE OF SUMMARIZED PDF REPORT
                case "pdf":
                    UsersCollection = new Dictionary<string, Dictionary<string, object>>();
                    jSerializer = new JavaScriptSerializer();
                    usersData = jSerializer.Deserialize<List<LyncBillingBase.DataModels.User>>(e.Json);

                    foreach (var user in usersData)
                    {
                        tempUserDataContainer = new Dictionary<string, object>();
                        //tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.DisplayName), user.FullName);
                        //tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.EmployeeID), user.EmployeeID);
                        //tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.ChargingParty), user.SipAccount);

                        if (!UsersCollection.Keys.Contains(user.SipAccount))
                            UsersCollection.Add(user.SipAccount, tempUserDataContainer);
                    }

                    beginningOfTheMonth = new DateTime(ReportDateField.SelectedDate.Year, ReportDateField.SelectedDate.Month, 1);
                    endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);

                    //Initialize the response.
                    pdfReportFileName = string.Format(
                        "{0}_Monthly_Summary_Report_{1}_{2}.pdf",
                        SelectedSite.Name.ToUpper(), beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year, callsTypeString.Replace(' ', '_')
                    );

                    pdfDocumentHeaders = new Dictionary<string, string>()
                    {
                        {"siteName", SelectedSite.Name},
                        {"title", "Accounting Monthly Report [Summary]"},
                        {"subTitle", String.Format("As Per: {0}; {1}", beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year, callsTypeString)}
                    };

                    pdfDocument = new Document();

                    //if (callsType == 1)
                    //    Global.DATABASE.UsersCallsSummaries.ExportUsersCallsSummaryToPDF(SelectedSite.Name, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders, notChargedCalls: true);
                    //else if (callsType == 2)
                    //    Global.DATABASE.UsersCallsSummaries.ExportUsersCallsSummaryToPDF(SelectedSite.Name, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders, pendingChargesCalls: true);
                    //else if (callsType == 3)
                    //    Global.DATABASE.UsersCallsSummaries.ExportUsersCallsSummaryToPDF(SelectedSite.Name, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders, chargedCalls: true);

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + pdfReportFileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDocument);

                    break;


                // ---------------------------
                // CASE OF DETAILED PDF REPORT
                case "pdf-d":
                    UsersCollection = new Dictionary<string, Dictionary<string, object>>();
                    jSerializer = new JavaScriptSerializer();
                    usersData = jSerializer.Deserialize<List<LyncBillingBase.DataModels.User>>(e.Json);

                    foreach (var user in usersData)
                    {
                        tempUserDataContainer = new Dictionary<string, object>();
                        //tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.DisplayName), user.FullName);
                        //tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.EmployeeID), user.EmployeeID);
                        //tempUserDataContainer.Add(Enums.GetDescription(Enums.PhoneCallSummary.ChargingParty), user.SipAccount);

                        if (!UsersCollection.Keys.Contains(user.SipAccount))
                            UsersCollection.Add(user.SipAccount, tempUserDataContainer);
                    }

                    beginningOfTheMonth = new DateTime(ReportDateField.SelectedDate.Year, ReportDateField.SelectedDate.Month, 1);
                    endOfTheMonth = beginningOfTheMonth.AddMonths(1).AddDays(-1);

                    //Initialize the response.
                    pdfReportFileName = string.Format(
                        "{0}_Monthly_Detailed_Report_{1}_{2}.pdf",
                        SelectedSite.Name.ToUpper(), beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year, callsTypeString.Replace(' ', '_')
                    );

                    pdfDocumentHeaders = new Dictionary<string, string>()
                    {
                        {"siteName", SelectedSite.Name},
                        {"title", "Accounting Monthly Report [Detailed]"},
                        {"subTitle", String.Format("As Per: {0}; {1}", beginningOfTheMonth.Month + "-" + beginningOfTheMonth.Year, callsTypeString)}
                    };

                    pdfDocument = new Document();

                    //if (callsType == 1)
                    //    Global.DATABASE.UsersCallsSummaries.ExportUsersCallsDetailedToPDF(SelectedSite.Name, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders, notChargedCalls: true);
                    //else if (callsType == 2)
                    //    Global.DATABASE.UsersCallsSummaries.ExportUsersCallsDetailedToPDF(SelectedSite.Name, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders, pendingChargesCalls: true);
                    //else if (callsType == 3)
                    //    Global.DATABASE.UsersCallsSummaries.ExportUsersCallsDetailedToPDF(SelectedSite.Name, beginningOfTheMonth, endOfTheMonth, UsersCollection, Response, out pdfDocument, pdfDocumentHeaders, chargedCalls: true);

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + pdfReportFileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDocument);

                    break;
            }

            this.Response.End();

        }//End-function

    }

}