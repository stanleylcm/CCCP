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
    public class IncidentOHSController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentOHSModel incident = new IncidentOHSModel();

        // GET: IncidentOHSs
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<IncidentOHS> incidents = new IncidentOHSApiController().GetIncidentList();
            List<IncidentOHSModel> incidentModels = incidents.ConvertAll(x => new IncidentOHSModel(x));
            return View(incidentModels);
        }

        // GET: IncidentOHSs/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentOHS incidentOHS = db.IncidentOHS.Find(id);
            if (incidentOHS == null)
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

        // GET: IncidentOHSs/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        // POST: IncidentOHSs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentOHSId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,OHSType,NatureOfInjury,NoOfInjury,NoOfInfectiousDisease,NoOfDeath,NoOfInfected,Treatment,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentOHS incidentOHS)
        {
            if (ModelState.IsValid)
            {
                new IncidentOHSApiController().CreateIncident(incidentOHS);
                return RedirectToAction("Index", new { message = "Incident " + incidentOHS.IncidentNo + " had been created successfully!" });
            }

            return View(incidentOHS);
        }

        // GET: IncidentOHSs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentOHS incidentOHS = db.IncidentOHS.Find(id);
            if (incidentOHS == null) return HttpNotFound();
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

        // POST: IncidentOHSs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentOHSId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,Location,PossibleCause,OHSType,NatureOfInjury,NoOfInjury,NoOfInfectiousDisease,NoOfDeath,NoOfInfected,Treatment,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentOHS incidentOHS)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentOHSModel;
                incident.Entity = incidentOHS;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentOHSApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentOHSId, message = "The Incident had been updated successfully!" });
            }
            return View(incident);
        }

        // GET: IncidentOHSs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentOHS incidentOHS = db.IncidentOHS.Find(id);
            if (incidentOHS == null)
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

        // POST: IncidentOHSs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IncidentOHS incidentOHS = db.IncidentOHS.Find(id);
            db.IncidentOHS.Remove(incidentOHS);
            if (Session != null)
            {
                incident = Session["incident"] as IncidentOHSModel;
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
            IncidentOHS incidentOHS = db.IncidentOHS.Find(id);
            incidentOHS.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return RedirectToAction("Index", new { id = incident.Entity.IncidentOHSId, message = "The Incident had been cancelled successfully" });
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
            this.incident = new IncidentOHSApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentOHSModel;
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
                incident = Session["incident"] as IncidentOHSModel;
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
