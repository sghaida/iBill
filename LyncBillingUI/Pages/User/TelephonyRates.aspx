<%@ Page Title="Telephony Rates" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TelephonyRates.aspx.cs" Inherits="LyncBillingUI.Pages.User.TelephonyRates" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
    <ext:XScript ID="XScript1" runat="server">
        <script>       
            var applyFilter = function (field) {                
                var store = #{ViewRatesGrid}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{CountryCodeFilter}.reset();
                #{CountryNameFilter}.reset();
                
                #{ViewRatesStore}.clearFilter();
            }
 
            var filterString = function (value, dataIndex, record) {
                var val = record.get(dataIndex);
                
                if (typeof val != "string") {
                    return value.length == 0;
                }
                
                return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
            };
 
            var getRecordFilter = function () {
                var f = [];
 
                f.push({
                    filter: function (record) {                         
                        return filterString(#{CountryCodeFilter}.getValue(), "CountryCode", record);
                    }
                });
                 
                f.push({
                    filter: function (record) {                         
                        return filterString(#{CountryNameFilter}.getValue(), "CountryName", record);
                    }
                });
 
                var len = f.length;
                 
                return function (record) {
                    for (var i = 0; i < len; i++) {
                        if (!f[i].filter(record)) {
                            return false;
                        }
                    }
                    return true;
                };
            };
        </script>
    </ext:XScript>
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <ext:GridPanel
                ID="ViewRatesGrid"
                runat="server"
                MaxWidth="955"
                MinHeight="665"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Telephony Rates"
                ComponentCls="fix-ui-vertical-align">

                <Store>
                    <ext:Store
                        ID="ViewRatesStore"
                        runat="server"
                        RemoteSort="true"
                        IsPagingStore="true"
                        PageSize="25">
                        <Model>
                            <ext:Model ID="ViewRatesModel" runat="server" IDProperty="Iso3CountryCode">
                                <Fields>
                                    <ext:ModelField Name="Iso2CountryCode" Type="String" />
                                    <ext:ModelField Name="Iso3CountryCode" Type="String" />
                                    <ext:ModelField Name="CountryName" Type="String" />
                                    <ext:ModelField Name="FixedLineRate" Type="Float" />
                                    <ext:ModelField Name="MobileLineRate" Type="Float" />
                                </Fields>
                            </ext:Model>
                        </Model>

                        <Sorters>
                            <ext:DataSorter Property="CountryName" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <ColumnModel ID="ViewRatesColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:RowNumbererColumn
                            ID="RowNumbererColumn2"
                            runat="server"
                            Width="45" />

                        <ext:Column 
                            runat="server"
                            Text="Country"
                            Width="240"
                            DataIndex="CountryName">
                            <%--<HeaderItems>
                                <ext:TextField ID="CountryNameFilter"
                                    runat="server"
                                    Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearCountryNameFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column
                            runat="server"
                            Text="ISO2 Country Code"
                            Width="120"
                            DataIndex="Iso2CountryCode">
                            <%--<HeaderItems>
                                <ext:TextField ID="CountryCodeFilter"
                                    runat="server"
                                    Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearCountryCodeFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column
                            runat="server"
                            Text="ISO3 Country Code"
                            Width="120"
                            DataIndex="Iso3CountryCode">
                            <%--<HeaderItems>
                                <ext:TextField ID="CountryCodeFilter"
                                    runat="server"
                                    Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearCountryCodeFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column ID="RatesColumn"
                            runat="server"
                            Text="Rates"
                            Align="Center"
                            MenuDisabled="true"
                            Resizable="false"
                            Groupable="false"
                            Sortable="false">
                            <Columns>
                                <ext:Column
                                    runat="server"
                                    Text="Fixedline Rate"
                                    Width="160"
                                    DataIndex="FixedLineRate"
                                    MenuDisabled="true" />

                                <ext:Column
                                    runat="server"
                                    Text="Mobile Rate"
                                    Width="160"
                                    DataIndex="MobileLineRate"
                                    MenuDisabled="true" />
                            </Columns>
                        </ext:Column>

                    </Columns>
                </ColumnModel>

                <TopBar>
                    <ext:Toolbar ID="FilterDelegatesSitesToolBar" runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="GatewaysFilter"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Local"
                                DisplayField="Name"
                                ValueField="Id"
                                FieldLabel="Provider"
                                LabelWidth="50"
                                Width="300"
                                MarginSpec="5 5 5 5">
                                <Store>
                                    <ext:Store
                                        ID="GatewaysFilterStore"
                                        runat="server"
                                        OnLoad="GatewaysFilterStore_Load">
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
                                    <Select OnEvent="GatewaysFilter_Select" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <%--<Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>--%>
                            </ext:ComboBox>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ViewRatesPagingToolbar"
                        runat="server"
                        StoreID="ViewRatesStore"
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
