using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using CCCP.Business.Model;
using CCCP.Business.Service;
using CCCP.ViewModel;
using CCCP.Common;
using System.Data;
using System.Data.Entity;

namespace CCCP.Controllers.WebApi
{
    public class IncidentSystemITSystemApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentSystemITSystemModel GetNewIncident()
        {
            return new IncidentSystemITSystemModel();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentSystemITSystemModel GetIncident(int incidentId)
        {
            IncidentSystemITSystemModel result = new IncidentSystemITSystemModel();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                // load incident details
                result.Entity = db.IncidentSystemITSystem.Find(incidentId);
                result.OriginalLevelOfSeverity = result.Entity.LevelOfSeverity;

                // load checklists
                int checklistBatchID = result.Entity.ChecklistBatchId;
                result.ChecklistEntities = (from checklist in db.Checklist
                                            join department in db.Department on checklist.DepartmentId equals department.DepartmentId
                                            where checklist.ChecklistBatchId.Equals(checklistBatchID)
                                            orderby checklist.SortingOrder
                                            select new ChecklistExtend()
                                            {
                                                ChecklistId = checklist.ChecklistId,
                                                ChecklistBatchId = checklist.ChecklistBatchId,
                                                DepartmentId = checklist.DepartmentId,
                                                SortingOrder = checklist.SortingOrder,
                                                History = checklist.History,
                                                CreatedBy = checklist.CreatedBy,
                                                CreatedDateTime = checklist.CreatedDateTime,
                                                LastUpdatedBy = checklist.LastUpdatedBy,
                                                LastUpdatedDateTime = checklist.LastUpdatedDateTime,
                                                Department = department.Department1
                                            }).ToList<ChecklistExtend>();

                // load checklist actions
                foreach (ChecklistModel checklist in result.Checklists)
                {
                    List<ChecklistAction> actionEntities = (from checklistAction in db.ChecklistAction
                                                            where checklistAction.ChecklistId.Equals(checklist.Entity.ChecklistId)
                                                            orderby checklistAction.SortingOrder
                                                            select checklistAction).ToList<ChecklistAction>();
                    checklist.ChecklistActionEntities = actionEntities;
                }

                // load chat room
                result.Chatroom = new ChatRoomModel(result.Entity.ChatRoomId);

                result.Chatroom.ChatRoomMessagesEntites = (from crMessage in db.ChatRoomMessage
                                                           where crMessage.ChatRoomId.Equals(result.Chatroom.Entity.ChatRoomId)
                                                           orderby crMessage.SendDateTime
                                                           select new ChatRoomMessageModel()
                                                           {
                                                               Entity = crMessage
                                                           }).ToList<ChatRoomMessageModel>();

                foreach (ChatRoomMessageModel crMessage in result.Chatroom.ChatRoomMessagesEntites)
                {
                    crMessage.ChatRoomAttachmentsEntities = (from crAttachment in db.ChatRoomAttachment
                                                             where crAttachment.ChatRoomMessageId.Equals(crMessage.Entity.ChatRoomMessageId)
                                                             select crAttachment).ToList<ChatRoomAttachment>();
                }
                //

                // load linked incident
                result.LinkedIncidentEntities = db.usp_Incident_GetLinkedIncident(incidentId, MasterTableService.GetIncidentTypeId(IncidentTypeSubType.SystemITSystem)).ToList<usp_Incident_GetLinkedIncident_Result>();

                // load linked general enquiry
                //

                // load notification
                int incidentTypeId = MasterTableService.GetIncidentTypeId(IncidentTypeSubType.SystemITSystem);
                result.NotificationEntities = (from notice in db.Notification
                                               where notice.IncidentId == result.Entity.IncidentSystemITSystemId &&
                                                     notice.IncidentTypeId == incidentTypeId
                                               orderby notice.CreatedDateTime descending
                                               select notice).ToList<Notification>();
                //
            }

            // result
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentSystemITSystemModel GetIncident()
        {
            IncidentSystemITSystemModel result = new IncidentSystemITSystemModel();
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public List<IncidentSystemITSystem> GetIncidentList()
        {
            List<IncidentSystemITSystem> result = new List<IncidentSystemITSystem>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                result = db.IncidentSystemITSystem.ToList();
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CreateIncident(IncidentSystemITSystem incidentSystemITSystem)
        {
            CCCPDbContext db = new CCCPDbContext();
            IncidentSystemITSystemModel incident = new IncidentSystemITSystemModel();
            incidentSystemITSystem.IncidentStatus = Common.IncidentStatus.Pending.ToString();
            incident.Entity = incidentSystemITSystem;
            incident.PrepareSave(PrepareSaveMode.Created);
            incidentSystemITSystem = incident.Entity;

            int incidentTypeId = MasterTableService.GetIncidentTypeId(IncidentTypeSubType.SystemITSystem);
            db.IncidentSystemITSystem.Add(incidentSystemITSystem);
            db.SaveChanges();
            db.usp_Incident_PostCreate(incidentSystemITSystem.IncidentSystemITSystemId, incidentTypeId, incident.Entity.CreatedBy, Common.CheckListActionStatus.Pending.ToString());

            NotificationService.SendCreateIncidentNotification(incidentSystemITSystem.IncidentSystemITSystemId,
                                                               incidentSystemITSystem.IncidentNo,
                                                               IncidentTypeSubType.SystemITSystem);

            return incident.Entity.IncidentSystemITSystemId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int EditIncident(IncidentSystemITSystemModel incident)
        {
            CCCPDbContext db = new CCCPDbContext();

            // prepare history etc. before save
            if (incident.Entity.IncidentStatus == IncidentStatus.Closed.ToEnumString())
            {
                incident.PrepareSave(PrepareSaveMode.Closed);
            }
            else
            {
                incident.PrepareSave();
            }

            db.IncidentSystemITSystem.Attach(incident.Entity);
            foreach (ChecklistModel cl in incident.Checklists)
            {
                foreach (ChecklistActionModel clAction in cl.ChecklistActions)
                {
                    db.ChecklistAction.Attach(clAction.Entity);
                    db.Entry(clAction.Entity).State = EntityState.Modified;
                }
            }

            db.Entry(incident.Entity).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                // output validation errors
                Console.WriteLine(sb.ToString());
            }

            if (incident.IsLevelChanged())
            {
                NotificationService.SendUpdateIncidentLevelNotification(incident.Entity.IncidentSystemITSystemId,
                                                                        incident.Entity.IncidentNo,
                                                                        incident.OriginalLevelOfSeverity,
                                                                        incident.Entity.LevelOfSeverity,
                                                                        IncidentTypeSubType.SystemITSystem);
            }

            return incident.Entity.IncidentSystemITSystemId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CancelIncident(int id)
        {
            CCCPDbContext db = new CCCPDbContext();

            IncidentSystemITSystem incidentSystemITSystem = db.IncidentSystemITSystem.Find(id);
            incidentSystemITSystem.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return incidentSystemITSystem.IncidentSystemITSystemId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CloseIncident(int id)
        {
            CCCPDbContext db = new CCCPDbContext();

            IncidentSystemITSystem incidentSystemITSystem = db.IncidentSystemITSystem.Find(id);
            incidentSystemITSystem.IncidentStatus = IncidentStatus.Closed.ToEnumString();
            db.SaveChanges();

            return incidentSystemITSystem.IncidentSystemITSystemId;
        }
    }
}