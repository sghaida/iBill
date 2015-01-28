<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="LyncBillingUI.Error._404" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Page Not Found</h1>

    <br />

    <p>The page you request was not found.</p>
    <p>Go to <a href="~/Default">Homepage</a>?</p>
</asp:Content>
