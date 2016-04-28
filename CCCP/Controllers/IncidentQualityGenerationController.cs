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
    public class IncidentQualityGenerationController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentQualityGenerationModel incident = new IncidentQualityGenerationModel();

        // GET: IncidentQualityGenerations
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<IncidentQualityGeneration> incidents = new IncidentQualityGenerationApiController().GetIncidentList();
            List<IncidentQualityGenerationModel> incidentModels = incidents.ConvertAll(x => new IncidentQualityGenerationModel(x));
            return View(incidentModels);
        }

        // GET: IncidentQualityGenerations/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentQualityGeneration incidentQualityGeneration = db.IncidentQualityGeneration.Find(id);
            if (incidentQualityGeneration == null)
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

        // GET: IncidentQualityGenerations/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        public ActionResult CreateByEnquiry([Bind(Include = "IncidentQualityGenerationId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,NameOfPowerGenerator,PreliminaryCauseOfOutage,ExpectedRestorationTime,IsCEMNetworkBeingAffected,LossOfPower,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentQualityGeneration incidentEntity)
        {
            incident.Entity = incidentEntity;
            return View("Create", incident);
        }

        // POST: IncidentQualityGenerations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentQualityGenerationId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,NameOfPowerGenerator,PreliminaryCauseOfOutage,ExpectedRestorationTime,IsCEMNetworkBeingAffected,LossOfPower,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentQualityGeneration incidentQualityGeneration)
        {
            if (ModelState.IsValid)
            {
                new IncidentQualityGenerationApiController().CreateIncident(incidentQualityGeneration);
                return RedirectToAction("Index", new { message = "Incident " + incidentQualityGeneration.IncidentNo + " had been created successfully!" });
            }
            incident.Entity = incidentQualityGeneration;

            return View(incidentQualityGeneration);
        }

        // GET: IncidentQualityGenerations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentQualityGeneration incidentQualityGeneration = db.IncidentQualityGeneration.Find(id);
            if (incidentQualityGeneration == null) return HttpNotFound();
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

        // POST: IncidentQualityGenerations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentQualityGenerationId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,NameOfPowerGenerator,PreliminaryCauseOfOutage,ExpectedRestorationTime,FullRestoration,IsCEMNetworkBeingAffected,LossOfPower,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentQualityGeneration incidentQualityGeneration)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentQualityGenerationModel;
                incident.Entity = incidentQualityGeneration;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentQualityGenerationApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentQualityGenerationId, message = "Incident had been updated successfully!" });
            }
            return View(incident);
        }

        public ActionResult Cancel(int id)
        {
            IncidentQualityGeneration incidentQualityGeneration = db.IncidentQualityGeneration.Find(id);
            incidentQualityGeneration.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return RedirectToAction("Index", new { id = incident.Entity.IncidentQualityGenerationId, message = "The Incident had been cancelled successfully" });
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
            this.incident = new IncidentQualityGenerationApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentQualityGenerationModel;
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
                incident = Session["incident"] as IncidentQualityGenerationModel;
            }
            incident.Entity.IncidentStatus = IncidentStatus.Closed.ToEnumString();

            return Edit(incident.Entity);
        }

        public ActionResult EscalateToCrisis()
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentQualityGenerationModel;
            }
            int id = new IncidentApiController().EscalateToCrisis(incident.Entity.IncidentQualityGenerationId, MasterTableService.GetIncidentTypeId(IncidentTypeSubType.QualityGeneration));
            return RedirectToAction("Details", new { id = id, message = "Incident had been escalated to crisis successfully! The crisis is now pending for approval." });
        }

        public void Test()
        {
            incident.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}
