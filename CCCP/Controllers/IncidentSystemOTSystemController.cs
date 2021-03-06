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
    public class IncidentSystemOTSystemController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentSystemOTSystemModel incident = new IncidentSystemOTSystemModel();

        // GET: IncidentSystemOTSystems
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<IncidentSystemOTSystem> incidents = new IncidentSystemOTSystemApiController().GetIncidentList();
            List<IncidentSystemOTSystemModel> incidentModels = incidents.ConvertAll(x => new IncidentSystemOTSystemModel(x));
            return View(incidentModels);
        }

        // GET: IncidentSystemOTSystems/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemOTSystem incidentSystemOTSystem = db.IncidentSystemOTSystem.Find(id);
            if (incidentSystemOTSystem == null)
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

        // GET: IncidentSystemOTSystems/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        public ActionResult CreateByEnquiry([Bind(Include = "IncidentSystemOTSystemId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,Damage,AffectedSystem,AffectedArea,NoOfAffectedSubstation,ExpectedRestorationTime,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemOTSystem incidentEntity)
        {
            incident.Entity = incidentEntity;
            return View("Create", incident);
        }

        // POST: IncidentSystemOTSystems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentSystemOTSystemId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,Damage,AffectedSystem,AffectedArea,NoOfAffectedSubstation,ExpectedRestorationTime,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemOTSystem incidentSystemOTSystem)
        {
            if (ModelState.IsValid)
            {
                new IncidentSystemOTSystemApiController().CreateIncident(incidentSystemOTSystem);
                return RedirectToAction("Index", new { message = "Incident " + incidentSystemOTSystem.IncidentNo + " had been created successfully!" });
            }
            incident.Entity = incidentSystemOTSystem;

            return View(incidentSystemOTSystem);
        }

        // GET: IncidentSystemOTSystems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentSystemOTSystem incidentSystemOTSystem = db.IncidentSystemOTSystem.Find(id);
            if (incidentSystemOTSystem == null) return HttpNotFound();
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

        // POST: IncidentSystemOTSystems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentSystemOTSystemId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,Location,PossibleCause,Damage,AffectedSystem,AffectedArea,NoOfAffectedSubstation,ExpectedRestorationTime,FullRestoration,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemOTSystem incidentSystemOTSystem)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemOTSystemModel;
                incident.Entity = incidentSystemOTSystem;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentSystemOTSystemApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentSystemOTSystemId, message = "Incident had been updated successfully!" });
            }
            return View(incident);
        }

        public ActionResult Cancel(int id)
        {
            IncidentSystemOTSystem incidentSystemOTSystem = db.IncidentSystemOTSystem.Find(id);
            incidentSystemOTSystem.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return RedirectToAction("Index", new { id = incident.Entity.IncidentSystemOTSystemId, message = "The Incident had been cancelled successfully" });
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
            this.incident = new IncidentSystemOTSystemApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemOTSystemModel;
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
                incident = Session["incident"] as IncidentSystemOTSystemModel;
            }
            incident.Entity.IncidentStatus = IncidentStatus.Closed.ToEnumString();

            return Edit(incident.Entity);
        }

        public ActionResult EscalateToCrisis()
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemOTSystemModel;
            }
            int id = new IncidentApiController().EscalateToCrisis(incident.Entity.IncidentSystemOTSystemId, MasterTableService.GetIncidentTypeId(IncidentTypeSubType.SystemOTSystem));
            return RedirectToAction("Details", new { id = id, message = "Incident had been escalated to crisis successfully! The crisis is now pending for approval." });
        }

        public void Test()
        {
            incident.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}
