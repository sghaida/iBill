<%@ Page Title="My Bills" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bills.aspx.cs" Inherits="LyncBillingUI.Pages.User.Bills" %>

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
            <ext:GridPanel
                ID="BillsHistoryGrid" 
                runat="server" 
                Title="Bills History"
                MaxWidth="955"
                MinHeight="665"
                AutoScroll="true"
                Header="true"
                Scroll="Both" 
                Layout="FitLayout">

                <Store>
                    <ext:Store
                        ID="BillsStore" 
                        runat="server" 
                        OnLoad="BillsStore_Load"
                        IsPagingStore="true"  
                        PageSize="25"
                        GroupField="Year">
                        <Model>
                            <ext:Model ID="Model1" runat="server" IDProperty="BillsModel">
                                <Fields>
                                    <ext:ModelField Name="Year" Type="int" />
                                    <ext:ModelField Name="Month" Type="int" />
                                    <ext:ModelField Name="PersonalCallsCost" Type="Float" />
                                    <ext:ModelField Name="PersonalCallsCount" Type="Int" />
                                    <ext:ModelField Name="PersonalCallsDuration" Type="Int" />
                                </Fields>
                         </ext:Model>
                       </Model>
                    </ext:Store>
                </Store>

                <Features>
                    <ext:Grouping ID="GroupingFeatures" 
                        runat="server" 
                        HideGroupedHeader="false"
                        StartCollapsed="false" />
                </Features>

                <ColumnModel ID="BillsColumnModel" runat="server">
		            <Columns>
                        <ext:Column ID="BillMonthColumn" 
                            runat="server" 
                            Text="Month" 
                            Width="200" 
                            DataIndex="Month"
                            Groupable="false"
                            Align="Left">
                            <Renderer Fn="MonthsNumbersRenderer" />
                        </ext:Column>

                        <ext:Column ID="TotalCalls"
                            runat="server"
                            Text="Number of Calls"
                            Width="200"
                            DataIndex="PersonalCallsCount"
                            Groupable="false" 
                            Align="Left"/>
                        
                        <ext:Column ID="TotalDuration"
                            runat="server"
                            Text="Total Duration"
                            Width="200"
                            DataIndex="PersonalCallsDuration"
                            Groupable="false"
                            Align="Left">
                            <Renderer Fn="GetMinutes" />
                        </ext:Column>

		                <ext:Column ID="TotalCost"
                            runat="server"
                            Text="Total Cost"
                            Width="200"
                            DataIndex="PersonalCallsCost"
                            Groupable="false"
                            Align="Left">
                            <Renderer Fn="RoundCost"/>
                        </ext:Column>
                    </Columns>
                </ColumnModel>

                <BottomBar>
                    <ext:PagingToolbar 
                        ID="PhoneCallsPagingToolbar" 
                        runat="server" 
                        StoreID="BillsStore" 
                        DisplayInfo="true" 
                        Weight="25" 
                        DisplayMsg="Bills {0} - {1} of {2}" />
                </BottomBar>
            </ext:GridPanel>
        </div>
        <!-- *** END OF PHONE CALLS HISTORY GRID *** -->
    </div>
</asp:Content>

<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
</asp:Content>
