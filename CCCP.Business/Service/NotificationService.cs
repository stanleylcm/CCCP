using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.Business.Model;
using CCCP.ViewModel;
using System.Net;
using System.IO;

namespace CCCP.Business.Service
{
    public class NotificationService
    {
        public static void SendCreateIncidentNotification(int incidentId, string incidentNo, IncidentTypeSubType incidentType)
        {
            CCCPDbContext db = new CCCPDbContext();
            Notification notice = new Notification();
            notice.UserId = AccessControlService.CurrentUser.Entity.UserId;
            notice.IncidentId = incidentId;
            notice.IncidentTypeId = MasterTableService.GetIncidentTypeId(incidentType);
            notice.MessageType = NotificationMessageType.Incident.ToEnumString();
            notice.Message = string.Format("Incident {0} for type {1} has been created.",
                                            incidentNo,
                                            MasterTableService.GetIncidentTypeName(incidentType)
                                            );
            notice.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.CreatedDateTime = DateTime.Now;
            notice.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.LastUpdatedDateTime = DateTime.Now;

            db.Notification.Add(notice);
            db.SaveChanges();

            PushNotification(notice.NotificationId);
        }

        public static void SendUpdateIncidentLevelNotification(int incidentId, string incidentNo, string oriLevel, string newLevel, IncidentTypeSubType incidentType)
        {
            int iOLevel = Convert.ToInt32(oriLevel);
            IncidentLevel OLevel = (IncidentLevel)iOLevel;

            int iNLevel = Convert.ToInt32(newLevel);
            IncidentLevel NLevel = (IncidentLevel)iNLevel;

            CCCPDbContext db = new CCCPDbContext();
            Notification notice = new Notification();
            notice.UserId = AccessControlService.CurrentUser.Entity.UserId;
            notice.IncidentId = incidentId;
            notice.IncidentTypeId = MasterTableService.GetIncidentTypeId(incidentType);
            notice.MessageType = NotificationMessageType.Incident.ToEnumString();
            notice.Message = string.Format("Incident {0} for type {1} has been updated from {2} to {3}.",
                                            incidentNo,
                                            MasterTableService.GetIncidentTypeName(incidentType),
                                            OLevel.ToEnumString(),
                                            NLevel.ToEnumString()
                                            );
            notice.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.CreatedDateTime = DateTime.Now;
            notice.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.LastUpdatedDateTime = DateTime.Now;

            db.Notification.Add(notice);
            db.SaveChanges();

            PushNotification(notice.NotificationId);
        }

        public static void SendCloseIncidentNotification(int incidentId, string incidentNo, IncidentTypeSubType incidentType)
        {
            CCCPDbContext db = new CCCPDbContext();
            Notification notice = new Notification();
            notice.UserId = AccessControlService.CurrentUser.Entity.UserId;
            notice.IncidentId = incidentId;
            notice.IncidentTypeId = MasterTableService.GetIncidentTypeId(incidentType);
            notice.MessageType = NotificationMessageType.Incident.ToEnumString();
            notice.Message = string.Format("Incident {0} for type {1} has been closed.",
                                            incidentNo,
                                            MasterTableService.GetIncidentTypeName(incidentType)
                                            );
            notice.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.CreatedDateTime = DateTime.Now;
            notice.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.LastUpdatedDateTime = DateTime.Now;

            db.Notification.Add(notice);
            db.SaveChanges();

            PushNotification(notice.NotificationId);
        }

        public static void SendCancelIncidentNotification(int incidentId, string incidentNo, IncidentTypeSubType incidentType)
        {
            CCCPDbContext db = new CCCPDbContext();
            Notification notice = new Notification();
            notice.UserId = AccessControlService.CurrentUser.Entity.UserId;
            notice.IncidentId = incidentId;
            notice.IncidentTypeId = MasterTableService.GetIncidentTypeId(incidentType);
            notice.MessageType = NotificationMessageType.Incident.ToEnumString();
            notice.Message = string.Format("Incident {0} for type {1} has been cancelled.",
                                            incidentNo,
                                            MasterTableService.GetIncidentTypeName(incidentType)
                                            );
            notice.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.CreatedDateTime = DateTime.Now;
            notice.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.LastUpdatedDateTime = DateTime.Now;

            db.Notification.Add(notice);
            db.SaveChanges();

            PushNotification(notice.NotificationId);
        }

        public static void SendEscalateCrisisNotification(int crisisId, string incidentNo, IncidentTypeSubType incidentType)
        {
            CCCPDbContext db = new CCCPDbContext();
            Notification notice = new Notification();
            notice.UserId = AccessControlService.CurrentUser.Entity.UserId;
            notice.CrisisId = crisisId;
            notice.IncidentTypeId = MasterTableService.GetIncidentTypeId(incidentType);
            notice.MessageType = NotificationMessageType.Escalate_Crisis.ToEnumString();
            notice.Message = string.Format("Incident {0} for type {1} has been escalated to crisis and pending for approval.",
                                            incidentNo,
                                            MasterTableService.GetIncidentTypeName(incidentType)
                                            );
            notice.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.CreatedDateTime = DateTime.Now;
            notice.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.LastUpdatedDateTime = DateTime.Now;

            db.Notification.Add(notice);
            db.SaveChanges();

            PushNotification(notice.NotificationId);
        }

        public static void SendApproveCrisisNotification(int crisisId, string crisisNo, string incidentNo, IncidentTypeSubType incidentType)
        {
            CCCPDbContext db = new CCCPDbContext();
            Notification notice = new Notification();
            notice.UserId = AccessControlService.CurrentUser.Entity.UserId;
            notice.CrisisId = crisisId;
            notice.IncidentTypeId = MasterTableService.GetIncidentTypeId(incidentType);
            notice.MessageType = NotificationMessageType.Crisis.ToEnumString();
            notice.Message = string.Format("Crisis {0} for incident {1} of type {2} has been approved.",
                                            crisisNo,
                                            incidentNo,
                                            MasterTableService.GetIncidentTypeName(incidentType)
                                            );
            notice.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.CreatedDateTime = DateTime.Now;
            notice.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.LastUpdatedDateTime = DateTime.Now;

            db.Notification.Add(notice);
            db.SaveChanges();

            PushNotification(notice.NotificationId);
        }

        public static void PushNotification(int noticeId)
        {
            CCCPDbContext db = new CCCPDbContext();
            Notification notice = db.Notification.Find(noticeId);

            foreach (User user in db.User.ToList())
            {

                // Create a request using a URL that can receive a post. 
                string sendNotificationEndPoint = System.Configuration.ConfigurationManager.AppSettings["sendNotificationEndPoint"];
                WebRequest request = WebRequest.Create(sendNotificationEndPoint);
                // Set the Method property of the request to POST.
                request.Method = "POST";

                // Create POST data and convert it to a byte array.
                string postData = "params=['" + user.LoginName + "', '" + notice.Message + "']";
                byte[] data = Encoding.UTF8.GetBytes(postData);

                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";

                // Set the ContentLength property of the WebRequest.
                request.ContentLength = data.Length;

                // Get the request stream.
                Stream dataStream = request.GetRequestStream();

                // Write the data to the request stream.
                dataStream.Write(data, 0, data.Length);

                // Close the Stream object.
                dataStream.Close();

                // Get the response.
                WebResponse response = request.GetResponse();

                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();

                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);

                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                // Display the content.
                Console.WriteLine(responseFromServer);

                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
            }
        }
    }
}
