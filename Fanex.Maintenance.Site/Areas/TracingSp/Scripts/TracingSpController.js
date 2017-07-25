/* global NAP: false */
/* global sharedScriptManager: false */

$(document).ready(function () {
    tracingSPController.initPage();
});

var TracingSPController = function () {
    var self = this;
    self.params = {
        Site: null,
        TraceTimer: null,
        TextDataLike: null,
        TextDataLike2: null,
        TextDataLike3: null,
        TraceStartEvents: null,
        Mode: null,
        Server: null
    };

    self.initPage = function () {
        self.bindingEvent();
        self.validate();
    };

    self.bindingEvent = function () {
        $("#button-search").click(function () {
            self.getData();
        });
    };

    self.validate = function () {
        $('#TraceTimer').keypress(function (event) {
            if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });

        $("#TraceTimer").keyup(function () {
            var $this = $(this);
            if ($this.val() > 120) {
                $this.val('120');
            }

        });
    };

    self.getData = function () {
        sharedScriptManager.blockUI();
        self.params = {
            Site: $("#Site").val(),
            Server: $("#Server").val(),
            TraceTimer: $("#TraceTimer").val(),
            TextDataLike: $("#TextDataLike").val(),
            TextDataLike2: $("#TextDataLike2").val(),
            TextDataLike3: $("#TextDataLike3").val(),
        };
        sharedScriptManager.callAjax(self,
            window.rootUrl + "ProfilerSP/TracingSp/GetTracingSpData",
            self.params,
            self.renderData,
            null,
            sharedScriptManager.unblockUI);
    };

    self.renderData = function (result) {
        var bodyData = [];
        for (var i = 0; i < result.length; i++) {
            var record = result[i];

            var rowColumns = [];
            rowColumns.push({ Name: "ColEvent", Value: record.Event });
            rowColumns.push({ Name: "ColSPID", Value: record.SPID });
            rowColumns.push({ Name: "ColTextDataXML", Value: record.TextDataXML });
            rowColumns.push({ Name: "ColTextData", Value: record.TextData });
            rowColumns.push({ Name: "ColHostName", Value: record.HostName });
            rowColumns.push({ Name: "ColApplicationName", Value: record.ApplicationName });
            rowColumns.push({ Name: "ColLoginName", Value: record.LoginName });
            rowColumns.push({ Name: "ColDuration", Value: record.Duration });
            rowColumns.push({ Name: "ColReads", Value: record.Reads });
            rowColumns.push({ Name: "ColWrites", Value: record.Writes });
            rowColumns.push({ Name: "ColCPU", Value: record.CPU });
            rowColumns.push({ Name: "ColDatabaseName", Value: record.DatabaseName });
            rowColumns.push({ Name: "ColStartTime", Value: record.StartTime });
            rowColumns.push({ Name: "ColEndTime", Value: record.EndTime });

            bodyData.push({
                Columns: rowColumns
            });
        }

        var headerColumns = [];
        headerColumns.push({ HeaderText: "Event", Width: 150, Name: "ColEvent", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: "SPID", Width: 50, Name: "ColSPID", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: "TextDataXML", Width: 500, Name: "ColTextDataXML", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: "TextData", Width: 500, Name: "ColTextData", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: "Host Name", Width: 200, Name: "ColHostName", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: "Application Name", Width: 200, Name: "ColApplicationName", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: "Login Name", Width: 200, Name: "ColLoginName", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: "Duration", Width: 70, Name: "ColDuration", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: "Reads", Width: 70, Name: "ColReads", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: "Writes", Width: 70, Name: "ColWrites", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: "CPU", Width: 70, Name: "ColCPU", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: "Db Name", Width: 200, Name: "ColDatabaseName", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: "Start Time", Width: 150, Name: "ColStartTime", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: "End Time", Width: 150, Name: "ColEndTime", HeaderAlign: "center", CellAlign: "left" });

        var gridOptions = {
            columns: headerColumns,
            showTitle: false,
            bodyRows: bodyData,
            showDistinctRow: false,
            showHighlightRow: true,
            gridExpandHeight: 350,
            headerColumnHeight: 29,            
            showPager: true,
            pagerOption: {
                itemsPerPage: 50,
                currentPage: 0,
                itemsPerPageArray: [50, 100, 200, 500],
                showDetail: true
            },
            numberOfFrozenColumn: 4
        };

        $("#tracingsp-report").removeClass("display-none").fanexGrid(gridOptions);
    };
};


var tracingSPController = new TracingSPController();