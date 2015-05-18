<%@ Page Title="Delegee Roles" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DelegeeRoles.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAdministration.DelegeeRoles" %>

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
                ID="ManageDelegatesGrid"
                runat="server"
                MaxWidth="955"
                MinHeight="600"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage Delegates">
                <store>
                    <ext:Store
                        ID="ManageDelegatesStore"
                        runat="server"
                        RemoteSort="true"
                        IsPagingStore="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ManageDelegatesModel" runat="server" IDProperty="Id">
                                <Fields>
                                    <ext:ModelField Name="Id" Type="Int" />
                                    <ext:ModelField Name="DelegationType" Type="Int" />
                                    <ext:ModelField Name="ManagedUserSipAccount" Type="String" />
                                    <ext:ModelField Name="DelegeeSipAccount" Type="String" />
                                    <ext:ModelField Name="ManagedSiteId" Type="Int" />
                                    <ext:ModelField Name="ManagedSiteDepartmentId" Type="String" />
                                    <ext:ModelField Name="Description" Type="String" />
                                    <ext:ModelField Name="ManagedSiteName" Type="String" />
                                    <ext:ModelField Name="ManagedSiteDepartmentName" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>

                        <Sorters>
                            <ext:DataSorter Property="DelegationType" Direction="DESC" />
                        </Sorters>
                    </ext:Store>
                </store>

                <columnmodel id="ManageDelegatesColumnModel" runat="server" flex="1">
                    <Columns>
                        <ext:Column ID="DelegeeUserSipAccount"
                            runat="server"
                            Text="Assigned Delegee User"
                            Width="200"
                            DataIndex="DelegeeSipAccount"
                            Sortable="false"
                            Groupable="false">
                            <%--<HeaderItems>
                                <ext:TextField ID="DelegeeSipAccountFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDelegeeSipAccountFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column ID="ManagedUserSipAccountCol"
                            runat="server"
                            Text="On User Account"
                            Width="200"
                            DataIndex="ManagedUserSipAccount"
                            Sortable="false"
                            Groupable="false">
                            <%--<HeaderItems>
                                <ext:TextField ID="SipAccountFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSipAccountFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column ID="ManagedSiteCol"
                            runat="server"
                            Text="On Site Account"
                            Width="150"
                            DataIndex="ManagedSiteName"
                            Sortable="false"
                            Groupable="false">
                            <%--<Renderer Fn="getNameAttrFromSiteObject" />--%>
                            
                            <%--<HeaderItems>
                                <ext:TextField ID="SiteFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSiteFilter" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>


                        <ext:Column ID="managedDepartmentCol"
                            runat="server"
                            Text="On Department Account"
                            Width="150"
                            DataIndex="ManagedSiteDepartmentName"
                            Sortable="false"
                            Groupable="false">
                            <%--<Renderer Fn="getNameAttrFromDepartmentObject" />--%>

                            <%--<HeaderItems>
                                <ext:TextField ID="DepartmentFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDepartmentFiler" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column ID="Column3"
                            runat="server"
                            Text="Description"
                            Width="150"
                            DataIndex="Description"
                            Sortable="false"
                            Groupable="false">
                            <%--<HeaderItems>
                                <ext:TextField ID="DescriptionFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDescFilterBtn" runat="server" />
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
                                <Command Handler="this.up(#{ManageDelegatesGrid}.store.removeAt(recordIndex));" />
                            </Listeners>
                        </ext:ImageCommandColumn>

                    </Columns>
                </columnmodel>

                <TopBar>
                    <ext:Toolbar ID="FilterDelegatesSitesToolBar" runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterDelegatesBySite"
                                runat="server"
                                Icon="Find"
                                QueryMode="Local"
                                TypeAhead="false"
                                DisplayField="Name"
                                ValueField="Id"
                                FieldLabel="Site:"
                                LabelWidth="25"
                                MarginSpec="5 5 5 5"
                                Width="250">
                                <Store>
                                    <ext:Store ID="DelegatesSitesStore" runat="server" OnLoad="DelegatesSitesStore_Load">
                                        <Model>
                                            <ext:Model ID="DelegatesSitesStoreModel" runat="server">
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
                                    <ItemTpl ID="FilterSitesItemTpl" runat="server">
                                        <Html>
                                            <div>{Name}&nbsp;({CountryCode})</div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>

                                <DirectEvents>
                                    <Select OnEvent="GetDelegates" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <%--<Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>--%>
                            </ext:ComboBox>

                            <ext:ToolbarSeparator runat="server" MarginSpec="5 5 5 5" />

                            <ext:Button
                                ID="AddRecordButton"
                                runat="server"
                                Text="New Delegee"
                                Icon="Add"
                                Margins="5 5 0 260">
                                <DirectEvents>
                                    <Click OnEvent="ShowAddDelegeePanel" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:ToolbarSeparator runat="server" MarginSpec="5 5 5 5" />

                            <ext:Button
                                ID="SaveChangesButton"
                                runat="server"
                                Text="Save Changes"
                                Icon="DatabaseSave"
                                Margins="5 5 0 5">
                                <DirectEvents>
                                    <Click OnEvent="SaveChanges_DirectEvent" before="return #{ManageDelegatesStore}.isDirty();" Timeout="1000000">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageDelegatesStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageDelegatesPagingToolbar"
                        runat="server"
                        StoreID="ManageDelegatesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Users Delegates {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>


            <ext:Window 
                ID="AddNewDelegeeWindowPanel" 
                runat="server" 
                Frame="true"
                Resizable="false"
                Title="New Delegee Role" 
                Hidden="true"
                Width="410"
                Icon="Add" 
                X="250"
                Y="250"
                BodyStyle="background-color: #f9f9f9;">
                                
                <Defaults>
                    <ext:Parameter Name="Padding" Value="10" Mode="Raw" />
                </Defaults>

                <Items>
                    <ext:ComboBox
                        ID="NewDelegee_DelegeeTypeCombobox"
                        runat="server"
                        DisplayField="TypeName"
                        ValueField="TypeValue"
                        Width="380"
                        FieldLabel="Delegee Type"
                        LabelWidth="100">
                        <Items>
                            <ext:ListItem Text="User" Value="520" />
                            <%--<ext:ListItem Text="Department" Value="510" />--%>
                            <ext:ListItem Text="Site" Value="500" />
                        </Items>

                        <SelectedItems>
                            <ext:ListItem Text="User" Value="520" />
                        </SelectedItems>

                        <DirectEvents>
                            <Select OnEvent="DelegeeTypeMenu_Selected" />
                        </DirectEvents>
                    </ext:ComboBox>

                    <ext:ComboBox
                        ID="NewDelegee_DelegeeSipAccount"
                        runat="server"
                        Icon="Find"
                        TriggerAction="Query"
                        QueryMode="Remote"
                        Editable="true"
                        DisplayField="SipAccount"
                        ValueField="SipAccount"
                        FieldLabel="Delegee Email:"
                        EmptyText="Please Select a Delegee"
                        LabelWidth="100"
                        Width="380">
                        <Store>
                            <ext:Store 
                                ID="FilterUsersByDepartmentStore"
                                runat="server">
                                <Model>
                                    <ext:Model 
                                        ID="FilterUsersByDepartmentModel"
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
                            <BeforeQuery OnEvent="NewDelegee_DelegeeSipAccount_BeforeQuery" />
                        </DirectEvents>

                        <ListConfig
                            Border="true"
                            MinWidth="400"
                            MaxHeight="300"
                            EmptyText="Type User Name or Email...">
                            <ItemTpl ID="ItemTpl3" runat="server">
                                <Html>
                                    <div class="search-item">
                                        <h3><span>{DisplayName}</span>{SipAccount}</h3>
                                    </div>
                                </Html>
                            </ItemTpl>
                        </ListConfig>
                    </ext:ComboBox>

                    <ext:ComboBox
                        ID="NewDelegee_UserSipAccount"
                        runat="server"
                        Icon="Find"
                        TriggerAction="Query"
                        QueryMode="Remote"
                        Editable="true"
                        DisplayField="SipAccount"
                        ValueField="SipAccount"
                        FieldLabel="User Email:"
                        EmptyText="Please Select a User"
                        LabelWidth="100"
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
                                            <ext:ModelField Name="Department" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                                        
                        <DirectEvents>
                            <BeforeQuery OnEvent="NewDelegee_UserSipAccount_BeforeQuery" />
                        </DirectEvents>

                        <ListConfig
                            Border="true"
                            MinWidth="400"
                            MaxHeight="300"
                            EmptyText="Type User Name or SipAccount...">
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
                        ID="NewDelegee_SitesList"
                        runat="server"
                        DisplayField="Name"
                        ValueField="Id"
                        TriggerAction="Query"
                        QueryMode="Local"
                        Editable="true"
                        EmptyText="Please Select Site"
                        FieldLabel="Site:"
                        LabelWidth="100"
                        Width="380"
                        Hidden="true">
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

                        <ListConfig MinWidth="200">
                            <ItemTpl ID="SitesItemTpl" runat="server">
                                <Html>
                                    <div data-qtip="{Name} ({CountryCode})">{Name}&nbsp;({CountryCode})</div>
                                </Html>
                            </ItemTpl>
                        </ListConfig>

                        <DirectEvents>
                            <Select OnEvent="NewDelegee_SitesList_Selected" />
                        </DirectEvents>
                    </ext:ComboBox>

                    <ext:ComboBox
                        ID="NewDelegee_DepartmentsList"
                        runat="server"
                        DisplayField="DepartmentName"
                        ValueField="DepartmentId"
                        TriggerAction="Query"
                        QueryMode="Local"
                        Editable="true"
                        EmptyText="Please Select Department"
                        FieldLabel="Department:"
                        LabelWidth="125"
                        Width="350"
                        Hidden="true">
                        <Store>
                            <ext:Store ID="DepartmentsListStore" runat="server">
                                <Model>
                                    <ext:Model ID="DepartmentHeadsStoreModel" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="SiteId" />
                                            <ext:ModelField Name="SiteName" />
                                            <ext:ModelField Name="DepartmentId" />
                                            <ext:ModelField Name="DepartmentName" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                    </ext:ComboBox>
                </Items>

                <BottomBar>
                    <ext:StatusBar
                        ID="NewDelegeeWindowBottomBar"
                        runat="server"
                        StatusAlign="Right"
                        DefaultText=""
                        Height="30">
                        <Items>
                            <ext:Button
                                ID="AddNewDelegeeButton"
                                runat="server"
                                Text="Add"
                                Icon="ApplicationFormAdd"
                                Height="25">
                                <DirectEvents>
                                    <Click OnEvent="AddNewDelegeeButton_Click" Timeout="1000000" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:Button
                                ID="CancelNewDelegeeButton"
                                runat="server"
                                Text="Cancel"
                                Icon="Cancel"
                                Height="25">
                                <DirectEvents>
                                    <Click OnEvent="CancelNewDelegeeButton_Click" Timeout="1000000" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:ToolbarSeparator
                                ID="NewDelegeeWindow_BottomBarSeparator"
                                runat="server" />

                            <ext:ToolbarTextItem
                                ID="NewDelegee_StatusMessage"
                                runat="server"
                                Height="15"
                                Text=""
                                Margins="0 0 0 5" />
                        </Items>
                    </ext:StatusBar>
                </BottomBar>

                <DirectEvents>
                    <BeforeHide OnEvent="AddNewDelegeeWindowPanel_BeforeHide" />
                </DirectEvents>
            </ext:Window>
        </div>
    </div>
</asp:Content>


<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
    <script type="text/javascript">
        //
        // Given a "LyncBillingBase.DataModel.Site" object, return the Name attribute
        function getNameAttrFromSiteObject(site) {
            var isNullValue = (undefined === site || undefined === site.Name);

            if (false === isNullValue)
                return site.Name;
            else
                return "";
        }

        //
        // Given a "LyncBillingBase.DataModel.SiteDepartment" object, 
        // return the Name attribute of the inner Department object
        function getNameAttrFromDepartmentObject(siteDepartment) {
            var isNullValue = true;
            var nameAttr = "";

            debugger;

            isNullValue = (undefined === siteDepartment 
                || undefined === siteDepartment.Department 
                || undefined === siteDepartment.Department.Name
                || "" === siteDepartment.Department.Name);

            if (false === isNullValue) {
                nameAttr = siteDepartment.Department.Name.toString();
            }

            return nameAttr;
        }
    </script>
</asp:Content>
