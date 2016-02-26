using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using CCCP.Business.Model;
using CCCP.ViewModel;

namespace CCCP.Controllers.WebApi
{
    public class IncidentSystemBillingApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentSystemBillingModel GetIncident(int incidentId)
        {
            IncidentSystemBillingModel result = new IncidentSystemBillingModel();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                // load incident details
                result.Entity = db.IncidentSystemBilling.Find(incidentId);

                // load checklists
                int checklistBatchID = result.Entity.ChecklistBatchId;
                result.ChecklistEntities = (from c in db.Checklist
                                            where c.ChecklistBatchId.Equals(checklistBatchID)
                                            orderby c.SortingOrder
                                            select c).ToList<Checklist>();

                // load checklist actions
                foreach (ChecklistModel checklist in result.Checklists)
                {
                    List<ChecklistAction> actionEntities = (from ca in db.ChecklistAction
                                                            where ca.ChecklistId.Equals(checklist.Entity.ChecklistId)
                                                            orderby ca.SortingOrder
                                                            select ca).ToList<ChecklistAction>();
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
        public int CreateIncident([FromBody]IncidentSystemBilling incidentSystemBilling)
        {
            CCCPDbContext db = new CCCPDbContext();
            IncidentSystemBillingModel incident = new IncidentSystemBillingModel();
            incidentSystemBilling.IncidentStatus = Common.IncidentStatus.Pending.ToString();
            incident.Entity = incidentSystemBilling;
            incident.PrepareSave("Created");
            incidentSystemBilling = incident.Entity;

            db.IncidentSystemBilling.Add(incidentSystemBilling);
            db.SaveChanges();
            db.usp_Incident_PostCreate(incidentSystemBilling.IncidentSystemBillingId, 6, incident.Entity.CreatedBy, Common.CheckListActionStatus.Pending.ToString());

            return incident.Entity.IncidentSystemBillingId;
        }
    }
}