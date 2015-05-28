<%@ Page Title="User Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="LyncBillingUI.Pages.User.Dashboard" %>


<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <% if(unmarkedCallsCount > 0) { %>
    <div class="alert alert-warning" role="alert">
        <strong>Warning!</strong>&nbsp;You have a total of&nbsp;<strong><%= unmarkedCallsCount.ToString() %>&nbsp;unmarked</strong>&nbsp;calls, please click&nbsp;<a class='link bold' href='<%= global_asax.APPLICATION_URL %>/User/Phonecalls'><strong>here</strong></a>&nbsp;to mark them.
    </div>

    <br />
    <% } %>

    <div class="row" role="row">
        <div class="col-md-6">
            <ext:Panel
                ID="TopDestinationCountriesChartPanel"
                runat="server"
                Height="470"
                Width="470"
                Header="True"
                Title="Most Called Countries"
                Layout="FitLayout"
                Border="true"
                Frame="true">
                <Items>
                    <ext:PolarChart
                        ID="TopDestinationCountriesChart"
                        runat="server"
                        Width="470"
                        Height="400"
                        Animate="true"
                        Shadow="true"
                        InsetPadding="15"
                        InnerPadding="20">
                        
                        <LegendConfig runat="server" Dock="Bottom" />

                        <Store>
                            <ext:Store ID="TopDestinationCountriesStore"
                                OnLoad="TopDestinationCountriesStore_Load"
                                runat="server">
                                <Model>
                                    <ext:Model ID="TopDestinationCountriesModel" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="CountryName" />
                                            <ext:ModelField Name="CallsCost" />
                                            <ext:ModelField Name="CallsDuration" />
                                            <ext:ModelField Name="CallsCount" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <Series>
                            <ext:PieSeries
                                XField="CallsCount"
                                ShowInLegend="true"
                                Donut="30"
                                Highlight="true"
                                HighlightMargin="10">
                                <Label Field="CountryName" Display="Rotate" Font="16px Arial">
                                    <Renderer Fn="TopDestinationCountries_LableRenderer" />
                                </Label>
                                <Tooltip ID="TopDestinationCountriesChartTip" runat="server" TrackMouse="true" Width="260" Height="120">
                                    <Renderer Fn="TopDestinationCountries_TipRenderer" />
                                </Tooltip>
                            </ext:PieSeries>
                        </Series>
                    </ext:PolarChart>
                </Items>
            </ext:Panel>
        </div><!-- ./column-md-6 -->

        <div class="col-md-6">
            <ext:Panel
                ID="CallsCostsChartPanel"
                runat="server"
                Height="470"
                Width="470"
                Header="True"
                Title="Calls Cost Chart"
                Layout="FitLayout"
                Border="true"
                Frame="true">
                <Items>
                    <ext:CartesianChart 
                        ID="CallsCostsChart"
                        runat="server"
                        Width="470"
                        Height="400"
                        Animate="true"
                        InsetPadding="10"
                        InnerPadding="10">
                        <Store>
                            <ext:Store 
                                ID="CallsCostsChartStore" 
                                runat="server"
                                OnLoad="CallsCostsChartStore_Load">
                                <Model>
                                    <ext:Model ID="CallsCostsChartModel" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="Date" Type="Date" DateFormat="M" />
                                            <ext:ModelField Name="PersonalCallsCost" />
                                            <ext:ModelField Name="BusinessCallsCost" />
                                            <ext:ModelField Name="UnallocatedCallsCost" />
                                            <ext:ModelField Name="TotalCallsCost" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>

                        <Axes>
                            <ext:TimeAxis
                                Position="Bottom"
                                Fields="Date"
                                Title="Month"
                                DateFormat="MMM YY">
                            </ext:TimeAxis>

                            <ext:NumericAxis
                                Title="Cost in Local Currency"
                                Fields="PersonalCallsCost,BusinessCallsCost,UnallocatedCallsCost"
                                Position="Left">
                                <TitleConfig FillStyle="#115fa6" />
                                <Label FillStyle="#115fa6" />
                            </ext:NumericAxis>
                        </Axes>

                        <Series>
                            <ext:LineSeries
                                Titles="Personal"
                                XField="Date"
                                YField="PersonalCallsCost"
                                Smooth="3">
                                <HighlightConfig>
                                    <ext:CircleSprite Radius="7" />
                                </HighlightConfig>
                                <Marker>
                                    <ext:CircleSprite Radius="4" Width="0" />
                                </Marker>
                            </ext:LineSeries>
                                 
                            <ext:LineSeries
                                Titles="Business"
                                XField="Date"
                                YField="BusinessCallsCost"
                                Smooth="3">
                                <HighlightConfig>
                                    <ext:CircleSprite Radius="7" />
                                </HighlightConfig>
                                <Marker>
                                    <ext:CircleSprite Radius="4" Width="0" />
                                </Marker>
                            </ext:LineSeries>
                                
                            <ext:LineSeries
                                Titles="Unallocated"
                                XField="Date"
                                YField="UnallocatedCallsCost"
                                Smooth="3">
                                <HighlightConfig>
                                    <ext:CircleSprite Radius="7" />
                                </HighlightConfig>
                                <Marker>
                                    <ext:CircleSprite Radius="4" Width="0" />
                                </Marker>
                            </ext:LineSeries>
                        </Series>

                        <Plugins>
                            <ext:VerticalMarker ID="VerticalMarker1" runat="server">
                                <XLabelRenderer Handler="return Ext.util.Format.date(value, 'Y M');" />
                                <YLabelRenderer FormatHandler="true"></YLabelRenderer>
                            </ext:VerticalMarker>
                        </Plugins>

                        <LegendConfig runat="server" Dock="Bottom" />
                    </ext:CartesianChart>
                </Items>
            </ext:Panel>
        </div><!-- ./column-md-6 -->
    </div><!-- ./row -->

    <br />
    <br />

    <div class="row" role="row">
        <div class="col-md-6">
            <ext:GridPanel
                ID="TopDestinationNumbersGrid"
                runat="server"
                Width="470"
                Height="200"
                Border="true"
                Frame="true"
                Header="true"
                Title="Most Called Numbers"
                Layout="FitLayout"
                ComponentCls="table table-compact">
                <Store>
                    <ext:Store
                        ID="TopDestinationNumbersStore"
                        runat="server"
                        OnLoad="TopDestinationNumbersStore_Load">
                        <Model>
                            <ext:Model ID="TopDestinationNumbersModel" runat="server" IDProperty="SessionIdTime">
                                <Fields>
                                    <ext:ModelField Name="PhoneNumber" Type="String" />
                                    <ext:ModelField Name="DestinationContactName" Type="String" />
                                    <ext:ModelField Name="CallsCount" Type="Int" />
                                </Fields>
                            </ext:Model>
                        </Model>
                    </ext:Store>
                </Store>

                <ColumnModel ID="TOPDestinationNumbersColumnModel" runat="server" Flex="1">
                    <Columns>
                        <ext:Column
                            ID="PhoneNumber"
                            runat="server"
                            Text="Phone Number"
                            Width="150"
                            DataIndex="PhoneNumber">
                        </ext:Column>

                        <ext:Column
                            ID="DestinationContactName"
                            runat="server"
                            Text="Address Book Contact"
                            Width="170"
                            DataIndex="DestinationContactName" />

                        <ext:Column
                            ID="CallsCount"
                            runat="server"
                            Text="Number of Calls"
                            Width="110"
                            DataIndex="CallsCount" />
                    </Columns>
                </ColumnModel>
            </ext:GridPanel>
        </div>

        <div class="col-md-6">
            <ext:Panel
                ID="MailStatisticsPanel"
                runat="server"
                Width="470"
                Height="200"
                Header="True"
                Title="Mail Statistics (Previous Month)"
                Layout="FitLayout"
                Border="true"
                Frame="true"
                Html="<%# GetMailStatisticsHtml() %>"
                AutoDataBind="true">
            </ext:Panel>
        </div>
    </div><!-- ./row -->
</asp:Content>

<%--
<ext:Panel
    ID="MailStatisticsPanel"
    runat="server"
    Width="470"
    Height="200"
    Header="True"
    Title="Mail Statistics (Previous Month)"
    Layout="FitLayout"
    Border="true"
    Frame="true">
    <Content>
        <h5><strong>Received Mails:</strong></h5>
        <ul>
            <li><h5>Number:&nbsp;<span class="bold red-color"><%= userMailStatistics.ReceivedCount %></span></h5></li>
            <li><h5>Size:&nbsp;<span class="bold red-color"><%= userMailStatistics.ReceivedSize %> (in MB)</span></h5></li>
        </ul>
                    
        <br />
                    
        <h5><strong>Sent Mails:</strong></h5>
        <ul>
            <li><h5>Number:&nbsp;<span class="bold blue-color"><%= userMailStatistics.SentCount %></span></h5></li>
            <li><h5>Size:&nbsp;<span class="bold blue-color"><%= userMailStatistics.SentSize %> (in MB)</span></h5></li>
        </ul>
    </Content>
</...>
--%>


<asp:Content ID="DashboardScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
    <script type="text/javascript">
        function showCustomSettingsBlock() {
            $("#custom-settings-block").slideToggle(500);
        }

        //Pie Chart Data-Lable Renderer for Countries Destinations Calls
        var TopDestinationCountries_LableRenderer = function (storeItem, item) {
            var total = 0,
                percentage,
                all_countries_data = {},
                component_name = "MainContent_" + "TopDestinationCountriesChart";

            App[component_name].getStore().each(function (rec) {
                total += rec.get('CallsCount');

                var country_name = rec.get('CountryName');
                if (country_name != 0 && all_countries_data[country_name] == undefined) {
                    all_countries_data[country_name] = rec.get('CallsCount');
                }
            });

            if (all_countries_data[storeItem] != undefined) {
                percentage = 0;

                if (total > 0)
                    percentage = ((all_countries_data[storeItem] * 1.0 / total).toFixed(4) * 100.0).toFixed(2);

                return ((percentage < 3.5) ? '' : (percentage + '%'));
            }
        };


        //Pie Chart Data-Lable Renderer for Countries Destinations Calls
        var TopDestinationCountries_TipRenderer = function (storeItem, item) {
            //calculate percentage.
            var total = 0,
                component_name = "MainContent_" + "TopDestinationCountriesChart";

            App[component_name].getStore().each(function (rec) {
                total += rec.get('CallsCount');
            });

            this.setTitle(
                storeItem.get('CountryName') + ': ' + ((storeItem.get('CallsCount') / total).toFixed(4) * 100.0).toFixed(2) + '%' +
                '<br />' +
                '<br />' + 'Total Calls: ' + storeItem.get('CallsCount') +
                '<br />' + 'Total Duration: ' + chartsDurationFormat(storeItem.get('CallsDuration')) + ' hours.' +
                '<br />' + 'Total Cost: ' + RoundCostsToTwoDecimalDigits(storeItem.get('CallsCost')) + ' *' +
                '<br />' +
                '<br />' + "* The cost is in your site's local currency."
            );
        };


        //Ext.override(Ext.chart.LegendItem, {
        //    createSeriesMarkers: function (config) {
        //        var me = this,
        //            index = config.yFieldIndex,
        //            series = me.series,
        //            seriesType = series.type,
        //            surface = me.surface,
        //            z = me.zIndex;

        //        // Line series - display as short line with optional marker in the middle
        //        if (seriesType === 'line' || seriesType === 'scatter') {
        //            if (seriesType === 'line') {
        //                var seriesStyle = Ext.apply(series.seriesStyle, series.style);
        //                me.drawLine(0.5, 0.5, 16.5, 0.5, z, seriesStyle, index);
        //            };

        //            if (series.showMarkers || seriesType === 'scatter') {
        //                var markerConfig = Ext.apply(series.markerStyle, series.markerConfig || {}, {
        //                    fill: series.getLegendColor(index)
        //                });
        //                me.drawMarker(8.5, 0.5, z, markerConfig);
        //            }
        //        }
        //            // All other series types - display as filled box
        //        else {
        //            me.drawFilledBox(12, 12, z, index);
        //        }
        //    },

        //    /**
        //     * @private Creates line sprite for Line series.
        //     */
        //    drawLine: function (fromX, fromY, toX, toY, z, seriesStyle, index) {
        //        var me = this,
        //            surface = me.surface,
        //            series = me.series;

        //        return me.add('line', surface.add({
        //            type: 'path',
        //            path: 'M' + fromX + ',' + fromY + 'L' + toX + ',' + toY,
        //            zIndex: (z || 0) + 2,
        //            "stroke-width": series.lineWidth,
        //            "stroke-linejoin": "round",
        //            "stroke-dasharray": series.dash,
        //            stroke: seriesStyle.stroke || series.getLegendColor(index) || '#000',
        //            style: {
        //                cursor: 'pointer'
        //            }
        //        }));
        //    }
        //});
    </script>
</asp:Content>