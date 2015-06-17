<%@ Page Title="Exclusions List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExclusionsList.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAdministration.ExclusionsList" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <ext:GridPanel
        ID="ManageExceptionsListGrid"
        runat="server"
        MaxWidth="955"
        MinHeight="620"
        AutoScroll="true"
        Scroll="Both"
        Layout="FitLayout"
        Header="true"
        Title="Manage Exceptions"
        ComponentCls="fix-ui-vertical-align">

        <Store>
            <ext:Store
                ID="ManageExceptionsListStore"
                runat="server"
                RemoteSort="true"
                IsPagingStore="true"
                PageSize="25">
                <Model>
                    <ext:Model ID="ManageExceptionsListStoreModel" runat="server" IDProperty="ID">
                        <Fields>
                            <ext:ModelField Name="Id" Type="String" />
                            <ext:ModelField Name="ExclusionSubject" Type="String" />
                            <ext:ModelField Name="ExclusionType" Type="String" />
                            <ext:ModelField Name="ZeroCost" Type="String" />
                            <ext:ModelField Name="AutoMark" Type="String" />
                            <ext:ModelField Name="SiteId" Type="Auto" />
                            <ext:ModelField Name="Description" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>
        </Store>

        <Plugins>
            <ext:RowEditing ID="ManageDepartments_RowEditing" runat="server" ClicksToEdit="2" />
        </Plugins>

        <ColumnModel ID="ManageDelegatesColumnModel" runat="server" Flex="1">
            <Columns>
                        
                <ext:Column ID="EntityColumn"
                    runat="server"
                    Text="Phone Exception"
                    Width="200"
                    DataIndex="ExclusionSubject"
                    Sortable="false"
                    Groupable="false">
                    <Editor>
                        <ext:TextField
                            ID="Editor_Entity_TextField"
                            runat="server"
                            DataIndex="ExclusionSubject" />
                    </Editor>

                    <Renderer Fn="PhoneNumbersRenderer" />
                </ext:Column>

                <ext:Column ID="EntityTypeColumn"
                    runat="server"
                    Text="Type"
                    Width="120"
                    DataIndex="ExclusionType"
                    Sortable="false"
                    Groupable="false">
                    <Editor>
                        <ext:ComboBox
                            ID="Editor_EntityType_Combobox"
                            runat="server"
                            DataIndex="ExclusionType"
                            Editable="false">
                            <Items>
                                <ext:ListItem Text="Source" Value="Source" />
                                <ext:ListItem Text="Destination" Value="Destination"/>
                            </Items>
                        </ext:ComboBox>
                    </Editor>
                </ext:Column>

                <ext:Column ID="ZeroCostColumn"
                    runat="server"
                    Text="Zero Cost"
                    Width="120"
                    DataIndex="ZeroCost"
                    Sortable="false"
                    Groupable="false">
                    <Editor>
                        <ext:ComboBox
                            ID="Editor_ZeroCost_Combobox"
                            runat="server"
                            DataIndex="ZeroCost"
                            Editable="false">
                            <Items>
                                <ext:ListItem Text="Yes" Value="Yes" />
                                <ext:ListItem Text="No" Value="No"/>
                            </Items>
                        </ext:ComboBox>
                    </Editor>
                </ext:Column>

                <ext:Column ID="AutoMarkColumn"
                    runat="server"
                    Text="Auto-Mark"
                    Width="120"
                    DataIndex="AutoMark"
                    Sortable="false"
                    Groupable="false">
                    <Editor>
                        <ext:ComboBox
                            ID="Editor_AutoMark_Combobox"
                            runat="server"
                            DataIndex="AutoMark"
                            Editable="false">
                            <Items>
                                <ext:ListItem Text="As Business" Value="Business" />
                                <ext:ListItem Text="As Personal" Value="Personal" />
                                <ext:ListItem Text="DISABLED" Value="DISABLED" />
                            </Items>
                        </ext:ComboBox>
                    </Editor>
                </ext:Column>

                <ext:Column ID="DescriptionColumn"
                    runat="server"
                    Text="Description"
                    Width="280"
                    DataIndex="Description"
                    Sortable="false"
                    Groupable="false">
                    <Editor>
                        <ext:TextField
                            ID="Editor_Description_TextField"
                            runat="server"
                            DataIndex="Description" />
                    </Editor>
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

                <ext:ImageCommandColumn
                    ID="DeleteButtonsColumn"
                    runat="server"
                    Width="30"
                    Align="Center"
                    Sortable="false"
                    Groupable="false">
                    <Commands>
                        <ext:ImageCommand Icon="Decline" ToolTip-Text="Delete Exception" CommandName="delete">                            
                        </ext:ImageCommand>
                    </Commands>
                    <Listeners>
                        <Command Handler="this.up(#{ManageExceptionsListGrid}.store.removeAt(recordIndex));" />
                    </Listeners>
                </ext:ImageCommandColumn>

            </Columns>
        </ColumnModel>

        <TopBar>
            <ext:Toolbar ID="FilterDelegatesExceptionsToolBar" runat="server">
                <Items>
                    <ext:ComboBox
                        ID="FilterExceptionsBySite"
                        runat="server"
                        Icon="Find"
                        TriggerAction="All"
                        QueryMode="Local"
                        Editable="true"
                        DisplayField="Name"
                        ValueField="Id"
                        FieldLabel="Site"
                        LabelWidth="25"
                        Margins="5 5 0 5"
                        Width="250">
                        <Store>
                            <ext:Store
                                ID="FilterExceptionsBySiteStore"
                                runat="server"
                                OnLoad="FilterExceptionsBySiteStore_Load">
                                <Model>
                                    <ext:Model ID="SiteModel" runat="server">
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
                                    <div>{Name}&nbsp;({CountryCode})</div>
                                </Html>
                            </ItemTpl>
                        </ListConfig>

                        <DirectEvents>
                            <Select OnEvent="FilterExceptionsBySite_Selected">
                                <EventMask ShowMask="true" />
                            </Select>
                        </DirectEvents>
                    </ext:ComboBox>

                    <ext:Button
                        ID="AddExceptionButton"
                        runat="server"
                        Text="New Exception"
                        Icon="Add"
                        Margins="5 5 0 250">
                        <DirectEvents>
                            <Click OnEvent="AddExceptionButton_Click" />
                        </DirectEvents>
                    </ext:Button>

                    <ext:ToolbarSeparator
                        ID="ToolbarSeparaator"
                        runat="server" />

                    <ext:Button
                        ID="SaveChangesButton"
                        runat="server"
                        Text="Save Changes"
                        Icon="DatabaseSave"
                        Margins="5 5 0 5">
                        <DirectEvents>
                            <Click OnEvent="SaveChanges_DirectEvent" before="return #{ManageExceptionsListStore}.isDirty();">
                                <EventMask ShowMask="true" />
                                <ExtraParams>
                                    <ext:Parameter Name="Values" Value="#{ManageExceptionsListStore}.getChangedData()" Mode="Raw" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>

        <BottomBar>
            <ext:PagingToolbar
                ID="ManageExceptionsListPagingToolbar"
                runat="server"
                StoreID="ManageExceptionsListStore"
                DisplayInfo="true"
                Weight="25"
                DisplayMsg="Exceptions {0} - {1} of {2}" />
        </BottomBar>
    </ext:GridPanel>


    <ext:Window 
        ID="AddNewExceptionWindowPanel" 
        runat="server" 
        Frame="true"
        Resizable="false"
        Title="New Exception" 
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
                ID="NewException_ExceptionEntity"
                runat="server"
                EmptyText="Empty Exception"
                Width="380"
                FieldLabel="Exception For"
                LabelSeparator=":"
                LabelWidth="115"
                AllowBlank="false" />

            <ext:ComboBox
                ID="NewException_ExceptionType"
                runat="server"
                EmptyText="Please Select Type"
                FieldLabel="Exception Type"
                Editable="false"
                LabelSeparator=":"
                LabelWidth="115"
                Width="380"
                AllowBlank="false">
                <Items>
                    <ext:ListItem Text="Source" Value="Source" />
                    <ext:ListItem Text="Destination" Value="Destination" />
                </Items>

                <SelectedItems>
                    <ext:ListItem Text="Source" Value="Source" />
                </SelectedItems>
            </ext:ComboBox>

            <ext:ComboBox
                ID="NewException_ZeroCost"
                runat="server"
                EmptyText="Please Select Zero Cost Policy"
                FieldLabel="Apply Zero Cost"
                Editable="false"
                LabelSeparator=":"
                LabelWidth="115"
                Width="380"
                AllowBlank="false">
                <Items>
                    <ext:ListItem Text="No" Value="No" />
                    <ext:ListItem Text="Yes" Value="Yes" />
                </Items>

                <SelectedItems>
                    <ext:ListItem Text="No" Value="No" />
                </SelectedItems>
            </ext:ComboBox>

            <ext:ComboBox
                ID="NewException_AutoMark"
                runat="server"
                EmptyText="Please Select Auto-Marking Policy"
                FieldLabel="Auto-Mark Policy"
                Editable="false"
                LabelSeparator=":"
                LabelWidth="115"
                Width="380"
                AllowBlank="false">
                <Items>
                    <ext:ListItem Text="DISABLED" Value="DISABLED" />
                    <ext:ListItem Text="As Business" Value="Business" />
                    <ext:ListItem Text="As Personal" Value="Personal" />
                </Items>

                <SelectedItems>
                    <ext:ListItem Text="DISABLED" Value="DISABLED" />
                </SelectedItems>
            </ext:ComboBox>

            <ext:ComboBox
                ID="NewException_SitesList"
                runat="server"
                DisplayField="Name"
                ValueField="Id"
                TriggerAction="Query"
                QueryMode="Local"
                Editable="true"
                EmptyText="Please Select Site"
                FieldLabel="Site"
                LabelSeparator=":"
                LabelWidth="115"
                Width="380"
                AllowBlank="false">
                <Store>
                    <ext:Store
                        ID="NewException_SitesListStore"
                        runat="server"
                        OnLoad="NewException_SitesListStore_Load">
                        <Model>
                            <ext:Model ID="NewException_SitesListStoreModel" runat="server">
                                <Fields>
                                    <ext:ModelField Name="Id" />
                                    <ext:ModelField Name="Name" />
                                    <ext:ModelField Name="CountryCode" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ListConfig>
                    <ItemTpl ID="NewException_SitesListStoreTpl" runat="server">
                        <Html>
                            <div>{Name}&nbsp;({CountryCode})</div>
                        </Html>
                    </ItemTpl>
                </ListConfig>
            </ext:ComboBox>

            <ext:TextField
                ID="NewException_Description"
                runat="server"
                EmptyText="Empty Description"
                Width="380"
                FieldLabel="Description"
                LabelSeparator=":"
                LabelWidth="115"
                AllowBlank="false" />
        </Items>

        <BottomBar>
            <ext:StatusBar
                ID="NewExceptionWindowBottomBar"
                runat="server"
                StatusAlign="Right"
                DefaultText=""
                Height="30">
                <Items>
                    <ext:Button
                        ID="AddNewExceptionButton"
                        runat="server"
                        Text="Add"
                        Icon="ApplicationFormAdd"
                        Height="25">
                        <DirectEvents>
                            <Click OnEvent="AddNewExceptionButton_Click" />
                        </DirectEvents>
                    </ext:Button>

                    <ext:Button
                        ID="CancelNewExceptionButton"
                        runat="server"
                        Text="Cancel"
                        Icon="Cancel"
                        Height="25">
                        <DirectEvents>
                            <Click OnEvent="CancelNewExceptionButton_Click" />
                        </DirectEvents>
                    </ext:Button>

                    <ext:ToolbarSeparator
                        ID="NewExceptionWindow_BottomBarSeparator"
                        runat="server" />

                    <ext:ToolbarTextItem
                        ID="NewException_StatusMessage"
                        runat="server"
                        Height="15"
                        Text=""
                        Margins="0 0 0 5" />
                </Items>
            </ext:StatusBar>
        </BottomBar>

        <DirectEvents>
            <BeforeHide OnEvent="AddNewExceptionWindowPanel_BeforeHide" />
        </DirectEvents>
    </ext:Window>
</asp:Content>


<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
    <script>
        var PhoneNumbersRenderer = function(value) {
            if(typeof value !== undefined && value != 0) {
                var phoneNumber = value.toString();
                var _isNaNCheck = false;
                var phoneNumberParsedToInt = 0;

                if(phoneNumber[0] == '+') {
                    return phoneNumber;
                }
                    
                //If parsing the phonenumber was not successful, it will return NaN
                var phoneNumberParsedToInt = parseInt(phoneNumber);
                _isNaNCheck = phoneNumberParsedToInt ? false : true;

                //If it is a number, return it with a plus in the front
                if(_isNaNCheck == false) {
                    return ("+" + phoneNumberParsedToInt);
                }
            }

            return phoneNumber;
        }
    </script>
</asp:Content>
