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
using System.IO;

namespace CCCP.Controllers
{
    public class CrisisApprovalController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public CrisisModel crisis = new CrisisModel();

        // GET: CrisisApprovals
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<Crisis> crisiss = new CrisisApiController().GetCrisisApprovalList();
            List<CrisisModel> crisisModels = crisiss.ConvertAll(x => new CrisisModel(x));
            return View(crisisModels);
        }

        // GET: CrisisApprovals/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crisis crisisInstant = db.Crisis.Find(id);
            if (crisisInstant == null)
            {
                return HttpNotFound();
            }
            LoadData(id.Value);
            if (Session != null)
            {
                Session["crisis"] = crisis;
            }

            int incidentId = crisis.IncidentId;

            if (incidentId > 0)
            {
                switch (crisis.IncidentType)
                {
                    case IncidentTypeSubType.EnvironmentAirEmission:
                        IncidentEnvironmentAirEmissionModel incidentEnvironmentAirEmissionModel = new IncidentEnvironmentAirEmissionApiController().GetIncident(incidentId);
                        ViewBag.PartialView = ConvertPartialViewToString(PartialView("_Details" + crisis.IncidentType.ToEnumString(), incidentEnvironmentAirEmissionModel));
                        break;
                    case IncidentTypeSubType.EnvironmentLeakage:
                        IncidentEnvironmentLeakageModel incidentEnvironmentLeakageModel = new IncidentEnvironmentLeakageApiController().GetIncident(incidentId);
                        ViewBag.PartialView = ConvertPartialViewToString(PartialView("_Details" + crisis.IncidentType.ToEnumString(), incidentEnvironmentLeakageModel));
                        break;
                    case IncidentTypeSubType.OHS:
                        IncidentOHSModel incidentOHSModel = new IncidentOHSApiController().GetIncident(incidentId);
                        ViewBag.PartialView = ConvertPartialViewToString(PartialView("_Details" + crisis.IncidentType.ToEnumString(), incidentOHSModel));
                        break;
                    case IncidentTypeSubType.QualityCorporateImage:
                        IncidentQualityCorporateImageModel incidentQualityCorporateImageModel = new IncidentQualityCorporateImageApiController().GetIncident(incidentId);
                        ViewBag.PartialView = ConvertPartialViewToString(PartialView("_Details" + crisis.IncidentType.ToEnumString(), incidentQualityCorporateImageModel));
                        break;
                    case IncidentTypeSubType.QualityGeneration:
                        IncidentQualityGenerationModel incidentQualityGenerationModel = new IncidentQualityGenerationApiController().GetIncident(incidentId);
                        ViewBag.PartialView = ConvertPartialViewToString(PartialView("_Details" + crisis.IncidentType.ToEnumString(), incidentQualityGenerationModel));
                        break;
                    case IncidentTypeSubType.QualityNetwork:
                        IncidentQualityNetworkModel incidentQualityNetworkModel = new IncidentQualityNetworkApiController().GetIncident(incidentId);
                        ViewBag.PartialView = ConvertPartialViewToString(PartialView("_Details" + crisis.IncidentType.ToEnumString(), incidentQualityNetworkModel));
                        break;
                    case IncidentTypeSubType.SystemBilling:
                        IncidentSystemBillingModel incidentSystemBillingModel = new IncidentSystemBillingApiController().GetIncident(incidentId);
                        ViewBag.PartialView = ConvertPartialViewToString(PartialView("_Details" + crisis.IncidentType.ToEnumString(), incidentSystemBillingModel));
                        break;
                    case IncidentTypeSubType.SystemCallCentre:
                        IncidentSystemCallCentreModel incidentSystemCallCentreModel = new IncidentSystemCallCentreApiController().GetIncident(incidentId);
                        ViewBag.PartialView = ConvertPartialViewToString(PartialView("_Details" + crisis.IncidentType.ToEnumString(), incidentSystemCallCentreModel));
                        break;
                    case IncidentTypeSubType.SystemInvoicing:
                        IncidentSystemInvoicingModel incidentSystemInvoicingModel = new IncidentSystemInvoicingApiController().GetIncident(incidentId);
                        ViewBag.PartialView = ConvertPartialViewToString(PartialView("_Details" + crisis.IncidentType.ToEnumString(), incidentSystemInvoicingModel));
                        break;
                    case IncidentTypeSubType.SystemITSystem:
                        IncidentSystemITSystemModel incidentSystemITSystemModel = new IncidentSystemITSystemApiController().GetIncident(incidentId);
                        ViewBag.PartialView = ConvertPartialViewToString(PartialView("_Details" + crisis.IncidentType.ToEnumString(), incidentSystemITSystemModel));
                        break;
                    case IncidentTypeSubType.SystemNetworkConnectivity:
                        IncidentSystemNetworkConnectivityModel incidentSystemNetworkConnectivityModel = new IncidentSystemNetworkConnectivityApiController().GetIncident(incidentId);
                        ViewBag.PartialView = ConvertPartialViewToString(PartialView("_Details" + crisis.IncidentType.ToEnumString(), incidentSystemNetworkConnectivityModel));
                        break;
                    case IncidentTypeSubType.SystemOTSystem:
                        IncidentSystemOTSystemModel incidentSystemOTSystemModel = new IncidentSystemOTSystemApiController().GetIncident(incidentId);
                        ViewBag.PartialView = ConvertPartialViewToString(PartialView("_Details" + crisis.IncidentType.ToEnumString(), incidentSystemOTSystemModel));
                        break;
                }
            }

            return View(crisis);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void LoadData(int crisisId)
        {
            this.crisis = new CrisisApiController().GetCrisis(crisisId);
        }

        public ActionResult Approve(int id)
        {
            new CrisisApiController().Approve(id);

            if (Session != null && Session["crisis"] != null)
            {
                crisis = Session["crisis"] as CrisisModel;
            }

            return RedirectToAction("Details", "CrisisManagement", new { id = crisis.Entity.CrisisId, message = "Crisis " + crisis.Entity.CrisisNo + " had been approved!" });
        }

        public ActionResult Reject(int id, string rejectReason)
        {
            new CrisisApiController().Reject(id, rejectReason);

            if (Session != null && Session["crisis"] != null)
            {
                crisis = Session["crisis"] as CrisisModel;
            }

            return RedirectToAction("Details", new { id = crisis.Entity.CrisisId, message = "Crisis " + crisis.Entity.CrisisNo + " had been rejected!" });
        }

        public void Test()
        {
            crisis.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }

        protected string ConvertPartialViewToString(PartialViewResult partialView)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines
                  .FindPartialView(ControllerContext, partialView.ViewName).View;

                var vc = new ViewContext(
                  ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }
    }
}
