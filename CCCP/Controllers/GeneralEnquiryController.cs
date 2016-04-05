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
    }
}
