function deleteSession(matchId, session) {
    var confirmMessage = confirm("Session Info \nMatchId: " + matchId + "\nSession: " + session + " \nAre you sure you want to DELETE this row?");
    if (confirmMessage) {
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

function deleteSettlementProcessing(matchId, sessionId, dbServer, isSettleLock, total) {
    var confirmMessage = "";
    if (isSettleLock) {
        confirmMessage = confirm("MatchId: " + matchId + "\nSession: " + sessionId + "\nAction: Delete SettlementLock \nAre you sure you want to DELETE this row?");
    } else {
        confirmMessage = dbServer === "DBTrans" ? "Please check and delete locked match from function Delete Temp Variable Number before recalculate.\n" : "";
        confirmMessage = confirm(confirmMessage + "MatchId: " + matchId + "\nDelete " + total +" rows belong to sessionid " + sessionId + "\nAction: Delete SettlementProcessing \nAre you sure you want to DELETE this row?");
    }
    if (confirmMessage) {
        sharedScriptManager.blockUI();
        $.ajax({
            type: 'POST',
            url: window.rootUrl + 'TempVariable/DeleteSettlementLock/Delete',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
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
    var confirmMessage = confirm("Please ensure you check and delete data in table SettlementProcessing before updating.\nMatchId: " + matchId + "\nSession: " + sessionId + " \nAre you sure you want to UPDATE Resolved?");
    if (confirmMessage) {
        sharedScriptManager.blockUI();
        $.ajax({
            type: 'POST',
            url: window.rootUrl + 'TempVariable/DeleteSettlementLock/Update',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
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