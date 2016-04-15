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
    public class OMSEventController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public OMSEventModel omsEventModel = new OMSEventModel();

        // GET: OMSEvents
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<OMSEvent> omsEvents = new OMSEventApiController().GetOMSEventList();
            List<OMSEventModel> omsEventModels = omsEvents.ConvertAll(x => new OMSEventModel(x));
            return View(omsEventModels);
        }

        // GET: OMSEvents/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OMSEvent omsEvent = db.OMSEvent.Find(id);
            if (omsEvent == null)
            {
                return HttpNotFound();
            }
            LoadData(id.Value);
            if (Session != null)
            {
                Session["OMSEvent"] = omsEventModel;
            }

            return View(omsEventModel);
        }

        public void LoadData(int id)
        {
            this.omsEventModel = new OMSEventApiController().GetOMSEvent(id);
        }

        public ActionResult MarkReviewed(int id)
        {
            string action = "Details";

            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
            // call from detail
            if (Session != null && Session["OMSEvent"] != null)
            {
                this.omsEventModel = Session["OMSEvent"] as OMSEventModel;
            }

            // call from index
            if (this.omsEventModel == null || this.omsEventModel.Entity.OMSEventId != id)
            {
                LoadData(id);
                action = "Index";
            }

            this.omsEventModel.MarkReviewed();

            if (Session != null)
            {
                Session["OMSEvent"] = omsEventModel;
            }

            return RedirectToAction(action, new { id = id, message = "The OMS Event had been reviewed successfully" });
        }

        public ActionResult CreateIncident(int id)
        {
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
            if (Session != null && Session["OMSEvent"] != null)
            {
                omsEventModel = Session["OMSEvent"] as OMSEventModel;
            }

            // call from index
            if (this.omsEventModel == null || this.omsEventModel.Entity.OMSEventId != id)
            {
                LoadData(id);
            }
            
            IncidentQualityNetwork incidentQualityNetwork = new IncidentQualityNetwork();

            incidentQualityNetwork.OMSEventId = omsEventModel.Entity.OMSEventId;
            incidentQualityNetwork.AffectedArea = omsEventModel.Entity.AffectedArea; // chi? affectedBuilding? affectedStreet?
            incidentQualityNetwork.OutageStartTime = omsEventModel.Entity.OutageStartTime;
            incidentQualityNetwork.FullRestoration = omsEventModel.Entity.FullRestoration;
            incidentQualityNetwork.NoOfBuilding = omsEventModel.Entity.NoOfBuilding;
            incidentQualityNetwork.NoOfCustomerAffected = omsEventModel.Entity.NoOfCustomerAffected;
            incidentQualityNetwork.NoOfPlatinumCustomer = omsEventModel.Entity.NoOfPlatinumCustomer;
            incidentQualityNetwork.NoOfDiamondCustomer = omsEventModel.Entity.NoOfDiamondCustomer;
            incidentQualityNetwork.NoOfGoldCustomer = omsEventModel.Entity.NoOfGoldCustomer;
            incidentQualityNetwork.NoOfSilverCustomer = omsEventModel.Entity.NoOfSilverCustomer;
            incidentQualityNetwork.PossibleCause = omsEventModel.Entity.PossibleCause; // chi?
            incidentQualityNetwork.ExpectedRestorationTime = omsEventModel.Entity.ExpectedRestorationTime;
            incidentQualityNetwork.RestorationMethod = omsEventModel.Entity.RestorationMethod; // chi?
            incidentQualityNetwork.StatusUpdate = omsEventModel.Entity.StatusUpdate;
            incidentQualityNetwork.RootCause = omsEventModel.Entity.RootCause; // Chi?
            //incidentQualityNetwork.MVOutage = omsEventModel.Entity.MVOutage; // type not correct...
            //incidentQualityNetwork.LVOutage = omsEventModel.Entity.LVOutage; // type not correct...
            //incidentQualityNetwork.IsCriticalPoint = omsEventModel.Entity.CriticalPoint; // type....

            return RedirectToAction("CreateByOMSEvent", "IncidentQualityNetwork", incidentQualityNetwork);
        }
    }
}
