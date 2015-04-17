<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LyncBillingUI.Pages.Account.Login" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Welcome!</h1>
        <p>iBill is the centralized telephony management service. Kindly login to manage your phone calls, bills, view your telephony history and usage statistics.</p>
    </div>

    <br />

    <div class="row">
        <div class="col-md-6">
            <asp:HiddenField ID="RedirectToUrl" runat="server" />

            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                <p class="text-danger">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
            </asp:PlaceHolder>

            <br />

            <div class="input-group">
                <span class="input-group-addon">Email</span>
                <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
            </div>

            <br />

            <div class="input-group">
                <span class="input-group-addon">Password</span>
                <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
            </div>

            <br />

            <div class="check">
                <asp:CheckBox runat="server" ID="RememberMe" CssClass="" />
                <asp:Label runat="server" AssociatedControlID="RememberMe" Text="Remember Me?"></asp:Label>
            </div>

            <br />
            <br />

            <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-info" />
        </div>

        <div class="col-md-6">
            <h5><%= AuthenticationMessage.ToString() %></h5>
        </div>
    </div>
</asp:Content>
