//FIRST CALL THE BROWSER-DETECTOR INIT FUNCTION
//Note that you need to require the BrowserDetector script before this in the documents headers.
BrowserDetect.init();


////This is a function that handles the zero ids of departments and sites
////It is called from the System Admin ->>> Manage Delegates page.
//function RenderZeroAsNA(value, meta, record, rowIndex, colIndex, store) {
//    //if (typeof record.data.DepartmentID !== undefined){}
//    if (typeof value !== undefined) {
//        return ((value == 0) ? "N/A" : value);
//    }
//}


//This function reads the cost value from stores and returns a percentage.
//Handles the Bills grid, PhoneCalls grid, History page, and Delegees PhoneCalls grid
function RoundCost(value, meta, record, rowIndex, colIndex, store) {
    if (typeof value !== undefined) {
        return Math.round(parseFloat(value) * 100) / 100;
    }
    else if (typeof record !== undefined && typeof record.data !== undefined) {
        if (record.data.PersonalCallsCost != undefined) {
            return Math.round(parseFloat(record.data.PersonalCallsCost) * 100) / 100;
        }
        else if (record.data.Marker_CallCost != undefined) {
            return Math.round(parseFloat(record.data.Marker_CallCost) * 100) / 100;
        }
    }

    return "";
}


//This function reads the cost value from stores and and returns a rounded decimal value to two decimal digits.
//Handles the Accounting Monthly and Periodical Reporting grids.
function RoundCostsToTwoDecimalDigits(value) {
    if (typeof value != undefined && value != "0") {
        costValue = parseFloat(value);
        return costValue.toFixed(2).toString();
    }

    return value;
}


//This handles the PhoneCalls grid, History page, and Delegees PhoneCalls grid
//Input should be of this format: "2013-10-30 11:02:51.073"
var DateRenderer = function (value) {
    if (typeof value !== undefined && value != 0) {

        var my_date = {};
        var value_array = value.toString().split(' ');
        var months = ['', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

        my_date["date"] = value_array[0];
        my_date["time"] = value_array[1];

        var date_parts = my_date["date"].split('-');
        my_date["date"] = {
            year: date_parts[0],
            month: months[parseInt(date_parts[1])],
            day: date_parts[2]
        }

        var time_parts = my_date["time"].split(':');
        my_date["time"] = {
            hours: time_parts[0],
            minutes: time_parts[1],
            period: (time_parts[0] < 12 ? 'AM' : 'PM')
        }

        //var date_format = Date(my_date["date"].year, my_date["date"].month, my_date["date"].day, my_date["time"].hours, my_date["time"].minutes);
        return (
            my_date.date.day + " " + my_date.date.month + " " + my_date.date.year + " " +
            my_date.time.hours + ":" + my_date.time.minutes + " " + my_date.time.period
        );

    }//END OUTER IF
}


//This function handles a special case of server-side generated date-and-time value in the Bills History page
//This function does not return days. It only returns the month names and the year!
var SpecialDateRenderer = function (value) {
    if (typeof value != undefined && value != 0) {
        var months_array = ['', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        var months_long_names = {
            'Jan': 'January',
            'Feb': 'February',
            'Mar': 'March',
            'Apr': 'April',
            'May': 'May',
            'Jun': 'June',
            'Jul': 'July',
            'Aug': 'August',
            'Sep': 'September',
            'Oct': 'October',
            'Nov': 'November',
            'Dec': 'December'
        };

        var date = value.toString();
        var date_array = date.split(' ');

        //The following is a weird IE bugfix
        //@date string appears in IE like this: "Thu Jan 31 00:00:00 UTC+0200 2013"
        //@date string appears in other browsers like this: "Thu Jan 31 2013 00:00:00 GMT+0200 (GTB Standard Time)"
        //So by splitting the string on different browsers, you get different index of the year substring!
        if (BrowserDetect.browser == "Explorer") {
            return (months_long_names[date_array[1]] + ", " + date_array[date_array.length - 1]); //year is at last index
        } else {
            return (months_long_names[date_array[1]] + ", " + date_array[3]); //year is at 4th index
        }
    }
}


//This function renders months names instead of numbers the Bills History page
//This function does not return days. It only returns the month names!
var MonthsNumbersRenderer = function (value) {
    if (typeof value != undefined && value != 0) {
        var month_number = parseInt(value);
        var months_long_names = ['', 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

        if (month_number > 0 && month_number <= 12)
            return months_long_names[month_number]
        else
            return '';
    }
}


//This is used in the PhoneCalls page, History page, and Delegees page
function getCssColorForPhoneCallRow(value, meta, record, rowIndex, colIndex, store) {
    if (record.data != null) {
        if (record.data.UI_CallType == 'Personal') {
            meta.style = "color: rgb(201, 20, 20);";
        }
        else if (record.data.UI_CallType == 'Business') {
            meta.style = "color: rgb(46, 143, 42);";
        }
        else if (record.data.UI_CallType == 'Disputed') {
            meta.style = "color: rgb(31, 115, 164);";
        }
        else if (record.data.UI_CallType == 'Unallocated') {
            meta.style = "color: rgb(31, 115, 164);";
        }

        if (value == "") {
            meta.style = "color: rgb(31, 115, 164);";
            return "Unallocated";
        }
        else {
            return value
        }
    }
}


//This is used in the PhoneCalls page, History page, and Delegees page
function getRowClassForIsInvoiced(value, meta, record, rowIndex, colIndex, store) {
    if (record.data.AC_IsInvoiced == 'NO') {
        meta.style = "color: rgb(201, 20, 20);";
    }
    if (record.data.AC_IsInvoiced == 'YES') {
        meta.style = "color: rgb(46, 143, 42);";
    }
    return value
}


//This is used in the PhoneCalls page, Bills page, History page, and Delegees page
function GetMinutes(value, meta, record, rowIndex, colIndex, store) {
    var sec_num = 0;

    if (typeof value !== undefined && value != 0) {
        sec_num = parseInt(value, 10);
    }

        //Handles the case of Mangage Phone Calls Grid in the Phone Calls page
    else if (typeof record.data !== undefined) {
        if (record.data.Duration != undefined) {
            sec_num = parseInt(record.data.Duration, 10);
        }
            //Handles the case of Bills History Grid in the User-Bills page, and Users Bills in the Admin->Bills page.
        else if (record.data.PersonalCallsDuration != undefined) {
            sec_num = parseInt(record.data.PersonalCallsDuration, 10);
        }

            //Handles the case of UnmarkedCallsNotification grid in the Admin->Calls page
        else if (record.data.UnmarkedCallsDuration != undefined) {
            sec_num = parseInt(record.data.UnmarkedCallsDuration, 10);
        }

        else if (record.data.TotalCallsDuration != undefined) {
            sec_num = parseInt(record.data.TotalCallsDuration, 10);
        }
    }

    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) {
        hours = "0" + hours;
    }
    if (minutes < 10) {
        minutes = "0" + minutes;
    }
    if (seconds < 10) {
        seconds = "0" + seconds;
    }

    return hours + ':' + minutes + ':' + seconds;
}

//This is used in the PhoneCalls page, User Statistics, and User Dashboard page
var GetHoursFromMinutes = function (value) {
    var sec_num = parseInt(value, 10);
    var hours = Math.floor(sec_num / 60);
    var minutes = Math.floor((sec_num - (hours * 60)));
    return hours + "." + minutes;
};


var submitValue = function (grid, hiddenFormat, format, selectedRowsOnly) {
    var config = {};

    if (selectedRowsOnly !== undefined && selectedRowsOnly == true) {
        if (grid.getSelectionModel().selected.length > 0) {
            config = { selectedOnly: true };
        }
    }

    if (typeof hiddenFormat !== undefined && hiddenFormat != 0)
        hiddenFormat.setValue(format);

    grid.submitData(config, { isUpload: true });
};


var onShow = function (toolTip, grid) {
    var view = grid.getView(),
        store = grid.getStore(),
        record = view.getRecord(view.findItemByChild(toolTip.triggerElement)),
        column = view.getHeaderByCell(toolTip.triggerElement),
        data = record.get(column.dataIndex);

    if (column.id == "main_content_place_holder_DestinationNumberUri") {
        data = record.get("PhoneBookName");
    }

    toolTip.update(data);
};


//This function is used in the User Dashboard page, and User Statistics page for handling the duration value in the charts
var chartsDurationFormat = function (seconds) {
    var sec_num = parseInt(seconds, 10);
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) hours = "0" + hours;
    if (minutes < 10) minutes = "0" + minutes;
    if (seconds < 10) seconds = "0" + seconds;

    return hours + ':' + minutes + ':' + seconds;
}


//This function is used in Manage Disputed Calls page in the accounting page.
function getRowClassForstatus(value, meta, record, rowIndex, colIndex, store) {
    if (record.data.AC_DisputeStatus == 'Rejected') {
        meta.style = "color: rgb(201, 20, 20);";
    }
    if (record.data.AC_DisputeStatus == 'Accepted') {
        meta.style = "color: rgb(46, 143, 42);";
    }
    return value
}


//This function is used in all of the Accounting Pages
var onKeyUp = function () {
    var me = this,
        v = me.getValue(),
        field;

    if (me.startDateField) {
        field = Ext.getCmp(me.startDateField);
        field.setMaxValue(v);
        me.dateRangeMax = v;
    } else if (me.endDateField) {
        field = Ext.getCmp(me.endDateField);
        field.setMinValue(v);
        me.dateRangeMin = v;
    }

    field.validate();
};




/*
<ext:XScript ID="XScript1" runat="server">
    <script type="text/javascript">
        var applyFilter = function (field) {
            if(#{FilterTypeComboBox}.getValue() == "1") {
                clearFilter();
            } else {
                #{PhoneCallsHistoryGrid}.getStore().filterBy(getRecordFilter());
            }
        };

        var getRecordFilter = function () {
            var f = [];
                
            var FilterValue = #{FilterTypeComboBox}.getValue() || "";
            switch(FilterValue) {
                case "2":
                    f.push({ filter: function (record) { return filterMarkedCriteria("Marked", 'UI_CallType', record); }});
                    break;

                case "3":
                    f.push({ filter: function (record) { return filterMarkedCriteria("Unmarked", 'UI_CallType', record); }});
                    break;

                case "4":
                    f.push({ filter: function (record) { return filterString('Business', 'UI_CallType', record); }});
                    break;

                case "5":
                    f.push({ filter: function (record) { return filterString('Personal', 'UI_CallType', record); }});
                    break;

                case "6":
                    f.push({ filter: function (record) { return filterInvoiceCriteria("YES", 'AC_IsInvoiced', record); }});
                    break;

                case "7":
                    f.push({ filter: function (record) { return filterInvoiceCriteria("NO", 'AC_IsInvoiced', record); }});
                    break;
            }
                
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
             
        var clearFilter = function () {
            #{FilterTypeComboBox}.reset();
            #{PhoneCallsHistoryGrid}.getStore().clearFilter();
        }
            

    //FILTERS BY CRITERIA
    var filterMarkedCriteria = function(value, dataIndex, record) {
        var val = record.get(dataIndex);
                
        if(value == "Marked") {
            if (typeof val == "string" && val != 0) {
                return true;
            } else {
                return false;
            }
        } else {
            if(typeof val != "string" || (typeof val == "string" && val == 0)) {
                return true;
            } else {
                return false;
            }
        }
    };

    var filterInvoiceCriteria = function (value, dataIndex, record) {
        var val = record.get(dataIndex);
                
        if(value == "Charged") {
            if (typeof val == "string" && val.toLowerCase().indexOf("yes") > -1) {
                return true;
            } else {
                return false;
            }
        } else {
            //if(typeof val != "string" || (typeof val == "string" && val == 0) || (typeof val == "string" && val.toLowerCase().indexOf("no") > -1)) {
            if (typeof val == "string" && val.toLowerCase().indexOf("yes") > -1) {
                //this returns the invesre vale of the previous identical if condition
                return false;
            } else {
                return true;
            }
        }
    };

    //FILTERS BY DATA TYPE
    var filterString = function (value, dataIndex, record) {
        var val = record.get(dataIndex);
                
        if (typeof val != "string") {
            return value.length == 0;
        }
                
        return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
    };

    var filterDate = function (value, dataIndex, record) {
        var val = Ext.Date.clearTime(record.get(dataIndex), true).getTime();
 
        if (!Ext.isEmpty(value, false) && val != Ext.Date.clearTime(value, true).getTime()) {
            return false;
        }
        return true;
    };
            
    var filterNumber = function (value, dataIndex, record) {
        var val = record.get(dataIndex);                
 
        if (!Ext.isEmpty(value, false) && val != value) {
            return false;
        }
                
        return true;
    };
    </script>
</ext:XScript>
*/