function deleteSession(matchId, session) {
    var con = confirm("Session Info \nMatchId: " + matchId + "\nSession: " + session + " \nAre you sure you want to DELETE this row ?");
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