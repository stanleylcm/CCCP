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
    public class GeneralEnquiryController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public GeneralEnquiryModel enquiry = new GeneralEnquiryModel();

        // GET: GeneralEnquirys
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<GeneralEnquiry> enquiries = new GeneralEnquiryApiController().GetGeneralEnquiryList();
            List<GeneralEnquiryModel> enquiryModels = enquiries.ConvertAll(x => new GeneralEnquiryModel(x));
            return View(enquiryModels);
        }

        // GET: GeneralEnquirys/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralEnquiry generalEnquiry = db.GeneralEnquiry.Find(id);
            if (generalEnquiry == null)
            {
                return HttpNotFound();
            }
            LoadData(id.Value);
            if (Session != null)
            {
                Session["enquiry"] = enquiry;
            }

            return View(enquiry);
        }

        // GET: GeneralEnquirys/Create
        public ActionResult Create()
        {
            return View(enquiry);
        }

        // POST: GeneralEnquirys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GeneralEnquiryId,ChatRoomId,IncidentTypeId,IncidentNo,Background,Status,IssueById,IssueDateTime,CloseById,CloseDateTime,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] GeneralEnquiry generalEnquiry)
        {
            if (ModelState.IsValid)
            {
                new GeneralEnquiryApiController().CreateGeneralEnquiry(generalEnquiry);
                return RedirectToAction("Index", new { message = "General enquiry " + generalEnquiry.GeneralEnquiryId + " had been created successfully!" });
            }

            return View(generalEnquiry);
        }

        // GET: GeneralEnquirys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            GeneralEnquiry generalEnquiry = db.GeneralEnquiry.Find(id);
            if (generalEnquiry == null) return HttpNotFound();
            else
            {
                LoadData(id.Value);
                if (Session != null)
                {
                    Session["enquiry"] = enquiry;
                }

                return View(enquiry);
            }
        }

        // POST: GeneralEnquirys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GeneralEnquiryId,ChatRoomId,IncidentTypeId,IncidentNo,Background,Status,IssueById,IssueDateTime,CloseById,CloseDateTime,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] GeneralEnquiry generalEnquiry)
        {
            if (Session != null && Session["enquiry"] != null)
            {
                enquiry = Session["enquiry"] as GeneralEnquiryModel;
                enquiry.Entity = generalEnquiry;
            }

            if (ModelState.IsValid)
            {
                new GeneralEnquiryApiController().EditGeneralEnquiry(enquiry);

                return RedirectToAction("Details", new { id = enquiry.Entity.GeneralEnquiryId, message = "The Incident had been updated successfully!" });
            }
            return View(enquiry);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void LoadData(int enquiryId)
        {
            this.enquiry = new GeneralEnquiryApiController().GetGeneralEnquiry(enquiryId);
        }

        public ActionResult CloseEnquiry()
        {
            if (Session != null && Session["enquiry"] != null)
            {
                enquiry = Session["enquiry"] as GeneralEnquiryModel;
            }
            enquiry.Entity.Status = EnquiryStatus.Closed.ToEnumString();

            return Edit(enquiry.Entity);
        }

        public ActionResult CreateIncident()
        {
            if (Session != null && Session["enquiry"] != null)
            {
                enquiry = Session["enquiry"] as GeneralEnquiryModel;
            }

            string targetController = "IncidentEnvironmentAirEmission";

            foreach (IncidentTypeSubType incidentType in Enum.GetValues(typeof(IncidentTypeSubType)))
            {
                if (MasterTableService.GetIncidentTypeId(incidentType) == enquiry.Entity.IncidentTypeId)
                {
                    targetController = "Incident" + incidentType.ToEnumString();
                    break;
                }
            }

            IncidentEnvironmentAirEmission incidentEnvironmentAirEmission = new IncidentEnvironmentAirEmission();
            IncidentEnvironmentLeakage incidentEnvironmentLeakage = new IncidentEnvironmentLeakage();
            IncidentOHS incidentOHS = new IncidentOHS();
            IncidentQualityCorporateImage incidentQualityCorporateImage = new IncidentQualityCorporateImage();
            IncidentQualityGeneration incidentQualityGeneration = new IncidentQualityGeneration();
            IncidentQualityNetwork incidentQualityNetwork = new IncidentQualityNetwork();
            IncidentSystemBilling incidentSystemBilling = new IncidentSystemBilling();
            IncidentSystemCallCentre incidentSystemCallCentre = new IncidentSystemCallCentre();
            IncidentSystemInvoicing incidentSystemInvoicing = new IncidentSystemInvoicing();
            IncidentSystemITSystem incidentSystemITSystem = new IncidentSystemITSystem();
            IncidentSystemNetworkConnectivity incidentSystemNetworkConnectivity = new IncidentSystemNetworkConnectivity();
            IncidentSystemOTSystem incidentSystemOTSystem = new IncidentSystemOTSystem();

            incidentEnvironmentAirEmission.IncidentBackground = enquiry.Entity.Background;
            incidentEnvironmentAirEmission.IssueById = enquiry.Entity.IssueById;
            incidentEnvironmentAirEmission.IssueDateTime = enquiry.Entity.IssueDateTime;
            incidentEnvironmentAirEmission.GeneralEnquiryId = enquiry.Entity.GeneralEnquiryId;

            incidentEnvironmentLeakage.IncidentBackground = enquiry.Entity.Background;
            incidentEnvironmentLeakage.IssueById = enquiry.Entity.IssueById;
            incidentEnvironmentLeakage.IssueDateTime = enquiry.Entity.IssueDateTime;
            incidentEnvironmentLeakage.GeneralEnquiryId = enquiry.Entity.GeneralEnquiryId;

            incidentOHS.IncidentBackground = enquiry.Entity.Background;
            incidentOHS.IssueById = enquiry.Entity.IssueById;
            incidentOHS.IssueDateTime = enquiry.Entity.IssueDateTime;
            incidentOHS.GeneralEnquiryId = enquiry.Entity.GeneralEnquiryId;

            incidentQualityCorporateImage.IncidentBackground = enquiry.Entity.Background;
            incidentQualityCorporateImage.IssueById = enquiry.Entity.IssueById;
            incidentQualityCorporateImage.IssueDateTime = enquiry.Entity.IssueDateTime;
            incidentQualityCorporateImage.GeneralEnquiryId = enquiry.Entity.GeneralEnquiryId;

            incidentQualityGeneration.IncidentBackground = enquiry.Entity.Background;
            incidentQualityGeneration.IssueById = enquiry.Entity.IssueById;
            incidentQualityGeneration.IssueDateTime = enquiry.Entity.IssueDateTime;
            incidentQualityGeneration.GeneralEnquiryId = enquiry.Entity.GeneralEnquiryId;

            incidentQualityNetwork.IncidentBackground = enquiry.Entity.Background;
            incidentQualityNetwork.IssueById = enquiry.Entity.IssueById;
            incidentQualityNetwork.IssueDateTime = enquiry.Entity.IssueDateTime;
            incidentQualityNetwork.GeneralEnquiryId = enquiry.Entity.GeneralEnquiryId;

            incidentSystemBilling.IncidentBackground = enquiry.Entity.Background;
            incidentSystemBilling.IssueById = enquiry.Entity.IssueById;
            incidentSystemBilling.IssueDateTime = enquiry.Entity.IssueDateTime;
            incidentSystemBilling.GeneralEnquiryId = enquiry.Entity.GeneralEnquiryId;

            incidentSystemCallCentre.IncidentBackground = enquiry.Entity.Background;
            incidentSystemCallCentre.IssueById = enquiry.Entity.IssueById;
            incidentSystemCallCentre.IssueDateTime = enquiry.Entity.IssueDateTime;
            incidentSystemCallCentre.GeneralEnquiryId = enquiry.Entity.GeneralEnquiryId;

            incidentSystemInvoicing.IncidentBackground = enquiry.Entity.Background;
            incidentSystemInvoicing.IssueById = enquiry.Entity.IssueById;
            incidentSystemInvoicing.IssueDateTime = enquiry.Entity.IssueDateTime;
            incidentSystemInvoicing.GeneralEnquiryId = enquiry.Entity.GeneralEnquiryId;

            incidentSystemITSystem.IncidentBackground = enquiry.Entity.Background;
            incidentSystemITSystem.IssueById = enquiry.Entity.IssueById;
            incidentSystemITSystem.IssueDateTime = enquiry.Entity.IssueDateTime;
            incidentSystemITSystem.GeneralEnquiryId = enquiry.Entity.GeneralEnquiryId;

            incidentSystemNetworkConnectivity.IncidentBackground = enquiry.Entity.Background;
            incidentSystemNetworkConnectivity.IssueById = enquiry.Entity.IssueById;
            incidentSystemNetworkConnectivity.IssueDateTime = enquiry.Entity.IssueDateTime;
            incidentSystemNetworkConnectivity.GeneralEnquiryId = enquiry.Entity.GeneralEnquiryId;

            incidentSystemOTSystem.IncidentBackground = enquiry.Entity.Background;
            incidentSystemOTSystem.IssueById = enquiry.Entity.IssueById;
            incidentSystemOTSystem.IssueDateTime = enquiry.Entity.IssueDateTime;
            incidentSystemOTSystem.GeneralEnquiryId = enquiry.Entity.GeneralEnquiryId;

            switch (MasterTableService.GetIncidentTypeSubType(enquiry.Entity.IncidentTypeId))
            {
                case IncidentTypeSubType.EnvironmentAirEmission:
                    return RedirectToAction("CreateByEnquiry", targetController, incidentEnvironmentAirEmission);
                case IncidentTypeSubType.EnvironmentLeakage:
                    return RedirectToAction("CreateByEnquiry", targetController, incidentEnvironmentLeakage);
                case IncidentTypeSubType.OHS:
                    return RedirectToAction("CreateByEnquiry", targetController, incidentOHS);
                case IncidentTypeSubType.QualityCorporateImage:
                    return RedirectToAction("CreateByEnquiry", targetController, incidentQualityCorporateImage);
                case IncidentTypeSubType.QualityGeneration:
                    return RedirectToAction("CreateByEnquiry", targetController, incidentQualityGeneration);
                case IncidentTypeSubType.QualityNetwork:
                    return RedirectToAction("CreateByEnquiry", targetController, incidentQualityNetwork);
                case IncidentTypeSubType.SystemBilling:
                    return RedirectToAction("CreateByEnquiry", targetController, incidentSystemBilling);
                case IncidentTypeSubType.SystemInvoicing:
                    return RedirectToAction("CreateByEnquiry", targetController, incidentSystemInvoicing);
                case IncidentTypeSubType.SystemCallCentre:
                    return RedirectToAction("CreateByEnquiry", targetController, incidentSystemCallCentre);
                case IncidentTypeSubType.SystemITSystem:
                    return RedirectToAction("CreateByEnquiry", targetController, incidentSystemITSystem);
                case IncidentTypeSubType.SystemNetworkConnectivity:
                    return RedirectToAction("CreateByEnquiry", targetController, incidentSystemNetworkConnectivity);
                case IncidentTypeSubType.SystemOTSystem:
                    return RedirectToAction("CreateByEnquiry", targetController, incidentSystemOTSystem);
            }

            return Details(enquiry.Entity.IncidentTypeId, null);
        }
    }
}
