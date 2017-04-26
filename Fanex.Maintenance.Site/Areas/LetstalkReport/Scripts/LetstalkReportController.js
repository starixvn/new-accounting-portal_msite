/* global NAP: false */
/* global sharedScriptManager: false */

$(document).ready(function () {
    letstalkReportController.initPage();
});

var LetstalkReportController = function () {
    var self = this;

    self.cachedData = {
        getReportParams: null,
        resendParams: null,
        bodyRows: null
    };

    self.initPage = function () {
        self.loadFilter();
        self.bindEvents();
        self.getReport();
    };

    self.loadFilter = function () {
        self.initDatePicker();
        var cookie = $.cookie('MailReport');
        if (cookie) {
            $('#FromDate').datepicker('setDate', new Date(cookie.FromDate));
            $('#ToDate').datepicker('setDate', new Date(cookie.ToDate));
        }
    };

    self.initDatePicker = function () {
        $("#FromDate").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "From",
            buttonImageOnly: true
        }).datepicker('init', '#FromDate');

        $("#ToDate").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "To",
            buttonImageOnly: true
        }).datepicker('init', '#ToDate');
    };

    self.getReport = function () {
        sharedScriptManager.blockUI();
        var getReportParams = {
            FromDate: $("#FromDate").val(),
            ToDate: $("#ToDate").val(),
        };
        self.cachedData.getReportParams = getReportParams;
        $.cookie('MailReport', getReportParams);
        sharedScriptManager.callAjax(self,
            window.rootUrl + "LetstalkNotification/LetstalkReport/GetReport",
            getReportParams,
            self.renderReport,
            null,
            sharedScriptManager.unblockUI);
    };

    self.renderReport = function (result) {
        var reportLength = result.length;
        var bodyData = [];
        for (var i = 0; i < reportLength; i++) {
            var dataRow = result[i];
            var rowColumns = [];

            var linkResend = "<a href='#' class='resendLetstalk' data-letstak-id='" + dataRow.Id + "' data-letstak-ismessage='" + dataRow.IsMessage + "'>Resend</a>";

            rowColumns.push({ Name: "TargetName", Value: dataRow.TargetName });
            rowColumns.push({ Name: "Subject", Value: dataRow.Subject });
            rowColumns.push({ Name: "Content", Value: dataRow.Content });
            rowColumns.push({ Name: "Group", Value: dataRow.Group });
            rowColumns.push({ Name: "CreatedDate", Value: dataRow.CreatedDate });
            rowColumns.push({ Name: "LatestSendDate", Value: dataRow.LastSentDate });
            rowColumns.push({ Name: "SendSuccess", Value: dataRow.SendSuccess });
            rowColumns.push({ Name: "SendFail", Value: dataRow.SendFail });
            rowColumns.push({ Name: "IsMessage", Value: dataRow.IsMessage });
            rowColumns.push({ Name: "Resend", Value: linkResend });

            bodyData.push({
                Columns: rowColumns
            });
        }

        var headerColumns = [
                            { HeaderText: "TargetName", Width: 125, Name: "TargetName", HeaderAlign: "center", CellAlign: "center" },
							{ HeaderText: "Subject", Width: 300, Name: "Subject", HeaderAlign: "center", CellAlign: "left" },
                            { HeaderText: "Content", Width: 300, Name: "Content", HeaderAlign: "center", CellAlign: "left" },
							{ HeaderText: "Group", Width: 125, Name: "Group", HeaderAlign: "center", CellAlign: "center" },
							{ HeaderText: "Created Date<br/> (GMT -4)", Width: 200, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
							{ HeaderText: "Latest Send Date<br/> (GMT -4)", Width: 200, Name: "LatestSendDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" },
							{ HeaderText: "Success", Width: 50, Name: "SendSuccess", HeaderAlign: "center", CellAlign: "center" },
							{ HeaderText: "Fail", Width: 40, Name: "SendFail", HeaderAlign: "center", CellAlign: "center" },
                            { HeaderText: "IsMessage", Width: 40, Name: "IsMessage", HeaderAlign: "center", CellAlign: "center" },
							{ HeaderText: "Resend", Width: 70, Name: "Resend", HeaderAlign: "center", CellAlign: "center" }];

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
            }
        };

        $("#letstalk-report").removeClass("display-none").fanexGrid(gridOptions);
    };

    self.bindEvents = function () {
        $("#button-search").click(function () {
            self.getReport();
        });

        $('.resendLetstalk').live('click', function () {
            var letstalkId = $(this).attr("data-letstak-id");
            var isMessage = $(this).attr("data-letstak-ismessage") == 'true';
            var messageConfirm = isMessage ? "Resend this message ?" : "Resend this email";
            if (confirm(messageConfirm)) {
                self.reSend(letstalkId, isMessage);
            }
        });
    };
  
    self.reSend = function (id, isMessage) {
        var resendParams = {
            FromDate: $("#FromDate").val(),
            ToDate: $("#ToDate").val(),
            Id: id
        };

        sharedScriptManager.blockUI();
        var url = isMessage ? window.rootUrl + "LetstalkNotification/LetstalkReport/ResendMessage" : window.rootUrl + "LetstalkNotification/LetstalkReport/ReSendMail";

        self.cachedData.resendParams = resendParams;
        sharedScriptManager.callAjax(self,
            url,
            resendParams,
            self.renderResend,
            null,
            sharedScriptManager.unblockUI);
    };

    self.renderResend = function (result) {
        if (result) {
            NAP.headlineMsgBox.showInfo("Resend Success !", { closeBtn: false });
        }
        else {
            NAP.headlineMsgBox.showInfo("Resend Faild !", { closeBtn: false });
        }

        setTimeout(function () {
            self.getReport();
        }, 3000);

        window.location.href = window.location.href;
    };
};

var letstalkReportController = new LetstalkReportController();