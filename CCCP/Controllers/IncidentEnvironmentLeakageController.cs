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
    public class IncidentEnvironmentLeakageController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentEnvironmentLeakageModel incident = new IncidentEnvironmentLeakageModel();

        // GET: IncidentEnvironmentLeakages
        public ActionResult Index()
        {
            return View(new IncidentEnvironmentLeakageApiController().GetIncidentList());
        }

        // GET: IncidentEnvironmentLeakages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentEnvironmentLeakage incidentEnvironmentLeakage = db.IncidentEnvironmentLeakage.Find(id);
            if (incidentEnvironmentLeakage == null)
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

        // GET: IncidentEnvironmentLeakages/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        // POST: IncidentEnvironmentLeakages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentEnvironmentLeakageId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisId,NotificationId,IssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,Damage,SourceOfInformation,TypeOfLeakage,AffectedArea,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentEnvironmentLeakage incidentEnvironmentLeakage)
        {
            if (ModelState.IsValid)
            {
                new IncidentEnvironmentLeakageApiController().CreateIncident(incidentEnvironmentLeakage);
                return RedirectToAction("Index");
            }

            return View(incidentEnvironmentLeakage);
        }

        // GET: IncidentEnvironmentLeakages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentEnvironmentLeakage incidentEnvironmentLeakage = db.IncidentEnvironmentLeakage.Find(id);
            if (incidentEnvironmentLeakage == null) return HttpNotFound();
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

        // POST: IncidentEnvironmentLeakages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentEnvironmentLeakageId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisId,NotificationId,IssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,Location,PossibleCause,Damage,SourceOfInformation,TypeOfLeakage,AffectedArea,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentEnvironmentLeakage incidentEnvironmentLeakage)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentEnvironmentLeakageModel;
                incident.Entity = incidentEnvironmentLeakage;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentEnvironmentLeakageApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentEnvironmentLeakageId });
            }
            return View(incident);
        }

        // GET: IncidentEnvironmentLeakages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentEnvironmentLeakage incidentEnvironmentLeakage = db.IncidentEnvironmentLeakage.Find(id);
            if (incidentEnvironmentLeakage == null)
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

        // POST: IncidentEnvironmentLeakages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IncidentEnvironmentLeakage incidentEnvironmentLeakage = db.IncidentEnvironmentLeakage.Find(id);
            db.IncidentEnvironmentLeakage.Remove(incidentEnvironmentLeakage);
            if (Session != null)
            {
                incident = Session["incident"] as IncidentEnvironmentLeakageModel;
                foreach (ChecklistModel cl in incident.Checklists)
                {
                    foreach (ChecklistActionModel clAction in cl.ChecklistActions)
                    {
                        db.ChecklistAction.Remove(clAction.Entity);
                    }
                    db.Checklist.Remove(cl.Entity);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
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
            this.incident = new IncidentEnvironmentLeakageApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentEnvironmentLeakageModel;
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
                incident = Session["incident"] as IncidentEnvironmentLeakageModel;
            }
            incident.Entity.IncidentStatus = IncidentStatus.Closed.ToEnumString();

            return Edit(incident.Entity);
        }

        public void Test()
        {
            incident.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}