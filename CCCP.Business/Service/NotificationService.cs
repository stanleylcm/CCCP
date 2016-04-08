using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.Business.Model;
using CCCP.ViewModel;

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
        }

        public static void SendCreateCrisisNotification(int crisisId, string crisisNo, IncidentTypeSubType incidentType)
        {
            CCCPDbContext db = new CCCPDbContext();
            Notification notice = new Notification();
            notice.UserId = AccessControlService.CurrentUser.Entity.UserId;
            notice.CrisisId = crisisId;
            notice.IncidentTypeId = MasterTableService.GetIncidentTypeId(incidentType);
            notice.MessageType = NotificationMessageType.Crisis.ToEnumString();
            notice.Message = string.Format("Crisis {0} for type {1} has been created.",
                                            crisisNo,
                                            MasterTableService.GetIncidentTypeName(incidentType)
                                            );
            notice.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.CreatedDateTime = DateTime.Now;
            notice.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            notice.LastUpdatedDateTime = DateTime.Now;

            db.Notification.Add(notice);
            db.SaveChanges();
        }
    }
}
