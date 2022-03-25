$(document).ready(function () {
    editLetstalkMessageController.initPage();
});

var EditLetstalkMessageController = function () {
    var self = this;

    var DOMElements = {
        DivScheduledTime: null,
        ConfigSendTime: null,
        LetstalkGroup: null,
        Create: null,
        ChkScheduledTime: null,
        Subject: null,
        Message: null,
        NotificationID: null
    };

    self.initPage = function () {
        self.cacheDOMElement();
        self.initData();
        self.initDatePicker();
        self.bindEvents();
    };

    self.cacheDOMElement = function () {
        DOMElements.DivScheduledTime = $('#divScheduledTime');
        DOMElements.ConfigSendTime = $('#ConfigSendTime');
        DOMElements.LetstalkGroup = $('#LetstalkGroup');
        DOMElements.Save = $('#btnSave');
        DOMElements.Cancel = $('#btnCancel');
        DOMElements.ChkScheduledTime = $('#chkScheduledTime');
        DOMElements.Subject = $('#Subject');
        DOMElements.Message = $('#Message');
        DOMElements.NotificationID = $("#NotificationID");
    };

    self.initData = function () {
        var data = JSON.parse($("#notificationModel").val());
        DOMElements.Subject.val(data.Title);
        DOMElements.Message.val(data.Content);
        DOMElements.LetstalkGroup.val(data.RecipientID);
        DOMElements.ChkScheduledTime.attr("checked", true);
        DOMElements.DivScheduledTime.show();
        //DOMElements.ConfigSendTime.val(data.ConfigSendTime);
    }

    self.initDatePicker = function () {
        // var time = moment(new Date()).zone("-04:00");
        DOMElements.ConfigSendTime.datetimepicker({
            dateFormat: "M dd,yy",
            timeFormat: 'HH:mm',
            controlType: 'select',
            oneLine: true,
        });
    };

    self.bindEvents = function () {
        DOMElements.Save.click(function () {
            self.editNotification();
        });

        DOMElements.Cancel.click(function () {
            window.location.replace(NAP.resolveClientUrl("LetstalkNotification/ManageLetstalkMessage/Index"));
        })
    }

    self.validateForm = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        $('.magic-suggest-error').removeClass('magic-suggest-error');
        if (DOMElements.LetstalkGroup.val() == '') {
            $('#LetstalkGroup').addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Please select at least one Letstalk Group!'], false);
            return false;
        }

        if (DOMElements.Subject.val() === '') {
            DOMElements.Subject.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Subject cannot be empty.'], false);
            return false;
        }

        if (DOMElements.Message.val() === '') {
            DOMElements.Message.addClass('input-validation-error');
            sharedScriptManager.createValidationSummary(['Message cannot be empty.'], false);
            return false;
        }

        return true;
    };

    self.cancelNotification = function (notificationId) {
        NAP.modal("Cancel Notification", "Do you want to cancel this notification?", function () {
            sharedScriptManager.blockUI();
            sharedScriptManager.callAjax(self,
                window.rootUrl + "LetstalkNotification/ManageLetstalkMessage/CancelNotification",
                { NotificationId: notificationId },
                function (result) {
                    var message = result.IsSuccessful ? "Notification has been cancelled successfully" : "Failed to cancel notification!!!";
                    NAP.alert("Notify", message, function () { window.location.replace(NAP.resolveClientUrl("LetstalkNotification/ManageLetstalkMessage/Index")); });
                },
                null,
                sharedScriptManager.unblockUI);
        })
    }

    self.editNotification = function () {
        NAP.modal("Edit Notification", "Are you sure you want to save the information above?", function () {
            sharedScriptManager.blockUI();
            var data = new FormData();
            var editNotificationParams = {
                ConfigSendTime: DOMElements.ChkScheduledTime.is(":checked") ? DOMElements.ConfigSendTime.val() : null,
                Title: DOMElements.Subject.val(),
                Content: DOMElements.Message.val(),
                LstRecipientID: DOMElements.LetstalkGroup.val(),
                NotificationID: DOMElements.NotificationID.val()
            };
            data.append("editNotificationCriteria", JSON.stringify(editNotificationParams));
            $.ajax({
                type: 'POST',
                url: window.rootUrl + "LetstalkNotification/ManageLetstalkMessage/EditNotification",
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    if (result.IsSuccessful) {
                        NAP.alert("Notify", result.Message, function () { window.location.replace(NAP.resolveClientUrl("LetstalkNotification/ManageLetstalkMessage/Index")); });
                    } else {
                        NAP.confirm("Notify", result.Message, function () { window.location.replace(NAP.resolveClientUrl("LetstalkNotification/ManageLetstalkMessage/Index")); });
                    }
                    
                },
                complete: function () {
                    sharedScriptManager.unblockUI();
                }
            })
        })
    }

};

var editLetstalkMessageController = new EditLetstalkMessageController();