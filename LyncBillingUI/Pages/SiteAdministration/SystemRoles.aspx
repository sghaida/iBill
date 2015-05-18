<%@ Page Title="System Roles" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SystemRoles.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAdministration.SystemRoles" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
    <style>
        /* start manage-phone-calls grid styling */
        .x-grid-with-row-lines .x-grid-cell { height: 25px !important; }
        /* end manage-phone-calls grid styling */

        /* start users search query result styling */
        .search-item {
            font          : normal 11px tahoma, arial, helvetica, sans-serif;
            padding       : 3px 10px 3px 10px;
            border        : 1px solid #fff;
            border-bottom : 1px solid #eeeeee;
            white-space   : normal;
            color         : #555;
        }
        
        .search-item h3 {
            display     : block;
            font        : inherit;
            font-weight : bold;
            color       : #222;
            margin      : 0px;
        }

        .search-item h3 span {
            float       : right;
            font-weight : normal;
            margin      : 0 0 5px 5px;
            width       : 185px;
            display     : block;
            clear       : none;
        } 
        
        p { width: 650px; }
        
        .ext-ie .x-form-text { position : static !important; }
        /* end users search query result styling */
    </style>
</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <ext:GridPanel
                ID="ManageSystemRolesGrid"
                runat="server"
                MaxWidth="955"
                MinHeight="620"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage Users Roles">

                <Store>
                    <ext:Store
                        ID="ManageSystemRolesStore"
                        runat="server"
                        RemoteSort="true"
                        IsPagingStore="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ManageSystemRolesStoreModel" runat="server" IDProperty="Id">
                                <Fields>
                                    <ext:ModelField Name="Id" Type="Int" />
                                    <ext:ModelField Name="RoleId" Type="Int" />
                                    <ext:ModelField Name="Description" Type="String" />
                                    <ext:ModelField Name="SipAccount" Type="String" />
                                    <ext:ModelField Name="SiteId" Type="Int" />
                                    <ext:ModelField Name="RoleOwnerName" Type="String" />
                                    <ext:ModelField Name="SiteName" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="ManageSystemRolesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column ID="RoleOwnerCol"
                            runat="server"
                            Text="Role Owner"
                            Sortable="false"
                            Groupable="false">
                            <Columns>
                                <ext:Column ID="SipAccountCol"
                                    runat="server"
                                    Text="Email"
                                    Width="200"
                                    DataIndex="SipAccount"
                                    Sortable="false"
                                    Groupable="false">
                                    <%--<HeaderItems>
                                        <ext:TextField ID="RoleOwnerNameFilter" runat="server" Icon="Magnifier">
                                            <Listeners>
                                                <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                            </Listeners>
                                            <Plugins>
                                                <ext:ClearButton ID="ClearSipAccountFilterBtn" runat="server" />
                                            </Plugins>
                                        </ext:TextField>
                                    </HeaderItems>--%>
                                </ext:Column>

                                <ext:Column ID="Column1"
                                    runat="server"
                                    Text="Name"
                                    Width="200"
                                    DataIndex="RoleOwnerName"
                                    Sortable="false"
                                    Groupable="false">
                                    <%--<HeaderItems>
                                        <ext:TextField ID="RoleOwnerNameFilter" runat="server" Icon="Magnifier">
                                            <Listeners>
                                                <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                            </Listeners>
                                            <Plugins>
                                                <ext:ClearButton ID="ClearSipAccountFilterBtn" runat="server" />
                                            </Plugins>
                                        </ext:TextField>
                                    </HeaderItems>--%>
                                </ext:Column>
                            </Columns>
                        </ext:Column>

                        <ext:Column ID="RoleDescriptionCol"
                            runat="server"
                            Text="Role Description"
                            Width="250"
                            DataIndex="Description"
                            Sortable="false"
                            Groupable="false">
                            <%--<HeaderItems>
                                <ext:TextField ID="RoleDescriptionFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearButton1" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column ID="SiteNameCol"
                            runat="server"
                            Text="Effective On Site"
                            Width="200"
                            DataIndex="SiteName"
                            Sortable="false"
                            Groupable="false">
                            <%--<HeaderItems>
                                <ext:TextField ID="SiteNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearButton2" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:ImageCommandColumn
                            ID="DeleteButtonsColumn"
                            runat="server"
                            Width="30"
                            Align="Center"
                            Sortable="false"
                            Groupable="false">
                            <Commands>
                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Delete Delegate" CommandName="delete">                            
                                </ext:ImageCommand>
                            </Commands>

                            <Listeners>
                                <Command Handler="this.up(#{ManageSystemRolesGrid}.store.removeAt(recordIndex));" />
                            </Listeners>
                        </ext:ImageCommandColumn>

                    </Columns>
                </ColumnModel>

                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                </SelectionModel>
                
                <TopBar>
                    <ext:Toolbar ID="FilterToolBar" runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterSystemRolesBySite"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Local"
                                Editable="true"
                                DisplayField="Name"
                                ValueField="Id"
                                FieldLabel="Site"
                                LabelWidth="25"
                                MarginSpec="5 5 0 5"
                                Width="250">
                                <Store>
                                    <ext:Store
                                        ID="FilterSystemRolesBySiteStore"
                                        runat="server"
                                        OnLoad="FilterSystemRolesBySiteStore_Load">
                                        <Model>
                                            <ext:Model ID="SiteModel" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="Id" />
                                                    <ext:ModelField Name="Name" />
                                                    <ext:ModelField Name="CountryCode" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>

                                        <Sorters>
                                            <ext:DataSorter Property="Name" Direction="ASC" />
                                        </Sorters>
                                    </ext:Store>
                                </Store>

                                <ListConfig>
                                    <ItemTpl ID="SitesItemTpl" runat="server">
                                        <Html><div>{Name}&nbsp;({CountryCode})</div></Html>
                                    </ItemTpl>
                                </ListConfig>

                                <DirectEvents>
                                    <Select OnEvent="GetSystemRolesPerSite" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <%--<Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>--%>
                            </ext:ComboBox>

                            <ext:Button
                                ID="AddRecordButton"
                                runat="server"
                                Text="Add New"
                                Icon="Add"
                                MarginSpec="5 5 5 500">
                                <DirectEvents>
                                    <Click OnEvent="ShowAddSystemRoleWindowPanel" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:ToolbarSeparator runat="server" />

                            <ext:Button
                                ID="SaveChangesButton"
                                runat="server"
                                Text="Save Changes"
                                Icon="DatabaseSave"
                                MarginSpec="5 5 5 5">
                                <DirectEvents>
                                    <Click OnEvent="SaveChanges_DirectEvent" before="return #{ManageSystemRolesStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageSystemRolesStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageSystemRolesPagingToolbar"
                        runat="server"
                        StoreID="ManageSystemRolesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="System Roles {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>


            <ext:Window 
                ID="AddNewSystemRoleWindowPanel" 
                runat="server" 
                Frame="true"
                Resizable="false"
                Title="New System Role" 
                Hidden="true"
                Width="410"
                Icon="Add" 
                X="600"
                Y="250"
                BodyStyle="background-color: #f9f9f9;">
                                
                <Defaults>
                    <ext:Parameter Name="Padding" Value="10" Mode="Raw" />
                </Defaults>

                <Items>
                    <ext:ComboBox
                        ID="NewSystemRole_RoleTypeCombobox"
                        runat="server"
                        DisplayField="TypeName"
                        ValueField="TypeValue"
                        Width="380"
                        FieldLabel="Role Type"
                        LabelWidth="70">
                        <Items>
                            <ext:ListItem Text="Site Administrator" Value="30" />
                            <ext:ListItem Text="Site Accountant" Value="40" />
                        </Items>

                        <SelectedItems>
                            <ext:ListItem Text="Site Administrator" Value="30" />
                        </SelectedItems>
                    </ext:ComboBox>

                    <ext:ComboBox
                        ID="NewSystemRole_UserSipAccount"
                        runat="server"
                        Icon="Find"
                        TriggerAction="Query"
                        QueryMode="Remote"
                        Editable="true"
                        DisplayField="SipAccount"
                        ValueField="SipAccount"
                        FieldLabel="User Email:"
                        EmptyText="Please Select a User"
                        LabelWidth="70"
                        Width="380">
                        <Store>
                            <ext:Store 
                                ID="Store1"
                                runat="server">
                                <Model>
                                    <ext:Model 
                                        ID="Model2"
                                        runat="server">
                                        <Fields>
                                            <ext:ModelField Name="EmployeeId" />
                                            <ext:ModelField Name="SipAccount" />
                                            <ext:ModelField Name="SiteName" />
                                            <ext:ModelField Name="FullName" />
                                            <ext:ModelField Name="DisplayName" />
                                            <ext:ModelField Name="DepartmentName" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                                        
                        <DirectEvents>
                            <BeforeQuery OnEvent="NewSystemRole_UserSipAccount_BeforeQuery" />
                        </DirectEvents>

                        <ListConfig
                            Border="true"
                            MinWidth="400"
                            MaxHeight="300"
                            EmptyText="Type User Name or Email...">
                            <ItemTpl ID="ItemTpl1" runat="server">
                                <Html>
                                    <div class="search-item">
                                        <h3><span>{DisplayName}</span>{SipAccount}</h3>
                                    </div>
                                </Html>
                            </ItemTpl>
                        </ListConfig>
                    </ext:ComboBox>

                    <ext:ComboBox
                        ID="NewSystemRole_SitesList"
                        runat="server"
                        QueryMode="Local"
                        TriggerAction="Query"
                        ValueField="Id"
                        DisplayField="Name"
                        Editable="true"
                        EmptyText="Please Select Site"
                        FieldLabel="Site:"
                        LabelWidth="70"
                        Width="380">
                        <Store>
                            <ext:Store ID="SitesListStore" runat="server" OnLoad="SitesListStore_Load">
                                <Model>
                                    <ext:Model ID="SitesListStoreModel" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="Id" />
                                            <ext:ModelField Name="Name" />
                                            <ext:ModelField Name="CountryCode" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>

                        <ListConfig>
                            <ItemTpl ID="NewSystemRoleSitesListTpl" runat="server">
                                <Html>
                                    <div>{Name}&nbsp;({CountryCode})</div>
                                </Html>
                            </ItemTpl>
                        </ListConfig>
                    </ext:ComboBox>
                </Items>

                <BottomBar>
                    <ext:StatusBar
                        ID="NewSystemRoleWindowBottomBar"
                        runat="server"
                        StatusAlign="Right"
                        DefaultText=""
                        Height="30">
                        <Items>
                            <ext:Button
                                ID="AddNewSystemRoleButton"
                                runat="server"
                                Text="Add"
                                Icon="ApplicationFormAdd"
                                Height="25">
                                <DirectEvents>
                                    <Click OnEvent="AddNewSystemRoleButton_Click" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:Button
                                ID="CancelNewSystemRoleButton"
                                runat="server"
                                Text="Cancel"
                                Icon="Cancel"
                                Height="25">
                                <DirectEvents>
                                    <Click OnEvent="CancelNewSystemRoleButton_Click" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:ToolbarSeparator
                                ID="AddNewSystemRoleWindow_BottomBarSeparator"
                                runat="server" />

                            <ext:ToolbarTextItem
                                ID="NewSystemRole_StatusMessage"
                                runat="server"
                                Height="15"
                                Text=""
                                Margins="0 0 0 5" />
                        </Items>
                    </ext:StatusBar>
                </BottomBar>

                <DirectEvents>
                    <BeforeHide OnEvent="AddNewSystemRoleWindowPanel_BeforeHide" />
                </DirectEvents>
            </ext:Window>
        </div>
    </div>
</asp:Content>


<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
</asp:Content>
