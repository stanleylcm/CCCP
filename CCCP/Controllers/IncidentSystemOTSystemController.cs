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
    public class IncidentSystemOTSystemController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentSystemOTSystemModel incident = new IncidentSystemOTSystemModel();

        // GET: IncidentSystemOTSystems
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<IncidentSystemOTSystem> incidents = new IncidentSystemOTSystemApiController().GetIncidentList();
            List<IncidentSystemOTSystemModel> incidentModels = incidents.ConvertAll(x => new IncidentSystemOTSystemModel(x));
            return View(incidentModels);
        }

        // GET: IncidentSystemOTSystems/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemOTSystem incidentSystemOTSystem = db.IncidentSystemOTSystem.Find(id);
            if (incidentSystemOTSystem == null)
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

        // GET: IncidentSystemOTSystems/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        public ActionResult CreateByEnquiry([Bind(Include = "IncidentSystemOTSystemId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,Damage,AffectedSystem,AffectedArea,NoOfAffectedSubstation,ExpectedRestorationTime,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemOTSystem incidentEntity)
        {
            incident.Entity = incidentEntity;
            return View("Create", incident);
        }

        // POST: IncidentSystemOTSystems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentSystemOTSystemId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,Damage,AffectedSystem,AffectedArea,NoOfAffectedSubstation,ExpectedRestorationTime,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemOTSystem incidentSystemOTSystem)
        {
            if (ModelState.IsValid)
            {
                new IncidentSystemOTSystemApiController().CreateIncident(incidentSystemOTSystem);
                return RedirectToAction("Index", new { message = "Incident " + incidentSystemOTSystem.IncidentNo + " had been created successfully!" });
            }

            return View(incidentSystemOTSystem);
        }

        // GET: IncidentSystemOTSystems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentSystemOTSystem incidentSystemOTSystem = db.IncidentSystemOTSystem.Find(id);
            if (incidentSystemOTSystem == null) return HttpNotFound();
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

        // POST: IncidentSystemOTSystems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentSystemOTSystemId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,Location,PossibleCause,Damage,AffectedSystem,AffectedArea,NoOfAffectedSubstation,ExpectedRestorationTime,FullRestoration,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemOTSystem incidentSystemOTSystem)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemOTSystemModel;
                incident.Entity = incidentSystemOTSystem;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentSystemOTSystemApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentSystemOTSystemId, message = "Incident had been updated successfully!" });
            }
            return View(incident);
        }

        public ActionResult Cancel(int id)
        {
            IncidentSystemOTSystem incidentSystemOTSystem = db.IncidentSystemOTSystem.Find(id);
            incidentSystemOTSystem.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return RedirectToAction("Index", new { id = incident.Entity.IncidentSystemOTSystemId, message = "The Incident had been cancelled successfully" });
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
            this.incident = new IncidentSystemOTSystemApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemOTSystemModel;
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
                incident = Session["incident"] as IncidentSystemOTSystemModel;
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

            IncidentSystemOTSystem incidentSystemOTSystem = db.IncidentSystemOTSystem.Find(Convert.ToInt32(incidentId));

            return Json(new { SelectedIncidentNo = incidentSystemOTSystem.IncidentNo });
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
