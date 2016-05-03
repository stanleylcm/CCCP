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
    public class CrisisApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public CrisisModel GetNewCrisis()
        {
            return new CrisisModel();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public CrisisModel GetCrisis(int Id)
        {
            CrisisModel result = new CrisisModel();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                // load enquiry details
                result.Entity = db.Crisis.Find(Id);

                // may not have chatroom and checklist if the crisis is pending for approval...

                // load chatroom
                if (result.Entity.ChatRoomId > 0)
                {
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
                }

                // load checklists
                int checklistBatchID = result.Entity.ChecklistBatchId;
                if (checklistBatchID > 0)
                {
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
                }
            }

            // result
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public CrisisModel GetCrisis()
        {
            CrisisModel result = new CrisisModel();
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public List<Crisis> GetCrisisApprovalList()
        {
            List<Crisis> result = new List<Crisis>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                string PendingForApproval = CrisisStatus.Pending_For_Approval.ToEnumString();
                string Rejected = CrisisStatus.Rejected.ToEnumString();
                result = db.Crisis.Where(m => m.Status == PendingForApproval).ToList();
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public List<Crisis> GetCrisisList()
        {
            List<Crisis> result = new List<Crisis>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                string PendingForApproval = CrisisStatus.Pending_For_Approval.ToEnumString();
                string Rejected = CrisisStatus.Rejected.ToEnumString();
                result = db.Crisis.Where(m => m.Status != PendingForApproval && m.Status != Rejected).ToList();
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int Edit(CrisisModel crisis)
        {
            CCCPDbContext db = new CCCPDbContext();

            // prepare history etc. before save
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
            if (crisis.Entity.Status == CrisisStatus.Closed.ToEnumString())
            {
                crisis.PrepareSave(PrepareSaveMode.Closed);
            }
            else
            {
                crisis.PrepareSave();
            }

            db.Crisis.Attach(crisis.Entity);
            foreach (ChecklistModel cl in crisis.Checklists)
            {
                foreach (ChecklistActionModel clAction in cl.ChecklistActions)
                {
                    db.ChecklistAction.Attach(clAction.Entity);
                    db.Entry(clAction.Entity).State = EntityState.Modified;
                }
            }

            db.Entry(crisis.Entity).State = EntityState.Modified;
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

            return crisis.Entity.CrisisId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int Approve(int id)
        {
            CCCPDbContext db = new CCCPDbContext();

            CrisisModel crisis = GetCrisis(id);
            crisis.PrepareSave(PrepareSaveMode.Approved);
            db.Crisis.Attach(crisis.Entity);
            db.Entry(crisis.Entity).State = EntityState.Modified;
            db.SaveChanges();
            db.usp_Crisis_PostCreate(crisis.Entity.CrisisId, crisis.Entity.CreatedBy, Common.CheckListActionStatus.Pending.ToEnumString());

            new IncidentApiController().SetIncidentStatus(crisis.IncidentId, MasterTableService.GetIncidentTypeId(crisis.IncidentType), IncidentStatus.In_Crisis.ToEnumString());

            NotificationService.SendApproveCrisisNotification(id, crisis.Entity.CrisisNo, crisis.IncidentNo, crisis.IncidentType);

            return crisis.Entity.CrisisId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int Reject(int id, string rejectReason)
        {
            CCCPDbContext db = new CCCPDbContext();

            CrisisModel crisis = GetCrisis(id);
            crisis.Entity.RejectReason = rejectReason;
            crisis.PrepareSave(PrepareSaveMode.Rejected);
            db.Crisis.Attach(crisis.Entity);
            db.Entry(crisis.Entity).State = EntityState.Modified;
            db.SaveChanges();

            new IncidentApiController().SetIncidentStatus(crisis.IncidentId, MasterTableService.GetIncidentTypeId(crisis.IncidentType), IncidentStatus.In_Progress.ToEnumString());

            return crisis.Entity.CrisisId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int Close(int id)
        {
            CCCPDbContext db = new CCCPDbContext();

            CrisisModel crisis = GetCrisis(id);
            crisis.PrepareSave(PrepareSaveMode.Closed);
            db.Crisis.Attach(crisis.Entity);
            db.Entry(crisis.Entity).State = EntityState.Modified;
            db.SaveChanges();

            new IncidentApiController().SetIncidentStatus(crisis.IncidentId, MasterTableService.GetIncidentTypeId(crisis.IncidentType), IncidentStatus.In_Progress.ToEnumString());

            return crisis.Entity.CrisisId;
        }
    }
}