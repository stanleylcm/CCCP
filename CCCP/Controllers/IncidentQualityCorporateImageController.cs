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
    public class IncidentQualityCorporateImageController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentQualityCorporateImageModel incident = new IncidentQualityCorporateImageModel();

        // GET: IncidentQualityCorporateImages
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<IncidentQualityCorporateImage> incidents = new IncidentQualityCorporateImageApiController().GetIncidentList();
            List<IncidentQualityCorporateImageModel> incidentModels = incidents.ConvertAll(x => new IncidentQualityCorporateImageModel(x));
            return View(incidentModels);
        }

        // GET: IncidentQualityCorporateImages/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentQualityCorporateImage incidentQualityCorporateImage = db.IncidentQualityCorporateImage.Find(id);
            if (incidentQualityCorporateImage == null)
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

        // GET: IncidentQualityCorporateImages/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        public ActionResult CreateByEnquiry([Bind(Include = "IncidentQualityCorporateImageId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,PossibleCause,PossibleCauseOthers,Impact,ImpactOthers,RemedyAction,StatusUpdate,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentQualityCorporateImage incidentEntity)
        {
            incident.Entity = incidentEntity;
            return View("Create", incident);
        }

        // POST: IncidentQualityCorporateImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentQualityCorporateImageId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,PossibleCause,PossibleCauseOthers,Impact,ImpactOthers,RemedyAction,StatusUpdate,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentQualityCorporateImage incidentQualityCorporateImage)
        {
            if (ModelState.IsValid)
            {
                new IncidentQualityCorporateImageApiController().CreateIncident(incidentQualityCorporateImage);
                return RedirectToAction("Index", new { message = "Incident " + incidentQualityCorporateImage.IncidentNo + " had been created successfully!" });
            }
            incident.Entity = incidentQualityCorporateImage;

            return View(incidentQualityCorporateImage);
        }

        // GET: IncidentQualityCorporateImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentQualityCorporateImage incidentQualityCorporateImage = db.IncidentQualityCorporateImage.Find(id);
            if (incidentQualityCorporateImage == null) return HttpNotFound();
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

        // POST: IncidentQualityCorporateImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentQualityCorporateImageId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,PossibleCause,PossibleCauseOthers,Impact,ImpactOthers,RemedyAction,StatusUpdate,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentQualityCorporateImage incidentQualityCorporateImage)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentQualityCorporateImageModel;
                incident.Entity = incidentQualityCorporateImage;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentQualityCorporateImageApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentQualityCorporateImageId, message = "Incident had been updated successfully!" });
            }
            return View(incident);
        }

        public ActionResult Cancel(int id)
        {
            IncidentQualityCorporateImage incidentQualityCorporateImage = db.IncidentQualityCorporateImage.Find(id);
            incidentQualityCorporateImage.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return RedirectToAction("Index", new { id = incident.Entity.IncidentQualityCorporateImageId, message = "The Incident had been cancelled successfully" });
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
            this.incident = new IncidentQualityCorporateImageApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentQualityCorporateImageModel;
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
                incident = Session["incident"] as IncidentQualityCorporateImageModel;
            }
            incident.Entity.IncidentStatus = IncidentStatus.Closed.ToEnumString();

            return Edit(incident.Entity);
        }

        public ActionResult EscalateToCrisis()
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentQualityCorporateImageModel;
            }
            int id = new IncidentApiController().EscalateToCrisis(incident.Entity.IncidentQualityCorporateImageId, MasterTableService.GetIncidentTypeId(IncidentTypeSubType.QualityCorporateImage));
            return RedirectToAction("Details", new { id = id, message = "Incident had been escalated to crisis successfully! The crisis is now pending for approval." });
        }

        public void Test()
        {
            incident.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}
