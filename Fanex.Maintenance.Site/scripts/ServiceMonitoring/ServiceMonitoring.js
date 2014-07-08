"use strict";
var serviceMonitoring;
var timeInterval;
var isRefreshOnChange;
var ServiceMonitoring = function () {
    var self = this;
    var DOMElements = {
        Server: $('#Server'),
        ServiceName: $('#ServiceName'),
        Status: $('#Status'),
        Submit: $('#Submit'),
        Grid: $('#Grid'),
        GridContainer: $('#Grid-container'),
        EditConfigGrid: $('#EditConfigGrid'),
        EditConfigGridContainer: $('#EditConfigGrid-container'),
        AddServer: $('#AddServer'),
        AddServerDialog: $('#AddServerDialog'),
        EditConfigDialog: $('#EditConfigDialog'),
        MainForm: $('#FormMain'),
        AddServerForm: $('#AddServerDialog>form:eq(0)'),
        ErrorServerLogin: $('#ErrorServerLogin'),
        ServerIP: $('#IP'),
        UserName: $('#UserName'),
        Password: $('#Password')
    };

    var ServerList = [];

    self.Init = function () {
        self.BindingEvents();
        self.GetListServer();
        timeInterval = window.setInterval(function () { return self.AutoRefresh(); }, 5000);
        DOMElements.AddServerDialog.dialog({
            autoOpen: false,
            width: 370,
            height: 250,
            modal: true,
            buttons: [
                {
                    text: 'OK',
                    align: 'left',
                    click: function () {
                        self.ServerLogin();
                    }
                },
                {
                    text: 'Cancel',
                    click: function () {
                        $(this).dialog('close');
                        NAP.loadingBox.close();
                    }
                }
            ]
        });

        DOMElements.EditConfigDialog.dialog({
            autoOpen: false,
            resize: "auto",
            modal: true,
            width: 'auto'
        });
    }

    self.BindingEvents = function () {
        DOMElements.Submit.click(function () {
            self.Submit(false);
        });

        DOMElements.AddServer.click(function () {
            self.AddServer();
        });

        DOMElements.ServiceName.keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) { //Enter keycode                        
                e.preventDefault();
                self.Submit(false);
            }
        });
    };

    self.AddServer = function () {
        DOMElements.ErrorServerLogin.hide();
        DOMElements.AddServerDialog.dialog('open');
    }

    self.ServerLogin = function () {
        $.ajax({
            url: NAP.resolveClientUrl("ServiceMonitoring/ServerLogin"),
            type: 'POST',
            dataType: 'JSON',
            data: {
                IP: DOMElements.ServerIP.val(),
                UserName: DOMElements.UserName.val(),
                Password: DOMElements.Password.val()
            },
            cache: false,
            beforeSend: function () {
                NAP.loadingBox.show();
            },
            complete: function () {
            },
            success: function (data) {
                if (data == true) {
                    var serverElement = {
                        IP: DOMElements.ServerIP.val(),
                        UserName: DOMElements.UserName.val(),
                        Password: DOMElements.Password.val()
                    };

                    self.RefreshServer(serverElement);
                    DOMElements.AddServerDialog.dialog('close');
                    NAP.loadingBox.close();
                }
                else {
                    DOMElements.ErrorServerLogin.show();
                }
            },
            error: function () {
                DOMElements.ErrorServerLogin.show();
            }
        });
    }

    self.GetListServer = function () {
        $.ajax({
            url: NAP.resolveClientUrl("ServiceMonitoring/GetListServer"),
            type: 'POST',
            dataType: 'JSON',
            data: null,
            cache: false,
            beforeSend: function () {
                NAP.loadingBox.show();
            },
            complete: function () {
            },
            success: function (data) {
                ServerList = data;
                self.RenderServerDropDownList();
                NAP.loadingBox.close();
            },
            error: function () {
                DOMElements.ErrorServerLogin.show();
            }
        });
    }

    self.RefreshServer = function (serverElement) {
        ServerList = $.grep(ServerList, function (value) {
            return value.IP != serverElement.IP;
        });

        ServerList.push(serverElement);
        self.UpdateListServer();
        self.RenderServerDropDownList();
    }

    self.UpdateListServer = function () {
        $.ajax({
            url: NAP.resolveClientUrl("ServiceMonitoring/UpdateListServer"),
            type: 'POST',
            dataType: 'josn',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ 'serverList': ServerList }),
            cache: false,
            beforeSend: function () {
                NAP.loadingBox.show();
            },
            complete: function () {
                NAP.loadingBox.close();
            },
            success: function (data) {
            },
            error: function () {
                DOMElements.ErrorServerLogin.show();
            }
        });
    }

    self.Relogin = function () {
        var serverElement = $.grep(ServerList, function (value) {
            return value.IP == DOMElements.Server.val();
        });

        self.AddServer();
        DOMElements.ServerIP.val(serverElement[0].IP);
        DOMElements.UserName.val(serverElement[0].UserName);
        DOMElements.Password.val(serverElement[0].Password);

    }

    self.RenderServerDropDownList = function () {
        DOMElements.Server.find('option').remove();
        for (var i = 0; i < ServerList.length; i++) {
            DOMElements.Server.append('<option value="' + ServerList[i].IP + '">' + ServerList[i].IP + '</option>');
        }
    }

    self.Submit = function (isBackGround) {
        if (isBackGround == false) {
            NAP.loadingBox.show();
        }

        var serverElement = $.grep(ServerList, function (value) {
            return value.IP == DOMElements.Server.val();
        });

        $.ajax({
            url: NAP.resolveClientUrl("ServiceMonitoring/GetServices"),
            type: 'POST',
            dataType: 'json',
            data: {
                IP: serverElement[0].IP,
                UserName: serverElement[0].UserName,
                Password: serverElement[0].Password,
                ServiceName: DOMElements.ServiceName.val(),
                Status: DOMElements.Status.val()
            },
            cache: true,
            ifModified: true,
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data, textStatus, jqXHR) {
                if (jqXHR.status !== 304) {
                    if (isBackGround == true) {
                        NAP.loadingBox.show();
                    }

                    self.RenderServicesGrid(data);
                }

                NAP.loadingBox.close();
            },
            error: function () {
                self.Relogin();
            }
        });
    };

    self.RenderServicesGrid = function (data) {
        var bodyData = [];
        for (var i = 0; i < data.length; i++) {
            var StartCommand = "<a href='javascript:serviceMonitoring.StartService(\"" + data[i].Name + "\")'>" +
                "<img src='" + NAP.resolveClientUrl("images/Play.png") + "' height='25' width='25' title='Start Service' /></a>";
            var StartCommandDisable = "<img src='" + NAP.resolveClientUrl("images/Play_disable.png") + "' height='25' width='25' title='Start Service Disabled' />";

            var StopCommand = "<a href='javascript:serviceMonitoring.StopService(\"" + data[i].Name + "\")'>" +
                "<img src='" + NAP.resolveClientUrl("images/Stop.png") + "' height='25' width='25' title='Stop Service' /></a>";
            var StopCommandDisable = "<img src='" + NAP.resolveClientUrl("images/Stop_disable.png") + "' height='25' width='25' title='Stop Service Disabled' />";

            var RestartCommand = "<a href='javascript:serviceMonitoring.RestartService(\"" + data[i].Name + "\")'>" +
                "<img src='" + NAP.resolveClientUrl("images/Rerun.png") + "' height='25' width='25' title='Restart Service' /></a>";
            var RestartCommandDisable = "<img src='" + NAP.resolveClientUrl("images/ReRun_disable.png") + "' height='25' width='25' title='Restart Service Disabled' />";

            var AddFavorite = "";

            data[i].EditConfig = "<a href='javascript:serviceMonitoring.GetServiceConfigs(\"" + data[i].Name + "\")'>Edit</a>";
            data[i].Action = "";
            if (data[i].Status == 'Running' || data[i].Status == 'StartPending') {
                data[i].Action += StopCommand;
            }
            else {
                data[i].Action += StartCommand;
            }

            //---------------------
            //            if (data[i].Status == 'Running' || data[i].Status == 'StartPending') {
            //                data[i].Action += StartCommandDisable;
            //            }
            //            else {
            //                data[i].Action += StartCommand;
            //            }

            //            if (data[i].Status == 'Stopped' || data[i].Status == 'StopPending') {
            //                data[i].Action += StopCommandDisable;
            //            }
            //            else {
            //                data[i].Action += StopCommand;
            //            }

            //            if (data[i].Status == 'Running') {
            //                data[i].Action += RestartCommand;
            //            }
            //            else {
            //                data[i].Action += RestartCommandDisable;
            //            }
            //-------------------------

            bodyData.push(
            {
                Columns: [
                    { Name: 'Name', Value: data[i].Name },
                    { Name: 'Description', Value: data[i].Description },
                    { Name: 'StartupType', Value: data[i].StartupType },
                    { Name: 'EditConfig', Value: data[i].EditConfig },
                    { Name: 'Action', Value: data[i].Action }
                ]
            });
        }

        var gridOptions = {
            columns: [
                { HeaderText: "Service Name", Name: "Name", Width: 350, HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                { HeaderText: "Description", Name: "Description", Width: 635, HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                { HeaderText: "Startup Type", Name: "StartupType", Width: 80, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Edit Config", Name: "EditConfig", Width: 70, HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Action", Name: "Action", Width: 70, HeaderAlign: "center", CellAlign: "center" }
            ],
            showPager: true,
            bodyRows: bodyData,
            gridExpandHeight: 360,
            pagerOption: {
                itemsPerPage: 20,
                currentPage: 0,
                itemsPerPageArray: [10, 20, 50, 100]
            }
        };

        DOMElements.Grid.fanexGrid(gridOptions);
        DOMElements.GridContainer.show();
    };

    self.StartService = function (serviceName) {
        NAP.loadingBox.show();
        var serverElement = $.grep(ServerList, function (value) {
            return value.IP == DOMElements.Server.val();
        });

        $.ajax({
            url: NAP.resolveClientUrl("ServiceMonitoring/StartService"),
            type: 'POST',
            dataType: 'json',
            data: {
                IP: serverElement[0].IP,
                UserName: serverElement[0].UserName,
                Password: serverElement[0].Password,
                ServiceName: serviceName,
                Status: ''
            },
            cache: false,
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                self.Submit();
            },
            error: function (err, errmsg, msg) {
                alert("Service cannot be started.");
                NAP.loadingBox.close();
            }
        });
    }

    self.StopService = function (serviceName) {
        NAP.loadingBox.show();
        var serverElement = $.grep(ServerList, function (value) {
            return value.IP == DOMElements.Server.val();
        });

        $.ajax({
            url: NAP.resolveClientUrl("ServiceMonitoring/StopService"),
            type: 'POST',
            dataType: 'json',
            data: {
                IP: serverElement[0].IP,
                UserName: serverElement[0].UserName,
                Password: serverElement[0].Password,
                ServiceName: serviceName,
                Status: ''
            },
            cache: false,
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                self.Submit();
            },
            error: function (err, errmsg, msg) {
                alert("Service cannot be stopped.");
                NAP.loadingBox.close();
            }
        });
    }

    self.RestartService = function (serviceName) {
        NAP.loadingBox.show();
        var serverElement = $.grep(ServerList, function (value) {
            return value.IP == DOMElements.Server.val();
        });

        $.ajax({
            url: NAP.resolveClientUrl("ServiceMonitoring/RestartService"),
            type: 'POST',
            dataType: 'json',
            data: {
                IP: serverElement[0].IP,
                UserName: serverElement[0].UserName,
                Password: serverElement[0].Password,
                ServiceName: serviceName,
                Status: ''
            },
            cache: false,
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                self.Submit();
            },
            error: function (err, errmsg, msg) {
                alert("Service cannot be restarted.");
                NAP.loadingBox.close();
            }
        });
    }

    self.GetServiceConfigs = function (serviceName) {
        NAP.loadingBox.show();
        var serverElement = $.grep(ServerList, function (value) {
            return value.IP == DOMElements.Server.val();
        });

        $.ajax({
            url: NAP.resolveClientUrl("ServiceMonitoring/GetServiceConfigs"),
            type: 'POST',
            dataType: 'json',
            data: {
                IP: serverElement[0].IP,
                UserName: serverElement[0].UserName,
                Password: serverElement[0].Password,
                ServiceName: serviceName,
                Status: ''
            },
            cache: false,
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                //DOMElements.Grid.hide();
                self.RenderServiceConfigsGrid(serviceName, data);
                NAP.loadingBox.close();
            },
            error: function () {
                alert('error');
            }
        });
    }

    self.RenderServiceConfigsGrid = function (serviceName, data) {
        var bodyData = [];
        for (var i = 0; i < data.length; i++) {
            var editCommand = "<a href='javascript:serviceMonitoring.EditConfigs(\"" + data[i] + "\", \"" + serviceName + "\")'>Edit</a>";
            bodyData.push({ Columns: [
                    { Name: 'ConfigFileName', Value: data[i] },
                    { Name: 'Edit', Value: editCommand }
                ]
            });
        }

        var gridOptions = {
            columns: [
                { HeaderText: "Config File Name", Name: "ConfigFileName", Width: 400, HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                { HeaderText: "Edit", Name: "Edit", Width: 100, HeaderAlign: "center", CellAlign: "center" }
            ],
            showTitle: true,
            showPager: false,
            titleInfo: {
                showExcelButton: false,
                text: "<a href='javascript:serviceMonitoring.BackToServiceGrid()'>" + serviceName + "</a>" + " > Configuration"
            },
            bodyRows: bodyData,
            gridExpandHeight: 360
        };

        DOMElements.EditConfigGrid.fanexGrid(gridOptions);
        DOMElements.GridContainer.hide();
        DOMElements.EditConfigGridContainer.show();
    }

    self.BackToServiceGrid = function () {
        DOMElements.EditConfigGrid.empty();
        DOMElements.EditConfigGridContainer.hide();
        DOMElements.GridContainer.show();
    }

    self.EditConfigs = function (fileName, serviceName) {
        NAP.loadingBox.show();
        var serverElement = $.grep(ServerList, function (value) {
            return value.IP == DOMElements.Server.val();
        });

        DOMElements.EditConfigDialog.empty();
        DOMElements.EditConfigDialog.append("<iframe src='" +
            NAP.resolveClientUrl("ServiceMonitoring/EditServiceConfigFile") +
            "?IP=" + serverElement[0].IP +
            "&UserName=" + serverElement[0].UserName +
            "&Password=" + serverElement[0].Password +
            "&ServiceName=" + serviceName +
            "&ConfigFileName=" + fileName + "' style='width: 1000px; height: 600px'></iframe>");
        NAP.loadingBox.close();
        DOMElements.EditConfigDialog.dialog('open');
    }

    self.AutoRefresh = function () {
        if (DOMElements.Grid.is(':visible') == true) {
            self.Submit(true);
        }
    }
}

$(document).ready(function () {
    serviceMonitoring = new ServiceMonitoring();
    serviceMonitoring.Init();
});