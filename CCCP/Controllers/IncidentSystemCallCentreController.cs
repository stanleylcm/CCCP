using System;
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
    public class IncidentSystemCallCentreController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentSystemCallCentreModel incident = new IncidentSystemCallCentreModel();

        // GET: IncidentSystemCallCentres
        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
            List<IncidentSystemCallCentre> incidents = new IncidentSystemCallCentreApiController().GetIncidentList();
            List<IncidentSystemCallCentreModel> incidentModels = incidents.ConvertAll(x => new IncidentSystemCallCentreModel(x));
            return View(incidentModels);
        }

        // GET: IncidentSystemCallCentres/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemCallCentre incidentSystemCallCentre = db.IncidentSystemCallCentre.Find(id);
            if (incidentSystemCallCentre == null)
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

        // GET: IncidentSystemCallCentres/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        // POST: IncidentSystemCallCentres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentSystemCallCentreId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisId,NotificationId,IssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Impact,PossibleCause,ExpectedRestorationTime,RequireMitigatingAction,MitigatingAction,StatusUpdate,FullRestoration,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemCallCentre incidentSystemCallCentre)
        {
            if (ModelState.IsValid)
            {
                new IncidentSystemCallCentreApiController().CreateIncident(incidentSystemCallCentre);
                return RedirectToAction("Index", new { message = "Incident " + incidentSystemCallCentre.IncidentNo + " had been created successfully!" });
            }

            return View(incidentSystemCallCentre);
        }

        // GET: IncidentSystemCallCentres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentSystemCallCentre incidentSystemCallCentre = db.IncidentSystemCallCentre.Find(id);
            if (incidentSystemCallCentre == null) return HttpNotFound();
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

        // POST: IncidentSystemCallCentres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentSystemCallCentreId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisId,NotificationId,IssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,Impact,PossibleCause,ExpectedRestorationTime,RequireMitigatingAction,MitigatingAction,StatusUpdate,FullRestoration,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemCallCentre incidentSystemCallCentre)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemCallCentreModel;
                incident.Entity = incidentSystemCallCentre;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentSystemCallCentreApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentSystemCallCentreId, message = "Incident had been updated successfully!" });
            }
            return View(incident);
        }

        // GET: IncidentSystemCallCentres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemCallCentre incidentSystemCallCentre = db.IncidentSystemCallCentre.Find(id);
            if (incidentSystemCallCentre == null)
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

        // POST: IncidentSystemCallCentres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IncidentSystemCallCentre incidentSystemCallCentre = db.IncidentSystemCallCentre.Find(id);
            db.IncidentSystemCallCentre.Remove(incidentSystemCallCentre);
            if (Session != null)
            {
                incident = Session["incident"] as IncidentSystemCallCentreModel;
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

        public ActionResult Cancel(int id)
        {
            IncidentSystemCallCentre incidentSystemCallCentre = db.IncidentSystemCallCentre.Find(id);
            incidentSystemCallCentre.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return RedirectToAction("Index", new { id = incident.Entity.IncidentSystemCallCentreId, message = "The Incident had been cancelled successfully" });
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
            this.incident = new IncidentSystemCallCentreApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemCallCentreModel;
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
                incident = Session["incident"] as IncidentSystemCallCentreModel;
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
