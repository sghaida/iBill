<%@ Page Title="My Address Book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Addressbook.aspx.cs" Inherits="LyncBillingUI.Pages.User.Addressbook" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">

</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <ul id="addressbook-tabs" class="nav nav-pills">
                <li id="my-addressbook-tab" class="active" role="presentation">
                    <a id="my-addressbook-tab-link" href="#" role="button" aria-expanded="false">My Address Book</a>
                </li>

                <li id="import-from-history-tab" role="presentation">
                    <a id="import-from-history-tab-link" href="#" role="button" aria-expanded="false">Import from History</a>
                </li>

                <li id="import-from-outlook-tab" role="presentation">
                    <a id="import-from-outlook-tab-link" href="#" role="button" aria-expanded="false">Import from Outlook</a>
                </li>
            </ul>
        </div>
    </div>
    
    <hr />

    <div id="my-addressbook-tab-body" class="row">
        <div class="col-md-8">
            <ext:GridPanel
                ID="AddressBookGrid"
                runat="server"
                Layout="TableLayout"
                Border="true"
                Frame="true"
                Header="true"
                Title="Contacts"
                Icon="BookAddresses"
                MinHeight="620"
                Width="660"
                ComponentCls="fix-ui-vertical-align">
                <Store>
                    <ext:Store
                        ID="AddressBookStore"
                        runat="server"
                        IsPagingStore="true"
                        PageSize="25"
                        OnLoad="AddressBookStore_Load"
                        OnReadData="AddressBookStore_Refresh">
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

                <SelectionModel>
                    <ext:RowSelectionModel runat="server" Mode="Single">
                        <DirectEvents>
                            <Select OnEvent="AddressBookContactSelect" Buffer="250">
                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{EditAddressBookContactForm}" />

                                <ExtraParams>
                                    <ext:Parameter Name="DestinationNumber" Value="record.getId()" Mode="Raw" />
                                </ExtraParams>
                            </Select>
                        </DirectEvents>
                    </ext:RowSelectionModel>
                </SelectionModel>

                <%--<Listeners>
                    <SelectionChange Handler="if (selected[0]) { this.up('form').getForm().loadRecord(selected[0]); }" />
                </Listeners>--%>

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

                <TopBar>
                    <ext:Toolbar ID="AddressBookToolbar" runat="server">
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
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingToolbar1"
                        runat="server"
                        StoreID="PhoneCallStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="Contacts {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>
        </div><!-- ./col-md-6 -->

        <div class="col-md-4">
            <div class="row">
                <div class="col-md-12">
                    <ext:FormPanel 
                        ID="EditAddressBookContactForm" 
                        runat="server" 
                        Region="East"
                        Split="true"
                        MarginSpec="0 5 5 5"
                        BodyPadding="2"
                        Border="true"
                        Frame="true" 
                        Title="Contact Details" 
                        Width="300"
                        Height="320"
                        Icon="User"
                        DefaultAnchor="100%" 
                        AutoScroll="True">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:Button
                                        ID="SaveChangesButton"
                                        Text="Save"
                                        Icon="DatabaseSave"
                                        runat="server"
                                        Margins="0 5 0 5">
                                        <DirectEvents>
                                            <Click OnEvent="SaveChangesButton_DirectEvent">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{EditAddressBookContactForm}" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:ToolbarSeparator runat="server" />

                                    <ext:Button
                                        ID="CancelChangesButton"
                                        Text="Reset"
                                        Icon="Cancel"
                                        runat="server"
                                        Margins="0 5 0 5">
                                        <DirectEvents>
                                            <Click OnEvent="CancelChangesButton_DirectEvent">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{EditAddressBookContactForm}" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:ToolbarSeparator runat="server" />

                                    <ext:Button
                                        ID="DeleteContactButton"
                                        Text="Delete"
                                        Icon="Delete"
                                        runat="server"
                                        Margins="0 5 0 5">
                                        <DirectEvents>
                                            <Click OnEvent="DeleteContactButton_DirectEvent">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{EditAddressBookContactForm}" />
                                            </Click>
                                        </DirectEvents>

                                        <%--<Listeners>
                                            <Click Handler="this.up('form').getForm().reset();" />
                                        </Listeners>--%>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>

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
                                LabelWidth="60" 
                                MarginSpec="10 0 0 0" />

                            <ext:ComboBox
                                ID="ContactDetails_ContactType"
                                Name="Type"
                                runat="server"
                                DataIndex="Type"
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
                    
                            <ext:TextField 
                                runat="server" 
                                ID="ContactDetails_Number"
                                Name="DestinationNumber" 
                                FieldLabel="Number" 
                                LabelWidth="60" 
                                AllowBlank="false" 
                                AllowOnlyWhitespace="false" 
                                MarginSpec="10 0 0 0" />
                        </Items>
                    </ext:FormPanel>
                </div>
            </div>

            <br />

            <%--<div class="row">
                <div class="col-md-12">
                    <ext:FormPanel 
                        ID="FormPanel1" 
                        runat="server" 
                        Region="East"
                        Split="true"
                        MarginSpec="0 5 5 5"
                        BodyPadding="2"
                        Frame="true" 
                        Title="New Contact" 
                        Width="300"
                        Height="220"
                        Icon="User"
                        DefaultAnchor="100%" 
                        AutoScroll="True">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar2" runat="server">
                                <Items>
                                    <ext:Button
                                        ID="Button1"
                                        Text="Save Changes"
                                        Icon="DatabaseSave"
                                        runat="server"
                                        Margins="0 5 0 5">
                                        <DirectEvents>
                                            <Click OnEvent="UpdateAddressBook_DirectEvent">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{EditAddressBookContactForm}" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>

                                    <ext:ToolbarSeparator runat="server" />

                                    <ext:Button
                                        ID="Button2"
                                        Text="Cancel Changes"
                                        Icon="Cancel"
                                        runat="server"
                                        Margins="0 5 0 5">
                                        <DirectEvents>
                                            <Click OnEvent="RejectAddressBookChanges_DirectEvent">
                                                <EventMask ShowMask="true" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>

                        <Items>
                            <ext:TextField runat="server" Name="Id" FieldLabel="Contact ID" LabelWidth="60" Visible="false" />
                    
                            <ext:TextField runat="server" Name="SipAccount" FieldLabel="SipAccount" LabelWidth="60" Visible="false" />

                            <ext:TextField runat="server" Name="Name" FieldLabel="Name" LabelWidth="60" MarginSpec="10 0 0 0" />

                            <ext:ComboBox
                                Name="Type"
                                runat="server"
                                DataIndex="Type"
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
                    
                            <ext:TextField runat="server" Name="DestinationCountry" FieldLabel="Country" LabelWidth="60" MarginSpec="10 0 0 0" />
                    
                            <ext:TextField runat="server" Name="DestinationNumber" FieldLabel="Number" LabelWidth="60" AllowBlank="false" AllowOnlyWhitespace="false" MarginSpec="10 0 0 0" />
                        </Items>
                    </ext:FormPanel>
                </div>
            </div>--%>
        </div><!-- ./col-md-6 -->
    </div>



    <div id="import-from-history-tab-body" class="row hidden">
        <div class="col-md-12">
            <ext:GridPanel
                ID="ImportContactsGrid"
                runat="server"
                Header="true"
                Border="true"
                Frame="true"
                Layout="TableLayout"
                Title="History of Destinations"
                MinHeight="580"
                Icon="BookEdit"
                ComponentCls="fix-ui-vertical-align">
                <Store>
                    <ext:Store
                        ID="ImportContactsStore"
                        runat="server"
                        IsPagingStore="true"
                        PageSize="25"
                        OnLoad="ImportContactsStore_Load">
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
        </div>
    </div>
</asp:Content>


<asp:Content ID="AfterBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            //
            // my-phone-calls-tab-link
            $("#my-addressbook-tab-link").click(function () {

                $("#addressbook-tabs li").each(function () {
                    $(this).removeClass("active");
                });

                $("#import-from-history-tab-body").addClass("hidden");
                $("#my-addressbook-tab").addClass("active");
                $("#my-addressbook-tab-body").removeClass("hidden");
            });


            //
            // my-department-phone-calls-tab-link
            $("#import-from-history-tab-link").click(function () {

                $("#addressbook-tabs li").each(function () {
                    $(this).removeClass("active");
                });

                $("#my-addressbook-tab-body").addClass("hidden");
                $("#import-from-history-tab").addClass("active");
                $("#import-from-history-tab-body").removeClass("hidden");
            });
        });
    </script>
</asp:Content>
