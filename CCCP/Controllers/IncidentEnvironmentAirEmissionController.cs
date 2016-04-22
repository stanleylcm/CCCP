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

        public ActionResult CreateByEnquiry([Bind(Include = "IncidentEnvironmentAirEmissionId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,SourceOfInformation,AbatementSystemUnavailable,ComplaintOfBlackSmoke,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentEnvironmentAirEmission incidentEntity)
        {
            incident.Entity = incidentEntity;
            return View("Create", incident);
        }

        // POST: IncidentEnvironmentAirEmissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentEnvironmentAirEmissionId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,Location,PossibleCause,SourceOfInformation,AbatementSystemUnavailable,ComplaintOfBlackSmoke,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentEnvironmentAirEmission incidentEnvironmentAirEmission)
        {
            if (ModelState.IsValid)
            {
                new IncidentEnvironmentAirEmissionApiController().CreateIncident(incidentEnvironmentAirEmission);
                return RedirectToAction("Index", new { message = "Incident " + incidentEnvironmentAirEmission.IncidentNo + " had been created successfully!" });
            }
            incident.Entity = incidentEnvironmentAirEmission;

            return View(incident);
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
        public ActionResult Edit([Bind(Include = "IncidentEnvironmentAirEmissionId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,Location,PossibleCause,SourceOfInformation,AbatementSystemUnavailable,ComplaintOfBlackSmoke,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentEnvironmentAirEmission incidentEnvironmentAirEmission)
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
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
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

            IncidentEnvironmentAirEmission incidentEnvironmentAirEmission = db.IncidentEnvironmentAirEmission.Find(Convert.ToInt32(incidentId));

            return Json(new { SelectedIncidentNo = incidentEnvironmentAirEmission.IncidentNo });
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
            returnTable += "\n          <th>Level of Severity</th>";
            returnTable += "\n          <th>Incident Status</th>";
            returnTable += "\n          <th>Incident Background</th>";
            returnTable += "\n          <th>Created By</th>";
            returnTable += "\n          <th>Issue Date Time</th>";
            returnTable += "\n      </tr>";
            returnTable += "\n  </thead>";
            returnTable += "\n  <tbody>";
            foreach(usp_Incident_GetIncidentForLink_Result item in incidentListForLink)
            { 
                string sLevelOfSeverity = "";
                if (item.LevelOfSeverity != null)
                {
                    int level = Convert.ToInt32(item.LevelOfSeverity);
                    IncidentLevel iLevel = (IncidentLevel)level;
                    sLevelOfSeverity = iLevel.ToEnumString();
                }
                
                returnTable += "\n      <tr>";
                returnTable += "\n          <td><input id=\"chkLinkIncident_" + incidentId + "_" + incidentTypeId +"\" type=\"checkbox\" data-linkincidentid=\"" + item.IncidentId + "\" data-linkincidenttypeid=\"" + item.IncidentTypeId + "\" /></td>";
                returnTable += "\n          <td>" + item.IncidentNo + "</td>";
                returnTable += "\n          <td>" + item.IncidentType + "</td>";
                returnTable += "\n          <td>" + sLevelOfSeverity + "</td>";
                returnTable += "\n          <td>" + item.IncidentStatus + "</td>";
                returnTable += "\n          <td>" + (item.IncidentBackground != null ? item.IncidentBackground.Replace(System.Environment.NewLine, "<br/>") : "") +"</td>";
                returnTable += "\n          <td>" + item.CreatedBy + "</td>";
                returnTable += "\n          <td>" + item.IssueDateTime + "</td>";
                returnTable += "\n      </tr>";
            }
            returnTable += "\n  </tbody>";
            returnTable += "\n</table>";
            returnTable += "\n<input type=\"hidden\" id=\"LinkIncidentLinkList\" name=\"LinkIncidentLinkList\" value=\"\" />";

            return Content(returnTable, "text/html");
        }

        public ActionResult LinkGeneralEnquiry(string incidentId, string incidentTypeId, string linkList)
        {
            string[] linkLists = linkList.Split(new string[] { ";" }, StringSplitOptions.None);

            foreach (string geId in linkLists)
            {
                new IncidentApiController().LinkGeneralEnquiry(Convert.ToInt32(incidentId), Convert.ToInt32(incidentTypeId), Convert.ToInt32(geId));
            }

            IncidentEnvironmentAirEmission incidentEnvironmentAirEmission = db.IncidentEnvironmentAirEmission.Find(Convert.ToInt32(incidentId));

            return Json(new { SelectedIncidentNo = incidentEnvironmentAirEmission.IncidentNo });
        }

        public ActionResult GetGeneralEnquiryListForLink(string incidentId, string incidentTypeId)
        {
            List<GeneralEnquiry> geForLink = (from ge in db.GeneralEnquiry
                                              where !(from linkge in db.GeneralEnquiryIncidentLink
                                                      select linkge.GeneralEnquiryId).Contains(ge.GeneralEnquiryId)
                                              select ge).ToList<GeneralEnquiry>();

            string returnTable = "<div class=\"pull-left\" style=\"padding-right:20px\">";
            returnTable += "\n   <div class=\"form-group\">";
            returnTable += "\n       <button class=\"btn btn-primary\" type=\"button\" name=\"LinkGeneralEnquiry\" data-incidentid=\"" + incidentId + "\" data-incidenttypeid=\"" + incidentTypeId + "\">" + Resources.global.Func_LinkGeneralEnquiry + "</button>";
            returnTable += "\n  </div>";
            returnTable += "\n</div>";
            returnTable += "\n<div class=\"pull-right\">Search: <input id=\"LinkGeneralEnquiryFilterText\" /></div>";
            returnTable += "\n<table id=\"LinkGeneralEnquiryList\" class=\"table table-striped table-bordered\" cellspacing=\"0\" width=\"100%\">";
            returnTable += "\n  <thead>";
            returnTable += "\n      <tr>";
            returnTable += "\n          <th></th>";
            returnTable += "\n          <th>Enquiry Type</th>";
            returnTable += "\n          <th>Enquiry Status</th>";
            returnTable += "\n          <th>Background</th>";
            returnTable += "\n          <th>Created By</th>";
            returnTable += "\n          <th>Issue Date Time</th>";
            returnTable += "\n      </tr>";
            returnTable += "\n  </thead>";
            returnTable += "\n  <tbody>";
            foreach (GeneralEnquiry item in geForLink)
            {
                returnTable += "\n      <tr>";
                returnTable += "\n          <td><input id=\"chkLinkGeneralEnquiry_" + incidentId + "_" + incidentTypeId + "\" type=\"checkbox\" data-linkgeneralid=\"" + item.GeneralEnquiryId + "\" /></td>";
                returnTable += "\n          <td>" + MasterTableService.GetIncidentTypeName(MasterTableService.GetIncidentTypeSubType(item.GeneralEnquiryTypeId.Value)) + "</td>";
                returnTable += "\n          <td>" + item.Status + "</td>";
                returnTable += "\n          <td>" + (item.Background != null ? item.Background.Replace(System.Environment.NewLine, "<br/>") : "") + "</td>";
                returnTable += "\n          <td>" + item.CreatedBy + "</td>";
                returnTable += "\n          <td>" + item.IssueDateTime + "</td>";
                returnTable += "\n      </tr>";
            }
            returnTable += "\n  </tbody>";
            returnTable += "\n</table>";
            returnTable += "\n<input type=\"hidden\" id=\"LinkGeneralEnquiryLinkList\" name=\"LinkGeneralEnquiryLinkList\" value=\"\" />";

            return Content(returnTable, "text/html");
        }

        public void Test()
        {
            incident.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}
