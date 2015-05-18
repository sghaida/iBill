<%@ Page Title="Authorization" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Authorize.aspx.cs" Inherits="LyncBillingUI.Pages.Account.Authorize" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />

    <div class="row">
        <div class="col-md-8">
            <h3>You have requested to elevate your access.</h3>
            <h3>Kindly provide your credentials for access-role authorization.</h3>
        </div>

        <div class="col-md-4">
            <div class="well">
                <div class="input-group">
                    <span class="input-group-addon">Email</span>

				    <label id="email" class="form-control bold">
                        <%= sipAccount %>
				    </label>
			    </div>

                <br />

			    <div class="input-group">
            	    <span class="input-group-addon">Password</span>

                    <asp:TextBox 
                        ID="password" 
                        runat="server" 
                        TextMode="Password" 
                        TabIndex="0"
                        CssClass="form-control">
                    </asp:TextBox>
                </div>

                <br />

                <asp:Button 
                    ID="signin_submit" 
                    runat="server" 
                    Text="Log In" 
                    CssClass="btn btn-default"
                    OnClick="AuthenticateUser" />

                <div class="auth-msg red-color"><%= AuthenticationMessage.ToString() %></div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="redirect_to_url" runat="server" />
    <asp:HiddenField ID="DELEGEE_IDENTITY" runat="server" />
    <asp:HiddenField ID="ACCESS_LEVEL_FIELD" runat="server" />
</asp:Content>
