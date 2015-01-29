<%@ Page Title="Manage Phonecalls" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManagePhoneCalls.aspx.cs" Inherits="LyncBillingUI.Pages.User.ManagePhoneCalls" %>

<asp:Content ID="HeaderScripts" ContentPlaceHolderID="HeaderContent" runat="server">
    <ext:XScript ID="XScript1" runat="server">
        <script>       
            var applyFilter = function (field) {                
                var store = #{ManagePhoneCallsGrid}.getStore();
                store.filterBy(getRecordFilter());                                                
            };
             
            var clearFilter = function () {
                #{CountryCodeFilter}.reset();
                #{DestinationNumberFilter}.reset();
                #{PhoneBookNameFilter}.reset();

                #{PhoneCallsStore}.clearFilter();
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
                        return filterString(#{CountryCodeFilter}.getValue(), "Marker_CallToCountry", record);
                    }
                });
                 
                f.push({
                    filter: function (record) {                         
                        return filterString(#{DestinationNumberFilter}.getValue(), "DestinationNumberUri", record);
                    }
                });

                f.push({
                    filter: function (record) {                         
                        return filterString(#{PhoneBookNameFilter}.getValue(), "PhoneBookName", record);
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

<asp:Content ID="BodyContents" ContentPlaceHolderID="MainContent" runat="server">
    <ext:Hidden ID="FormatType" runat="server" />

    <ext:GridPanel
        ID="ManagePhoneCallsGrid"
        runat="server"
        Header="true"
        Title="My Phone Calls"
        Border="true"
        AutoScroll="true"
        Scroll="Both"
        Layout="AutoLayout"
        ContextMenuID="PhoneCallsAllocationToolsMenu">

        <Store>
            <ext:Store
                ID="PhoneCallsStore"
                runat="server"
                PageSize="25"
                IsPagingStore="true"
                OnLoad="PhoneCallsStore_Load">

                <Model>
                    <ext:Model ID="PhoneCallsStoreModel" runat="server" IDProperty="SessionIdTime">
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
                            <ext:ModelField Name="PhoneCallsTable" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>

                <Sorters>
                    <ext:DataSorter Property="SessionIdTime" Direction="DESC" />
                </Sorters>
            </ext:Store>
        </Store>

        <Plugins>
            <ext:CellEditing ID="CellEditingPlugin" runat="server" ClicksToEdit="2" />
        </Plugins>

        <ColumnModel ID="ColumnModel1" runat="server" Flex="1">
            <Columns>
                <ext:RowNumbererColumn
                    ID="RowNumbererColumn2"
                    runat="server"
                    MinWidth="40" />

                <ext:Column
                    ID="SessionIdTime"
                    runat="server"
                    Text="Date"
                    MinWidth="140"
                    DataIndex="SessionIdTime">
                    <Renderer Fn="formatDate" />
                </ext:Column>

                <ext:Column
                    ID="MarkerCallToCountry"
                    runat="server"
                    Text="Country"
                    MinWidth="120"
                    DataIndex="MarkerCallToCountry">
                </ext:Column>

                <ext:Column
                    ID="DestinationNumberUri"
                    runat="server"
                    Text="Destination"
                    MinWidth="110"
                    DataIndex="DestinationNumberUri">
                </ext:Column>

                <ext:Column ID="PhoneBookNameCol"
                    runat="server"
                    Text="Contact Name"
                    MinWidth="150"
                    DataIndex="PhoneBookName">
                    <Editor>
                        <ext:TextField
                            ID="PhoneBookNameEditorTextbox"
                            runat="server"
                            DataIndex="PhoneBookName"
                            EmptyText="Add a contact name"
                            ReadOnly="false" />
                    </Editor>
                </ext:Column>

                <ext:Column
                    ID="Duration"
                    runat="server"
                    Text="Duration"
                    MinWidth="80"
                    DataIndex="Duration">
                    <Renderer Fn="GetMinutes" />
                </ext:Column>

                <ext:Column
                    ID="MarkerCallCost"
                    runat="server"
                    Text="Cost"
                    MinWidth="80"
                    DataIndex="MarkerCallCost">
                    <Renderer Fn="RoundCost" />
                </ext:Column>

                <ext:Column
                    ID="MarkerCallTypeCol"
                    runat="server"
                    Text="MarkerCallType"
                    MinWidth="150"
                    DataIndex="MarkerCallType">
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

        <TopBar>
            <ext:Toolbar
                ID="FilterToolbar1"
                runat="server">
                <Items>
                    <ext:ComboBox
                        ID="FilterTypeComboBox"
                        runat="server"
                        Icon="Find"
                        TriggerAction="All"
                        QueryMode="Local"
                        DisplayField="TypeName"
                        ValueField="TypeValue"
                        FieldLabel="View Calls:"
                        LabelWidth="60"
                        Width="200"
                        Margins="5 5 5 5">
                        <Items>
                            <ext:ListItem Text="Unallocated" Value="Unmarked" />
                            <ext:ListItem Text="Business" Value="Business" />
                            <ext:ListItem Text="Personal" Value="Personal" />
                            <ext:ListItem Text="Disputed" Value="Disputed" />
                        </Items>
                                        
                        <SelectedItems>
                            <ext:ListItem Text="Unallocated" Value="Unmarked" />
                        </SelectedItems>

                        <DirectEvents>
                            <Select OnEvent="PhoneCallsTypeFilter" Timeout="500000" />
                        </DirectEvents>

                        <Listeners>
                            <BeforeSelect Fn="clearFilter" />
                        </Listeners>
                    </ext:ComboBox>

                    <ext:Button
                        ID="HelpDialogButton"
                        runat="server"
                        Text="Help"
                        Icon="Help"
                        Margins="5 5 5 465">
                        <DirectEvents>
                            <Click OnEvent="ShowUserHelpPanel" />
                        </DirectEvents>
                    </ext:Button>

                    <ext:Window 
                        ID="UserHelpPanel" 
                        runat="server" 
                        Layout="Accordion" 
                        Icon="Help" 
                        Title="User Help" 
                        Hidden="true" 
                        Width="320" 
                        Height="420" 
                        Frame="true" 
                        X="150" 
                        Y="100"
                        ComponentCls="fix-ui-vertical-align">
                        <Items>
                            <ext:Panel ID="MultipleSelectPanel" runat="server" Icon="Anchor" Title="How do I select multiple Phonecalls?">
                                <Content>
                                    <div class="text-left p10">
                                        <p class='font-14 line-height-1-5'>You can select multiple phonecalls by pressing either of the <span class="bold red-color">&nbsp;[Ctrl]&nbsp;</span> or the <span class="bold red-color">&nbsp;[Shift]&nbsp;</span> buttons.</p>
                                    </div>
                                </Content>
                            </ext:Panel>

                            <ext:Panel ID="AllocatePhonecalls" runat="server" Icon="ApplicationEdit" Title="How do I allocate my Phonecalls?">
                                <Content>
                                    <div class="p10 text-left font-14 line-height-1-5 over-h">
                                        <p class="mb10">You can allocate your phonecalls by <span class="bold red-color">&nbsp;[Right Clicking]&nbsp;</span> on the selected phonecalls and choosing your preferred action from the first section of the menu - <span class="blue-color">Selected Phonecalls</span> section.</p>
                                        <p>The list of actions is:</p>
                                        <ol class="ml35" style="list-style-type: decimal">
                                            <li>As Business</li>
                                            <li>As Personal</li>
                                            <li>As Disputed</li>
                                        </ol>
                                    </div>
                                </Content>
                            </ext:Panel>

                            <ext:Panel ID="MarkPhoneCallsAndDestinations" runat="server" Icon="ApplicationSideList" Title="How do I allocate Destinations?">
                                <Content>
                                    <div class="p10 text-left font-14 line-height-1-5">
                                        <p class="mb10">If you <span class="bold red-color">&nbsp;[Right Click]&nbsp;</span> on the grid, you can mark the destination(s) of your selected phonecall(s) as either <span class="bold">Always Business</span> or <span class="bold">Always Personal</span> from the second section of menu - <span class="blue-color">Selected Destinations</span> section.</p>
                                        <p>Please note that marking a destination results in adding it to your phonebook, and from that moment on any phonecall to that destination will be marked automatically as the type of this phonebook contact (Business/Personal).</p>
                                    </div>
                                </Content>
                            </ext:Panel>

                            <ext:Panel ID="AssignContactNamesToDestinations" runat="server" Icon="User" Title="How do I assign &quot;Contact Names&quot; to Destinations?">
                                <Content>
                                    <div class="text-left p10">
                                        <p class="font-14 line-height-1-5">You can add Contact Name to a phonecall destination by <span class="bold red-color">&nbsp;[Double Clicking]&nbsp;</span> on the <span class="blue-color">&nbsp;&quot;Contact Name&quot;&nbsp;</span> field and then filling the text box, please note that this works for the Unallocated phonecalls.</p>
                                    </div>
                                </Content>
                            </ext:Panel>
                        </Items>
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
                DisplayMsg="Phone Calls {0} - {1} of {2}" />
        </BottomBar>
    </ext:GridPanel>
</asp:Content>
