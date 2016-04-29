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
    public class IncidentQualityNetworkController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentQualityNetworkModel incident = new IncidentQualityNetworkModel();

        // GET: IncidentQualityNetworks
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<IncidentQualityNetwork> incidents = new IncidentQualityNetworkApiController().GetIncidentList();
            List<IncidentQualityNetworkModel> incidentModels = incidents.ConvertAll(x => new IncidentQualityNetworkModel(x));
            return View(incidentModels);
        }

        // GET: IncidentQualityNetworks/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentQualityNetwork incidentQualityNetwork = db.IncidentQualityNetwork.Find(id);
            if (incidentQualityNetwork == null)
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

        // GET: IncidentQualityNetworks/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        public ActionResult CreateByEnquiry([Bind(Include = "IncidentQualityNetworkId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,OMSEventId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,AffectedArea,OutageStartTime,FullRestoration,NoOfBuilding,NoOfCustomerAffected,NoOfPlatinumCustomer,NoOfDiamondCustomer,NoOfGoldCustomer,NoOfSilverCustomer,PossibleCause,ExpectedRestorationTime,RestorationMethod,StatusUpdate,RootCause,LossGeneration,LossInterconnection,LossTransmission,OutageLevel,IsDoubleFault,IsPQEventAffectLargeCustomer,IsCriticalPoint,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentQualityNetwork incidentEntity)
        {
            incident.Entity = incidentEntity;
            return View("Create", incident);
        }

        public ActionResult CreateByOMSEvent([Bind(Include = "IncidentQualityNetworkId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,OMSEventId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,AffectedArea,OutageStartTime,FullRestoration,NoOfBuilding,NoOfCustomerAffected,NoOfPlatinumCustomer,NoOfDiamondCustomer,NoOfGoldCustomer,NoOfSilverCustomer,PossibleCause,ExpectedRestorationTime,RestorationMethod,StatusUpdate,RootCause,LossGeneration,LossInterconnection,LossTransmission,OutageLevel,IsDoubleFault,IsPQEventAffectLargeCustomer,IsCriticalPoint,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentQualityNetwork incidentEntity)
        {
            incident.Entity = incidentEntity;
            return View("Create", incident);
        }

        // POST: IncidentQualityNetworks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentQualityNetworkId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,OMSEventId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,AffectedArea,OutageStartTime,FullRestoration,NoOfBuilding,NoOfCustomerAffected,NoOfPlatinumCustomer,NoOfDiamondCustomer,NoOfGoldCustomer,NoOfSilverCustomer,PossibleCause,ExpectedRestorationTime,RestorationMethod,StatusUpdate,RootCause,LossGeneration,LossInterconnection,LossTransmission,OutageLevel,IsDoubleFault,IsPQEventAffectLargeCustomer,IsCriticalPoint,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentQualityNetwork incidentQualityNetwork)
        {
            if (ModelState.IsValid)
            {
                new IncidentQualityNetworkApiController().CreateIncident(incidentQualityNetwork);
                return RedirectToAction("Index", new { message = "Incident " + incidentQualityNetwork.IncidentNo + " had been created successfully!" });
            }
            incident.Entity = incidentQualityNetwork;

            return View(incidentQualityNetwork);
        }

        // GET: IncidentQualityNetworks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentQualityNetwork incidentQualityNetwork = db.IncidentQualityNetwork.Find(id);
            if (incidentQualityNetwork == null) return HttpNotFound();
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

        // POST: IncidentQualityNetworks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentQualityNetworkId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,OMSEventId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,AffectedArea,OutageStartTime,FullRestoration,NoOfBuilding,NoOfCustomerAffected,NoOfPlatinumCustomer,NoOfDiamondCustomer,NoOfGoldCustomer,NoOfSilverCustomer,PossibleCause,ExpectedRestorationTime,RestorationMethod,StatusUpdate,RootCause,LossGeneration,LossInterconnection,LossTransmission,OutageLevel,IsDoubleFault,IsPQEventAffectLargeCustomer,IsCriticalPoint,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentQualityNetwork incidentQualityNetwork)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentQualityNetworkModel;
                incident.Entity = incidentQualityNetwork;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentQualityNetworkApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentQualityNetworkId, message = "Incident had been updated successfully!" });
            }
            return View(incident);
        }

        public ActionResult Cancel(int id)
        {
            IncidentQualityNetwork incidentQualityNetwork = db.IncidentQualityNetwork.Find(id);
            incidentQualityNetwork.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return RedirectToAction("Index", new { id = incident.Entity.IncidentQualityNetworkId, message = "The Incident had been cancelled successfully" });
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
            this.incident = new IncidentQualityNetworkApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentQualityNetworkModel;
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
                incident = Session["incident"] as IncidentQualityNetworkModel;
            }
            incident.Entity.IncidentStatus = IncidentStatus.Closed.ToEnumString();

            return Edit(incident.Entity);
        }

        public ActionResult EscalateToCrisis()
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentQualityNetworkModel;
            }
            int id = new IncidentApiController().EscalateToCrisis(incident.Entity.IncidentQualityNetworkId, MasterTableService.GetIncidentTypeId(IncidentTypeSubType.QualityNetwork));
            return RedirectToAction("Details", new { id = id, message = "Incident had been escalated to crisis successfully! The crisis is now pending for approval." });
        }

        public void Test()
        {
            incident.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}
