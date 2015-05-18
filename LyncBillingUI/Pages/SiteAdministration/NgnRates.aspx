<%@ Page Title="NGN Rates" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NgnRates.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAdministration.NgnRates" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <ext:GridPanel
                ID="ManageRatesGrid"
                runat="server"
                MaxWidth="955"
                MinHeight="620"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="NGN Rates">
                <Store>
                    <ext:Store
                        ID="ManageRatesStore"
                        runat="server"
                        PageSize="25"
                        GroupField="CountryName">
                        <Model>
                            <ext:Model ID="ManageRatesModel" runat="server" IDProperty="Id">
                                <Fields>
                                    <ext:ModelField Name="Id" Type="Int" />
                                    <ext:ModelField Name="DialingCodeId" Type="Int" />
                                    <ext:ModelField Name="Rate" Type="Float" />
                                    <ext:ModelField Name="DialingCode" Type="String" />
                                    <ext:ModelField Name="Iso3CountryCode" Type="String" />
                                    <ext:ModelField Name="CountryName" Type="String" />
                                    <ext:ModelField Name="TypeOfService" Type="String" />
                                    <ext:ModelField Name="Description" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                
                <Plugins>
                    <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2" />
                </Plugins>


                <Features>
                    <ext:Grouping  ID="GroupingFeatures" 
                        runat="server" 
                        HideGroupedHeader="false"
                        StartCollapsed="false"
                        GroupHeaderTplString="{name}" />
                </Features>


                <ColumnModel ID="ManageRatesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="35" />

                        <ext:Column
                            ID="DialingCodeColumn"
                            runat="server"
                            Text="Dialing Code"
                            Width="170"
                            DataIndex="DialingCode"
                            MenuDisabled="true"
                            Groupable="false"
                            Sortable="false">
                            <%--<HeaderItems>
                                <ext:TextField ID="DialingCodeFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDialingCodeFilter" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column
                            ID="TypeOfServiceColumn"
                            runat="server"
                            Text="Type of Service"
                            Width="140"
                            DataIndex="TypeOfService"
                            MenuDisabled="true"
                            Sortable="true"
                            Groupable="false">
                            <%--<HeaderItems>
                                <ext:TextField ID="CallTypeFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearCallTypeFilter" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column
                            ID="RateCol"
                            runat="server"
                            Text="Rate"
                            Width="100"
                            DataIndex="Rate"
                            MenuDisabled="true"
                            Sortable="false"
                            Groupable="false">
                            <Editor>
                                <ext:TextField
                                    ID="Editor_Rate_Textbox"
                                    runat="server"
                                    DataIndex="Rate" />
                            </Editor>
                        </ext:Column>

                        <ext:Column
                            ID="CountryCodeCol"
                            runat="server"
                            Text="Country Code"
                            Width="210"
                            DataIndex="Iso3CountryCode"
                            MenuDisabled="true"
                            Sortable="false"
                            Groupable="false">
                        </ext:Column>

                        <ext:Column
                            ID="DescriptionColumn"
                            runat="server"
                            Text="Description"
                            Width="210"
                            DataIndex="Description"
                            MenuDisabled="true"
                            Sortable="false"
                            Groupable="false">
                        </ext:Column>

                        <ext:CommandColumn ID="RejectChange" runat="server" Width="70">
                            <Commands>
                                <ext:GridCommand Text="Reject" ToolTip-Text="Reject row changes" CommandName="reject" Icon="ArrowUndo" />
                            </Commands>
                            <PrepareToolbar Handler="toolbar.items.get(0).setVisible(record.dirty);" />
                            <Listeners>
                                <Command Handler="record.reject();" />
                            </Listeners>
                        </ext:CommandColumn>

                    </Columns>
                </ColumnModel>


                <TopBar>
                    <ext:Toolbar ID="FilterGatewaysRatesToolBar" runat="server">
                        <Items>
                            <ext:ComboBox 
                                ID="FilterGatewaysBySite" 
                                runat="server" 
                                Icon="Find" 
                                TriggerAction="All" 
                                QueryMode="Local" 
                                DisplayField="Name" 
                                ValueField="Id"
                                Width="200"
                                MarginSpec="5 5 5 5"
                                FieldLabel="Site"
                                LabelSeparator=":"
                                LabelWidth="30"
                                ValidateBlank="true"
                                ValidateOnChange="true">
                                <Store>
                                    <ext:Store
                                        ID="FilterGatewaysBySiteStore"
                                        runat="server"
                                        OnLoad="FilterGatewaysBySiteStore_Load">
                                        <Model>
                                            <ext:Model ID="SitesModel" runat="server">
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
                                            <div data-qtip="{Name} ({CountryCode})">
                                                {Name} ({CountryCode})
                                            </div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>

                                <DirectEvents>
                                    <Select OnEvent="GetGatewaysForSite" Timeout="1000000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <%--<Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>--%>
                            </ext:ComboBox>

                            <ext:ToolbarSeparator ID="ToolbarSeparator1" MarginSpec="5 5 5 5" runat="server" />

                            <ext:ComboBox
                                ID="FilterRatesByGateway"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Local"
                                DisplayField="Name"
                                ValueField="Id"
                                FieldLabel="Gateway"
                                LabelWidth="50"
                                Width="230"
                                MarginSpec="5 5 5 5"
                                Disabled="true">
                                <Store>
                                    <ext:Store
                                        ID="GatewaysStore"
                                        runat="server">
                                        <Model>
                                            <ext:Model ID="GatewaysModel" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="Id" Type="Int" />
                                                    <ext:ModelField Name="Name" Type="String" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>

                                <DirectEvents>
                                    <Select OnEvent="FilterRatesByGateway_Selected" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <%--<Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>--%>
                            </ext:ComboBox>

                            <ext:Button
                                ID="UpdateEditedRecords"
                                runat="server"
                                Text="Save"
                                Icon="ApplicationEdit"
                                MarginSpec="5 5 5 350">
                                <DirectEvents>
                                    <Click OnEvent="UpdateEdited_DirectEvent" before="return #{ManageRatesStore}.isDirty();" Timeout="600000">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageRatesStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>

                            <ext:ToolbarSeparator runat="server" />

                            <ext:Button
                                ID="CancelChangesButton"
                                Text="Cancel"
                                Icon="Cancel"
                                runat="server"
                                MarginSpec="5 5 5 5">
                                <DirectEvents>
                                    <Click OnEvent="RejectChanges_DirectEvent">
                                        <EventMask ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageRatesPagingToolbar"
                        runat="server"
                        StoreID="ManageRatesStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="NGN Rates {0} - {1} of {2}" />
                </BottomBar>

            </ext:GridPanel>
        </div>
    </div>
</asp:Content>


<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
</asp:Content>
