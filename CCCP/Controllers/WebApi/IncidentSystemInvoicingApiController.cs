﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using CCCP.Business.Model;
using CCCP.ViewModel;
using CCCP.Common;
using System.Data;
using System.Data.Entity;

namespace CCCP.Controllers.WebApi
{
    public class IncidentSystemInvoicingApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public IncidentSystemInvoicingModel GetIncident(int incidentId)
        {
            IncidentSystemInvoicingModel result = new IncidentSystemInvoicingModel();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                // load incident details
                result.Entity = db.IncidentSystemInvoicing.Find(incidentId);

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
        public List<IncidentSystemInvoicing> GetIncidentList()
        {
            List<IncidentSystemInvoicing> result = new List<IncidentSystemInvoicing>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                result = db.IncidentSystemInvoicing.ToList();
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CreateIncident(IncidentSystemInvoicing incidentSystemInvoicing)
        {
            CCCPDbContext db = new CCCPDbContext();
            IncidentSystemInvoicingModel incident = new IncidentSystemInvoicingModel();
            incidentSystemInvoicing.IncidentStatus = Common.IncidentStatus.Pending.ToString();
            incident.Entity = incidentSystemInvoicing;
            incident.PrepareSave("Created");
            incidentSystemInvoicing = incident.Entity;

            db.IncidentSystemInvoicing.Add(incidentSystemInvoicing);
            db.SaveChanges();
            db.usp_Incident_PostCreate(incidentSystemInvoicing.IncidentSystemInvoicingId, 7, incident.Entity.CreatedBy, Common.CheckListActionStatus.Pending.ToString());

            return incident.Entity.IncidentSystemInvoicingId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int EditIncident(IncidentSystemInvoicingModel incident)
        {
            CCCPDbContext db = new CCCPDbContext();

            // prepare history etc. before save
            if (incident.Entity.IncidentStatus == IncidentStatus.Closed.ToEnumString())
            {
                incident.PrepareSave("Closed");
            }
            else
            {
                incident.PrepareSave();
            }

            db.IncidentSystemInvoicing.Attach(incident.Entity);
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

            return incident.Entity.IncidentSystemInvoicingId;
        }
    }
}