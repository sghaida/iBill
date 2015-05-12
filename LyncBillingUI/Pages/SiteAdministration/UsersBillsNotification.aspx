<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UsersBillsNotification.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAdministration.UsersBillsNotification" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <ext:GridPanel
                ID="UsersBillsGrid" 
                runat="server" 
                Title="Users Bills Notifications"
                MaxWidth="955"
                MinHeight="620"  
                AutoScroll="true"
                Header="true"
                Scroll="Both" 
                Layout="FitLayout">

                <Store>
                    <ext:Store
                        ID="UsersBillsStore" 
                        runat="server"
                        IsPagingStore="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="UsersBillsModel" runat="server" IDProperty="SipAccount">
                                <Fields>
                                    <ext:ModelField Name="UserId" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="UserName" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="UserSipAccount" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="Date" Type="Date" />
                                    <ext:ModelField Name="PersonalCallsCost" Type="Float" />
                                    <ext:ModelField Name="PersonalCallsCount" Type="Int" />
                                    <ext:ModelField Name="PersonalCallsDuration" Type="Int" />
                                </Fields>
                         </ext:Model>
                       </Model>
                         <Sorters>
                            <ext:DataSorter Property="SipAccount" Direction="ASC" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <ColumnModel ID="BillsColumnModel" runat="server" Flex="1">
		            <Columns>
                        <ext:Column ID="UserSipAccountCol" 
                            runat="server" 
                            Text="Sip Account" 
                            Width="200" 
                            DataIndex="UserSipAccount"
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

                        <ext:Column ID="UserFullName" 
                            runat="server" 
                            Text="Full Name" 
                            Width="150" 
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

                        <ext:Column ID="MonthDate" 
                            runat="server" 
                            Text="Date" 
                            Width="200" 
                            DataIndex="Date"
                            Groupable="false"
                            Align="Left"
                            Sortable="true">
                            <%--<Renderer Fn="SpecialDateRenderer" />--%>
                        </ext:Column>

                        <ext:Column ID="CallsInformationColum"
                            runat="server"
                            Text="Calls Information"
                            MenuDisabled="true"
                            Sortable="false"
                            Resizable="false">
                            <Columns>
                                <ext:Column ID="TotalDuration"
                                    runat="server"
                                    Text="Duration"
                                    Width="120"
                                    DataIndex="PersonalCallsDuration"
                                    MenuDisabled="true"
                                    Sortable="false">
                                    <Renderer Fn="GetMinutes" />
                                </ext:Column>

                                <ext:Column ID="TotalCalls"
                                    runat="server"
                                    Text="Number of Calls"
                                    Width="120"
                                    DataIndex="PersonalCallsCount"
                                    MenuDisabled="true"
                                    Sortable="false" />
                                
		                        <ext:Column ID="TotalCost"
                                    runat="server"
                                    Text="Total Cost"
                                    Width="120"
                                    DataIndex="PersonalCallsCost"
                                    MenuDisabled="true"
                                    Sortable="false">
                                    <Renderer Fn="RoundCostsToTwoDecimalDigits"/>
                                </ext:Column>
                            </Columns>
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <TopBar>
                    <ext:Toolbar ID="FilterToolBar" runat="server">
                        <Items>
                            <ext:ComboBox 
                                ID="FilterUsersBySite" 
                                runat="server" 
                                Icon="Find" 
                                TriggerAction="All" 
                                QueryMode="Local" 
                                DisplayField="Name" 
                                ValueField="Id"
                                FieldLabel="Site"
                                LabelWidth="25"
                                Width="250"
                                MarginSpec="5 5 5 5"
                                Disabled="false">
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

                                <ListConfig>
                                    <ItemTpl ID="SitesItemTpl" runat="server">
                                        <Html>
                                            <div data-qtip="{Name} ({CountryCode})">{Name}&nbsp;({CountryCode})</div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>

                                <DirectEvents>
                                    <Select OnEvent="FilterUsersBySite_Selected" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <%--<Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>--%>
                            </ext:ComboBox>

                            <ext:ToolbarSeparator MarginSpec="5 5 5 5" runat="server" />

                            <ext:DateField 
                                ID="BillDateField"
                                runat="server" 
                                FieldLabel="Date:"
                                LabelWidth="30"
                                EmptyText="Empty Date"
                                Width="190"
                                MarginSpec="5 5 5 5"
                                Editable="false"
                                Disabled="true">

                                <DirectEvents>
                                    <Select OnEvent="GetUsersBillsForSite">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <%--<Listeners>
                                    <Select Fn="clearFilter" />
                                </Listeners>--%>
                            </ext:DateField>

                            <ext:Button 
                                ID="EmailAlertButton" 
                                runat="server" 
                                Text="Email Alert" 
                                Icon="EmailAdd" 
                                MarginSpec="5 5 5 380">
                                <DirectEvents>
                                    <Click OnEvent="NotifyUsers">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                           <ext:Parameter Name="Values" Value="Ext.encode(#{UsersBillsGrid}.getRowsValues({selectedOnly : true}))" Mode="Raw"/>
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <SelectionModel>
                    <ext:CheckboxSelectionModel 
                        ID="CheckboxSelectionModel1"
                        runat="server"
                        Mode="Multi"
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingBottomBar"
                        runat="server"
                        StoreID="UsersBillsStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Users' Bills {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>
        </div>
    </div>
</asp:Content>


<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
</asp:Content>
