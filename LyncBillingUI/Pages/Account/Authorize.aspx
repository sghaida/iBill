<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Authorize.aspx.cs" Inherits="LyncBillingUI.Pages.Account.Authorize" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="placeholding-input username">
                <asp:label ID="Email_Label" 
                    Text="Email"  
                    CssClass="placeholder" 
                    runat="server"
                    Width="70">
                </asp:label>

				<label id="email" class="bold">
                    <%= sipAccount %>
				</label>
			</div>

			<div class="placeholding-input password">
            	<asp:label ID="Label2" 
                    Text="Password"  
                    CssClass="placeholder" 
                    runat="server"
                    Width="70">
                </asp:label>

                <asp:TextBox 
                    id="password" 
                    runat="server" 
                    TextMode="Password" 
                    Width="180"
                    tabindex="2">
                </asp:TextBox>
            </div>

            <div class="placeholding-input">
                <asp:Button 
                    ID="signin_submit" 
                    runat="server" 
                    Text="Log In" 
                    OnClick="AuthenticateUser" />
			</div>

            <div class="auth-msg red-color"><%= AuthenticationMessage.ToString() %></div>
        </div>
    </div>

    <asp:HiddenField ID="redirect_to_url" runat="server" />
    <asp:HiddenField ID="DELEGEE_IDENTITY" runat="server" />
    <asp:HiddenField ID="ACCESS_LEVEL_FIELD" runat="server" />
</asp:Content>
