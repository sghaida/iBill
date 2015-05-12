<%@ Page Title="DIDs" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DIDs.aspx.cs" Inherits="LyncBillingUI.Pages.SiteAdministration.DIDs" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <ext:GridPanel
                ID="ManageDIDsGrid"
                runat="server"
                MaxWidth="955"
                MinHeight="620"
                AutoScroll="true"
                Scroll="Both"
                Layout="FitLayout"
                Header="true"
                Title="Manage DIDs">
                <Store>
                    <ext:Store
                        ID="ManageDIDsGridStore"
                        runat="server"
                        RemoteSort="true"
                        IsPagingStore="true"
                        PageSize="25"
                        OnLoad="ManageDIDsGridStore_Load">
                        <Model>
                            <ext:Model ID="ManageDIDsGridStoreModel" runat="server" IDProperty="Id">
                                <Fields>
                                    <ext:ModelField Name="Id" Type="Int" />
                                    <ext:ModelField Name="SiteId" Type="Int" />
                                    <ext:ModelField Name="SiteName" Type="String" />
                                    <ext:ModelField Name="Regex" Type="String" />
                                    <ext:ModelField Name="Description" Type="String" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <Plugins>
                    <ext:RowEditing ID="ManageDIDs_RowEditing" runat="server" ClicksToEdit="2" />
                </Plugins>

                <ColumnModel ID="ManageDIDsColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column ID="SiteColumn"
                            runat="server"
                            Text="Site Name"
                            Width="170"
                            DataIndex="SiteName"
                            Sortable="false"
                            Groupable="false">
                            <%--<HeaderItems>
                                <ext:TextField ID="SiteNameFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearSiteNameFilterButton" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>

                            <Editor>
                                <ext:ComboBox 
                                    ID="Editor_SitesCombobox"
                                    runat="server"
                                    TriggerAction="All"
                                    QueryMode="Local"
                                    Editable="false"
                                    DisplayField="Name"
                                    ValueField="Name">
                                    <Store>
                                        <ext:Store ID="Editor_SitesComboboxStore" runat="server" OnLoad="Editor_SitesComboboxStore_Load">
                                            <Model>
                                                <ext:Model ID="Editor_SitesComboboxStoreModel" runat="server">
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
                                        <ItemTpl ID="FilterSitesItemTpl" runat="server">
                                            <Html>
                                                <div>{Name}&nbsp;({CountryCode})</div>
                                            </Html>
                                        </ItemTpl>
                                    </ListConfig>
                                </ext:ComboBox>
                            </Editor>
                        </ext:Column>

                        <ext:Column ID="DIDPatternColumn"
                            runat="server"
                            Text="DID Regex"
                            Width="250"
                            DataIndex="Regex"
                            Sortable="false"
                            Groupable="false">
                            <%--<HeaderItems>
                                <ext:TextField ID="DIDPatternFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDIDFilter" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>

                            <Editor>
                                <ext:TextField
                                    ID="Editor_DIDPatternTextField"
                                    runat="server"
                                    DataIndex="DIDPattern" />
                            </Editor>
                        </ext:Column>

                        <ext:Column ID="DescriptionColumn"
                            runat="server"
                            Text="Description"
                            Width="300"
                            DataIndex="Description"
                            Sortable="false"
                            Groupable="false">
                            <%--<HeaderItems>
                                <ext:TextField ID="DescriptionFilter" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Change Handler="applyFilter(this);" Buffer="260" />                                                
                                    </Listeners>
                                    <Plugins>
                                        <ext:ClearButton ID="ClearDescFilterBtn" runat="server" />
                                    </Plugins>
                                </ext:TextField>
                            </HeaderItems>--%>

                            <Editor>
                                <ext:TextField
                                    ID="Editor_DescriptionTextField"
                                    runat="server"
                                    DataIndex="Description" />
                            </Editor>
                        </ext:Column>

                        <ext:CommandColumn ID="RejectChange"  runat="server"  Width="70">
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
                            Width="25"
                            Align="Center"
                            Sortable="false"
                            Groupable="false">
                            <Commands>
                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Delete Delegate" CommandName="delete">                            
                                </ext:ImageCommand>
                            </Commands>

                            <Listeners>
                                <Command Handler="this.up(#{ManageDIDsGrid}.store.removeAt(recordIndex));" />
                            </Listeners>
                        </ext:ImageCommandColumn>
                    </Columns>
                </ColumnModel>


                <TopBar>
                    <ext:Toolbar ID="FilterDIDsDIDsToolBar" runat="server">
                        <Items>
                            <ext:Button
                                ID="AddRecordButton"
                                runat="server"
                                Text="New DID"
                                Icon="Add"
                                MarginSpec="5 5 5 5">
                                <DirectEvents>
                                    <Click OnEvent="ShowAddNewDIDWindowPanel" />
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
                                MarginSpec="5 5 5 5">
                                <DirectEvents>
                                    <Click OnEvent="SaveChanges_DirectEvent" before="return #{ManageDIDsGridStore}.isDirty();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="#{ManageDIDsGridStore}.getChangedData()" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>

                <BottomBar>
                    <ext:PagingToolbar
                        ID="ManageDIDsPagingToolbar"
                        runat="server"
                        StoreID="ManageDIDsStore"
                        DisplayInfo="true"
                        Weight="25"
                        DisplayMsg="DIDs {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>


            <ext:Window 
                ID="AddNewDIDWindowPanel" 
                runat="server" 
                Frame="true"
                Resizable="false"
                Title="New DID" 
                Hidden="true"
                Width="510"
                Icon="Add" 
                X="550"
                Y="200">
                                
                <Defaults>
                    <ext:Parameter Name="Padding" Value="10" Mode="Raw" />
                </Defaults>

                <Items>
                    <ext:TextField
                        ID="NewDID_DIDPattern"
                        runat="server"
                        AllowBlank="false"
                        AllowOnlyWhitespace="false"
                        EmptyText="Empty Regex"
                        Width="480"
                        FieldLabel="DID Regex"
                        LabelSeparator=":"
                        LabelWidth="80" />
                                    
                    <ext:ComboBox 
                        ID="NewDID_DIDSite"
                        runat="server"
                        TriggerAction="All"
                        QueryMode="Local"
                        Editable="true"
                        DisplayField="Name"
                        ValueField="Id"
                        FieldLabel="Site"
                        EmptyText="No Site is Selected"
                        LabelSeparator=":"
                        LabelWidth="80"
                        Width="480">
                        <Store>
                            <ext:Store ID="NewDID_DIDSiteStore" runat="server" OnLoad="NewDID_DIDSiteStore_Load">
                                <Model>
                                    <ext:Model ID="NewDID_DIDSiteStoreModel" runat="server">
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
                            <ItemTpl ID="ItemTpl1" runat="server">
                                <Html>
                                    <div>{Name}&nbsp;({CountryCode})</div>
                                </Html>
                            </ItemTpl>
                        </ListConfig>
                    </ext:ComboBox>

                    <ext:TextField
                        ID="NewDID_Description"
                        runat="server"
                        AllowBlank="false"
                        AllowOnlyWhitespace="false"
                        EmptyText="Empty Description"
                        Width="480"
                        FieldLabel="Description"
                        LabelSeparator=":"
                        LabelWidth="80" />
                </Items>

                <BottomBar>
                    <ext:StatusBar
                        ID="NewDIDWindowBottomBar"
                        runat="server"
                        StatusAlign="Right"
                        DefaultText=""
                        Height="30">
                        <Items>
                            <ext:Button
                                ID="AddNewDIDButton"
                                runat="server"
                                Text="Add"
                                Icon="ApplicationFormAdd"
                                Height="25">
                                <DirectEvents>
                                    <Click OnEvent="AddNewDIDButton_Click" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:Button
                                ID="CancelNewDIDButton"
                                runat="server"
                                Text="Cancel"
                                Icon="Cancel"
                                Height="25">
                                <DirectEvents>
                                    <Click OnEvent="CancelNewDIDButton_Click" />
                                </DirectEvents>
                            </ext:Button>

                            <ext:ToolbarSeparator
                                ID="NewDIDWindow_BottomBarSeparator"
                                runat="server" />

                            <ext:ToolbarTextItem
                                ID="NewDID_StatusMessage"
                                runat="server"
                                Height="15"
                                Text=""
                                MarginSpec="0 0 0 5" />
                        </Items>
                    </ext:StatusBar>
                </BottomBar>

                <DirectEvents>
                    <BeforeHide OnEvent="AddNewDIDWindowPanel_BeforeHide" />
                </DirectEvents>
            </ext:Window>
        </div>
    </div>
</asp:Content>


<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
</asp:Content>
