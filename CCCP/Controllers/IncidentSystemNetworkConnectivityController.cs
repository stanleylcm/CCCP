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
    public class IncidentSystemNetworkConnectivityController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentSystemNetworkConnectivityModel incident = new IncidentSystemNetworkConnectivityModel();

        // GET: IncidentSystemNetworkConnectivitys
        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
            List<IncidentSystemNetworkConnectivity> incidents = new IncidentSystemNetworkConnectivityApiController().GetIncidentList();
            List<IncidentSystemNetworkConnectivityModel> incidentModels = incidents.ConvertAll(x => new IncidentSystemNetworkConnectivityModel(x));
            return View(incidentModels);
        }

        // GET: IncidentSystemNetworkConnectivitys/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemNetworkConnectivity incidentSystemNetworkConnectivity = db.IncidentSystemNetworkConnectivity.Find(id);
            if (incidentSystemNetworkConnectivity == null)
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

        // GET: IncidentSystemNetworkConnectivitys/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        // POST: IncidentSystemNetworkConnectivitys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentSystemNetworkConnectivityId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisId,NotificationId,IssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,Damage,AffectedArea,ExpectedRestorationTime,FullRestoration,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemNetworkConnectivity incidentSystemNetworkConnectivity)
        {
            if (ModelState.IsValid)
            {
                new IncidentSystemNetworkConnectivityApiController().CreateIncident(incidentSystemNetworkConnectivity);
                return RedirectToAction("Index", new { message = "Incident " + incidentSystemNetworkConnectivity.IncidentNo + " had been created successfully!" });
            }

            return View(incidentSystemNetworkConnectivity);
        }

        // GET: IncidentSystemNetworkConnectivitys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentSystemNetworkConnectivity incidentSystemNetworkConnectivity = db.IncidentSystemNetworkConnectivity.Find(id);
            if (incidentSystemNetworkConnectivity == null) return HttpNotFound();
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

        // POST: IncidentSystemNetworkConnectivitys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentSystemNetworkConnectivityId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisId,NotificationId,IssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,Location,PossibleCause,Damage,AffectedArea,ExpectedRestorationTime,FullRestoration,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemNetworkConnectivity incidentSystemNetworkConnectivity)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemNetworkConnectivityModel;
                incident.Entity = incidentSystemNetworkConnectivity;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentSystemNetworkConnectivityApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentSystemNetworkConnectivityId, message = "Incident had been updated successfully!" });
            }
            return View(incident);
        }

        // GET: IncidentSystemNetworkConnectivitys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemNetworkConnectivity incidentSystemNetworkConnectivity = db.IncidentSystemNetworkConnectivity.Find(id);
            if (incidentSystemNetworkConnectivity == null)
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

        // POST: IncidentSystemNetworkConnectivitys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IncidentSystemNetworkConnectivity incidentSystemNetworkConnectivity = db.IncidentSystemNetworkConnectivity.Find(id);
            db.IncidentSystemNetworkConnectivity.Remove(incidentSystemNetworkConnectivity);
            if (Session != null)
            {
                incident = Session["incident"] as IncidentSystemNetworkConnectivityModel;
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
            IncidentSystemNetworkConnectivity incidentSystemNetworkConnectivity = db.IncidentSystemNetworkConnectivity.Find(id);
            incidentSystemNetworkConnectivity.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return RedirectToAction("Index", new { id = incident.Entity.IncidentSystemNetworkConnectivityId, message = "The Incident had been cancelled successfully" });
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
            this.incident = new IncidentSystemNetworkConnectivityApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemNetworkConnectivityModel;
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
                incident = Session["incident"] as IncidentSystemNetworkConnectivityModel;
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
