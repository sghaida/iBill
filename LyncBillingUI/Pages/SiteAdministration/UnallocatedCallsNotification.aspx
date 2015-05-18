<%@ Page Title="Unallocated Calls Notification" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UnallocatedCallsNotification.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAdministration.UnallocatedCallsNotification" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">

</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <ext:GridPanel
                ID="UnmarkedCallsGrid"
                Header="true"
                Title="Users With Unallocated Calls"
                runat="server"
                MaxWidth="955"
                MinHeight="620"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout">

                <Store>
                    <ext:Store
                        ID="UnmarkedCallsStore"
                        runat="server"
                        IsPagingStore="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="UnmarkedCallsModel" runat="server" IDProperty="SipAccount">
                                <Fields>
                                    <ext:ModelField Name="UserId" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="UserName" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="UserSipAccount" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="UnallocatedCallsCost" Type="Float" />
                                    <ext:ModelField Name="UnallocatedCallsCount" Type="Int" />
                                    <ext:ModelField Name="UnallocatedCallsDuration" Type="Int" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Sorters>
                            <ext:DataSorter Property="SipAccount" Direction="ASC" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <ColumnModel ID="UnmarkedCallsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column
                            ID="EmployeeIDCol"
                            runat="server"
                            Text="Employee ID"
                            Width="120"
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
                            ID="SipAccountCol"
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
                            ID="FullNameCol"
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

                        <ext:Column ID="CallsInformationColum"
                            runat="server"
                            Text="Calls Information"
                            MenuDisabled="true"
                            Sortable="false"
                            Resizable="false">
                            <Columns>
                                <ext:Column
                                    ID="UnmarkedCallsDurationCol"
                                    runat="server"
                                    Text="Duration"
                                    Width="120"
                                    DataIndex="UnallocatedCallsDuration"
                                    MenuDisabled="true"
                                    Sortable="false">
                                    <Renderer Fn="GetMinutes" />
                                </ext:Column>

                                <ext:Column
                                    ID="UnmarkedCallsCountCol"
                                    runat="server"
                                    Text="Number of Calls"
                                    Width="120"
                                    DataIndex="UnallocatedCallsCount"
                                    MenuDisabled="true"
                                    Sortable="false" />

                                <ext:Column
                                    ID="UnmarkedCallsCostCol"
                                    runat="server"
                                    Text="Cost"
                                    Width="120"
                                    DataIndex="UnallocatedCallsCost"
                                    MenuDisabled="true"
                                    Sortable="false">
                                    <Renderer Fn="RoundCostsToTwoDecimalDigits" />
                                </ext:Column>
                            </Columns>
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                
                <TopBar>
                    <ext:Toolbar ID="UnamrkedCalls_FilterToolBar" runat="server">
                        <Items>
                            <ext:ComboBox 
                                ID="FilterUsersBySite" 
                                runat="server" 
                                Icon="Find" 
                                TriggerAction="All"
                                QueryMode="Local"
                                DisplayField="Name" 
                                ValueField="Id"
                                Width="250"
                                MarginSpec="5 5 5 5"
                                FieldLabel="Site"
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

                                <ListConfig>
                                    <ItemTpl ID="SitesItemTpl" runat="server">
                                        <Html>
                                            <div data-qtip="{Name} ({CountryCode})">{Name}&nbsp;({CountryCode})</div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>

                                <DirectEvents>
                                    <Select OnEvent="GetUnmarkedCallsForSite" Timeout="500000">
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
                                Text="Email Alert" 
                                Icon="EmailAdd" 
                                MarginSpec="5 5 5 600">
                               <DirectEvents>
                                   <Click OnEvent="NotifyUsers" Timeout="600000">
                                       <EventMask ShowMask="true" />
                                       <ExtraParams>
                                           <ext:Parameter Name="Values" Value="Ext.encode(#{UnmarkedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw"/>
                                       </ExtraParams>
                                   </Click>
                               </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <SelectionModel>
                    <ext:CheckboxSelectionModel 
                        ID="UnmarkedCallsCheckboxSelectionModel"
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
                        StoreID="UnmarkedCallsStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Users with Unmarked Calls {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>
        </div>
    </div>
</asp:Content>


<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">

</asp:Content>
