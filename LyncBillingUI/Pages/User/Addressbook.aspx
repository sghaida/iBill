<%@ Page Title="My Address Book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Addressbook.aspx.cs" Inherits="LyncBillingUI.Pages.User.Addressbook" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">

</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <ext:TabPanel
                ID="AddressbookTabsPanel"
                runat="server"
                Title="Address Book"
                MaxWidth="955"
                MinHeight="500"
                Plain="false">

                <Defaults>
                    <ext:Parameter Name="autoScroll" Value="true" Mode="Raw" />
                </Defaults>

                <DirectEvents>
                    <TabChange OnEvent="AddressbookTabsPanel_TabChange"></TabChange>
                </DirectEvents>

                <Items>
                    <%---  
                        ===========================================
                        ===== FIRST TAB
                        ===========================================
                    ---%>
                    <ext:FormPanel 
                        ID="ManageMyAddressBookFormPanel" 
                        runat="server"
                        Split="true"
                        Border="true"
                        Title="Contacts" 
                        MaxWidth="955"
                        MinHeight="580"
                        Icon="BookAddresses"
                        Layout="ColumnLayout">
                        
                        <FieldDefaults LabelAlign="Left" MsgTarget="Side" />

                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:Button
                                        ID="AddNewAddressBookContact"
                                        Text="Add New Contact"
                                        Icon="Add"
                                        runat="server"
                                        Margins="0 5 0 5">
                                        <DirectEvents>
                                            <Click OnEvent="AddNewAddressBookContact_Click" />
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>

                        <Items>
                            <ext:GridPanel
                                ID="AddressBookGrid"
                                runat="server"
                                Layout="FitLayout"
                                MinHeight="580"
                                MaxHeight="580"
                                ColumnWidth="0.7"
                                Scroll="Vertical"
                                AutoScroll="true"
                                ComponentCls="fix-ui-vertical-align">
                                <Store>
                                    <ext:Store
                                        ID="AddressBookStore"
                                        runat="server"
                                        OnLoad="AddressBookStore_Load">
                                        <Model>
                                            <ext:Model ID="AddressBookStoreModel" runat="server" IDProperty="DestinationNumber">
                                                <Fields>
                                                    <ext:ModelField Name="Id" Type="Int" />
                                                    <ext:ModelField Name="SipAccount" Type="String" />
                                                    <ext:ModelField Name="DestinationNumber" Type="String" />
                                                    <ext:ModelField Name="DestinationCountry" Type="String" />
                                                    <ext:ModelField Name="Name" Type="String" />
                                                    <ext:ModelField Name="Type" Type="String" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>

                                <Listeners>
                                    <SelectionChange Handler="if (selected[0]) { this.up('form').getForm().loadRecord(selected[0]); }" />
                                </Listeners>

                                <ColumnModel ID="AddressBookColumnModel" runat="server" Flex="1">
                                    <Columns>
                                        <ext:RowNumbererColumn ID="RowNumbererColumn2" runat="server" Width="25" />

                                        <ext:Column
                                            ID="DestNumber"
                                            runat="server"
                                            Text="Number"
                                            Width="150"
                                            DataIndex="DestinationNumber">
                                            <%--<HeaderItems>
                                                <ext:TextField ID="DestNumberFilter"
                                                    runat="server"
                                                    Icon="Magnifier">
                                                    <Listeners>
                                                        <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                                    </Listeners>
                                                    <Plugins>
                                                        <ext:ClearButton ID="ClearDestNumFilterBtn" runat="server" />
                                                    </Plugins>
                                                </ext:TextField>
                                            </HeaderItems>--%>
                                        </ext:Column>

                                        <ext:Column
                                            ID="Column1" 
                                            runat="server"
                                            DataIndex="Name"
                                            Width="250"
                                            Text="Contact Name"
                                            Selectable="true"
                                            Flex="1">
                                            <%--<HeaderItems>
                                                <ext:TextField ID="ContactNameFilter"
                                                    runat="server"
                                                    Icon="Magnifier">
                                                    <Listeners>
                                                        <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                                    </Listeners>
                                                    <Plugins>
                                                        <ext:ClearButton ID="ClearContactNameFilterBtn" runat="server" />
                                                    </Plugins>
                                                </ext:TextField>
                                            </HeaderItems>--%>
                                        </ext:Column>

                                        <ext:Column ID="Column2"
                                            runat="server"
                                            Text="Contact Information"
                                            MenuDisabled="true"
                                            Sortable="false"
                                            Resizable="false">
                                            <Columns>
                                                <ext:Column
                                                    ID="Column3"
                                                    runat="server"
                                                    Text="Country"
                                                    Width="120"
                                                    DataIndex="DestinationCountry"
                                                    Sortable="true"
                                                    Groupable="true"
                                                    Resizable="false"
                                                    MenuDisabled="true" />

                                                <ext:Column
                                                    ID="Column4"
                                                    runat="server"
                                                    DataIndex="Type"
                                                    Text="Type"
                                                    Width="90"
                                                    Flex="1"
                                                    Selectable="true"
                                                    Sortable="true"
                                                    Groupable="true"
                                                    Resizable="false"
                                                    MenuDisabled="true" />
                                            </Columns>
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                            </ext:GridPanel>

                            <ext:FieldSet
                                runat="server" 
                                ColumnWidth="0.3"
                                Title="Contact Details" 
                                MarginSpec="0 0 0 10"
                                ButtonAlign="Right"
                                MinHeight="180"
                                Layout="VBoxLayout">
                                
                                <Defaults>
                                    <ext:Parameter Name="LabelWidth" Value="115" />
                                </Defaults>

                                <Items>
                                    <ext:Hidden 
                                        runat="server" 
                                        ID="ContactDetails_ContactID" 
                                        Name="Id" 
                                        FieldLabel="Contact ID" 
                                        LabelWidth="60" />
                    
                                    <ext:Hidden 
                                        runat="server" 
                                        ID="ContactDetails_SipAccount" 
                                        Name="SipAccount" 
                                        FieldLabel="SipAccount" 
                                        LabelWidth="60" />

                                    <ext:TextField 
                                        runat="server" 
                                        ID="ContactDetails_ContactName" 
                                        Name="Name" 
                                        FieldLabel="Name" 
                                        LabelWidth="60" />

                                    <ext:TextField 
                                        runat="server" 
                                        ID="ContactDetails_Number"
                                        Name="DestinationNumber" 
                                        FieldLabel="Number" 
                                        LabelWidth="60" 
                                        AllowBlank="false" 
                                        AllowOnlyWhitespace="false" 
                                        MarginSpec="10 0 0 0" />

                                    <ext:ComboBox
                                        ID="ContactDetails_ContactType"
                                        Name="Type"
                                        runat="server"
                                        EmptyText="Please Select Type"
                                        SelectOnFocus="true"
                                        SelectOnTab="true"
                                        Selectable="true"
                                        AllowBlank="false"
                                        AllowOnlyWhitespace="false"
                                        FieldLabel="Type"
                                        LabelWidth="60"
                                        MarginSpec="10 0 0 0">
                                        <Items>
                                            <ext:ListItem Text="Personal" Value="Personal" Mode="Value" />
                                            <ext:ListItem Text="Business" Value="Business" Mode="Value" />
                                        </Items>
                                    </ext:ComboBox>
                    
                                    <ext:TextField 
                                        runat="server" 
                                        ID="ContactDetails_Country" 
                                        Name="DestinationCountry" 
                                        FieldLabel="Country" 
                                        LabelWidth="60" 
                                        MarginSpec="10 0 0 0" />
                                </Items>
                            </ext:FieldSet>
                        </Items>

                        <Buttons>
                            <ext:Button
                                ID="SaveChangesButton"
                                Text="Save"
                                Icon="DatabaseSave"
                                runat="server"
                                Margins="0 5 0 5">
                                <DirectEvents>
                                    <Click OnEvent="SaveChangesButton_DirectEvent" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>

                            <ext:Button
                                ID="CancelChangesButton"
                                Text="Reset"
                                Icon="Cancel"
                                runat="server"
                                Margins="0 5 0 5">
                                <DirectEvents>
                                    <Click OnEvent="CancelChangesButton_DirectEvent">
                                        <EventMask ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>

                            <ext:Button
                                ID="DeleteContactButton"
                                Text="Delete"
                                Icon="Delete"
                                runat="server"
                                Margins="0 5 0 5">
                                <DirectEvents>
                                    <Click OnEvent="DeleteContactButton_DirectEvent" Timeout="500000">
                                        <EventMask ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Buttons>
                    </ext:FormPanel>


                    <%---  
                        ===========================================
                        ===== SECOND TAB
                        ===========================================
                    ---%>
                    <ext:GridPanel
                        ID="ImportContactsFromHistoryGrid"
                        runat="server"
                        Header="true"
                        Title="Import from History"
                        Border="true"
                        MaxWidth="955"
                        MinHeight="580"
                        Layout="FitLayout"
                        AutoScroll="false"
                        Icon="BookEdit">
                        <Store>
                            <ext:Store
                                ID="ImportContactsFromHistoryStore"
                                runat="server"
                                IsPagingStore="true"
                                PageSize="25"
                                OnLoad="ImportContactsFromHistoryStore_Load">
                                <Model>
                                    <ext:Model ID="ImportContactsStoreModel" runat="server" IDProperty="DestinationNumber">
                                        <Fields>
                                            <ext:ModelField Name="ID" Type="Int" />
                                            <ext:ModelField Name="SipAccount" Type="String" />
                                            <ext:ModelField Name="DestinationNumber" Type="String" />
                                            <ext:ModelField Name="DestinationCountry" Type="String" />
                                            <ext:ModelField Name="Name" Type="String" />
                                            <ext:ModelField Name="Type" Type="String" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>

                        <Plugins>
                            <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2" />
                        </Plugins>

                        <ColumnModel ID="ImportContactsColumnModel" runat="server" Flex="1">
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererColumn1" runat="server" Width="25" />
                                <ext:Column
                                    ID="ImportedContactNumber"
                                    runat="server"
                                    Text="Number"
                                    Width="150"
                                    DataIndex="DestinationNumber">
                                    <%--<HeaderItems>
                                        <ext:TextField ID="ImportContactNumberFilter"
                                            runat="server"
                                            Icon="Magnifier">
                                            <Listeners>
                                                <Change Handler="ImportContactGrid_ApplyFilter(this);" Buffer="250" />                                                
                                            </Listeners>
                                            <Plugins>
                                                <ext:ClearButton ID="ClearButton1" runat="server" />
                                            </Plugins>
                                        </ext:TextField>
                                    </HeaderItems>--%>
                                </ext:Column>

                                <ext:Column
                                    ID="ImportedContactDestinationCountry"
                                    runat="server"
                                    Text="Country"
                                    Width="130"
                                    DataIndex="DestinationCountry"
                                    Sortable="true"
                                    Resizable="false"
                                    Groupable="true">
                                    <%--<HeaderItems>
                                        <ext:TextField ID="ImportContactDestCountryFilter"
                                            runat="server"
                                            Icon="Magnifier">
                                            <Listeners>
                                                <Change Handler="ImportContactGrid_ApplyFilter(this);" Buffer="250" />                                                
                                            </Listeners>
                                            <Plugins>
                                                <ext:ClearButton ID="ClearCountryNameFilterBtn" runat="server" />
                                            </Plugins>
                                        </ext:TextField>
                                    </HeaderItems>--%>
                                </ext:Column>

                                <ext:Column ID="ContactInformationColumn"
                                    runat="server"
                                    Text="Contact Information"
                                    MenuDisabled="true"
                                    Sortable="false"
                                    Resizable="false">
                                    <Columns>
                                        <ext:Column
                                            ID="ContanctNameCol" 
                                            runat="server"
                                            DataIndex="Name"
                                            Width="250"
                                            Text="Name"
                                            Selectable="true"
                                            Flex="1"
                                            Resizable="false">
                                            <Editor>
                                                <ext:TextField
                                                    ID="NameTextbox"
                                                    runat="server"
                                                    DataIndex="Name" />
                                            </Editor>
                                        </ext:Column>

                                        <ext:Column
                                            ID="ContactTypeCol"
                                            runat="server"
                                            DataIndex="Type"
                                            Text="Type"
                                            Width="110"
                                            Flex="1"
                                            Selectable="true"
                                            Resizable="false">
                                            <Editor>
                                                <ext:ComboBox
                                                    ID="ContactTypeDropdown"
                                                    runat="server"
                                                    DataIndex="Type"
                                                    EmptyText="Please Select Type"
                                                    SelectOnFocus="true"
                                                    SelectOnTab="true"
                                                    Selectable="true"
                                                    Width="110">
                                                    <Items>
                                                        <ext:ListItem Text="Personal" Value="Personal" Mode="Value" />
                                                        <ext:ListItem Text="Business" Value="Business" Mode="Value" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                    </Columns>
                                </ext:Column>

                                <ext:CommandColumn ID="RejectChanges" runat="server" Width="70">
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

                        <SelectionModel>
                            <ext:RowSelectionModel ID="ImportContactsGridRowSelectionModel"
                                runat="server"
                                Mode="Single"
                                AllowDeselect="true"
                                CheckOnly="true">
                            </ext:RowSelectionModel>
                        </SelectionModel>

                        <TopBar>
                            <ext:Toolbar
                                ID="ImportContactsToolbar"
                                runat="server">
                                <Items>
                                    <ext:Button
                                        ID="ImportItems"
                                        Text="Sync with Address Book"
                                        Icon="Add"
                                        runat="server"
                                        Margins="0 5 0 5">
                                        <DirectEvents>
                                            <Click OnEvent="ImportContactsFromHistory">
                                                <EventMask ShowMask="true" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="
                                                        (#{ImportContactsGrid}.getRowsValues(true))" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:ToolbarSeparator runat="server" />

                                    <ext:Button
                                        ID="CancelImportContactsChangesChanges"
                                        Text="Cancel Changes"
                                        Icon="Cancel"
                                        runat="server"
                                        Margins="0 5 0 5">
                                        <DirectEvents>
                                            <Click OnEvent="RejectImportChanges_DirectEvent">
                                                <EventMask ShowMask="true" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>

                        <BottomBar>
                            <ext:PagingToolbar
                                ID="ImportContactsPagingBottomBar"
                                runat="server"
                                StoreID="PhoneCallStore"
                                DisplayInfo="true"
                                Weight="25"
                                DisplayMsg="Contacts {0} - {1} of {2}" />
                        </BottomBar>
                    </ext:GridPanel>


                    <%---  
                        ===========================================
                        ===== THIRD TAB
                        ===========================================
                    ---%>
                    <ext:GridPanel
                        ID="ImportContactsFromOutlookGrid"
                        runat="server"
                        Layout="FitLayout"
                        Border="true"
                        Title="Import from Outlook" 
                        MaxWidth="955"
                        MinHeight="580"
                        Icon="Mail">
                        <Store>
                            <ext:Store
                                ID="ImportContactsFromOutlookStore"
                                runat="server"
                                IsPagingStore="true"
                                PageSize="25"
                                OnLoad="ImportContactsFromOutlookStore_Load">
                                <Model>
                                    <ext:Model ID="Model1" runat="server" IDProperty="DestinationNumber">
                                        <Fields>
                                            <ext:ModelField Name="Id" Type="Int" />
                                            <ext:ModelField Name="SipAccount" Type="String" />
                                            <ext:ModelField Name="DestinationNumber" Type="String" />
                                            <ext:ModelField Name="DestinationCountry" Type="String" />
                                            <ext:ModelField Name="Name" Type="String" />
                                            <ext:ModelField Name="Type" Type="String" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>

                        <%--<Listeners>
                            <SelectionChange Handler="if (selected[0]) { this.up('form').getForm().loadRecord(selected[0]); }" />
                        </Listeners>--%>

                        <ColumnModel ID="ColumnModel1" runat="server" Flex="1">
                            <Columns>
                                <ext:RowNumbererColumn ID="RowNumbererColumn3" runat="server" Width="25" />

                                <ext:Column
                                    ID="Column5"
                                    runat="server"
                                    Text="Number"
                                    MinWidth="150"
                                    MaxWidth="200"
                                    DataIndex="DestinationNumber">
                                    <%--<HeaderItems>
                                        <ext:TextField ID="DestNumberFilter"
                                            runat="server"
                                            Icon="Magnifier">
                                            <Listeners>
                                                <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                            </Listeners>
                                            <Plugins>
                                                <ext:ClearButton ID="ClearDestNumFilterBtn" runat="server" />
                                            </Plugins>
                                        </ext:TextField>
                                    </HeaderItems>--%>
                                </ext:Column>

                                <ext:Column
                                    ID="Column6" 
                                    runat="server"
                                    DataIndex="Name"
                                    MinWidth="150"
                                    Text="Contact Name"
                                    Selectable="true"
                                    Flex="1">
                                    <%--<HeaderItems>
                                        <ext:TextField ID="ContactNameFilter"
                                            runat="server"
                                            Icon="Magnifier">
                                            <Listeners>
                                                <Change Handler="applyFilter(this);" Buffer="250" />                                                
                                            </Listeners>
                                            <Plugins>
                                                <ext:ClearButton ID="ClearContactNameFilterBtn" runat="server" />
                                            </Plugins>
                                        </ext:TextField>
                                    </HeaderItems>--%>
                                </ext:Column>

                                <ext:Column ID="Column7"
                                    runat="server"
                                    Text="Contact Information"
                                    MenuDisabled="true"
                                    Sortable="false"
                                    Resizable="false"
                                    MinWidth="220"
                                    MaxHeight="300">
                                    <Columns>
                                        <ext:Column
                                            ID="Column8"
                                            runat="server"
                                            Text="Country"
                                            Width="120"
                                            DataIndex="DestinationCountry"
                                            Sortable="true"
                                            Groupable="true"
                                            Resizable="false"
                                            MenuDisabled="true" />

                                        <ext:Column
                                            ID="Column9"
                                            runat="server"
                                            DataIndex="Type"
                                            Text="Type"
                                            Width="100"
                                            Flex="1"
                                            Selectable="true"
                                            Sortable="true"
                                            Groupable="true"
                                            Resizable="false"
                                            MenuDisabled="true" />
                                    </Columns>
                                </ext:Column>
                            </Columns>
                        </ColumnModel>

                        <SelectionModel>
                            <ext:RowSelectionModel ID="CheckboxSelectionModel1"
                                runat="server"
                                Mode="Multi"
                                AllowDeselect="true"
                                IgnoreRightMouseSelection="true"
                                CheckOnly="true">
                            </ext:RowSelectionModel>
                        </SelectionModel>

                        <BottomBar>
                            <ext:PagingToolbar
                                ID="OutlookContactsPaginBottomBar"
                                runat="server"
                                StoreID="PhoneCallStore"
                                DisplayInfo="true"
                                Weight="25"
                                DisplayMsg="Outlook Contacts {0} - {1} of {2}" />
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:TabPanel>
        </div>
    </div>

    <%---  
        ===========================================
        ===== CONTEXT WINDOW (POPUP)
        ===========================================
    ---%>
    <ext:Window 
        ID="AddNewContactWindowPanel" 
        runat="server" 
        Frame="true"
        Resizable="false"
        Title="New Contact" 
        Hidden="true"
        Width="410"
        Icon="Add" 
        X="100"
        Y="100"
        BodyStyle="background-color: #f9f9f9;"
        ComponentCls="fix-ui-vertical-align">
                                
        <Defaults>
            <ext:Parameter Name="Padding" Value="10" Mode="Raw" />
        </Defaults>

        <Items>
            <ext:TextField
                ID="NewContact_ContactNumber"
                runat="server"
                AllowBlank="false"
                LabelWidth="100"
                Width="380"
                FieldLabel="Contact Number"
                LabelSeparator=":" />

            <ext:ComboBox
                ID="NewContact_ContactType"
                runat="server"
                DisplayField="TypeName"
                ValueField="TypeValue"
                Width="380"
                FieldLabel="Contact Type"
                LabelWidth="100">
                <Items>
                    <ext:ListItem Text="Personal" Value="Personal" Mode="Value" />
                    <ext:ListItem Text="Business" Value="Business" Mode="Value" />
                </Items>

                <SelectedItems>
                    <ext:ListItem Text="Personal" Value="Personal" Mode="Value" />
                </SelectedItems>
            </ext:ComboBox>

            <ext:TextField
                ID="NewContact_ContactName"
                runat="server"
                AllowBlank="false"
                LabelWidth="100"
                Width="380"
                FieldLabel="Full Name"
                LabelSeparator=":" />
        </Items>

        <BottomBar>
            <ext:StatusBar
                ID="NewContactWindowBottomBar"
                runat="server"
                StatusAlign="Right"
                DefaultText=""
                Height="30">
                <Items>
                    <ext:Button
                        ID="AddNewContactButton"
                        runat="server"
                        Text="Add"
                        Icon="ApplicationFormAdd"
                        Height="25">
                        <DirectEvents>
                            <Click OnEvent="AddNewContactButton_Click" Timeout="30000">
                                <EventMask ShowMask="true" Msg="Validating the number, please wait!"/>
                            </Click>
                        </DirectEvents>
                    </ext:Button>

                    <ext:Button
                        ID="CancelNewContactButton"
                        runat="server"
                        Text="Cancel"
                        Icon="Cancel"
                        Height="25">
                        <DirectEvents>
                            <Click OnEvent="CancelNewContactButton_Click" />
                        </DirectEvents>
                    </ext:Button>

                    <ext:ToolbarSeparator
                        ID="AddNewContactWindow_BottomBarSeparator"
                        runat="server" />

                    <ext:ToolbarTextItem
                        ID="NewContact_StatusMessage"
                        runat="server"
                        Height="15"
                        Text=""
                        Margins="0 0 0 5" />
                </Items>
            </ext:StatusBar>
        </BottomBar>

        <DirectEvents>
            <BeforeHide OnEvent="AddNewContactWindowPanel_BeforeHide" />
        </DirectEvents>
    </ext:Window>
</asp:Content>


<asp:Content ID="AfterBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">

</asp:Content>
