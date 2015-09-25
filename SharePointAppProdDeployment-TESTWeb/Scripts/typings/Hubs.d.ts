// Get signalr.d.ts.ts from https://github.com/borisyankov/DefinitelyTyped (or delete the reference)
/// <reference path="./signalr/signalr.d.ts" />
/// <reference path="./jquery/jquery.d.ts" />

////////////////////
// available hubs //
////////////////////
//#region available hubs

interface SignalR {

    /**
      * The hub implemented by SharePointAppProdDeployment_TESTWeb.SignalR.NotificationsHub
      */
    notificationsHub : NotificationsHub;
}
//#endregion available hubs

///////////////////////
// Service Contracts //
///////////////////////
//#region service contracts

//#region NotificationsHub hub

interface NotificationsHub {
    
    /**
      * This property lets you send messages to the NotificationsHub hub.
      */
    server : NotificationsHubServer;

    /**
      * The functions on this property should be replaced if you want to receive messages from the NotificationsHub hub.
      */
    client : INotificationsHubClient;
}

interface NotificationsHubServer {

    /** 
      * Sends a "getAllNotifications" message to the NotificationsHub hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of Notification[]}
      */
    getAllNotifications() : JQueryPromise<Notification[]>

    /** 
      * Sends a "broadcastNotification" message to the NotificationsHub hub.
      * Contract Documentation: ---
      * @param notification {Notification} 
      * @return {JQueryPromise of void}
      */
    broadcastNotification(notification : Notification) : JQueryPromise<void>
}

interface INotificationsHubClient
{

    /**
      * Set this function with a "function(){}" to receive the "getAllNotifications" message from the NotificationsHub hub.
      * Contract Documentation: ---
      * @return {void}
      */
    getAllNotifications : () => void;

    /**
      * Set this function with a "function(notification : Notification){}" to receive the "broadcastNotification" message from the NotificationsHub hub.
      * Contract Documentation: ---
      * @param notification {Notification} 
      * @return {void}
      */
    broadcastNotification : (notification : Notification) => void;
}

//#endregion NotificationsHub hub

//#endregion service contracts



////////////////////
// Data Contracts //
////////////////////
//#region data contracts


/**
  * Data contract for SharePointAppProdDeployment_TESTWeb.SignalR.Notification
  */
interface Notification {
    Originator : string;
    Message : string;
}

//#endregion data contracts

