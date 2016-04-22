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
    public class IncidentController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();

        public ActionResult LinkIncident(string incidentId, string incidentTypeId, string incidentNo, string linkList)
        {
            string[] linkLists = linkList.Split(new string[] { ";" }, StringSplitOptions.None);

            foreach (string link in linkLists)
            {
                string[] tmp = link.Split(new string[] { ":" }, StringSplitOptions.None);
                int linkIncidentId = Convert.ToInt32(tmp[0]);
                int linkIncidentTypeId = Convert.ToInt32(tmp[1]);
                new IncidentApiController().LinkIncident(Convert.ToInt32(incidentId), Convert.ToInt32(incidentTypeId), linkIncidentId, linkIncidentTypeId);
            }

            return Json(new { SelectedIncidentNo = incidentNo });
        }

        public ActionResult GetIncidentListForLink(string incidentId, string incidentTypeId, string incidentNo)
        {
            List<usp_Incident_GetIncidentForLink_Result> incidentListForLink = db.usp_Incident_GetIncidentForLink(incidentId, incidentTypeId).ToList<usp_Incident_GetIncidentForLink_Result>();

            string returnTable = "<div class=\"pull-left\" style=\"padding-right:20px\">";
            returnTable += "\n   <div class=\"form-group\">";
            returnTable += "\n       <button class=\"btn btn-primary\" type=\"button\" name=\"LinkIncident\" data-incidentid=\"" + incidentId + "\" data-incidenttypeid=\"" + incidentTypeId + "\" data-incidentno=\"" + incidentNo + "\">" + Resources.global.Func_LinkIncident + "</button>";
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

        public ActionResult LinkGeneralEnquiry(string incidentId, string incidentTypeId, string incidentNo, string linkList)
        {
            string[] linkLists = linkList.Split(new string[] { ";" }, StringSplitOptions.None);

            foreach (string geId in linkLists)
            {
                new IncidentApiController().LinkGeneralEnquiry(Convert.ToInt32(incidentId), Convert.ToInt32(incidentTypeId), Convert.ToInt32(geId));
            }

            return Json(new { SelectedIncidentNo = incidentNo });
        }

        public ActionResult GetGeneralEnquiryListForLink(string incidentId, string incidentTypeId, string incidentNo)
        {
            List<GeneralEnquiry> geForLink = (from ge in db.GeneralEnquiry
                                              where !(from linkge in db.GeneralEnquiryIncidentLink
                                                      select linkge.GeneralEnquiryId).Contains(ge.GeneralEnquiryId)
                                              select ge).ToList<GeneralEnquiry>();

            string returnTable = "<div class=\"pull-left\" style=\"padding-right:20px\">";
            returnTable += "\n   <div class=\"form-group\">";
            returnTable += "\n       <button class=\"btn btn-primary\" type=\"button\" name=\"LinkGeneralEnquiry\" data-incidentid=\"" + incidentId + "\" data-incidenttypeid=\"" + incidentTypeId + "\" data-incidentno=\"" + incidentNo + "\">" + Resources.global.Func_LinkGeneralEnquiry + "</button>";
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
    }
}
