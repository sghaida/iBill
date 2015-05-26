<%@ Page Title="Statistics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="LyncBillingUI.Pages.User.Statistics" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeaderContent" runat="server">
    <style type="text/css">
        .x-grid-cell-fullName .x-grid-cell-inner { font-family: tahoma, verdana; display: block; font-weight: normal; font-style: normal; color:#385F95; white-space: normal; }
        .x-grid-rowbody div { margin: 2px 5px 20px 5px !important; width: 99%; color: Gray; }
        .x-grid-row-expanded td.x-grid-cell { border-bottom-width: 0px; }
    </style>
</asp:Content>


<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <!-- TOP DOWN SLIDER COMPONENT -->
    <div class="row">
        <div class="col-md-12">
            <div id="top-down-slider-component" class="top-down-slider-container" style="width:100%">
                <div id="top-down-slider-block" class="top-down-slider-block float-right">
                    <ext:ComboBox
                        ID="CustomizeStats_Years"
                        runat="server"
                        Editable="false"
                        DisplayField="YearAsText"
                        ValueField="YearAsNumber"
                        Width="250"
                        LabelWidth="40"
                        LabelSeparator=":"
                        FieldLabel="Year"
                        MarginSpec="5 20 5 5"
                        ComponentCls="float-left">
                        <Store>
                            <ext:Store ID="CustomizeStats_YearStore" runat="server" OnLoad="CustomizeStats_YearStore_Load">
                                <Model>
                                    <ext:Model ID="CustomizeStats_YearStoreModel" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="YearAsText" Type="String" />
                                            <ext:ModelField Name="YearAsNumber" Type="Int" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>

                        <SelectedItems>
                            <ext:ListItem Index="0" />
                        </SelectedItems>

                        <DirectEvents>
                            <Select OnEvent="CustomizeStats_Years_Select" />
                        </DirectEvents>
                    </ext:ComboBox>

                    <ext:ComboBox
                        ID="CustomizeStats_Quarters"
                        runat="server"
                        DisplayField="QuarterAsText"
                        ValueField="QuarterAsNumber"
                        Editable="false"
                        FieldLabel="Quarter"
                        Width="250"
                        LabelWidth="50"
                        LabelSeparator=":"
                        Disabled="true"
                        Margin="5"
                        ComponentCls="float-left">
                        <Store>
                            <ext:Store ID="CustomizeStats_QuartersStore" runat="server" OnLoad="CustomizeStats_QuartersStore_Load">
                                <Model>
                                    <ext:Model ID="Model1" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="QuarterAsText" Type="String" />
                                            <ext:ModelField Name="QuarterAsNumber" Type="Int" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>

                        <SelectedItems>
                            <ext:ListItem Index="4" />
                        </SelectedItems>
                    </ext:ComboBox>

                    <ext:Button
                        ID="SubmitCustomizeStatisticsBtn"
                        runat="server"
                        Text="Apply Filters"
                        Icon="ApplicationGo"
                        Height="25"
                        Margin="5"
                        ComponentCls="float-right">
                        <DirectEvents>
                            <Click OnEvent="SubmitCustomizeStatisticsBtn_Click">
                                <EventMask ShowMask="true" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </div>

                <div class="top-down-slider-button float-right">
                    <a href="#" id="top-down-slider-button">Customize</a>
                </div>
            </div>
        </div>
        <!-- ./col-md-12 -->
    </div>
    <!-- ./row -->
    <!-- TOP DOWN SLIDER COMPONENT -->

    <br />
    <br />

    <div class="row">
        <div class="col-md-12">
            <ext:Panel
                ID="DurationCostChartPanel"
                runat="server"
                Height="500"
                Width="970"
                Header="True"
                Title="Business/Personal Calls"
                Layout="FitLayout"
                Border="true"
                Frame="true">
                <Items>
                    <ext:CartesianChart
                        ID="DurationCostChart"
                        runat="server"
                        Animate="true">
                        <Store>
                            <ext:Store ID="DurationCostChartStore" runat="server" OnLoad="DurationCostChartStore_Load">
                                <Model>
                                    <ext:Model ID="DurationCostChartModel" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="Date" Type="Date" />
                                            <ext:ModelField Name="BusinessCallsCost" />
                                            <ext:ModelField Name="PersonalCallsCost" />
                                            <ext:ModelField Name="BusinessCallsDuration" />
                                            <ext:ModelField Name="PersonalCallsDuration" />
                                            <ext:ModelField Name="TotalCallsCost" />
                                            <ext:ModelField Name="TotalCallsDuration" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>

                        <Axes>
                            <ext:TimeAxis
                                Position="Bottom"
                                Fields="Date"
                                Title="Current Year"
                                DateFormat="MMM YY">
                            </ext:TimeAxis>

                            <ext:NumericAxis
                                Title="Duration in Seconds"
                                Fields="TotalCallsDuration"
                                Position="Left">
                                <Label FillStyle="#115fa6" />
                                <Label>
                                    <Renderer Fn="GetHoursFromMinutes" />
                                </Label>
                            </ext:NumericAxis>

                            <ext:NumericAxis
                                Title="Cost in Local Currency"
                                Fields="TotalCallsCost"
                                Position="Right">
                                <Label FillStyle="#94ae0a" />
                            </ext:NumericAxis>
                        </Axes>

                        <Series>
                            <ext:LineSeries
                                Titles="Personal Duartion"
                                XField="Date"
                                YField="PersonalCallsDuration"
                                Smooth="3">
                                <HighlightConfig>
                                    <ext:CircleSprite Width="7" Radius="7" />
                                </HighlightConfig>
                                <Marker>
                                    <ext:CircleSprite Width="4" Radius="4" />
                                </Marker>
                            </ext:LineSeries>

                            <ext:LineSeries
                                Titles="Personal Cost"
                                XField="Date"
                                YField="PersonalCallsCost"
                                Smooth="3">
                                <HighlightConfig>
                                    <ext:CircleSprite Radius="7" />
                                </HighlightConfig>
                                <Marker>
                                    <ext:CircleSprite Radius="4" LineWidth="0" />
                                </Marker>
                            </ext:LineSeries>

                            <ext:LineSeries
                                Titles="Business Duartion"
                                XField="Date"
                                YField="BusinessCallsDuration"
                                Smooth="3">
                                <HighlightConfig>
                                    <ext:CircleSprite Radius="7" />
                                </HighlightConfig>
                                <Marker>
                                    <ext:CircleSprite Radius="4" LineWidth="0" />
                                </Marker>
                            </ext:LineSeries>

                            <ext:LineSeries
                                Titles="Business Cost"
                                XField="Date"
                                YField="BusinessCallsCost"
                                Smooth="3">
                                <HighlightConfig>
                                    <ext:CircleSprite Radius="7" />
                                </HighlightConfig>
                                <Marker>
                                    <ext:CircleSprite Radius="4" LineWidth="0" />
                                </Marker>
                            </ext:LineSeries>

                        </Series>

                        <Plugins>
                            <%--<ext:VerticalMarker ID="VerticalMarker1" runat="server">
                                <XLabelRenderer Handler="return Ext.util.Format.date(value, 'MMM');" />
                            </ext:VerticalMarker>--%>
                        </Plugins>

                        <LegendConfig runat="server" Dock="Bottom" />
                    </ext:CartesianChart>
                </Items>
            </ext:Panel>
        </div>
        <!-- ./col-md-12 -->
    </div>
    <!-- ./row -->


    <br />
    <br />


    <div class="row">
        <div class="col-md-6">
            <ext:Panel ID="PhoneCallsDuartionChartPanel"
                runat="server"
                Width="470"
                Height="370"
                Layout="FitLayout"
                Border="true"
                Frame="true"
                Header="true"
                Title="Calls Duration">
                <Items>
                    <ext:PolarChart
                        ID="PhoneCallsDuartionChart"
                        runat="server"
                        Animate="true"
                        Shadow="true"
                        InsetPadding="20"
                        Width="350"
                        Height="300">

                        <LegendConfig runat="server" Position="Right" />
                        
                        <Store>
                            <ext:Store ID="PhoneCallsDuartionChartStore"
                                OnLoad="PhoneCallsDuartionChartStore_Load"
                                runat="server">
                                <Model>
                                    <ext:Model ID="PhoneCallsDuartionCharModel" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="Name" />
                                            <ext:ModelField Name="TotalCalls" />
                                            <ext:ModelField Name="TotalDuration" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>

                        <Series>
                            <ext:PieSeries
                                XField="TotalDuration"
                                ShowInLegend="true"
                                Donut="30"
                                Highlight="true"
                                HighlightMargin="10">
                                <Label Field="Name" Display="Rotate" Font="16px Arial">
                                    <Renderer Fn="TotalDuration_LableRenderer" />
                                </Label>
                                <Tooltip ID="Tips1" runat="server" TrackMouse="true" Width="200" Height="75">
                                    <Renderer Fn="TotalDuration_TipRenderer" />
                                </Tooltip>
                            </ext:PieSeries>
                        </Series>
                    </ext:PolarChart>
                </Items>
            </ext:Panel>
        </div>
        <!-- ./col-md-6 -->

        <div class="col-md-6">
            <ext:Panel ID="PhoneCallsCostChartPanel"
                runat="server"
                Width="470"
                Height="370"
                Layout="FitLayout"
                Border="true"
                Frame="true"
                Header="true"
                Title="Calls Costs">
                <Items>
                    <ext:PolarChart
                        ID="PhoneCallsCostChart"
                        runat="server"
                        Animate="true"
                        Shadow="true"
                        InsetPadding="20"
                        Width="350"
                        Height="300">

                        <LegendConfig runat="server" Position="Right" />
                        
                        <Store>
                            <ext:Store ID="PhoneCallsCostChartStore"
                                OnLoad="PhoneCallsCostChartStore_Load"
                                runat="server">
                                <Model>
                                    <ext:Model ID="PhoneCallsCostChartModel" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="Name" />
                                            <ext:ModelField Name="TotalCalls" />
                                            <ext:ModelField Name="TotalCost" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>

                        <Series>
                            <ext:PieSeries
                                XField="TotalCost"
                                ShowInLegend="true"
                                Donut="30"
                                Highlight="true"
                                HighlightMargin="10">
                                <Label Field="Name" Display="Rotate" Font="16px Arial">
                                    <Renderer Fn="TotalCost_LableRenderer" />
                                </Label>    
                                <Tooltip ID="Tips2" runat="server" TrackMouse="true" Width="250" Height="105">
                                    <Renderer Fn="TotalCost_TipRenderer" />
                                </Tooltip>
                            </ext:PieSeries>
                        </Series>
                    </ext:PolarChart>
                </Items>
            </ext:Panel>
        </div>
        <!-- ./col-md-6 -->
    </div>
    <!-- ./row -->
</asp:Content>


<asp:Content ID="EndOfBodyScripts" ContentPlaceHolderID="EndOfBodyScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#top-down-slider-button").click(function () {
                var block = $("#top-down-slider-block");

                if (block.css("display") == "none") {
                    $("#top-down-slider-component").css("border-top", "0px");
                    block.slideDown(500);
                }
                else {
                    $("#top-down-slider-component").css("border-top", "2px solid #CECFCE");
                    block.slideUp(500);
                }

                event.preventDefault();
            });
        });


        function thisDateHandler(value)
        {
            debugger;
        }


        //Pie Chart Data-Lable Renderer for Personal Calls
        var TotalDuration_LableRenderer = function (storeItem, item) {
            var total = 0,
                percentage = 0,
                business_duration = 0,
                personal_duration = 0,
                unmarked_duration = 0,
                dispute_duration = 0,
                chart_element_id = "MainContent_" + "PhoneCallsDuartionChart";

            //App.PhoneCallsDuartionChart
            App[chart_element_id].getStore().each(function (rec) {
                total += rec.get('TotalDuration');

                if (rec.get('Name') == 'Business') {
                    business_duration = rec.get('TotalDuration');
                }
                else if (rec.get('Name') == 'Personal') {
                    personal_duration = rec.get('TotalDuration');
                }
                else if (rec.get('Name') == 'Disputed') {
                    dispute_duration = rec.get('TotalDuration');
                }
                else if (rec.get('Name') == 'Unmarked') {
                    unmarked_duration = rec.get('TotalDuration');
                }
            });

            if (storeItem == "Business") {
                if (total > 0)
                    percentage = ((business_duration / total).toFixed(4) * 100.0).toFixed(2);

                return ((percentage < 3.0) ? '' : percentage + '%');
            }
            else if (storeItem == "Personal") {
                if (total > 0)
                    percentage = ((personal_duration / total).toFixed(4) * 100.0).toFixed(2);

                return ((percentage < 3.0) ? '' : percentage + '%');
            }
            else if (storeItem == "Unmarked") {
                if (total > 0)
                    percentage = ((unmarked_duration / total).toFixed(4) * 100.0).toFixed(2);

                return ((percentage < 3.0) ? '' : percentage + '%');
            }
            else if (storeItem == "Disputed") {
                if (total > 0)
                    percentage = ((dispute_duration / total).toFixed(4) * 100.0).toFixed(2);

                return ((percentage < 3.0) ? '' : percentage + '%');
            }
        };


        //Pie Chart Data-Tip Renderer for Personal Calls
        var TotalDuration_TipRenderer = function (storeItem, item) {
            var total = 0,
                chart_element_id = "MainContent_" + "PhoneCallsDuartionChart";

            //App.PhoneCallsDuartionChart
            App[chart_element_id].getStore().each(function (rec) {
                total += rec.get('TotalDuration');
            });

            this.setTitle(
                storeItem.get('Name') + ': ' +
                ((storeItem.get('TotalDuration') / total).toFixed(4) * 100.0).toFixed(2) + '%' +
                '<br />' +
                '<br />' + 'Total Calls: ' + storeItem.get('TotalCalls') +
                '<br />' + 'Total Duration: ' + chartsDurationFormat(storeItem.get('TotalDuration')) + ' hours.'
            );
            //'<br>' + 'Total Cost: ' + storeItem.get('TotalCost') + ' euros'
        };


        //Pie Chart Data-Lable Renderer for Personal Calls
        var TotalCost_LableRenderer = function (storeItem, item) {
            var total = 0,
                percentage = 0,
                business_cost = 0,
                personal_cost = 0,
                unmarked_cost = 0,
                dispute_cost = 0,
                chart_element_id = "MainContent_" + "PhoneCallsCostChart";

            //App.PhoneCallsDuartionChart
            App[chart_element_id].getStore().each(function (rec) {
                total += rec.get('TotalCost');

                if (rec.get('Name') == 'Business') {
                    business_cost = rec.get('TotalCost');
                }
                else if (rec.get('Name') == 'Personal') {
                    personal_cost = rec.get('TotalCost');
                }
                else if (rec.get('Name') == 'Disputed') {
                    dispute_cost = rec.get('TotalCost');
                }
                else if (rec.get('Name') == 'Unmarked') {
                    unmarked_cost = rec.get('TotalCost');
                }
            });

            if (storeItem == "Business") {
                if (total > 0)
                    percentage = ((business_cost / total).toFixed(4) * 100.0).toFixed(2);

                return ((percentage < 3.0) ? '' : percentage + '%');
            }
            else if (storeItem == "Personal") {
                if (total > 0)
                    percentage = ((personal_cost / total).toFixed(4) * 100.0).toFixed(2);

                return ((percentage < 3.0) ? '' : percentage + '%');
            }
            else if (storeItem == "Unmarked") {
                if (total > 0)
                    percentage = ((unmarked_cost / total).toFixed(4) * 100.0).toFixed(2);

                return ((percentage < 3.0) ? '' : percentage + '%');
            }
            else if (storeItem == "Disputed") {
                if (total > 0)
                    percentage = ((dispute_cost / total).toFixed(4) * 100.0).toFixed(2);

                return ((percentage < 3.0) ? '' : percentage + '%');
            }
        };


        //Pie Chart Data-Tip Renderer for Personal Calls
        var TotalCost_TipRenderer = function (storeItem, item) {
            var total = 0,
                chart_element_id = "MainContent_" + "PhoneCallsCostChart";

            //App.PhoneCallsDuartionChart
            App[chart_element_id].getStore().each(function (rec) {
                total += rec.get('TotalCost');
            });

            this.setTitle(
                storeItem.get('Name') + ': ' +
                ((storeItem.get('TotalCost') / total).toFixed(4) * 100.0).toFixed(2) + '%' +
                '<br />' +
                '<br />' + 'Total Calls: ' + storeItem.get('TotalCalls') +
                '<br />' + 'Total Cost: ' + storeItem.get('TotalCost') + ' *' +
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
