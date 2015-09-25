using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharePointAppProdDeployment_TESTWeb.SignalR
{
    [HubName("")]
    public class NotificationsHub : Hub
    {

        Notifications _notifications = null;
        public NotificationsHub() : this(Notifications.Instance)
        {

        }

        public NotificationsHub(Notifications instance)
        {
            _notifications = instance;
        }

        public List<Notification> GetAllNotifications()
        {
            return _notifications.Notifactions;
        }

        //public void Send(string name, string message)
        //{
        //    Clients.All.broadcastMessage(name, message);
        //}
    }

    public class Notifications
    {
        private Notifications(IHubConnectionContext<dynamic> clients ) {
            Clients = clients;
        }

        private readonly static Lazy<Notifications> _instance = new Lazy<Notifications>(() => new Notifications(GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>().Clients));

        private IHubConnectionContext<dynamic> Clients { get; set; }

        public static Notifications Instance
        {

            get
            {
                return _instance.Value;
            }
        }

        public void BroadcastNotification(Notification notification)
        {
            Clients.All.publishNotification(notification);
        }

        internal List<Notification> Notifactions { get; set; }
    }

    public class Notification
    {
        public string Originator { get; set; }
        public string Message { get; set; }
    }
}