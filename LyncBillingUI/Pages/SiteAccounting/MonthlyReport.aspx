<%@ Page Title="Monthly Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MonthlyReport.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAccounting.MonthlyReport" %>

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
                Title="Monthly Report"
                MaxWidth="955"
                Layout="AnchorLayout">
                
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
                                DisplayField="Name"
                                ValueField="Id"
                                FieldLabel="Site:"
                                LabelWidth="25"
                                Width="160"
                                MarginSpec="5 15 0 5">
                                <Store>
                                    <ext:Store
                                        ID="SitesStore"
                                        runat="server"
                                        OnLoad="SitesStore_Load">
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
                                    <Select OnEvent="FilterReportsBySite_Selecting" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <%--<Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>--%>
                            </ext:ComboBox>

                            <ext:DateField
                                ID="ReportDateField"
                                runat="server"
                                FieldLabel="Date:"
                                LabelWidth="30"
                                EmptyText="Empty Date"
                                Width="150"
                                MarginSpec="5 15 0 5"
                                Disabled="true">

                                <DirectEvents>
                                    <Select OnEvent="ReportDateField_Selection" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Select>
                                </DirectEvents>

                                <%--<Listeners>
                                    <Select Fn="clearFilter" />
                                </Listeners>--%>
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
                                MarginSpec="5 325 0 5"
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

                                <%--<Listeners>
                                    <BeforeSelect Fn="clearFilter" />
                                </Listeners>--%>
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
                MaxWidth="955"
                MinHeight="500"
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
                            <ext:Model ID="MonthlyReportsModel" runat="server" IDProperty="UserSipAccount">
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
                        StoreID="MonthlyReportsStore"
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

