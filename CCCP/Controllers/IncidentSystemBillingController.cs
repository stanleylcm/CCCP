﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CCCP.ViewModel;
using CCCP.Business.Model;
using CCCP.Business.Service;
using CCCP.Common;
using CCCP.Controllers.WebApi;

namespace CCCP.Controllers
{
    public class IncidentSystemBillingController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentSystemBillingModel incident = new IncidentSystemBillingModel();

        // GET: IncidentSystemBillings
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<IncidentSystemBilling> incidents = new IncidentSystemBillingApiController().GetIncidentList();
            List<IncidentSystemBillingModel> incidentModels = incidents.ConvertAll(x => new IncidentSystemBillingModel(x));
            return View(incidentModels);
        }

        // GET: IncidentSystemBillings/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemBilling incidentSystemBilling = db.IncidentSystemBilling.Find(id);
            if (incidentSystemBilling == null)
            {
                return HttpNotFound();
            }
            LoadData(id.Value);
            if (Session != null)
            {
                Session["incident"] = incident;
            }

            return View(incident);
        }

        // GET: IncidentSystemBillings/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        public ActionResult CreateByEnquiry([Bind(Include = "IncidentSystemBillingId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,ProblemArea,PossibleCause,BillingErrorSeriousness,ExpectedAffectedCustomerBill,ContactedBy,Impact,StatusUpdate,RequireMitigatingAction,MitigatingAction,MitigatingActionOthers,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemBilling incidentEntity)
        {
            incident.Entity = incidentEntity;
            return View("Create", incident);
        }

        // POST: IncidentSystemBillings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentSystemBillingId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,ProblemArea,PossibleCause,BillingErrorSeriousness,ExpectedAffectedCustomerBill,ContactedBy,Impact,StatusUpdate,RequireMitigatingAction,MitigatingAction,MitigatingActionOthers,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemBilling incidentSystemBilling)
        {
            if (ModelState.IsValid)
            {
                new IncidentSystemBillingApiController().CreateIncident(incidentSystemBilling);
                return RedirectToAction("Index", new { message = "Incident " + incidentSystemBilling.IncidentNo + " had been created successfully!" });
            }
            incident.Entity = incidentSystemBilling;

            return View(incidentSystemBilling);
        }

        // GET: IncidentSystemBillings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentSystemBilling incidentSystemBilling = db.IncidentSystemBilling.Find(id);
            if (incidentSystemBilling == null) return HttpNotFound();
            else
            {
                LoadData(id.Value);
                if (Session != null)
                {
                    Session["incident"] = incident;
                }

                return View(incident);
            }
        }

        // POST: IncidentSystemBillings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentSystemBillingId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,ProblemArea,PossibleCause,BillingErrorSeriousness,ExpectedAffectedCustomerBill,ContactedBy,Impact,StatusUpdate,RequireMitigatingAction,MitigatingAction,MitigatingActionOthers,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemBilling incidentSystemBilling)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemBillingModel;
                incident.Entity = incidentSystemBilling;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentSystemBillingApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentSystemBillingId, message = "The Incident had been updated successfully!" });
            }
            return View(incident);
        }

        public ActionResult Cancel(int id)
        {
            IncidentSystemBilling incidentSystemBilling = db.IncidentSystemBilling.Find(id);
            incidentSystemBilling.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return RedirectToAction("Index", new { id = incident.Entity.IncidentSystemBillingId, message = "The Incident had been cancelled successfully" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void LoadData(int incidentId)
        {
            this.incident = new IncidentSystemBillingApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemBillingModel;
            }

            incident.Checklists[checklist].ChecklistActions[checklistAction].ToggleActionStatus();

            if (Session != null)
            {
                Session["incident"] = incident;
            }

            return Json(new { result = true });
        }

        public ActionResult CloseIncident()
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemBillingModel;
            }
            incident.Entity.IncidentStatus = IncidentStatus.Closed.ToEnumString();

            return Edit(incident.Entity);
        }

        public ActionResult EscalateToCrisis()
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemBillingModel;
            }
            int id = new IncidentApiController().EscalateToCrisis(incident.Entity.IncidentSystemBillingId, MasterTableService.GetIncidentTypeId(IncidentTypeSubType.SystemBilling));
            return RedirectToAction("Details", new { id = id, message = "Incident had been escalated to crisis successfully! The crisis is now pending for approval." });
        }

        public void Test()
        {
            incident.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}
