<%@ Page Title="Accounting Dashboard | iBill" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAccounting.Dashboard" %>


<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    
    <div class="row">
        <div class="col-md-6">
            <h2>Disputed Phone Calls</h2>
            <p>View, accept, and/or reject all the phone calls that were marked as "Disputed" by the users in your sites.</p>
            <p><a class="btn btn-default" href="<%= global_asax.APPLICATION_URL %>/Site/Accounting/DisputedCalls" role="button">Go to page &raquo;</a></p>
        </div>

        <div class="col-md-6">
            <h2>Billing Cycle Notifications</h2>
            <p>This helps you view all the users in any site who still have phone calls that were not charged yet. Send them email notifications to make sure they finalize their phone calls allocation, and warn them of not leaving any phone calls not allocated, if they choose to leave any, they will be charged as personal.</p>
            <p><a class="btn btn-default" href="<%= global_asax.APPLICATION_URL %>/Site/Accounting/BillingCycle" role="button">Go to page &raquo;</a></p>
        </div>
    </div>

    <br />
    <br />

    <div class="row">
        <div class="col-md-6">
            <h2>Monthly Reports</h2>
            <p>Generate either a summary or a detailed report for all the users in your sites, per month. Execute an invoicing operation on any given month. View and filter the reports based on type of phone calls: 1. Not Charged, 2. Pending Charges, 3. Charged Already.</p>
            <p><a class="btn btn-default" href="<%= global_asax.APPLICATION_URL %>/Site/Accounting/MonthlyReports" role="button">Go to page &raquo;</a></p>
        </div>

        <div class="col-md-6">
            <h2>Periodical Reports</h2>
            <p>Provides the same features and functionalities of monthly reports, but supports extended period of time, such as generating a single report for multiple months, or multiple years.</p>
            <p><a class="btn btn-default" href="<%= global_asax.APPLICATION_URL %>/Site/Accounting/PeriodicalReports" role="button">Go to page &raquo;</a></p>
        </div>
    </div>

    <%--<div class="row">
        <div class="col-md-3">
            <h2>Disputed Phone Calls</h2>
            <p>View, accept, and/or reject all the phone calls that were marked as "Disputed" by the users in your sites.</p>
            <p><a class="btn btn-default" href="<%= global_asax.APPLICATION_URL %>/Site/Accounting/DisputedCalls" role="button">Go to page &raquo;</a></p>
        </div>

        <div class="col-md-3">
            <h2>Billing Cycle Notifications</h2>
            <p>This helps you view all the users in any site who still have phone calls that were not charged yet. Send them email notifications to make sure they finalize their phone calls allocation, and warn them of not leaving any phone calls not allocated, if they choose to leave any, they will be charged as personal.</p>
            <p><a class="btn btn-default" href="<%= global_asax.APPLICATION_URL %>/Site/Accounting/BillingCycle" role="button">Go to page &raquo;</a></p>
        </div>

        <div class="col-md-3">
            <h2>Monthly Reports</h2>
            <p>Generate either a summary or a detailed report for all the users in your sites, per month. Execute an invoicing operation on any given month. View and filter the reports based on type of phone calls: 1. Not Charged, 2. Pending Charges, 3. Charged Already.</p>
            <p><a class="btn btn-default" href="<%= global_asax.APPLICATION_URL %>/Site/Accounting/MonthlyReports" role="button">Go to page &raquo;</a></p>
        </div>

        <div class="col-md-3">
            <h2>Periodical Reports</h2>
            <p>Provides the same features and functionalities of monthly reports, but supports extended period of time, such as generating a single report for multiple months, or multiple years.</p>
            <p><a class="btn btn-default" href="<%= global_asax.APPLICATION_URL %>/Site/Accounting/PeriodicalReports" role="button">Go to page &raquo;</a></p>
        </div>
    </div>--%>

    <br />
</asp:Content>


<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
</asp:Content>
