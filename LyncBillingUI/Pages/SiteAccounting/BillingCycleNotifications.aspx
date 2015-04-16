<%@ Page Title="Billing Cycle" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BillingCycleNotifications.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAccounting.BillingCycleNotifications" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <ext:GridPanel
                ID="BillingCycleNotificationGrid"
                Header="true"
                Title="Billing Cycle Notification"
                runat="server"
                MaxWidth="955"
                MinHeight="600"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout">

                <Store>
                    <ext:Store
                        ID="BillingCycleNotificationStore"
                        runat="server"
                        IsPagingStore="true"
                        PageSize="25"
                        OnSubmitData="BillingCycleNotificationStore_SubmitData">
                        <Model>
                            <ext:Model ID="BillingCycleNotificationModel" runat="server" IDProperty="UserSipAccount">
                                <Fields>
                                    <ext:ModelField Name="UserId" Type="Int" SortType="AsText" />
                                    <ext:ModelField Name="UserSipAccount" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="UserName" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="UserDepartment" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="PersonalCallsCost" Type="Float" />
                                    <ext:ModelField Name="BusinessCallsCost" Type="Float" />
                                    <ext:ModelField Name="UnallocatedCallsCost" Type="Float" />
                                </Fields>
                            </ext:Model>
                        </Model>

                        <Sorters>
                            <ext:DataSorter Property="UserSipAccount" Direction="ASC" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <ColumnModel ID="MonthlyReportsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column
                            ID="UserIdCol"
                            runat="server"
                            Text="Employee ID"
                            Width="100"
                            DataIndex="UserId"
                            Sortable="true">
                            <%--<HeaderItems>
                                <ext:TextField ID="EmployeeIDFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearEmployeeIDFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column
                            ID="UserSipAccountCol"
                            runat="server"
                            Text="Sip Account"
                            Width="200"
                            DataIndex="UserSipAccount"
                            Sortable="true">
                            <%--<HeaderItems>
                                <ext:TextField ID="SipAccountFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSipAccountFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column
                            ID="UserNameCol"
                            runat="server"
                            Text="Full Name"
                            Width="200"
                            DataIndex="UserName"
                            Sortable="true">
                            <%--<HeaderItems>
                                <ext:TextField ID="FullNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearFullNameFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column
                            ID="UserDepartmentCol"
                            runat="server"
                            Text="Department"
                            Width="100"
                            DataIndex="UserDepartment"
                            Sortable="true">
                            <%--<HeaderItems>
                                <ext:TextField ID="FullNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearFullNameFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column
                            ID="GroupedCostsColumnsCol"
                            runat="server"
                            MenuDisabled="true"
                            Sortable="false"
                            Groupable="false"
                            Resizable="false"
                            Text="Calls Costs">
                            <Columns>
                                <ext:Column
                                    ID="PersonalCallsCostCol"
                                    runat="server"
                                    Text="Personal"
                                    Width="90"
                                    DataIndex="PersonalCallsCost"
                                    MenuDisabled="true">
                                </ext:Column>

                                <ext:Column
                                    ID="BusinessCallsCostCol"
                                    runat="server"
                                    Text="Business"
                                    Width="90"
                                    DataIndex="BusinessCallsCost"
                                    MenuDisabled="true">
                                </ext:Column>

                                <ext:Column
                                    ID="UnallocatedCallsCostCol"
                                    runat="server"
                                    Text="Unallocated"
                                    Width="90"
                                    DataIndex="UnallocatedCallsCost"
                                    MenuDisabled="true">
                                </ext:Column>
                            </Columns>
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                

                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:ComboBox 
                                ID="FilterUsersBySite" 
                                runat="server" 
                                Icon="Find" 
                                TriggerAction="Query"
                                QueryMode="Local"
                                DisplayField="Name"
                                ValueField="Id"
                                FieldLabel="Site"
                                Width="160"
                                MarginSpec="5 5 5 5"
                                LabelSeparator=":"
                                LabelWidth="25"
                                Editable="true">
                                <Store>
                                    <ext:Store
                                        ID="FilterUsersBySiteStore"
                                        runat="server"
                                        OnLoad="FilterUsersBySiteStore_Load">
                                        <Model>
                                            <ext:Model ID="FilterUsersBySiteStoreModel" runat="server">
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

                                <ListConfig MinWidth="200">
                                    <ItemTpl ID="SitesItemTpl" runat="server">
                                        <Html>
                                            <div data-qtip="{Name} ({CountryCode})">
                                                {Name} ({CountryCode})
                                            </div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>

                                <DirectEvents>
                                    <Select OnEvent="GetUsersForForSite" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <%--<Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>--%>
                            </ext:ComboBox>


                            <ext:Button 
                                ID="UnamrkedCalls_EmailAlertButton" 
                                runat="server" 
                                Text="Send Notifications" 
                                Icon="EmailAdd" 
                                MarginSpec="5 5 5 650">
                                <%--<DirectEvents>
                                   <Click OnEvent="NotifyUsers" Timeout="600000">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                           <ext:Parameter Name="Values" Value="Ext.encode(#{BillingCycleNotificationGrid}.getRowsValues()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>--%>

                                <Listeners>
                                    <Click Handler="submitValue(#{BillingCycleNotificationGrid}, '', '', true);" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingBottomBar"
                        runat="server"
                        StoreID="BillingCycleNotificationStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Users {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>
        </div>
    </div>
</asp:Content>


<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
</asp:Content>
