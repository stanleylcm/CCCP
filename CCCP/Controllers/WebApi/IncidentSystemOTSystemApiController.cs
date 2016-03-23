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
    public class IncidentSystemOTSystemApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentSystemOTSystemModel GetNewIncident()
        {
            return new IncidentSystemOTSystemModel();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentSystemOTSystemModel GetIncident(int incidentId)
        {
            IncidentSystemOTSystemModel result = new IncidentSystemOTSystemModel();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                // load incident details
                result.Entity = db.IncidentSystemOTSystem.Find(incidentId);

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
            }

            // load chat room
            //

            // result
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentSystemOTSystemModel GetIncident()
        {
            IncidentSystemOTSystemModel result = new IncidentSystemOTSystemModel();
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public List<IncidentSystemOTSystem> GetIncidentList()
        {
            List<IncidentSystemOTSystem> result = new List<IncidentSystemOTSystem>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                result = db.IncidentSystemOTSystem.ToList();
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CreateIncident(IncidentSystemOTSystem incidentSystemOTSystem)
        {
            CCCPDbContext db = new CCCPDbContext();
            IncidentSystemOTSystemModel incident = new IncidentSystemOTSystemModel();
            incidentSystemOTSystem.IncidentStatus = Common.IncidentStatus.Pending.ToString();
            incident.Entity = incidentSystemOTSystem;
            incident.PrepareSave(PrepareSaveMode.Created);
            incidentSystemOTSystem = incident.Entity;

            int incidentTypeId = MasterTableService.GetIncidentTypeId(IncidentTypeSubType.SystemOTSystem);
            db.IncidentSystemOTSystem.Add(incidentSystemOTSystem);
            db.SaveChanges();
            db.usp_Incident_PostCreate(incidentSystemOTSystem.IncidentSystemOTSystemId, incidentTypeId, incident.Entity.CreatedBy, Common.CheckListActionStatus.Pending.ToString());

            return incident.Entity.IncidentSystemOTSystemId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int EditIncident(IncidentSystemOTSystemModel incident)
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

            db.IncidentSystemOTSystem.Attach(incident.Entity);
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

            return incident.Entity.IncidentSystemOTSystemId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CancelIncident(int id)
        {
            CCCPDbContext db = new CCCPDbContext();

            IncidentSystemOTSystem incidentSystemOTSystem = db.IncidentSystemOTSystem.Find(id);
            incidentSystemOTSystem.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return incidentSystemOTSystem.IncidentSystemOTSystemId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CloseIncident(int id)
        {
            CCCPDbContext db = new CCCPDbContext();

            IncidentSystemOTSystem incidentSystemOTSystem = db.IncidentSystemOTSystem.Find(id);
            incidentSystemOTSystem.IncidentStatus = IncidentStatus.Closed.ToEnumString();
            db.SaveChanges();

            return incidentSystemOTSystem.IncidentSystemOTSystemId;
        }
    }
}