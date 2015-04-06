<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MonthlyReport.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAccounting.MonthlyReport" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <ext:Hidden ID="FormatType" runat="server" />

            <ext:Panel
                ID="MonthlyReportsTools"
                runat="server"
                Header="true"
                Title="Generate Monthly Reports"
                Width="740"
                Height="56"
                Layout="AnchorLayout"
                ComponentCls="fix-ui-vertical-align">
                
                <TopBar>
                    <ext:Toolbar
                        ID="FilterAndSearthToolbar"
                        runat="server">
                        <Items>
                            <ext:ComboBox
                                ID="FilterReportsBySite"
                                runat="server"
                                Icon="Find"
                                TriggerAction="All"
                                QueryMode="Local"
                                DisplayField="SiteName"
                                ValueField="SiteID"
                                FieldLabel="Site:"
                                LabelWidth="25"
                                Width="160"
                                Margins="5 15 0 5">
                                <Store>
                                    <ext:Store
                                        ID="SitesStore"
                                        runat="server"
                                        OnLoad="SitesStore_Load">
                                        <Model>
                                            <ext:Model ID="SitesModel" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="SiteID" />
                                                    <ext:ModelField Name="SiteName" />
                                                    <ext:ModelField Name="CountryCode" />
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
                                    <Select OnEvent="FilterReportsBySite_Selecting" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>
                            </ext:ComboBox>

                            <ext:DateField
                                ID="ReportDateField"
                                runat="server"
                                FieldLabel="Date:"
                                LabelWidth="30"
                                EmptyText="Empty Date"
                                Width="150"
                                Margins="5 15 0 5"
                                Disabled="true">

                                <DirectEvents>
                                    <Select OnEvent="ReportDateField_Selection" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <Listeners>
                                    <Select Fn="clearFilter" />
                                </Listeners>
                            </ext:DateField>

                            <ext:ComboBox
                                ID="CallsTypesComboBox"
                                runat="server"
                                Icon="Find"
                                DisplayField="TypeName"
                                ValueField="TypeValue"
                                FieldLabel="Display"
                                LabelSeparator=":"
                                LabelWidth="40"
                                Width="200"
                                Margins="5 105 0 5"
                                Disabled="true"
                                Editable="false">
                                <Items>
                                    <ext:ListItem Text="Not Charged" Value="1" />
                                    <ext:ListItem Text="Pending Charges" Value="2" />
                                    <ext:ListItem Text="Charged" Value="3" />
                                </Items>

                                <SelectedItems>
                                    <ext:ListItem Text="Not Charged" Value="1" />
                                </SelectedItems>

                                <DirectEvents>
                                    <Select OnEvent="FilterReportsByCallsTypes_Select" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>
                            </ext:ComboBox>

                            <ext:Button
                                ID="AdvancedToolsMenu"
                                runat="server"
                                Text="Tools"
                                Icon="ApplicationOsxGo"
                                Disabled="true">
                                <Menu>
                                    <ext:Menu ID="ExportReportMenu" runat="server">
                                        <Items>
                                            <ext:MenuItem ID="ExportReportToExcel" runat="server" Text="Export to Excel" Icon="PageExcel">
                                                <Menu>
                                                    <ext:Menu ID="ExportReportToExcelSubMenu" runat="server">
                                                        <Items>
                                                            <ext:MenuItem ID="ExportExcelSummaryReport" runat="server" Text="Summary Report">
                                                                <Listeners>
                                                                    <Click Handler="submitValue(#{MonthlyReportsGrids}, #{FormatType}, 'xls', true);" />
                                                                </Listeners>
                                                            </ext:MenuItem>
                                                        </Items>
                                                    </ext:Menu>
                                                </Menu>
                                            </ext:MenuItem>

                                            <ext:MenuItem ID="ExportReportToPDF" runat="server" Text="Export to PDF" Icon="PagePortrait">
                                                <Menu>
                                                    <ext:Menu ID="ExportReportToPDFSubMenu" runat="server">
                                                        <Items>
                                                            <ext:MenuItem ID="ExportPDFSummaryRreport" runat="server" Text="Summary Report">
                                                                <Listeners>
                                                                    <Click Handler="submitValue(#{MonthlyReportsGrids}, #{FormatType}, 'pdf', true);" />
                                                                </Listeners>
                                                            </ext:MenuItem>

                                                            <ext:MenuItem ID="ExportPDFDetailedRreport" runat="server" Text="Detailed Report">
                                                                <Listeners>
                                                                    <Click Handler="submitValue(#{MonthlyReportsGrids}, #{FormatType}, 'pdf-d', true);" />
                                                                </Listeners>
                                                            </ext:MenuItem>
                                                        </Items>
                                                    </ext:Menu>
                                                </Menu>
                                            </ext:MenuItem>

                                            <ext:MenuSeparator Margin="5" runat="server" />

                                            <ext:MenuItem ID="InvoiceUsers" runat="server" Text="Invoice!" Icon="Money">
                                                <DirectEvents>
                                                    <Click OnEvent="InvoiceUsers_Button_Click" />
                                                </DirectEvents>
                                            </ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                            </ext:Button>

                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:Panel>

            <div class="h5 clear"></div>

            <ext:GridPanel
                ID="MonthlyReportsGrids"
                runat="server"
                Width="740"
                Height="710"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout">

                <Store>
                    <ext:Store
                        ID="MonthlyReportsStore"
                        runat="server"
                        IsPagingStore="true"
                        PageSize="25"
                        OnSubmitData="MonthlyReportsStore_SubmitData">
                        <Model>
                            <ext:Model ID="MonthlyReportsModel" runat="server" IDProperty="SipAccount">
                                <Fields>
                                    <ext:ModelField Name="EmployeeID" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="FullName" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="SipAccount" Type="String" SortType="AsText" />
                                    <ext:ModelField Name="PersonalCallsCost" Type="String" />
                                    <ext:ModelField Name="BusinessCallsCost" Type="String" />
                                    <ext:ModelField Name="UnmarkedCallsCost" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                        <Sorters>
                            <ext:DataSorter Property="SipAccount" Direction="ASC" />
                        </Sorters>
                    </ext:Store>
                </Store>

                <ColumnModel ID="MonthlyReportsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column
                            ID="EmployeeIDCol"
                            runat="server"
                            Text="Employee ID"
                            Width="100"
                            DataIndex="EmployeeID"
                            Sortable="true">
                            <HeaderItems>
                                <ext:TextField ID="EmployeeIDFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearEmployeeIDFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column
                            ID="SipAccountCol"
                            runat="server"
                            Text="Sip Account"
                            Width="160"
                            DataIndex="SipAccount"
                            Sortable="true">
                            <HeaderItems>
                                <ext:TextField ID="SipAccountFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSipAccountFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column
                            ID="FullNameCol"
                            runat="server"
                            Text="Full Name"
                            Width="190"
                            DataIndex="FullName"
                            Sortable="true">
                            <HeaderItems>
                                <ext:TextField ID="FullNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="250" />
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearFullNameFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>
                        </ext:Column>

                        <ext:Column
                            ID="GrouopedCostsColumnsCol"
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
                                    Width="85"
                                    DataIndex="PersonalCallsCost"
                                    MenuDisabled="true">
                                    <Renderer Fn="RoundCostsToTwoDecimalDigits" />
                                </ext:Column>

                                <ext:Column
                                    ID="BusinessCallsCostCol"
                                    runat="server"
                                    Text="Business"
                                    Width="85"
                                    DataIndex="BusinessCallsCost"
                                    MenuDisabled="true">
                                    <Renderer Fn="RoundCostsToTwoDecimalDigits" />
                                </ext:Column>

                                <ext:Column
                                    ID="UnmarkedCallsCostCol"
                                    runat="server"
                                    Text="Unallocated"
                                    Width="85"
                                    DataIndex="UnmarkedCallsCost"
                                    MenuDisabled="true">
                                    <Renderer Fn="RoundCostsToTwoDecimalDigits" />
                                </ext:Column>
                            </Columns>
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1"
                        runat="server"
                        Mode="Multi"
                        AllowDeselect="true"
                        IgnoreRightMouseSelection="true"
                        CheckOnly="true">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="MonthlyReportsPagingToolbar"
                        runat="server"
                        StoreID="PhoneCallStore"
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

