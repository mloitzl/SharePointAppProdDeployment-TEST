using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;
using SharePointAppProdDeployment_TESTWeb.Models;
using System.ServiceModel;

namespace SharePointAppProdDeployment_TESTWeb.Services
{
    public class AppEventReceiver : IRemoteEventService
    {
        private const string taskListTitle = "AppTasks";
        private const string taskListRERTitle = "AppTasksRER";

        private static EventReceiverType[] itemEvents = new EventReceiverType[] {
            EventReceiverType.ItemAdded,
            EventReceiverType.ItemAdding,
            EventReceiverType.ItemDeleted,
            EventReceiverType.ItemDeleting
        };

        /// <summary>
        /// Handles app events that occur after the app is installed or upgraded, or when app is being uninstalled.
        /// </summary>
        /// <param name="properties">Holds information about the app event.</param>
        /// <returns>Holds information returned from the app event.</returns>
        public SPRemoteEventResult ProcessEvent(SPRemoteEventProperties properties)
        {
            SPRemoteEventResult result = new SPRemoteEventResult();

            var list = EnsureTasksList(properties);

            EnsureItemEventReceiver(properties, list);


            using (ClientContext clientContext = TokenHelper.CreateAppEventClientContext(properties, useAppWeb: false))
            {
                if (clientContext != null)
                {
                    clientContext.Load(clientContext.Web, w => w.Title);
                    clientContext.ExecuteQuery();

                    using (Models.Model1 db = new Models.Model1())
                    {
                        db.AuditLogEntries.Add(new Models.AuditLogEntry
                        {
                            Date = DateTime.Now,
                            Message = string.Format("App Installed in {0}", clientContext.Web.Title)
                        });

                        db.SaveChanges();
                    }
                }
            }

            return result;
        }

        private void EnsureItemEventReceiver(SPRemoteEventProperties properties, List list)
        {
            using (Models.Model1 db = new Models.Model1())
            {
                using (ClientContext clientContext = TokenHelper.CreateAppEventClientContext(properties, useAppWeb: false))
                {
                    if (clientContext != null)
                    {
                        var lst = clientContext.Web.Lists.GetById(list.Id);
                        var receivers = lst.EventReceivers;
                        clientContext.Load(lst);
                        clientContext.Load(receivers);
                        clientContext.ExecuteQuery();

                        List<EventReceiverDefinition> myReceivers = new List<EventReceiverDefinition>();
                        foreach (var rcvr in receivers)
                        {
                            db.Log("Found Receiver {0}", rcvr.ReceiverName);

                            for (int i = 0; i < itemEvents.Length; i++)
                            {
                                if (rcvr.ReceiverName.Equals(GetReceivername(taskListRERTitle, (int)itemEvents[i])))
                                    myReceivers.Add(rcvr);
                            }
                        }

                        if (myReceivers.Count > 0)
                        {
                            foreach (var rcvr in myReceivers)
                            {
                                rcvr.DeleteObject();
                                db.Log("Deleting Receiver {0}", rcvr.ReceiverName);
                            }
                            try
                            {
                                clientContext.ExecuteQuery();
                            }
                            catch (Exception)
                            {
                                // dev only problem
                            }
                            
                            myReceivers.Clear();
                        }

                        if (myReceivers.Count < 1)
                        {
                            for (int i = 0; i < itemEvents.Length; i++)
                            {
                                EventReceiverDefinitionCreationInformation erdci = new EventReceiverDefinitionCreationInformation();
                                erdci.Synchronization = EventReceiverSynchronization.Synchronous;
                                erdci.ReceiverName = GetReceivername(taskListRERTitle, (int)itemEvents[i]);
                                erdci.EventType = itemEvents[i];
                                var uri = OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri.Substring(0, OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri.LastIndexOf("/"));
                                erdci.ReceiverUrl = string.Format("{0}/ListEventReceiver.svc", uri);
                                lst.EventReceivers.Add(erdci);
                                db.Log("Added Receiver {0}", erdci.ReceiverName);
                            }
                            clientContext.ExecuteQuery();
                        }
                    }
                }
            }
        }

        private static string GetReceivername(string prefix, int id)
        {
            return string.Format("{0}-{1}", prefix, id.ToString());
        }

        private List EnsureTasksList(SPRemoteEventProperties properties)
        {
            List list = null;
            using (Models.Model1 db = new Models.Model1())
            {
                using (ClientContext clientContext = TokenHelper.CreateAppEventClientContext(properties, useAppWeb: false))
                {
                    if (clientContext != null)
                    {
                        clientContext.Load(clientContext.Web, w => w.ServerRelativeUrl);
                        clientContext.ExecuteQuery();

                        list = clientContext.Web.Lists.GetByTitle(taskListTitle);
                        clientContext.Load(list, l => l.Id);

                        try
                        {
                            clientContext.ExecuteQuery();
                            Models.Model1Extensions.Log(db, "List {0} exists", taskListTitle);
                        }
                        catch (ServerException sex)
                        {
                            if (sex.ServerErrorTypeName == typeof(System.ArgumentException).FullName)
                            {
                                Models.Model1Extensions.Log(db, "List {0} does not exist, creating", taskListTitle);
                                ListCreationInformation lci = new ListCreationInformation();
                                lci.Title = taskListTitle;
                                lci.TemplateType = (int)ListTemplateType.TasksWithTimelineAndHierarchy;
                                lci.QuickLaunchOption = QuickLaunchOptions.On;
                                list = clientContext.Web.Lists.Add(lci);

                                clientContext.Load(list, l => l.DefaultViewUrl, l => l.Id);
                                clientContext.ExecuteQuery();
                                Models.Model1Extensions.Log(db, "List {0}{1} ({2}) created", properties.AppEventProperties.HostWebFullUrl, list.DefaultViewUrl, list.Id);
                            }
                        }
                    }
                }
            }
            return list;
        }



        /// <summary>
        /// This method is a required placeholder, but is not used by app events.
        /// </summary>
        /// <param name="properties">Unused.</param>
        public void ProcessOneWayEvent(SPRemoteEventProperties properties)
        {
            throw new NotImplementedException();
        }

    }
}
