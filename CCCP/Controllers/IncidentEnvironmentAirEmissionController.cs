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
    public class IncidentEnvironmentAirEmissionController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentEnvironmentAirEmissionModel incident = new IncidentEnvironmentAirEmissionModel();

        // GET: IncidentEnvironmentAirEmissions
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<IncidentEnvironmentAirEmission> incidents = new IncidentEnvironmentAirEmissionApiController().GetIncidentList();
            List<IncidentEnvironmentAirEmissionModel> incidentModels = incidents.ConvertAll(x => new IncidentEnvironmentAirEmissionModel(x));
            return View(incidentModels);
        }

        // GET: IncidentEnvironmentAirEmissions/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentEnvironmentAirEmission incidentEnvironmentAirEmission = db.IncidentEnvironmentAirEmission.Find(id);
            if (incidentEnvironmentAirEmission == null)
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

        // GET: IncidentEnvironmentAirEmissions/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        // POST: IncidentEnvironmentAirEmissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentEnvironmentAirEmissionId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,SourceOfInformation,AbatementSystemUnavailability,ComplaintOfBlackSmoke,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentEnvironmentAirEmission incidentEnvironmentAirEmission)
        {
            if (ModelState.IsValid)
            {
                new IncidentEnvironmentAirEmissionApiController().CreateIncident(incidentEnvironmentAirEmission);
                return RedirectToAction("Index", new { message = "Incident " + incidentEnvironmentAirEmission.IncidentNo + " had been created successfully!" });
            }

            return View(incidentEnvironmentAirEmission);
        }

        // GET: IncidentEnvironmentAirEmissions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentEnvironmentAirEmission incidentEnvironmentAirEmission = db.IncidentEnvironmentAirEmission.Find(id);
            if (incidentEnvironmentAirEmission == null) return HttpNotFound();
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

        // POST: IncidentEnvironmentAirEmissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentEnvironmentAirEmissionId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,Location,PossibleCause,SourceOfInformation,AbatementSystemUnavailability,ComplaintOfBlackSmoke,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentEnvironmentAirEmission incidentEnvironmentAirEmission)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentEnvironmentAirEmissionModel;
                incident.Entity = incidentEnvironmentAirEmission;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentEnvironmentAirEmissionApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentEnvironmentAirEmissionId, message = "Incident had been updated successfully!" });
            }
            return View(incident);
        }

        // GET: IncidentEnvironmentAirEmissions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentEnvironmentAirEmission incidentEnvironmentAirEmission = db.IncidentEnvironmentAirEmission.Find(id);
            if (incidentEnvironmentAirEmission == null)
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

        // POST: IncidentEnvironmentAirEmissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IncidentEnvironmentAirEmission incidentEnvironmentAirEmission = db.IncidentEnvironmentAirEmission.Find(id);
            db.IncidentEnvironmentAirEmission.Remove(incidentEnvironmentAirEmission);
            if (Session != null)
            {
                incident = Session["incident"] as IncidentEnvironmentAirEmissionModel;
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
            IncidentEnvironmentAirEmission incidentEnvironmentAirEmission = db.IncidentEnvironmentAirEmission.Find(id);
            incidentEnvironmentAirEmission.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return RedirectToAction("Index", new { id = incident.Entity.IncidentEnvironmentAirEmissionId, message = "The Incident had been cancelled successfully" });
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
            this.incident = new IncidentEnvironmentAirEmissionApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentEnvironmentAirEmissionModel;
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
                incident = Session["incident"] as IncidentEnvironmentAirEmissionModel;
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
