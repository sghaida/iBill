<%@ Page Title="User Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="LyncBillingUI.Pages.User.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>User Dashboad</h2>
    <br />
    <br />

    <h3>Phone Calls Counts:</h3>
    
    <p>Business:&nbsp;<ext:Label
        ID="BusinessCallsCountLabel"
        runat="server"
        OnLoad="BusinessCallsCountLabel_Load">
    </ext:Label></p>

    <p>Personal:&nbsp;<ext:Label
        ID="PersonalCallsCountLabel"
        runat="server"
        OnLoad="PersonalCallsCountLabel_Load">
    </ext:Label></p>

    <p>Unallocated:&nbsp;<ext:Label
        ID="UnallocatedCallsCountLabel"
        runat="server"
        OnLoad="UnallocatedCallsCountLabel_Load">
    </ext:Label></p>
</asp:Content>
