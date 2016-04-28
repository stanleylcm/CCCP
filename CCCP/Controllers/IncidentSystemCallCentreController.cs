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
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
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

        public ActionResult CreateByEnquiry([Bind(Include = "IncidentSystemCallCentreId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Impact,ImpactWorkstationFailure,ImpactOthers,PossibleCause,ExpectedRestorationTime,RequireMitigatingAction,MitigatingAction,StatusUpdate,StatusUpdateOthers,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemCallCentre incidentEntity)
        {
            incident.Entity = incidentEntity;
            return View("Create", incident);
        }

        // POST: IncidentSystemCallCentres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentSystemCallCentreId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Impact,ImpactWorkstationFailure,ImpactOthers,PossibleCause,ExpectedRestorationTime,RequireMitigatingAction,MitigatingAction,StatusUpdate,StatusUpdateOthers,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemCallCentre incidentSystemCallCentre)
        {
            if (ModelState.IsValid)
            {
                new IncidentSystemCallCentreApiController().CreateIncident(incidentSystemCallCentre);
                return RedirectToAction("Index", new { message = "Incident " + incidentSystemCallCentre.IncidentNo + " had been created successfully!" });
            }
            incident.Entity = incidentSystemCallCentre;

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
        public ActionResult Edit([Bind(Include = "IncidentSystemCallCentreId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,Impact,ImpactWorkstationFailure,ImpactOthers,PossibleCause,ExpectedRestorationTime,RequireMitigatingAction,MitigatingAction,StatusUpdate,StatusUpdateOthers,FullRestoration,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemCallCentre incidentSystemCallCentre)
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
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
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

        public ActionResult EscalateToCrisis()
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemCallCentreModel;
            }
            int id = new IncidentApiController().EscalateToCrisis(incident.Entity.IncidentSystemCallCentreId, MasterTableService.GetIncidentTypeId(IncidentTypeSubType.SystemCallCentre));
            return RedirectToAction("Details", new { id = id, message = "Incident had been escalated to crisis successfully! The crisis is now pending for approval." });
        }

        public void Test()
        {
            incident.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}
