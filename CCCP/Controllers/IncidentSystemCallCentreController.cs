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

        public ActionResult LinkIncident(string incidentId, string incidentTypeId, string linkList)
        {
            string[] linkLists = linkList.Split(new string[] { ";" }, StringSplitOptions.None);

            foreach (string link in linkLists)
            {
                string[] tmp = link.Split(new string[] { ":" }, StringSplitOptions.None);
                int linkIncidentId = Convert.ToInt32(tmp[0]);
                int linkIncidentTypeId = Convert.ToInt32(tmp[1]);
                new IncidentApiController().LinkIncident(Convert.ToInt32(incidentId), Convert.ToInt32(incidentTypeId), linkIncidentId, linkIncidentTypeId);
            }

            IncidentSystemCallCentre incidentSystemCallCentre = db.IncidentSystemCallCentre.Find(Convert.ToInt32(incidentId));

            return Json(new { SelectedIncidentNo = incidentSystemCallCentre.IncidentNo });
        }

        public ActionResult GetIncidentListForLink(string incidentId, string incidentTypeId)
        {
            List<usp_Incident_GetIncidentForLink_Result> incidentListForLink = db.usp_Incident_GetIncidentForLink(incidentId, incidentTypeId).ToList<usp_Incident_GetIncidentForLink_Result>();

            string returnTable = "<div class=\"pull-left\" style=\"padding-right:20px\">";
            returnTable += "\n   <div class=\"form-group\">";
            returnTable += "\n       <button class=\"btn btn-primary\" type=\"button\" name=\"LinkIncident\" data-incidentid=\"" + incidentId + "\" data-incidenttypeid=\"" + incidentTypeId + "\">" + Resources.global.Func_LinkIncident + "</button>";
            returnTable += "\n  </div>";
            returnTable += "\n</div>";
            returnTable += "\n<div class=\"pull-right\">Search: <input id=\"LinkIncidentFilterText\" /></div>";
            returnTable += "\n<table id=\"LinkIncidentList\" class=\"table table-striped table-bordered\" cellspacing=\"0\" width=\"100%\">";
            returnTable += "\n  <thead>";
            returnTable += "\n      <tr>";
            returnTable += "\n          <th></th>";
            returnTable += "\n          <th>Incident No</th>";
            returnTable += "\n          <th>Incident Type</th>";
            returnTable += "\n          <th>Level Of Severity</th>";
            returnTable += "\n          <th>Incident Status</th>";
            returnTable += "\n          <th>Incident Background</th>";
            returnTable += "\n          <th>Created By</th>";
            returnTable += "\n          <th>Issue Date Time</th>";
            returnTable += "\n      </tr>";
            returnTable += "\n  </thead>";
            returnTable += "\n  <tbody>";
            foreach (usp_Incident_GetIncidentForLink_Result item in incidentListForLink)
            {
                string sLevelOfSeverity = "";
                if (item.LevelOfSeverity != null)
                {
                    int level = Convert.ToInt32(item.LevelOfSeverity);
                    IncidentLevel iLevel = (IncidentLevel)level;
                    sLevelOfSeverity = iLevel.ToEnumString();
                }

                returnTable += "\n      <tr>";
                returnTable += "\n          <td><input id=\"chkLinkIncident_" + incidentId + "_" + incidentTypeId + "\" type=\"checkbox\" data-linkincidentid=\"" + item.IncidentId + "\" data-linkincidenttypeid=\"" + item.IncidentTypeId + "\" /></td>";
                returnTable += "\n          <td>" + item.IncidentNo + "</td>";
                returnTable += "\n          <td>" + item.IncidentType + "</td>";
                returnTable += "\n          <td>" + sLevelOfSeverity + "</td>";
                returnTable += "\n          <td>" + item.IncidentStatus + "</td>";
                returnTable += "\n          <td>" + (item.IncidentBackground != null ? item.IncidentBackground.Replace(System.Environment.NewLine, "<br/>") : "") + "</td>";
                returnTable += "\n          <td>" + item.CreatedBy + "</td>";
                returnTable += "\n          <td>" + item.IssueDateTime + "</td>";
                returnTable += "\n      </tr>";
            }
            returnTable += "\n  </tbody>";
            returnTable += "\n</table>";
            returnTable += "\n<input type=\"hidden\" id=\"LinkIncidentLinkList\" name=\"LinkIncidentLinkList\" value=\"\" />";

            return Content(returnTable, "text/html");
        }

        public void Test()
        {
            incident.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}
