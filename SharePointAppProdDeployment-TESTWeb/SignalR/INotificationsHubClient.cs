using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointAppProdDeployment_TESTWeb.SignalR
{
   public  interface INotificationsHubClient
    {
        List<Notification> GetAllNotifications();
        void BroadcastNotification(Notification notification);

        //void Hello();
    }
}
