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
    public class IncidentSystemITSystemController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentSystemITSystemModel incident = new IncidentSystemITSystemModel();

        // GET: IncidentSystemITSystems
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<IncidentSystemITSystem> incidents = new IncidentSystemITSystemApiController().GetIncidentList();
            List<IncidentSystemITSystemModel> incidentModels = incidents.ConvertAll(x => new IncidentSystemITSystemModel(x));
            return View(incidentModels);
        }

        // GET: IncidentSystemITSystems/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemITSystem incidentSystemITSystem = db.IncidentSystemITSystem.Find(id);
            if (incidentSystemITSystem == null)
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

        // GET: IncidentSystemITSystems/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        // POST: IncidentSystemITSystems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentSystemITSystemId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,Damage,AffectedSystem,AffectedArea,ExpectedRestorationTime,FullRestoration,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemITSystem incidentSystemITSystem)
        {
            if (ModelState.IsValid)
            {
                new IncidentSystemITSystemApiController().CreateIncident(incidentSystemITSystem);
                return RedirectToAction("Index", new { message = "Incident " + incidentSystemITSystem.IncidentNo + " had been created successfully!" });
            }

            return View(incidentSystemITSystem);
        }

        // GET: IncidentSystemITSystems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentSystemITSystem incidentSystemITSystem = db.IncidentSystemITSystem.Find(id);
            if (incidentSystemITSystem == null) return HttpNotFound();
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

        // POST: IncidentSystemITSystems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentSystemITSystemId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,Location,PossibleCause,Damage,AffectedSystem,AffectedArea,ExpectedRestorationTime,FullRestoration,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemITSystem incidentSystemITSystem)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemITSystemModel;
                incident.Entity = incidentSystemITSystem;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentSystemITSystemApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentSystemITSystemId, message = "Incident had been updated successfully!" });
            }
            return View(incident);
        }

        // GET: IncidentSystemITSystems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemITSystem incidentSystemITSystem = db.IncidentSystemITSystem.Find(id);
            if (incidentSystemITSystem == null)
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

        // POST: IncidentSystemITSystems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IncidentSystemITSystem incidentSystemITSystem = db.IncidentSystemITSystem.Find(id);
            db.IncidentSystemITSystem.Remove(incidentSystemITSystem);
            if (Session != null)
            {
                incident = Session["incident"] as IncidentSystemITSystemModel;
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
            IncidentSystemITSystem incidentSystemITSystem = db.IncidentSystemITSystem.Find(id);
            incidentSystemITSystem.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return RedirectToAction("Index", new { id = incident.Entity.IncidentSystemITSystemId, message = "The Incident had been cancelled successfully" });
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
            this.incident = new IncidentSystemITSystemApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemITSystemModel;
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
                incident = Session["incident"] as IncidentSystemITSystemModel;
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
