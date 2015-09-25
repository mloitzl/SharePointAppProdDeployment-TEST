using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;
using SharePointAppProdDeployment_TESTWeb.Models;
using SharePointAppProdDeployment_TESTWeb.SignalR;

namespace SharePointAppProdDeployment_TESTWeb.Services
{
    public class ListEventReceiver : IRemoteEventService
    {
        /// <summary>
        /// Handles events that occur before an action occurs, such as when a user adds or deletes a list item.
        /// </summary>
        /// <param name="properties">Holds information about the remote event.</param>
        /// <returns>Holds information returned from the remote event.</returns>
        public SPRemoteEventResult ProcessEvent(SPRemoteEventProperties properties)
        {
            SPRemoteEventResult result = new SPRemoteEventResult();

            using (Models.Model1 db = new Models.Model1())
            {
                db.Log("ProcessEvent Event {0}", properties.EventType.ToString());
            }

            Notifications.Instance.BroadcastNotification(new Notification
            {
                Originator = properties.ItemEventProperties.UserDisplayName,
                Message = string.Format(
                    "performed Event {0} on List {1} item {2}", 
                    properties.EventType.ToString(), "'" + properties.ItemEventProperties.ListTitle + "'", 
                    properties.ItemEventProperties.AfterProperties.ContainsKey("Title") ? 
                    properties.ItemEventProperties.AfterProperties["Title"] : " with id: " + properties.ItemEventProperties.ListItemId)
            });

            using (ClientContext clientContext = TokenHelper.CreateRemoteEventReceiverClientContext(properties))
            {
                if (clientContext != null)
                {
                    clientContext.Load(clientContext.Web);
                    clientContext.ExecuteQuery();
                }
            }

            return result;
        }

        /// <summary>
        /// Handles events that occur after an action occurs, such as after a user adds an item to a list or deletes an item from a list.
        /// </summary>
        /// <param name="properties">Holds information about the remote event.</param>
        public void ProcessOneWayEvent(SPRemoteEventProperties properties)
        {
            using (Models.Model1 db = new Models.Model1())
            {
                db.Log("ProcessOneWayEvent Event {0}", properties.EventType.ToString());
            }

            using (ClientContext clientContext = TokenHelper.CreateRemoteEventReceiverClientContext(properties))
            {
                if (clientContext != null)
                {
                    clientContext.Load(clientContext.Web);
                    clientContext.ExecuteQuery();
                }
            }
        }
    }
}
