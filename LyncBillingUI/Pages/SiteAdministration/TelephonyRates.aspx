<%@ Page Title="Telephony Rates" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TelephonyRates.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAdministration.TelephonyRates" %>

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
                Title="Telephony Rates">
                <Store>
                    <ext:Store
                        ID="ManageRatesStore"
                        runat="server"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ManageRatesModel" runat="server" IDProperty="Iso3CountryCode">
                                <Fields>
                                    <ext:ModelField Name="Iso2CountryCode" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="Iso3CountryCode" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="CountryName" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="FixedLineRate" Type="String" />
                                    <ext:ModelField Name="MobileLineRate" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>

                        <Sorters>
                            <ext:DataSorter Property="CountryName" Direction="ASC" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <Plugins>
                    <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2" />
                </Plugins>

                <ColumnModel ID="ManageRatesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="35" />

                        <ext:Column
                            ID="RateIDCol"
                            runat="server"
                            Text="ID"
                            Width="160"
                            DataIndex="RateID"
                            Visible="false" />

                         <ext:Column
                            ID="CountryNameCol"
                            runat="server"
                            Text="Country Name"
                            Width="250"
                            DataIndex="CountryName">
                             <%--<HeaderItems>
                                <ext:TextField ID="CountryNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearCountryNameFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                         </ext:Column>

                        <ext:Column
                            ID="CountryCodeCol"
                            runat="server"
                            Text="Code"
                            Width="150"
                            DataIndex="Iso3CountryCode">
                            <%--<HeaderItems>
                                <ext:TextField ID="CountryCodeFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearCountryCodeFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column ID="ManageRatesGroupedColumn"
                            runat="server"
                            Text="Telephony Rates"
                            Resizable="false"
                            Sortable="false"
                            Groupable="false"
                            MenuDisabled="true"
                            Align="Center">
                            <Columns>
                                <ext:Column
                                    ID="FixedlineRateCol"
                                    runat="server"
                                    Text="Fixedline Rate"
                                    Width="155"
                                    DataIndex="FixedLineRate"
                                    MenuDisabled="true"
                                    Sortable="false">
                                    <Editor>
                                        <ext:TextField
                                            ID="FixedLineRateTextbox"
                                            runat="server"
                                            DataIndex="FixedLineRate" />
                                    </Editor>
                                </ext:Column>

                                <ext:Column
                                    ID="MobileLineRateCol"
                                    runat="server"
                                    Text="Mobile Rate"
                                    Width="155"
                                    DataIndex="MobileLineRate"
                                    MenuDisabled="true"
                                    Sortable="false">
                                    <Editor>
                                        <ext:TextField
                                            ID="MobileLineRateTextbox"
                                            runat="server"
                                            DataIndex="MobileLineRate" />
                                    </Editor>
                                </ext:Column>
                            </Columns>
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
                                            <div data-qtip="{Name} ({CountryCode})">{Name}&nbsp;({CountryCode})</div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>

                                <DirectEvents>
                                    <Select OnEvent="GetGatewaysForSite" Timeout="2000000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <%--<Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>--%>
                            </ext:ComboBox>

                            <ext:ToolbarSeparator MarginSpec="5 5 5 5" runat="server" />

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
                                MarginSpec="5 45 5 5"
                                Disabled="true">
                                <Store>
                                    <ext:Store
                                        ID="GatewaysStore"
                                        runat="server">
                                        <Model>
                                            <ext:Model ID="GatewaysModel" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="Id" />
                                                    <ext:ModelField Name="Name" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>

                                <DirectEvents>
                                    <Select OnEvent="GetRates">
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
                                MarginSpec="5 5 5 315">
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
                                Margins="5 5 5 5">
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
                        DisplayMsg="Telephony Rates {0} - {1} of {2}" />
                </BottomBar>

            </ext:GridPanel>
        </div>
    </div>
</asp:Content>

<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
</asp:Content>
