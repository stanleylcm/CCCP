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

        public ActionResult CreateByEnquiry([Bind(Include = "IncidentOHSId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,PossibleCauseOthers,OHSType,NatureOfInjury,NatureOfInjuryOthers,NoOfInjury,NoOfInfectiousDisease,NoOfDeath,NoOfInfected,Treatment,TreatmentOthers,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentOHS incidentEntity)
        {
            incident.Entity = incidentEntity;
            return View("Create", incident);
        }

        // POST: IncidentOHSs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentOHSId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,PossibleCauseOthers,OHSType,NatureOfInjury,NatureOfInjuryOthers,NoOfInjury,NoOfInfectiousDisease,NoOfDeath,NoOfInfected,Treatment,TreatmentOthers,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentOHS incidentOHS)
        {
            if (ModelState.IsValid)
            {
                new IncidentOHSApiController().CreateIncident(incidentOHS);
                return RedirectToAction("Index", new { message = "Incident " + incidentOHS.IncidentNo + " had been created successfully!" });
            }
            incident.Entity = incidentOHS;

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
        public ActionResult Edit([Bind(Include = "IncidentOHSId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,Location,PossibleCause,PossibleCauseOthers,OHSType,NatureOfInjury,NatureOfInjuryOthers,NoOfInjury,NoOfInfectiousDisease,NoOfDeath,NoOfInfected,Treatment,TreatmentOthers,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentOHS incidentOHS)
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
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
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

        public ActionResult EscalateToCrisis()
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentOHSModel;
            }
            int id = new IncidentApiController().EscalateToCrisis(incident.Entity.IncidentOHSId, MasterTableService.GetIncidentTypeId(IncidentTypeSubType.OHS));
            return RedirectToAction("Details", new { id = id, message = "Incident had been escalated to crisis successfully! The crisis is now pending for approval." });
        }

        public void Test()
        {
            incident.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}
