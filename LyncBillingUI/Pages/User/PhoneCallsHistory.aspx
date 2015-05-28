<%@ Page Title="My Phone Calls History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhoneCallsHistory.aspx.cs" Inherits="LyncBillingUI.Pages.User.PhoneCallsHistory" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
    <style type="text/css">
        .x-grid-cell-fullName .x-grid-cell-inner { font-family: tahoma, verdana; display: block; font-weight: normal; font-style: normal; color:#385F95; white-space: normal; }
        .x-grid-rowbody div { margin: 2px 5px 20px 5px !important; width: 99%; color: Gray; }
        .x-grid-row-expanded td.x-grid-cell { border-bottom-width: 0px; }
    </style>
</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <ext:Hidden ID="FormatType" runat="server" />

            <ext:GridPanel
                ID="PhoneCallsHistoryGrid" 
                runat="server"
                Header="true" 
                Title="Phone Calls History"
                MaxWidth="955"
                MinHeight="665"
                Layout="TableLayout"
                ComponentCls="fix-ui-vertical-align">
                <Store>
                    <ext:Store
                        ID="PhoneCallStore" 
                        runat="server" 
                        PageSize="25"
                        IsPagingStore="true"
                        OnLoad="PhoneCallStore_Load">
                        <Model>
                            <ext:Model ID="PhoneCallStoreModel" runat="server" IDProperty="SessionIdTime">
                                <Fields>
                                    <ext:ModelField Name="ChargingParty" Type="String" />
                                    <ext:ModelField Name="SessionIdTime" Type="Date" />
                                    <ext:ModelField Name="SessionIdSeq" Type="Int" />
                                    <ext:ModelField Name="ResponseTime" Type="Date" />
                                    <ext:ModelField Name="SessionEndTime" Type="Date" />
                                    <ext:ModelField Name="MarkerCallToCountry" Type="String" />
                                    <ext:ModelField Name="MarkerCallType" Type="String" />
                                    <ext:ModelField Name="DestinationNumberUri" Type="String" />
                                    <ext:ModelField Name="Duration" Type="Float" />
                                    <ext:ModelField Name="MarkerCallCost" Type="Float" />
                                    <ext:ModelField Name="UiAssignedByUser" Type="String" />
                                    <ext:ModelField Name="UiAssignedToUser" Type="String" />
                                    <ext:ModelField Name="UiAssignedOn" Type="Date" />
                                    <ext:ModelField Name="UiCallType" Type="String" />
                                    <ext:ModelField Name="UiMarkedOn" Type="Date" />
                                    <ext:ModelField Name="PhoneBookName" Type="String" />
                                    <ext:ModelField Name="PhoneCallsTableName" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <Plugins>
                    <ext:FilterHeader runat="server" />
                </Plugins>

                <ColumnModel ID="PhoneCallsColumnModel" runat="server" Flex="1">
		            <Columns>
                        <ext:RowNumbererColumn
                            ID="RowNumbererColumn2"
                            runat="server"
                            Width="45" />

                        <ext:Column ID="SessionIdTime" 
                            runat="server" 
                            Text="Date" 
                            Width="140" 
                            DataIndex="SessionIdTime"
                            Groupable="false">
                            <Renderer Fn="DateRenderer" />
                        </ext:Column>

                        <ext:Column ID="Marker_CallToCountry"
                            runat="server"
                            Text="Country"
                            Width="100"
                            DataIndex="Marker_CallToCountry" 
                            Groupable="true">
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

                        <ext:Column ID="DestinationNumberUri"
                            runat="server"
                            Text="Destination"
                            Width="120"
                            DataIndex="DestinationNumberUri"
                            Groupable="true">
                            <%--<HeaderItems>
                                <ext:TextField ID="DestinationNumberFilter"
                                    runat="server"
                                    Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDestinationNumberFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>
                        </ext:Column>

                        <ext:Column ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="90"
                            DataIndex="Duration"
                            Groupable="false" >
                            <Renderer Fn="GetMinutes"/>
                        </ext:Column>

                        <ext:Column ID="Marker_CallCost"
                            runat="server"
                            Text="Cost"
                            Width="60"
                            DataIndex="Marker_CallCost"
                            Groupable="false">
                            <Renderer Fn="RoundCost" />
                        </ext:Column>

                        <ext:Column ID="UI_CallType"
                            runat="server"
                            Text="Type"
                            Width="80"
                            DataIndex="UI_CallType" 
                            Groupable="false">
                            <Renderer Fn="getCssColorForPhoneCallRow" />
                        </ext:Column>

                        <ext:Column ID="UI_MarkedOn"
                            runat="server"
                            Text="Updated On"
                            Width="100"
                            DataIndex="UI_MarkedOn"
                             Groupable="true">
                            <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');"/>
                        </ext:Column>
		            </Columns>
                </ColumnModel>

                <TopBar>
                    <ext:Toolbar ID="FilterToolBar" runat="server">
                        <Items>
                            <ext:ComboBox 
                                ID="FilterTypeComboBox" 
                                runat="server" 
                                Icon="Find" 
                                TriggerAction="All" 
                                QueryMode="Local" 
                                DisplayField="TypeName" 
                                ValueField="TypeValue"
                                Width="200"
                                FieldLabel="View:"
                                LabelWidth="30"
                                MarginSpec="5 5 5 5">
                                
                                <Items>
                                    <ext:ListItem Text="Everything" Value="1"/>
                                    <ext:ListItem Text="Business" Value="2" />
                                    <ext:ListItem Text="Personal" Value="3" />
                                </Items>

                                <DirectEvents>
                                    <Select OnEvent="PhoneCallsHistoryFilter">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <%--<Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>--%>

                                <SelectedItems>
                                    <ext:ListItem Text="Everything" Value="1" />
                                </SelectedItems>
                            </ext:ComboBox>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar 
                        ID="PhoneCallsPagingToolbar" 
                        runat="server" 
                        StoreID="PhoneCallStore" 
                        DisplayInfo="true" 
                        Weight="25" 
                        DisplayMsg="Phone Calls {0} - {1} of {2}"
                         />
                </BottomBar>

            </ext:GridPanel>
        </div>
        <!-- *** END OF PHONE CALLS HISTORY GRID *** -->
    </div>
</asp:Content>

<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">

</asp:Content>
