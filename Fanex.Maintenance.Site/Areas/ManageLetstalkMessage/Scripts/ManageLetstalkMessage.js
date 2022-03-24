$(document).ready(function () {
    manageLetstalkMessageController.initPage();
});

var ManageLetstalkMessageController = function () {
    var self = this;

    var DOMElements = {
        DivScheduledTime: null,
        FromDate: null,
        ToDate: null,
        ConfigSendTime: null,
        CreatedLetstalkGroup: null,
        Submit: null,
        Create: null,
        ChkScheduledTime: null,
        AddedTitle: null,
        AddedMessage: null
    };

    self.initPage = function () {
        self.cacheDOMElement();
        self.initScheduledTime();
        self.initDatePicker();
        self.renderGroupFilter();
        self.bindEvents();
    };

    self.cacheDOMElement = function () {
        DOMElements.DivScheduledTime = $('#divScheduledTime');
        DOMElements.FromDate = $('#FromDate');
        DOMElements.ToDate = $('#ToDate');
        DOMElements.ConfigSendTime = $('#ConfigSendTime');
        DOMElements.CreatedLetstalkGroup = $('#CreatedLetstalkGroup');
        DOMElements.Submit = $('#btnSubmit');
        DOMElements.Create = $('#btnCreate');
        DOMElements.Clear = $('#btnClear');
        DOMElements.ChkScheduledTime = $('#chkScheduledTime');
        DOMElements.AddedTitle = $('#AddedTitle');
        DOMElements.AddedMessage = $('#AddedMessage');
    };

    self.initScheduledTime = function () {
        DOMElements.DivScheduledTime.hide();
    }

    self.initDatePicker = function () {
        DOMElements.FromDate.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "To",
            buttonImageOnly: true
        }).datepicker('init', '#FromDate');

        DOMElements.ToDate.datepicker({
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

        // var time = moment(new Date()).zone("-04:00");
        DOMElements.ConfigSendTime.datetimepicker({
            dateFormat: "M dd,yy",
            timeFormat: 'HH:mm',
            controlType: 'select',
            oneLine: true,
        });
    };

    self.renderGroupFilter = function () {
        sharedScriptManager.blockUI();
        sharedScriptManager.callAjax(self,
            window.rootUrl + "LetstalkNotification/ManageLetstalkMessage/GetGroups",
            null,
            function (result) {
                if (result && result.length > 0) {
                    var optionLength = result.length;
                    var ddlData = [];
                    var prefixId = 'ddlGroup_';
                    for (var i = 0; i < optionLength; i++) {
                        var info = result[i];
                        ddlData.push({
                            id: prefixId + info.Value,
                            value: info.Value,
                            text: info.Text,
                            checked: false,
                            enable: true,
                            cssClass: "",
                            items: []

                        });
                    }

                    DOMElements.CreatedLetstalkGroup.fanexMultiSelect({
                        minWidth: 300,
                        maxHeight: 450,
                        showFilter: true,
                        shadow: true,
                        showHeader: true,
                        isCheckAll: false,
                        showFooter: false,
                        checkAllText: 'All',
                        isUsedCustomCheckAllText: true,
                        useCustomCheckAllText: 'All',
                        noneSelectedText: 'Select an option',
                        noItemText: 'No item found.',
                        disabled: false,
                        listData: ddlData,
                    });
                }
            },
            null,
            sharedScriptManager.unblockUI);

    };

    self.getFiles = function () {
        var input = document.getElementById('myFile');
        var output = document.getElementById('fileList');
        output.innerHTML = '<div>';
        for (var i = 0; i < input.files.length; ++i) {
            output.innerHTML += '<span class="mr-5">' + input.files.item(i).name + ', </span>';
        }
        output.innerHTML += '</div>';
    }

    self.loadData = function (data) {
        var reportLength = data.length;
        var bodyData = [];
        for (var i = 0; i < reportLength; i++) {
            var dataRow = data[i];
            var rowColumns = [];
            var actionCancel = "<div class='pointer-cursor' onclick='manageLetstalkMessage.cancelNotification(" + dataRow.Id + ")'>Cancel</div>";
            var actionResend = "<div class='pointer-cursor' onclick='manageLetstalkMessageController.updatelNotificationStatus(" + dataRow.Id + ", 0)'>Resend</div>";
            rowColumns.push({ Name: "No", Value: dataRow.Id });
            rowColumns.push({ Name: "Title", Value: dataRow.Title });
            rowColumns.push({ Name: "Message", Value: dataRow.Content });
            rowColumns.push({ Name: "LetstalkGroup", Value: dataRow.RecipientID + " - " + dataRow.RecipientName });
            //rowColumns.push({ Name: "AttachesFile", Value: dataRow.NumberAttachment + " file(s) | <a href='#' class='link-color'>Download All</a>" });
            rowColumns.push({ Name: "SendType", Value: dataRow.NotificationType == 1 ? "Send Now" : "By Schedule" });
            rowColumns.push({ Name: "SendTime", Value: dataRow.ConfigSendTime });
            rowColumns.push({ Name: "Status", Value: dataRow.Status == "FAILED" ? "<div class='red-color'>" + dataRow.Status + "</div>" : dataRow.Status});
            rowColumns.push({ Name: "CreatedDate", Value: dataRow.CreatedDate });
            rowColumns.push({ Name: "Creator", Value: dataRow.CreatedBy });
            rowColumns.push({ Name: "LatestUpdateTime", Value: dataRow.LastUpdTime });
            rowColumns.push({ Name: "LatestEditor", Value: dataRow.LastModifiedBy });
            rowColumns.push({ Name: "Resend", Value: dataRow.Status == "FAILED" || "SENT" ? actionResend : "" });
            rowColumns.push({ Name: "Action", Value: actionCancel });
            bodyData.push({
                Columns: rowColumns
            });
        }

        var headerColumns = [
            { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Title", Width: 150, Name: "Title", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Message", Width: 150, Name: "Message", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Letstalk Group", Width: 200, Name: "LetstalkGroup", HeaderAlign: "center", CellAlign: "left" },
            //{ HeaderText: "Attaches file(s)", Width: 150, Name: "AttachesFile", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Send Type", Width: 100, Name: "SendType", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Send Time", Width: 120, Name: "SendTime", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Status", Width: 100, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Created Date", Width: 120, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Creator", Width: 100, Name: "Creator", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Latest Update Time", Width: 120, Name: "LatestUpdateTime", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Latest Editor", Width: 100, Name: "LatestEditor", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Resend", Width: 100, Name: "Resend", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Action", Width: 100, Name: "Action", HeaderAlign: "center", CellAlign: "center" },
            ];

        var gridOptions = {
            columns: headerColumns,
            showTitle: false,
            bodyRows: bodyData,
            showDistinctRow: false,
            showHighlightRow: true,
            gridExpandHeight: 500,
            headerColumnHeight: 29,
            showPager: false,
            pagerOption: {
                itemsPerPage: 50,
                currentPage: 0,
                itemsPerPageArray: [50, 100, 200, 500],
                showDetail: true
            }
        };

        $("#manage-letstalk-message").removeClass("display-none").fanexGrid(gridOptions);
    };

    self.bindEvents = function () {
        DOMElements.Submit.click(function () {
            self.GetNotificationList();
        });

        DOMElements.Create.click(function () {
            self.insertNotification();
        });

        DOMElements.Clear.click(function () {
            self.resetForm();
        });

        DOMElements.ChkScheduledTime.change(function () {
            if ($(this).is(":checked")) {
                DOMElements.DivScheduledTime.show();
                DOMElements.Create.attr('value', 'CREATE')
            } else {
                DOMElements.DivScheduledTime.hide();
                DOMElements.Create.attr('value', 'SEND NOW')
            }            
        });
    }

    self.resetForm = function () {
        self.initGroupFilter();
        DOMElements.AddedTitle.val('');
        DOMElements.AddedMessage.val('');
    }

    self.validateForm = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.magic-suggest-error').removeClass('magic-suggest-error');
        if (DOMElements.CreatedLetstalkGroup.fanexMultiSelect('getItems', true).toString() === '') {
            $('#CreatedLetstalkGroup').addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Please select at least one Letstalk Group!'], false);
            return false;
        }

        if (DOMElements.AddedTitle.val() === '') {
            DOMElements.AddedTitle.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Title cannot be empty.'], false);
            return false;
        }

        if (DOMElements.AddedMessage.val() === '') {
            DOMElements.AddedMessage.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Message cannot be empty.'], false);
            return false;
        }

        return true;
    };

    self.GetNotificationList = function () {
        var params = {
            FromDate: DOMElements.FromDate.val(),
            ToDate: DOMElements.ToDate.val(),
            RecipientID: $('#LetstalkGroup').val(),
            Statusid: $('#Status').val(),
            NotificationID: null
        };
        sharedScriptManager.blockUI();
        sharedScriptManager.callAjax(self,
            window.rootUrl + "LetstalkNotification/ManageLetstalkMessage/GetNotificationList",
            params,
            function (result) {
                self.loadData(result);
            },
            null,
            sharedScriptManager.unblockUI);
    }

    self.insertNotification = function () {
        if (self.validateForm()) {
            sharedScriptManager.blockUI();
            var data = new FormData();
            //$.each($("input[type='file']")[0].files, function (i, file) {
            //    data.append('files', file);
            //});
            var insertNotificationParams = {
                ConfigSendTime: DOMElements.ChkScheduledTime.is(":checked") ? DOMElements.ConfigSendTime.val() : null,
                Title: DOMElements.AddedTitle.val(),
                Content: DOMElements.AddedMessage.val(),
                LstRecipientID: DOMElements.CreatedLetstalkGroup.fanexMultiSelect('getItems', true).toString()
            };
            data.append("insertNotificationCriteria", JSON.stringify(insertNotificationParams));
            $.ajax({
                type: 'POST',
                url: window.rootUrl + "LetstalkNotification/ManageLetstalkMessage/InsertNotification",
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    sharedScriptManager.showNonQueryResult(result, null, null)
                },
                complete: function () {
                    sharedScriptManager.unblockUI();
                }
            })
        }
    }

    self.cancelNotification = function (notificationId) {
        NAP.modal("Cancel Notification", "Do you want to cancel this notification?", function () {
            sharedScriptManager.blockUI();
            sharedScriptManager.callAjax(self,
                window.rootUrl + "LetstalkNotification/ManageLetstalkMessage/CancelNotification",
                { NotificationId : notificationId },
                function (result) {
                    var message = result.IsSuccessful ? "Notification has been cancelled successfully" : "Failed to cancel notification!!!";
                    NAP.alert("Notify", message, function () { window.location.replace(NAP.resolveClientUrl("LetstalkNotification/ManageLetstalkMessage/Index")); });
                },
                null,
                sharedScriptManager.unblockUI);
        })
    }

    self.updatelNotificationStatus = function (notificationId, recipientId) {
        NAP.modal("Update Notification Status", "Do you want to update this notification?", function () {
            sharedScriptManager.blockUI();
            sharedScriptManager.callAjax(self,
                window.rootUrl + "LetstalkNotification/ManageLetstalkMessage/UpdateNotificationStatus",
                { NotificationID: notificationId, RecipientID: recipientId, StatusID: 0 },
                function (result) {
                    var message = result.IsSuccessful ? "Notification has been updated successfully" : "Failed to update notification!!!";
                    NAP.alert("Notify", message, function () { window.location.replace(NAP.resolveClientUrl("LetstalkNotification/ManageLetstalkMessage/Index")); });
                },
                null,
                sharedScriptManager.unblockUI);
        })
    }

};

var manageLetstalkMessageController = new ManageLetstalkMessageController();