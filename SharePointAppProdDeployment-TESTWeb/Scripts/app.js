/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/signalr/signalr.d.ts" />
$(function () {
    //// Declare a proxy to reference the hub.
    var chat = $.connection.notificationsHub;
    //// Create a function that the hub can call to broadcast messages.
    chat.client.broadcastNotification = function (notification) {
        // Html encode display name and message.
        var encodedName = $('<div />').text(notification.Originator).html();
        var encodedMsg = $('<div />').text(notification.Message).html();
        // Add the message to the page.
        $('#discussion').append('<p><strong>' + encodedName
            + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</p>');
    };
    //// Get the user name and store it to prepend to messages.
    //$('#displayname').val(prompt('Enter your name:', ''));
    //// Set initial focus to message input box.
    //$('#message').focus();
    //// Start the connection.
    $.connection.hub.start().done(function () {
        console.log("hub started");
        //    //    $('#sendmessage').click(function () {
        //    //        // Call the Send method on the hub.
        //    //        chat.server.send($('#displayname').val(), $('#message').val());
        //    //        // Clear text box and reset focus for next comment.
        //    //        $('#message').val('').focus();
        //    //    });
        //    //
    });
});
//# sourceMappingURL=app.js.map