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
    public class IncidentEnvironmentAirEmissionApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentEnvironmentAirEmissionModel GetNewIncident()
        {
            return new IncidentEnvironmentAirEmissionModel();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentEnvironmentAirEmissionModel GetIncident(int incidentId)
        {
            IncidentEnvironmentAirEmissionModel result = new IncidentEnvironmentAirEmissionModel();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                // load incident details
                result.Entity = db.IncidentEnvironmentAirEmission.Find(incidentId);
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
                result.LinkedIncidentEntities = db.usp_Incident_GetLinkedIncident(incidentId, MasterTableService.GetIncidentTypeId(IncidentTypeSubType.EnvironmentAirEmission)).ToList<usp_Incident_GetLinkedIncident_Result>();

                // load linked general enquiry
                result.LinkedGeneralEnquiryEntities = db.usp_Incident_GetLinkedGeneralEnquiry(incidentId, MasterTableService.GetIncidentTypeId(IncidentTypeSubType.EnvironmentAirEmission)).ToList<usp_Incident_GetLinkedGeneralEnquiry_Result>();

                // load notification
                int incidentTypeId = MasterTableService.GetIncidentTypeId(IncidentTypeSubType.EnvironmentAirEmission);
                result.NotificationEntities = (from notice in db.Notification
                                               where notice.IncidentId == result.Entity.IncidentEnvironmentAirEmissionId &&
                                                     notice.IncidentTypeId == incidentTypeId
                                               orderby notice.CreatedDateTime descending
                                               select notice).ToList<Notification>();
                //

                if (result.Entity.CrisisId != null && result.Entity.CrisisId > 0)
                {
                    result.CrisisEntity.Entity = db.Crisis.Find(result.Entity.CrisisId);
                }
            }

            // result
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentEnvironmentAirEmissionModel GetIncident()
        {
            IncidentEnvironmentAirEmissionModel result = new IncidentEnvironmentAirEmissionModel();
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public List<IncidentEnvironmentAirEmission> GetIncidentList()
        {
            List<IncidentEnvironmentAirEmission> result = new List<IncidentEnvironmentAirEmission>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                result = db.IncidentEnvironmentAirEmission.ToList();
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CreateIncident(IncidentEnvironmentAirEmission incidentEnvironmentAirEmission)
        {
            CCCPDbContext db = new CCCPDbContext();
            IncidentEnvironmentAirEmissionModel incident = new IncidentEnvironmentAirEmissionModel();

            incident.Entity = incidentEnvironmentAirEmission;
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
            incident.PrepareSave(PrepareSaveMode.Created);
            incidentEnvironmentAirEmission = incident.Entity;

            int incidentTypeId = MasterTableService.GetIncidentTypeId(IncidentTypeSubType.EnvironmentAirEmission);
            db.IncidentEnvironmentAirEmission.Add(incidentEnvironmentAirEmission);
            db.SaveChanges();
            db.usp_Incident_PostCreate(incidentEnvironmentAirEmission.IncidentEnvironmentAirEmissionId, incidentTypeId, incident.Entity.CreatedBy, Common.CheckListActionStatus.Pending.ToString());

            NotificationService.SendCreateIncidentNotification(incidentEnvironmentAirEmission.IncidentEnvironmentAirEmissionId,
                                                               incidentEnvironmentAirEmission.IncidentNo,
                                                               IncidentTypeSubType.EnvironmentAirEmission);

            return incident.Entity.IncidentEnvironmentAirEmissionId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int EditIncident(IncidentEnvironmentAirEmissionModel incident)
        {
            CCCPDbContext db = new CCCPDbContext();

            // prepare history etc. before save
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
            if (incident.Entity.IncidentStatus == IncidentStatus.Closed.ToEnumString())
            {
                incident.PrepareSave(PrepareSaveMode.Closed);
            }
            else
            {
                incident.PrepareSave();
            }

            db.IncidentEnvironmentAirEmission.Attach(incident.Entity);
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
                NotificationService.SendUpdateIncidentLevelNotification(incident.Entity.IncidentEnvironmentAirEmissionId,
                                                                        incident.Entity.IncidentNo,
                                                                        incident.OriginalLevelOfSeverity,
                                                                        incident.Entity.LevelOfSeverity,
                                                                        IncidentTypeSubType.EnvironmentAirEmission);
            }

            return incident.Entity.IncidentEnvironmentAirEmissionId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CancelIncident(int id)
        {
            CCCPDbContext db = new CCCPDbContext();

            IncidentEnvironmentAirEmission incidentEnvironmentAirEmission = db.IncidentEnvironmentAirEmission.Find(id);

            IncidentEnvironmentAirEmissionModel incidentModel = new IncidentEnvironmentAirEmissionModel();
            incidentModel.Entity = incidentEnvironmentAirEmission;
            incidentModel.PrepareSave(PrepareSaveMode.Cancelled);

            db.SaveChanges();

            NotificationService.SendCancelIncidentNotification(id, incidentModel.Entity.IncidentNo, IncidentTypeSubType.EnvironmentAirEmission);

            return incidentEnvironmentAirEmission.IncidentEnvironmentAirEmissionId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CloseIncident(int id)
        {
            CCCPDbContext db = new CCCPDbContext();

            IncidentEnvironmentAirEmission incidentEnvironmentAirEmission = db.IncidentEnvironmentAirEmission.Find(id);

            IncidentEnvironmentAirEmissionModel incidentModel = new IncidentEnvironmentAirEmissionModel();
            incidentModel.Entity = incidentEnvironmentAirEmission;
            incidentModel.PrepareSave(PrepareSaveMode.Closed);

            db.SaveChanges();

            NotificationService.SendCloseIncidentNotification(id, incidentModel.Entity.IncidentNo, IncidentTypeSubType.EnvironmentAirEmission);

            return incidentEnvironmentAirEmission.IncidentEnvironmentAirEmissionId;
        }
    }
}