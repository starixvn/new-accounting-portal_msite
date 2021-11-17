function deleteSession(matchId, session) {
    var con = confirm("Session Info \nMatchId: " + matchId + "\nSession: " + session + " \nAre you sure you want to DELETE this row?");
    if (con) {
        $.ajax({
            type: 'POST',
            url: window.rootUrl + 'TempVariable/DeleteParlayLocking/Delete',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            async: false,
            data: JSON.stringify({
                Session: session
            }),
            success: function () {
                document.getElementById('parlayLockingForm').submit();
            }
        });
    }
}

function deleteSettlementProcessing(matchId, sessionId, dbServer, isSettleLock) {
    var actionName = isSettleLock ? "Delete SettlementLock" : "Delete SettlementProcessing";
    var con = confirm("Session Info \nMatchId: " + matchId + "\nSession: " + sessionId + "\nAction: " + actionName + " \nAre you sure you want to DELETE this row?");
    if (con) {
        $.ajax({
            type: 'POST',
            url: window.rootUrl + 'TempVariable/DeleteSettlementLock/Delete',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            async: false,
            data: JSON.stringify({
                sessionId,
                dbServer,
                isSettleLock
            }),
            success: function () {
                document.getElementById('deleteSettlementLockForm').submit();
            }
        });
    }
}

function updateSettlementLogs(matchId, sessionId, startTime, dbServer) {
    var con = confirm("Session Info \nMatchId: " + matchId + "\nSession: " + sessionId + " \nAre you sure you want to UPDATE Resolved?");
    if (con) {
        $.ajax({
            type: 'POST',
            url: window.rootUrl + 'TempVariable/DeleteSettlementLock/Update',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            async: false,
            data: JSON.stringify({
                sessionId,
                startTime,
                dbServer
            }),
            success: function () {
                document.getElementById('deleteSettlementLockForm').submit();
            }
        });
    }
}