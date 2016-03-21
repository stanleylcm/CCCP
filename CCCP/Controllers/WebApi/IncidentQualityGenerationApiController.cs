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
    public class IncidentQualityGenerationApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentQualityGenerationModel GetNewIncident()
        {
            return new IncidentQualityGenerationModel();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentQualityGenerationModel GetIncident(int incidentId)
        {
            IncidentQualityGenerationModel result = new IncidentQualityGenerationModel();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                // load incident details
                result.Entity = db.IncidentQualityGeneration.Find(incidentId);

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
        public List<IncidentQualityGeneration> GetIncidentList()
        {
            List<IncidentQualityGeneration> result = new List<IncidentQualityGeneration>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                result = db.IncidentQualityGeneration.ToList();
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CreateIncident(IncidentQualityGeneration incidentQualityGeneration)
        {
            CCCPDbContext db = new CCCPDbContext();
            IncidentQualityGenerationModel incident = new IncidentQualityGenerationModel();
            incidentQualityGeneration.IncidentStatus = Common.IncidentStatus.Pending.ToString();
            incident.Entity = incidentQualityGeneration;
            incident.PrepareSave(PrepareSaveMode.Created);
            incidentQualityGeneration = incident.Entity;

            int incidentTypeId = MasterTableService.GetIncidentTypeId(IncidentTypeSubType.QualityGeneration);
            db.IncidentQualityGeneration.Add(incidentQualityGeneration);
            db.SaveChanges();
            db.usp_Incident_PostCreate(incidentQualityGeneration.IncidentQualityGenerationId, incidentTypeId, incident.Entity.CreatedBy, Common.CheckListActionStatus.Pending.ToString());

            return incident.Entity.IncidentQualityGenerationId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int EditIncident(IncidentQualityGenerationModel incident)
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

            db.IncidentQualityGeneration.Attach(incident.Entity);
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

            return incident.Entity.IncidentQualityGenerationId;
        }
    }
}