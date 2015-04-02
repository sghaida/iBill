<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisputedCalls.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAccounting.DisputedCalls" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">

</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <ext:GridPanel
                ID="ManageDisputedCallsGrid"
                runat="server"
                Title="Manage Disputed Calls"
                Width="740"
                Height="775"
                AutoScroll="true"
                Header="true"
                Scroll="Both"
                Layout="FitLayout"
                ComponentCls="fix-ui-vertical-align">
                
                <Store>
                    <ext:Store 
                        ID="DisputedCallsStore" 
                        runat="server" 
                        IsPagingStore="true" 
                        PageSize="25"
                        GroupField="ChargingParty"
                        OnSubmitData="DisputedCallsStore_SubmitData">
                        <Model>
                            <ext:Model ID="DisputedCallsModel" runat="server" IDProperty="SessionIdTime">
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

                <ColumnModel ID="DisputedCallsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column
                            ID="SessionIdTime"
                            runat="server"
                            Text="Date"
                            Width="150"
                            DataIndex="SessionIdTime">
                            <Renderer Fn="formatDate" />
                        </ext:Column>

                        <ext:Column
                            ID="MarkerCallToCountry"
                            runat="server"
                            Text="Country Code"
                            Width="90"
                            DataIndex="MarkerCallToCountry"
                            Align="Center" />

                        <ext:Column
                            ID="DestinationNumberUri"
                            runat="server"
                            Text="Destination"
                            Width="120"
                            DataIndex="DestinationNumberUri" />

                        <ext:Column
                            ID="Duration"
                            runat="server"
                            Text="Duration"
                            Width="80"
                            DataIndex="Duration">
                            <Renderer Fn="GetMinutes" />
                        </ext:Column>

                        <ext:Column
                            ID="MarkerCallCost"
                            runat="server"
                            Text="Cost"
                            Width="60"
                            DataIndex="MarkerCallCost">
                            <Renderer Fn="RoundCost"/>
                        </ext:Column>

                        <ext:Column
                            ID="UiMarkedOn"
                            runat="server"
                            Text="Allocated On"
                            Width="100"
                            DataIndex="UiMarkedOn">
                            <Renderer Handler="return Ext.util.Format.date(value, 'd M Y');" />
                        </ext:Column>

                         <ext:Column 
                             ID="AcDisputeStatus"
                             runat="server"
                             Text="Status"
                             Width="90"
                             DataIndex="AcDisputeStatus">
                             <Renderer fn="getRowClassForstatus" />
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="DisputedCallsCheckboxSelectionModel" 
                        runat="server" 
                        Mode="Multi" 
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>

                <TopBar>
                    <ext:Toolbar ID="DisputedCallsToolbar" runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterDisputedCallsBySite"
                                runat="server"
                                Icon="Find"
                                TriggerAction="Query"
                                QueryMode="Local"
                                DisplayField="SiteName"
                                RawText="SiteName"
                                ValueField="SiteID"
                                FieldLabel="Site:"
                                LabelWidth="25"
                                Width="200"
                                Margins="0 0 0 5"
                                ComponentCls="fix-ui-vertical-align float-left mr10">
                                <Store>
                                    <ext:Store ID="FilterDisputedCallsBySiteStore" runat="server" OnLoad="FilterDisputedCallsBySiteStore_Load">
                                        <Model>
                                            <ext:Model ID="SitesModel" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="SiteID" Type="Int" />
                                                    <ext:ModelField Name="SiteName" Type="String" />
                                                    <ext:ModelField Name="CountryCode" Type="String" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>

                                        <Sorters>
                                            <ext:DataSorter Property="SiteName" Direction="ASC" />
                                        </Sorters>
                                    </ext:Store>
                                </Store>

                                <ListConfig MinWidth="200">
                                    <ItemTpl ID="SitesItemTpl" runat="server">
                                        <Html>
                                            <div data-qtip="{SiteName}. {CountryCode}">
                                                {SiteName} ({CountryCode})
                                            </div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>

                                <DirectEvents>
                                    <Select OnEvent="FilterDisputedCallsBySite_Selecting" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>
                            </ext:ComboBox>


                            <ext:ToolbarSeparator runat="server" Margins="5 15 5 15" />


                            <ext:ComboBox
                                ID="FilterDisputedCallsByType"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Default"
                                DisplayField="TypeName"
                                ValueField="TypeValue"
                                FieldLabel="View Calls:"
                                LabelWidth="60"
                                Width="190"
                                Disabled="true"
                                ComponentCls="fix-ui-vertical-align float-left mr10">
                                <Items>
                                    <ext:ListItem Text="Pending" Value="Pending" />
                                    <ext:ListItem Text="Accepted" Value="Accepted" />
                                    <ext:ListItem Text="Rejected" Value="Rejected" />
                                </Items>

                                <SelectedItems>
                                    <ext:ListItem Text="Pending" Value="Pending" />
                                </SelectedItems>

                                <DirectEvents>
                                    <Select OnEvent="FilterDisputedCallsByType_Selecting" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>
                            </ext:ComboBox>


                            <ext:ToolbarSeparator runat="server" Margins="5 15 5 15" />


                            <ext:Label
                               runat="server"
                               ID="button_group_lable"
                               Margins=""
                               Width="90">
                               <Content>Mark Selected As:</Content>
                            </ext:Label>

                            <ext:ButtonGroup ID="DisputedCallsMarkingBottonsGroup"
                                runat="server"
                                Layout="TableLayout"
                                Width="250"
                                Frame="false"
                                ButtonAlign="Left">
                                <Buttons>
                                    <ext:Button ID="MarkAsAcceptedBtn" Text="Accepted" runat="server" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="AcceptDispute">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManageDisputedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:Button ID="MarkAsRejectedBtn" Text="Rejected" runat="server" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="RejectDispute">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{ManageDisputedCallsGrid}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Buttons>
                            </ext:ButtonGroup>

                            <ext:Button ID="ExportToExcel" runat="server" Text="To Excel" Icon="PageExcel" Margins="0 0 0 290">
                                 <Listeners>
                                    <Click Handler="submitValue(#{ManageDisputedCallsGrid}, 'xls');" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="DisputedCallsPagingToolbar"
                        runat="server"
                        StoreID="DisputedCallsStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Disputed Calls {0} - {1} of {2}" />
                </BottomBar>

                 <Features>               
                   <ext:Grouping ID="Grouping1"
                       runat="server" 
                        HideGroupedHeader="false" 
                        EnableGroupingMenu="false"
                        GroupHeaderTplString='{name} : ({[values.rows.length]} {[values.rows.length > 1 ? "Disputed Calls" : "Disputed"]})' />
                 </Features>
            </ext:GridPanel>
        </div>
    </div>
</asp:Content>

<asp:Content ID="AfterBodyScript" ContentPlaceHolderID="EndOfBodyScripts" runat="server">

</asp:Content>
